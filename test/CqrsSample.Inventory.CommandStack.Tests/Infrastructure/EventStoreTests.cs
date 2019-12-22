using AutoFixture;
using AutoFixture.AutoMoq;
using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Infrastructure;
using Moq;
using NEventStore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  [TestFixture]
  public sealed class EventStoreTests
  {
    private Mock<IStoreEvents> _storeMock;

    [SetUp]
    public void Init() 
    {
      _storeMock = new Mock<IStoreEvents>();
    }

    [Test]
    public void GetEventsForAggregate_Returns_Events_Contained_In_Underlying_Event_Store()
    {
      // ARRANGE
      var fixture = new Fixture();
      fixture.Customize(new AutoMoqCustomization());

      var events = fixture.CreateMany<Event>(2);

      var streamMock = new Mock<IEventStream>();
      streamMock
        .Setup(m => m.CommittedEvents)
        .Returns(() => events.Select(e => new EventMessage { Body = e }).ToList());

      _storeMock
        .Setup(m =>
          m.OpenStream(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()
          )
        )
        .Returns(streamMock.Object);

      var target = this.CreateTarget();

      // ACT
       var result = target.GetEventsForAggregate(Guid.NewGuid());

      // ASSERT
      Assert.IsNotNull(result);
      Assert.AreEqual(2, result.Count);
      CollectionAssert.AreEqual(events, result);
    }

    [Test]
    public void GetEventsForAggregate_Reads_Events_From_Underlying_Event_Store()
    {
      // ARRANGE
      var fixture = new Fixture();
      fixture.Customize(new AutoMoqCustomization());

      var events = fixture.CreateMany<Event>(2);

      var streamMock = new Mock<IEventStream>();
      streamMock
        .Setup(m => m.CommittedEvents)
        .Returns(() => events.Select(e => new EventMessage { Body = e }).ToList());

      _storeMock
        .Setup(m =>
          m.OpenStream(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()
          )
        )
        .Returns(streamMock.Object);

      var target = this.CreateTarget();

      // ACT
      var aggregateId = Guid.NewGuid();
      _ = target.GetEventsForAggregate(aggregateId);

      // ASSERT
      _storeMock
        .Verify(m =>
          m.OpenStream(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()
          ),
          Times.Once()
        );

      _storeMock
        .Verify(m =>
          m.OpenStream(
            It.IsAny<string>(),
            aggregateId.ToString(),
            0,
            int.MaxValue
          ),
          Times.Once()
        );
    }

    private EventStore CreateTarget()
    {
      return new EventStore(_storeMock.Object);
    }
  }
}
