using System;

namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// The domain event raised when some items are removed from the inventory
  /// </summary>
  public class ItemsRemovedFromInventory : Event
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemsRemovedFromInventory"/> class.
    /// </summary>
    /// <param name="aggregateVersion">The aggregate version at the moment when the event was raised</param>
    /// <param name="id">The unique identifier of the inventory item</param>
    /// <param name="numberOfRemovedItems">The number of items removed from the inventory</param>
    public ItemsRemovedFromInventory(
      int aggregateVersion,
      Guid id,
      int numberOfRemovedItems)
      : base(aggregateVersion)
    {
      this.Id = id;
      this.NumberOfRemovedItems = numberOfRemovedItems;
    }

    /// <summary>
    /// Gets the unique identifier of the inventory item
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the number of items removed from the inventory
    /// </summary>
    public int NumberOfRemovedItems { get; }
  }
}
