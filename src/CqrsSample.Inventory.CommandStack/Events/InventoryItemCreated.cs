using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when an inventory item is created
  /// </summary>
  public sealed class InventoryItemCreated : Event
  {
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
