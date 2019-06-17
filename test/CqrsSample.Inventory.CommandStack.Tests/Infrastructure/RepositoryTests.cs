using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Infrastructure;
using CqrsSample.Inventory.CommandStack.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Reflection;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  [TestFixture]
  public class RepositoryTest
  {

    [Test]
    public void GetById_Is_Able_To_Rehydrate_Aggregate_From_Event_Stream()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var events = new Event[]
      {
        new PersonCreated("Bob", 26, 1),
        new NameChanged(2, "Bob", "Alice"),
        new AgeChanged(3, 26, 22)
      };

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
    }

    [Test]
    public void GetById_Returns_Aggregate_With_Default_Values_When_Event_Store_Contains_No_Events_For_The_Aggregate()
    {
      // ARRANGE
      var aggregateId = Guid.NewGuid();

      var events = new Event[]
      {
        new PersonCreated("Bob", 26, 1),
        new NameChanged(2, "Bob", "Alice"),
        new AgeChanged(3, 26, 22)
      };

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
    }

    public class Person : AggregateRoot
    {
      private Person()
      {
      }

      public string Name { get; private set; }
      public int Age { get; private set; }

      private void Apply(PersonCreated @event)
      {
        if (@event == null)
          throw new ArgumentNullException(nameof(@event));

        this.Name = @event.Name;
        this.Age = @event.Age;
      }

      private void Apply(NameChanged @event)
      {
        if (@event == null)
          throw new ArgumentNullException(nameof(@event));

        this.Name = @event.NewName;
      }

      private void Apply(AgeChanged @event)
      {
        if (@event == null)
          throw new ArgumentNullException(nameof(@event));

        this.Age = @event.NewAge;
      }
    }

    public class PersonCreated : Event
    {
      public PersonCreated(
        string name,
        int age,
        int aggregateVersion)
        : base(aggregateVersion)
      {
        this.Name = name;
        this.Age = age;
      }

      public string Name { get; }
      public int Age { get; }
    }

    public class NameChanged : Event
    {
      public NameChanged(
        int aggregateVersion,
        string oldName,
        string newName)
        : base(aggregateVersion)
      {
        this.OldName = oldName;
        this.NewName = newName;
      }

      public string OldName { get; }
      public string NewName { get; }
    }

    public class AgeChanged : Event
    {
      public AgeChanged(
        int aggregateVersion,
        int oldAge,
        int newAge)
        : base(aggregateVersion)
      {
        this.OldAge = oldAge;
        this.NewAge = newAge;
      }

      public int OldAge { get; }
      public int NewAge { get; }
    }
  }
}
