using CqrsSample.Inventory.CommandStack.Model;
using System;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  /// <summary>
  /// Provides an abstraction to be used to read the current state of an aggregate 
  /// and to persist its uncommitted events.
  /// </summary>
  public interface IRepository
  {
    /// <summary>
    /// Gets the current state of an aggregate by rehydrating it from its event stream.
    /// </summary>
    /// <typeparam name="TAggregate">
    /// The type of the aggregate being rehydrated from the event stream.
    /// </typeparam>
    /// <param name="aggregateId">
    /// The unique identifier of the aggregate being rehydrated from the event stream.
    /// </param>
    /// <returns>
    /// The aggregate at its current state.
    /// </returns>
    TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot;

    /// <summary>
    /// Saves the uncommitted events of an aggregate to the event store. 
    /// </summary>
    /// <param name="aggregate">
    /// The aggregate for which you want to save the uncommitted events to the event store.
    /// </param>
    /// <param name="expectedVersion">
    /// The aggregate version being expected by the method caller.
    /// This is used in order to perform the optimistic concurrency check before appending
    /// the new events to the aggregate's event stream.
    /// This parameter is expected to be a non negative integer.
    /// Pass the value zero (0) when you are saving an aggregate for the very first time.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Throws <see cref="ArgumentNullException"/> when parameter <paramref name="aggregate"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws <see cref="ArgumentOutOfRangeException"/> when parameter <paramref name="expectedVersion"/> 
    /// is less than zero.
    /// </exception>
    void Save(AggregateRoot aggregate, int expectedVersion);
  }
}
