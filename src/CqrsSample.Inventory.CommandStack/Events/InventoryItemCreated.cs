using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when an inventory item is created
  /// </summary>
  public sealed class InventoryItemCreated : Event
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemCreated"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the inventory item</param>
    /// <param name="name">The name of the inventory item</param>
    /// <param name="aggregateVersion">The aggregate version at the moment when the event was raised</param>
    public InventoryItemCreated(
      Guid id,
      string name,
      int aggregateVersion) : base(aggregateVersion)
    {
      this.Id = id;
      this.Name = name;
    }

    /// <summary>
    /// Gets the unique identifier for the inventory item
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the name of the inventory item
    /// </summary>
    public string Name { get; }
  }
}
