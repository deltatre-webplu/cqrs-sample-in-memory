using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when the name of an inventory item has been changed
  /// </summary>
  public sealed class InventoryItemRenamed : Event
  {
    public InventoryItemRenamed(
      int aggregateVersion,
      Guid id,
      string newName,
      string oldName)
      : base(aggregateVersion)
    {
      this.Id = id;
      this.NewName = newName;
      this.OldName = oldName;
    }

    /// <summary>
    /// Gets the unique identifier for the inventory item
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the new name of the inventory item
    /// </summary>
    public string NewName { get; }

    /// <summary>
    /// Gets the old name of the inventory item
    /// </summary>
    public string OldName { get; }
  }
}
