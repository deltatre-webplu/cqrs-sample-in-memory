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
    public void Add_Throws_ArgumentOutOfRangeException_When_NumberOfItemsToBeAdded_Equals_Zero()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");

      // ACT
      Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(0));
    }

    [Test]
    public void Add_Throws_ArgumentOutOfRangeException_When_NumberOfItemsToBeAdded_Is_Less_Than_Zero()
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");

      // ACT
      Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(-3));
    }

    [Test]
    public void Add_Raises_An_Event_Of_Type_ItemsAddedToInventory_When_Items_Are_Added_For_The_First_Time()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");
      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Add(3);

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, uncommittedChanges.Length);

      var raisedEvent = uncommittedChanges[0] as ItemsAddedToInventory;
      Assert.IsNotNull(raisedEvent);
      Assert.AreEqual(id, raisedEvent.Id);
      Assert.AreEqual(3, raisedEvent.NumberOfAddedItems);
    }

    [Test]
    public void Add_Changes_Aggregate_State_When_Items_Are_Added_For_The_First_Time()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");
      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Add(3);

      // ASSERT
      Assert.AreEqual(3, target.Count);
    }

    [Test]
    public void Add_Raises_An_Event_Of_Type_ItemsAddedToInventory_When_Items_Are_Added_Not_For_The_First_Time()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "test");

      target.Add(3);

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Add(2);

      // ASSERT
      var uncommittedChanges = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, uncommittedChanges.Length);

      var raisedEvent = uncommittedChanges[0] as ItemsAddedToInventory;
      Assert.IsNotNull(raisedEvent);
      Assert.AreEqual(id, raisedEvent.Id);
      Assert.AreEqual(2, raisedEvent.NumberOfAddedItems);
    }

    [Test]
    public void Add_Changes_Aggregate_State_When_Items_Are_Added_Not_For_The_First_Time()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");

      target.Add(3);

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Add(2);

      // ASSERT
      Assert.AreEqual(5, target.Count); // 5 = 3 + 2
    }
  }
}
