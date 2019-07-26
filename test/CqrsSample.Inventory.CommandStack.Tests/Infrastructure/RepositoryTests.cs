using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Model;
using System;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  public partial class RepositoryTests
  {
    public sealed class Person : AggregateRoot
    {
      private Person()
      {
      }

      public string Name { get; private set; }
      public int Age { get; private set; }

      public void ChangeName(string newName)
      {
        var @event = new NameChanged(this.Id, this.Version + 1, this.Name, newName);
        this.RaiseEvent(@event);
      }

      public void ChangeAge(int newAge)
      {
        var @event = new AgeChanged(this.Id, this.Version + 1, this.Age, newAge);
        this.RaiseEvent(@event);
      }

      private void Apply(PersonCreated @event)
      {
        if (@event == null)
          throw new ArgumentNullException(nameof(@event));

        this.Name = @event.Name;
        this.Age = @event.Age;
        this.Id = @event.Id;
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

      public static class Factory
      {
        public static Person CreateNewInstance(Guid id, string name, int age)
        {
          var @event = new PersonCreated(id, name, age, 1);
          var aggregate = new Person();
          aggregate.RaiseEvent(@event);

          return aggregate;
        }
      }
    }

    public sealed class PersonCreated : Event
    {
      public PersonCreated(
        Guid id,
        string name,
        int age,
        int aggregateVersion)
        : base(aggregateVersion)
      {
        this.Id = id;
        this.Name = name;
        this.Age = age;
      }

      public Guid Id { get; }
      public string Name { get; }
      public int Age { get; }
    }

    public sealed class NameChanged : Event
    {
      public NameChanged(
        Guid id,
        int aggregateVersion,
        string oldName,
        string newName)
        : base(aggregateVersion)
      {
        this.Id = id;
        this.OldName = oldName;
        this.NewName = newName;
      }

      public Guid Id { get; }
      public string OldName { get; }
      public string NewName { get; }
    }

    public sealed class AgeChanged : Event
    {
      public AgeChanged(
        Guid id,
        int aggregateVersion,
        int oldAge,
        int newAge)
        : base(aggregateVersion)
      {
        this.Id = id;
        this.OldAge = oldAge;
        this.NewAge = newAge;
      }

      public Guid Id { get; }
      public int OldAge { get; }
      public int NewAge { get; }
    }
  }
}
