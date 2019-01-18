using CqrsSample.Inventory.CommandStack.Events;
using System.Collections.Generic;

namespace CqrsSample.Inventory.CommandStack.Model
{
  /// <summary>
  /// Interface modelling the behaviour of an aggregate root
  /// </summary>
  public interface IAggregateRoot
  {
    /// <summary>
    /// Gets the uncommited aggregate changes
    /// </summary>
    /// <returns>A sequence containing all the uncommited aggregate changes</returns>
    IEnumerable<Event> GetUncommittedChanges();

    /// <summary>
    /// Marks all uncommited aggregate changes as committed
    /// </summary>
    void MarkChangesAsCommitted();

    /// <summary>
    /// Rehydrate the aggregate from a stream of domain events
    /// </summary>
    /// <param name="history">The aggregate history as a stream of domain events</param>
    void LoadsFromHistory(IEnumerable<Event> history);
  }
}
