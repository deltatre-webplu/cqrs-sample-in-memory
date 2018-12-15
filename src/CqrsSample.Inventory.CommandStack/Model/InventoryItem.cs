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

    public static class Factory
    {
      public static InventoryItem CreateNew(Guid id, string name)
      {
        throw new NotImplementedException();
      }
    }
  }
}
