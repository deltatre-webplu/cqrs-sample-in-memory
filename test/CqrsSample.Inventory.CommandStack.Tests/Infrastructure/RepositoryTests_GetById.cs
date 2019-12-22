using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Infrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  [TestFixture]
  public partial class RepositoryTests
  {
    [Test]
    public void GetById_Is_Able_To_Rehydrate_Aggregate_From_Event_Stream()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var events = new List<Event>
      {
        new PersonCreated(aggregateId, "Bob", 26, 1),
        new NameChanged(aggregateId, 2, "Bob", "Alice"),
        new AgeChanged(aggregateId, 3, 26, 22)
      }.AsReadOnly();

      var eventStoreMock = new Mock<IEventStore>(MockBehavior.Strict);
      eventStoreMock
        .Setup(m => m.GetEventsForAggregate(aggregateId))
        .Returns(events);

      var target = new Repository(eventStoreMock.Object);

      // ACT
      var result = target.GetById<Person>(aggregateId);

      // ASSERT
      Assert.IsNotNull(result);
      Assert.AreEqual(3, result.Version);
      Assert.AreEqual("Alice", result.Name);
      Assert.AreEqual(22, result.Age);
      Assert.AreEqual(aggregateId, result.Id);

      // chec mock calls
      eventStoreMock.Verify(m => m.GetEventsForAggregate(It.IsAny<Guid>()), Times.Once());
      eventStoreMock.Verify(m => m.GetEventsForAggregate(aggregateId), Times.Once());
    }

    [Test]
    public void GetById_Returns_Aggregate_With_Default_Values_When_Event_Store_Contains_No_Events_For_The_Aggregate()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var eventStoreMock = new Mock<IEventStore>(MockBehavior.Strict);
      eventStoreMock
        .Setup(m => m.GetEventsForAggregate(aggregateId))
        .Returns(Enumerable.Empty<Event>().ToList().AsReadOnly());

      var target = new Repository(eventStoreMock.Object);

      // ACT
      var result = target.GetById<Person>(aggregateId);

      // ASSERT
      Assert.IsNotNull(result);
      Assert.AreEqual(0, result.Version);
      Assert.IsNull(result.Name);
      Assert.AreEqual(0, result.Age);
      Assert.AreEqual(Guid.Empty, result.Id);

      // chec mock calls
      eventStoreMock.Verify(m => m.GetEventsForAggregate(It.IsAny<Guid>()), Times.Once());
      eventStoreMock.Verify(m => m.GetEventsForAggregate(aggregateId), Times.Once());
    }
  }
}
