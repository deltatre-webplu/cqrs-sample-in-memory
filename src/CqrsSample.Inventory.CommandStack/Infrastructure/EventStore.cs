using System;
using System.Collections.Generic;
using CqrsSample.Inventory.CommandStack.Events;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
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
