using CqrsSample.Inventory.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    /// Throws <see cref="ArgumentNullException"/> when parameter <paramref name="events"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws <see cref="ArgumentOutOfRangeException"/> when parameter <paramref name="expectedVersion"/> 
    /// is less than zero.
    /// </exception>
    void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);

    /// <summary>
    /// Gets the full event stream for an aggregate. The returned event stream is sorted ascending by aggregate version. 
    /// </summary>
    /// <param name="aggregateId">
    /// The unique identifier of the aggregate for which the event stream is returned.
    /// </param>
    /// <returns>
    /// The full aggregate event stream sorted by aggregate version ascending.
    /// </returns>
    ReadOnlyCollection<Event> GetEventsForAggregate(Guid aggregateId);
  }
}
