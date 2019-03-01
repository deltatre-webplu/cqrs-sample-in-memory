using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when some items are added to the inventory
  /// </summary>
  public sealed class ItemsAddedToInventory : Event
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemsAddedToInventory"/> class.
    /// </summary>
    /// <param name="aggregateVersion">The aggregate version at the moment when the event was raised</param>
    /// <param name="id">The unique identifier of the inventory item</param>
    /// <param name="numberOfAddedItems">The number of items added to the inventory</param>
    public ItemsAddedToInventory(
      int aggregateVersion,
      Guid id,
      int numberOfAddedItems)
      : base(aggregateVersion)
    {
      this.Id = id;
      this.NumberOfAddedItems = numberOfAddedItems;
    }

    /// <summary>
    /// Gets the unique identifier of the inventory item
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the number of items added to the inventory
    /// </summary>
    public int NumberOfAddedItems { get; }
  }
}
