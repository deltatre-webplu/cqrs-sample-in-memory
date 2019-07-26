using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Infrastructure;
using CqrsSample.Inventory.CommandStack.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  [TestFixture]
  public partial class RepositoryTests
  {
    [Test]
    public void Save_Throws_ArgumentNullException_When_Aggregate_Is_Null()
    {
      // ARRANGE
      var eventStoreMock = new Mock<IEventStore>();
      var target = new Repository(eventStoreMock.Object);

      // ACT
      var exception = Assert.Throws<ArgumentNullException>(() => target.Save(null, 3));

      // ASSERT
      Assert.IsNotNull(exception);
      Assert.AreEqual("aggregate", exception.ParamName);
    }

    [Test]
    public void Save_Throws_ArgumentOutOfRangeException_When_ExpectedVersion_Is_Less_Than_Zero()
    {
      // ARRANGE
      var eventStoreMock = new Mock<IEventStore>();
      var target = new Repository(eventStoreMock.Object);

      var aggregate = Person.Factory.CreateNewInstance(Guid.NewGuid(), "Bob", 26);

      // ACT
      var exception = Assert.Throws<ArgumentOutOfRangeException>(() => target.Save(aggregate, -1));

      // ASSERT
      Assert.IsNotNull(exception);
      Assert.AreEqual("expectedVersion", exception.ParamName);
    }

    [Test]
    public void Save_Is_Able_To_Save_Aggregate_For_The_Very_First_Time()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var aggregate = Person.Factory.CreateNewInstance(aggregateId, "Bob", 26); // newly created aggregate

      Expression<Func<IEnumerable<Event>, bool>> predicate =
        events =>
          events.Count() == 1
          && (events.Single() is PersonCreated)
          && ((PersonCreated)events.Single()).Age == 26
          && ((PersonCreated)events.Single()).Name == "Bob"
          && ((PersonCreated)events.Single()).Id == aggregateId
          && events.Single().AggregateVersion == 1;

      var eventStoreMock = new Mock<IEventStore>(MockBehavior.Strict);
      eventStoreMock
        .Setup(m => m.SaveEvents(aggregateId, It.Is<IEnumerable<Event>>(predicate), 0));

      var target = new Repository(eventStoreMock.Object);

      // ACT
      target.Save(aggregate, 0); // save the aggregate for the very first time

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)aggregate).GetUncommittedChanges();
      Assert.IsEmpty(uncommittedChanges);

      eventStoreMock
        .Verify(
          m => m.SaveEvents(It.IsAny<Guid>(), It.IsAny<IEnumerable<Event>>(), It.IsAny<int>()),
          Times.Once
        );

      eventStoreMock
        .Verify(
          m => m.SaveEvents(aggregateId, It.Is<IEnumerable<Event>>(predicate), 0),
          Times.Once
        );
    }

    [Test]
    public void Save_Is_Able_To_Save_Aggregate()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var aggregate = Person.Factory.CreateNewInstance(aggregateId, "Bob", 26); // newly created aggregate
      ((IAggregateRoot)aggregate).MarkChangesAsCommitted(); // simulate the first save of the aggregate after its creation
      aggregate.ChangeName("Alice");
      aggregate.ChangeAge(24);

      Expression<Func<IEnumerable<Event>, bool>> predicate =
        events =>
          events.Count() == 2

          && (events.ElementAt(0) is NameChanged)
          && ((NameChanged)events.ElementAt(0)).NewName == "Alice"
          && ((NameChanged)events.ElementAt(0)).OldName == "Bob"
          && ((NameChanged)events.ElementAt(0)).Id == aggregateId
          && events.ElementAt(0).AggregateVersion == 2

          && (events.ElementAt(1) is AgeChanged)
          && ((AgeChanged)events.ElementAt(1)).NewAge == 24
          && ((AgeChanged)events.ElementAt(1)).OldAge == 26
          && ((AgeChanged)events.ElementAt(1)).Id == aggregateId
          && events.ElementAt(1).AggregateVersion == 3;

      var eventStoreMock = new Mock<IEventStore>(MockBehavior.Strict);
      eventStoreMock
        .Setup(m => m.SaveEvents(aggregateId, It.Is<IEnumerable<Event>>(predicate), 1));

      var target = new Repository(eventStoreMock.Object);

      // ACT
      target.Save(aggregate, 1); // save the aggregate to the event store

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)aggregate).GetUncommittedChanges();
      Assert.IsEmpty(uncommittedChanges);

      eventStoreMock
        .Verify(
          m => m.SaveEvents(It.IsAny<Guid>(), It.IsAny<IEnumerable<Event>>(), It.IsAny<int>()),
          Times.Once
        );

      eventStoreMock
        .Verify(
          m => m.SaveEvents(aggregateId, It.Is<IEnumerable<Event>>(predicate), 1),
          Times.Once
        );
    }
  }
}
