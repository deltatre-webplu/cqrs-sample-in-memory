using CqrsSample.Inventory.CommandStack.Events;
using System;

namespace CqrsSample.Inventory.CommandStack.Model
{
  /// <summary>
  /// Represents an item which is part of an inventary
  /// </summary>
  public sealed class InventoryItem : AggregateRoot
  {
    private InventoryItem()
    {

    }

    /// <summary>
    /// Gets the name of the inventory item
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether the inventory item is active. Deactivated inventory item are not available for usage.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Change the new of the inventory item
    /// </summary>
    /// <param name="newName">The new name to be assigned to the inventory item</param>
    /// <exception cref="ArgumentException">Throws <see cref="ArgumentException"/> when <paramref name="newName"/> is null or white space</exception>
    public void ChangeName(string newName)
    {
      if (string.IsNullOrWhiteSpace(newName))
      {
        throw new ArgumentException(
          $"Parameter '{nameof(newName)}' cannot be a string null or white space",
          nameof(newName));
      }

      if (this.Name == newName)
      {
        return;
      }

      var @event = new InventoryItemRenamed(this.Version, this.Id, newName, this.Name);
      this.RaiseEvent(@event);
    }

    /// <summary>
    /// Deactivates the inventory item so that it is not available anymore
    /// </summary>
    public void Deactivate()
    {
      throw new NotImplementedException();
    }

    private void Apply(InventoryItemCreated @event)
    {
      this.Id = @event.Id;
      this.Name = @event.Name;
    }

    private void Apply(InventoryItemRenamed @event)
    {
      this.Name = @event.NewName;
    }

    public static class Factory
    {
      /// <summary>
      /// Creates a new inventory item
      /// </summary>
      /// <param name="id">The unique identifier of the inventory item. Cannot be the empty guid</param>
      /// <param name="name">The name of the inventory item. Cannot be null or white space</param>
      /// <returns>The newly created inventory item</returns>
      /// <exception cref="ArgumentException">Throws <see cref="ArgumentException"/> when <paramref name="id"/> equals <see cref="Guid.Empty"/> or <paramref name="name"/> is null or white space</exception>
      public static InventoryItem CreateNew(Guid id, string name)
      {
        if (id == Guid.Empty)
        {
          throw new ArgumentException(
            $"Parameter '{nameof(id)}' cannot be the empty guid",
            nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
          throw new ArgumentException(
            $"Parameter '{nameof(name)}' cannot be a string null or white space",
            nameof(name));
        }

        var inventoryItem = new InventoryItem();

        var @event = new InventoryItemCreated(
          id,
          name,
          AggregateRoot.StartingVersion);
        inventoryItem.RaiseEvent(@event);

        return inventoryItem;
      }
    }
  }
}
