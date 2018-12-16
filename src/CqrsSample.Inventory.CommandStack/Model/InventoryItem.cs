﻿using CqrsSample.Inventory.CommandStack.Events;
using System;

namespace CqrsSample.Inventory.CommandStack.Model
{
  /// <summary>
  /// Represents an item which is part of an inventary
  /// </summary>
  public sealed class InventoryItem : AggregateRoot
  {
    /// <summary>
    /// Gets the name of the inventory item
    /// </summary>
    public string Name { get; private set; }

    private void Apply(InventoryItemCreated @event)
    {
      this.Id = @event.Id;
      this.Name = @event.Name;
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
