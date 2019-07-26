using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CqrsSample.Inventory.CommandStack.Events;
using ReflectionMagic;

namespace CqrsSample.Inventory.CommandStack.Model
{
  /// <summary>
  /// The base class for all the aggregate roots of the domain.
  /// </summary>
  public abstract class AggregateRoot : IAggregateRoot
  {
    private readonly List<Event> uncommitedChanges = new List<Event>();

    /// <summary>
    /// Gets the aggregate unique identifier
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Gets or sets the aggregate version
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Gets the uncommited aggregate changes
    /// </summary>
    /// <returns>A sequence containing all the uncommited aggregate changes</returns>
    IEnumerable<Event> IAggregateRoot.GetUncommittedChanges() => this.uncommitedChanges.ToImmutableArray();

    /// <summary>
    /// Marks all uncommited aggregate changes as committed
    /// </summary>
    void IAggregateRoot.MarkChangesAsCommitted() => this.uncommitedChanges.Clear();

    /// <summary>
    /// Rehydrate the aggregate from a stream of domain events
    /// </summary>
    /// <param name="history">The aggregate history as a stream of domain events</param>
    void IAggregateRoot.LoadsFromHistory(IEnumerable<Event> history)
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
