using CqrsSample.Inventory.CommandStack.Infrastructure;
using Moq;
using NEventStore;
using NUnit.Framework;

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
    public void GetEventsForAggregate_Read_Events_From_Underlying_Event_Store() 
    {
      // ARRANGE
      
      // ACT

      // ASSERT
    }

    private EventStore CreateTarget()
    {
      return new EventStore(_storeMock.Object);
    }
  }
}
