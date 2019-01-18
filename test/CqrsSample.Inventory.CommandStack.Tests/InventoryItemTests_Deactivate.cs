using System;
using System.Linq;
using CqrsSample.Inventory.CommandStack.Events;
using CqrsSample.Inventory.CommandStack.Model;
using NUnit.Framework;

namespace CqrsSample.Inventory.CommandStack.Tests.Model
{
  public partial class InventoryItemTests
  {
    [Test]
    public void Deactivate_Raises_An_Event_Of_Type_InventoryItemDeactivated()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "tennis racket");

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Deactivate();
      var events = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();

      // ASSERT
      Assert.IsNotNull(events);
      Assert.AreEqual(1, events.Length);

      var @event = events[0] as InventoryItemDeactivated;
      Assert.IsNotNull(@event);
      Assert.AreEqual(id, @event.Id);
    }

    [Test]
    public void Deactivate_Changes_Aggregate_State()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "tennis racket");

      // ACT
      target.Deactivate();

      // ASSERT
      Assert.IsFalse(target.IsActive);
    }

    [Test]
    public void Deactivate_Does_Not_Raise_Events_If_Inventory_Item_Is_Not_Active()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "tennis racket");

      target.Deactivate();

      ((IAggregateRoot)target).MarkChangesAsCommitted();

      // ACT
      target.Deactivate();
      var events = ((IAggregateRoot)target).GetUncommittedChanges().ToArray();

      // ASSERT
      Assert.IsNotNull(events);
      Assert.IsEmpty(events);
    }

    [Test]
    public void Deactivate_Does_Not_Change_Aggregate_State_If_Inventory_Item_Is_Not_Active()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "tennis racket");

      target.Deactivate();

      // ACT
      target.Deactivate();

      // ASSERT
      Assert.IsFalse(target.IsActive);
    }
  }
}
