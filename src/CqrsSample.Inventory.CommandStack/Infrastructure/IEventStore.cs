using CqrsSample.Inventory.CommandStack.Events;
using System;
using System.Collections.Generic;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  public interface IEventStore
  {
    void SaveEvents(Guid aggregateId, IEnumerable<Event> events);
    IEnumerable<Event> GetEventsForAggregate(Guid aggregateId);
  }
}
