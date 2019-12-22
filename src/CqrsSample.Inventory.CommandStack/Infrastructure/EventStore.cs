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

    public EventStore(IStoreEvents store)
    {
      _store = store ?? throw new ArgumentNullException(nameof(store));
    }

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

    public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
    {
      throw new NotImplementedException();
    }
  }
}
