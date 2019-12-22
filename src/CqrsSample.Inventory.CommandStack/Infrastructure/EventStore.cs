using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CqrsSample.Inventory.CommandStack.Events;
using NEventStore;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  /// <summary>
  /// An implementation of <see cref="IEventStore"/> interface based on the NEventStore library 
  /// </summary>
  public sealed class EventStore : IEventStore
  {
    private readonly IStoreEvents _store;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStore"/> class.
    /// </summary>
    /// <param name="store">
    /// The proxy object used to access the underlying physical event store.
    /// Cannot be null.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Throws <see cref="ArgumentNullException"/> when <paramref name="store"/> is null.
    /// </exception>
    public EventStore(IStoreEvents store)
    {
      _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    /// <summary>
    /// Gets the full event stream for an aggregate. 
    /// The returned event stream is sorted ascending by aggregate version. 
    /// </summary>
    /// <param name="aggregateId">
    /// The unique identifier of the aggregate for which the event stream is returned.
    /// </param>
    /// <returns>
    /// The full aggregate event stream sorted by aggregate version ascending.
    /// </returns>
    public ReadOnlyCollection<Event> GetEventsForAggregate(Guid aggregateId)
    {
      using (var stream = _store.OpenStream(aggregateId, minRevision: 0, maxRevision: int.MaxValue))
      {
        var aggregateEvents = stream
          .CommittedEvents
          .Select(e => e.Body)
          .Cast<Event>();

        return new List<Event>(aggregateEvents).AsReadOnly();
      }
    }

    /// <summary>
    /// Appends some events to the event stream of a specific aggregate.
    /// </summary>
    /// <param name="aggregateId">
    /// The unique identifier of the aggregate for which the events must be appended to the event stream.
    /// </param>
    /// <param name="events">
    /// The events to be appended to the aggregate's event stream.
    /// </param>
    /// <param name="expectedVersion">
    /// The aggregate version being expected by the method caller.
    /// This is used in order to perform the optimistic concurrency check before appending the new events 
    /// to the event stream.
    /// This parameter is expected to be a non negative integer. 
    /// Pass the value zero (0) when you are creating a fresh new event stream.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Throws <see cref="ArgumentNullException" /> when parameter <paramref name="events" /> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws <see cref="ArgumentOutOfRangeException" /> when parameter <paramref name="expectedVersion" /> 
    /// is less than zero.
    /// </exception>
    public void SaveEvents(
      Guid aggregateId,
      IEnumerable<Event> events,
      int expectedVersion)
    {
      if (events == null)
        throw new ArgumentNullException(nameof(events));

      if (expectedVersion < 0)
      {
        throw new ArgumentOutOfRangeException(
          nameof(expectedVersion),
          "The expected aggregate version must be a non negative integer."
        );
      }

      if (IsNewStream())
      {
        CreateNewStream();
      }
      else
      {
        AppendEventsToExistingStream();
      }

      bool IsNewStream() => expectedVersion == 0;

      void CreateNewStream()
      {
        using (var stream = this._store.CreateStream(aggregateId))
        {
          foreach (var @event in events)
          {
            stream.Add(new EventMessage { Body = @event });
          }

          stream.CommitChanges(Guid.NewGuid());
        }
      }

      void AppendEventsToExistingStream()
      {
        using (var stream = this._store.OpenStream(aggregateId, minRevision: 0, maxRevision: expectedVersion))
        {
          foreach (var @event in events)
          {
            stream.Add(new EventMessage { Body = @event });
          }

          stream.CommitChanges(Guid.NewGuid());
        }
      }
    }
  }
}
