using System;
using System.Collections.Generic;
using CqrsSample.Inventory.CommandStack.Events;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  /// <summary>
  /// An implementation of <see cref="IEventStore"/> interface based on NEventStore library 
  /// </summary>
  public sealed class EventStore : IEventStore
  {
    public IEnumerable<Event> GetEventsForAggregate(Guid aggregateId)
    {
      throw new NotImplementedException();
    }

    public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
    {
      throw new NotImplementedException();
    }
  }
}
