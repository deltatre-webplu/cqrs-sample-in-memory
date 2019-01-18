using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when an inventory item has been deactivated
  /// </summary>
  public sealed class InventoryItemDeactivated : Event
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemDeactivated"/> class.
    /// </summary>
    /// <param name="aggregateVersion">The aggregate version at the moment when the event was raised</param>
    /// <param name="id">The unique identifier of the inventory item</param>
    public InventoryItemDeactivated(
      int aggregateVersion,
      Guid id) 
      : base(aggregateVersion)
    {
      this.Id = id;
    }

    /// <summary>
    /// Gets the unique identifier for the inventory item
    /// </summary>
    public Guid Id { get; }
  }
}
