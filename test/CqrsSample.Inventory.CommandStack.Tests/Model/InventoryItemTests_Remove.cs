using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace CqrsSample.Inventory.CommandStack.Tests.Model
{
  [TestFixture]
  public partial class InventoryItemTests
  {
    [Test]
    public void Remove_Throws_ArgumentOutOfRangeException_When_NumberOfItemsToBeAdded_Equals_Zero()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");
      target.Add(3);

      // ACT
      Assert.Throws<ArgumentOutOfRangeException>(() => target.Remove(0));
    }

    [Test]
    public void Remove_Throws_ArgumentOutOfRangeException_When_NumberOfItemsToBeAdded_Is_Less_Than_Zero()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");
      target.Add(3);

      // ACT
      Assert.Throws<ArgumentOutOfRangeException>(() => target.Remove(-3));
    }

    [Test]
    public void Remove_Throws_InvalidOperationException_When_Inventory_Is_Empty()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");

      // ACT
      Assert.Throws<InvalidOperationException>(() => target.Remove(4));
    }

    [Test]
    public void Remove_Throws_InvalidOperationException_When_NumberOfItemsToBeAdded_Is_Greater_Than_Inventory_Count()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");
      target.Add(3);

      // ACT
      Assert.Throws<InvalidOperationException>(() => target.Remove(4));
    }

    [Test]
    public void Remove_Raises_An_Event_Of_Type_ItemsRemovedFromInventory_When_All_Items_Are_Removed_From_Inventory()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");

      target.Add(3);

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Remove(3);

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, uncommittedChanges.Length);

      var raisedEvent = uncommittedChanges[0] as ItemsRemovedFromInventory;
      Assert.IsNotNull(raisedEvent);
      Assert.AreEqual(id, raisedEvent.Id);
      Assert.AreEqual(3, raisedEvent.NumberOfRemovedItems);
      Assert.AreEqual(3, raisedEvent.AggregateVersion);
    }

    [Test]
    public void Remove_Raises_An_Event_Of_Type_ItemsRemovedFromInventory_When_Some_Items_Are_Removed_From_Inventory()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");

      target.Add(3);

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Remove(2);

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, uncommittedChanges.Length);

      var raisedEvent = uncommittedChanges[0] as ItemsRemovedFromInventory;
      Assert.IsNotNull(raisedEvent);
      Assert.AreEqual(id, raisedEvent.Id);
      Assert.AreEqual(2, raisedEvent.NumberOfRemovedItems);
      Assert.AreEqual(3, raisedEvent.AggregateVersion);
    }

    [Test]
    public void Remove_Changes_Aggregate_State_When_All_Items_Are_Removed_From_Inventory()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");

      target.Add(3);

      // ACT
      target.Remove(3);

      // ASSERT
      Assert.AreEqual(0, target.Count);
      Assert.AreEqual(3, target.Version);
    }

    [Test]
    public void Remove_Changes_Aggregate_State_When_Some_Items_Are_Removed_From_Inventory()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");

      target.Add(4);

      // ACT
      target.Remove(3);

      // ASSERT
      Assert.AreEqual(1, target.Count);
      Assert.AreEqual(3, target.Version);
    }
  }
}
