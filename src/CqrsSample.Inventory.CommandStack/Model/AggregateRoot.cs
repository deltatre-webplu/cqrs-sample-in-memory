using System;
using System.Collections.Generic;
using CqrsSample.Inventory.CommandStack.Events;
using ReflectionMagic;

namespace CqrsSample.Inventory.CommandStack.Model
{
  /// <summary>
  /// An abstraction for aggregate roots
  /// </summary>
  public abstract class AggregateRoot
  {
    private readonly List<Event> uncommitedChanges = new List<Event>();

    /// <summary>
    /// Gets the aggregate unique identifier
    /// </summary>
    public abstract Guid Id { get; }

    /// <summary>
    /// Gets or sets the aggregate version
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Gets the uncommited aggregate changes
    /// </summary>
    /// <returns>A sequence containing all the uncommited aggregate changes</returns>
    public IEnumerable<Event> GetUncommittedChanges() => this.uncommitedChanges;

    /// <summary>
    /// Marks all uncommited aggregate changes as committed
    /// </summary>
    public void MarkChangesAsCommitted() => this.uncommitedChanges.Clear();

    /// <summary>
    /// Rehydrate the aggregate from a stream of domain events
    /// </summary>
    /// <param name="history">The aggregate history as a stream of domain events</param>
    public void LoadsFromHistory(IEnumerable<Event> history)
    {
      if (history == null)
        throw new ArgumentNullException(nameof(history));

      foreach (var @event in history)
      {
        this.ApplyChange(@event, isNew: false);
      }
    }

    /// <summary>
    /// Raise a new domain event
    /// </summary>
    /// <param name="event">The domain event to be raised</param>
    protected void RaiseEvent(Event @event)
    {
      if (@event == null)
        throw new ArgumentNullException(nameof(@event));

      this.ApplyChange(@event, isNew: true);
    }

    private void ApplyChange(Event @event, bool isNew)
    {
      this.AsDynamic().Apply(@event);

      if (isNew)
      {
        this.uncommitedChanges.Add(@event);
      }

      this.Version++;
    }
  }
}
