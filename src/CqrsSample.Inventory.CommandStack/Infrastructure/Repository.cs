using System;
using CqrsSample.Inventory.CommandStack.Model;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  /// <summary>
  /// An implementation of the interface <see cref="IRepository"/> which uses the <see cref="IEventStore"/> abstraction in order to access the event store.
  /// </summary>
  public sealed class Repository : IRepository
  {
    private readonly IEventStore eventStore;

    /// <summary>
    /// Initializes a new instance of the class <see cref="Repository"/>.
    /// </summary>
    /// <param name="eventStore">The <see cref="IEventStore"/> instance to be used to save and read events.</param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> when parameter <paramref name="eventStore"/> is null.</exception>
    public Repository(IEventStore eventStore)
    {
      this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    /// <summary>
    /// Gets the current state of an aggregate by rehydrating it from its event stream.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate being rehydrated from the event stream.</typeparam>
    /// <param name="aggregateId">The unique identifier of the aggregate being rehydrated from the event stream.</param>
    /// <returns>The aggregate at its current state.</returns>
    public TAggregate GetById<TAggregate>(Guid aggregateId)
      where TAggregate : AggregateRoot
    {
      var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), nonPublic: true);

      var eventStream = this.eventStore.GetEventsForAggregate(aggregateId);
      ((IAggregateRoot)aggregate).LoadsFromHistory(eventStream);

      return aggregate;
    }

    /// <summary>
    /// Saves the uncommitted events of an aggregate to the event store. 
    /// </summary>
    /// <param name="aggregate">The aggregate for which you want to save the uncommitted events to the event store.</param>
    /// <param name="expectedVersion">
    /// The aggregate version being expected by the method caller.
    /// This is used in order to perform the optimistic concurrency check before appending the new events to the aggregate's event stream.
    /// This parameter is expected to be a non negative integer. Pass the value zero (0) when you are saving an aggregate for the very first time.
    /// </param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> when parameter <paramref name="aggregate"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws <see cref="ArgumentOutOfRangeException"/> when parameter <paramref name="expectedVersion"/> is less than zero.</exception>
    public void Save(AggregateRoot aggregate, int expectedVersion)
    {
      if (aggregate == null)
        throw new ArgumentNullException(nameof(aggregate));

      if (expectedVersion < 0)
      {
        throw new ArgumentOutOfRangeException(
          nameof(expectedVersion),
          $"Invalid aggregate expected version: {expectedVersion}. The aggregate expected version must be a non negative integer.");
      }

      var uncommittedEvents = ((IAggregateRoot)aggregate).GetUncommittedChanges();

      this.eventStore.SaveEvents(aggregate.Id, uncommittedEvents, expectedVersion);

      ((IAggregateRoot)aggregate).MarkChangesAsCommitted();
    }
  }
}
