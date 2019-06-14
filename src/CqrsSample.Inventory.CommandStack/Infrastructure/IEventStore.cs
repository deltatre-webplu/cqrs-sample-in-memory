using CqrsSample.Inventory.CommandStack.Events;
using System;
using System.Collections.Generic;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  /// <summary>
  /// This is an abstraction over the event store used by the application in order to read and write events.
  /// </summary>
  public interface IEventStore
  {
    /// <summary>
    /// Appends some events to the event stream of a specific aggregate.
    /// </summary>
    /// <param name="aggregateId">The unique identifier of the aggregate for which the events must be appended to the event stream.</param>
    /// <param name="events">The events to be appended to the aggregate's event stream.</param>
    /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> when parameter <paramref name="events"/> is null.</exception>
    void SaveEvents(Guid aggregateId, IEnumerable<Event> events);

    /// <summary>
    /// Gets the full event stream for an aggregate. The returned event stream is sorted ascending by aggregate version. 
    /// </summary>
    /// <param name="aggregateId">The unique identifier of the aggregate for which the event stream is returned.</param>
    /// <returns>The full aggregate event stream sorted by aggregate version ascending.</returns>
    IEnumerable<Event> GetEventsForAggregate(Guid aggregateId);
  }
}
