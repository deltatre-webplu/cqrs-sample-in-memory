using System;
using CqrsSample.Inventory.CommandStack.Model;
using NUnit.Framework;
using System.Linq;
using CqrsSample.Inventory.CommandStack.Events;

namespace CqrsSample.Inventory.CommandStack.Tests.Model
{
  [TestFixture]
  public partial class InventoryItemTests
  {
    [TestCase(null)]
    [TestCase("")]
    [TestCase("    ")]
    public void ChangeName_Throws_ArgumenException_When_NewName_Is_Null_Or_White_Space(string newName)
    {
      // ARRANGE
      var target = InventoryItem.Factory.CreateNew(Guid.NewGuid(), "test");
      
      // ACT
      Assert.Throws<ArgumentException>(() => target.ChangeName(newName));
    }

    [Test]
    public void ChangeName_Raises_An_Event_Of_Type_InventoryItemRenamed()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "Old name");
      target.MarkChangesAsCommitted();

      // ACT
      target.ChangeName("New name");

      // ASSERT
      var uncommittedChanges = target.GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, uncommittedChanges.Length);

      var raisedEvent = uncommittedChanges[0] as InventoryItemRenamed;
      Assert.IsNotNull(raisedEvent);
      Assert.AreEqual(id, raisedEvent.Id);
      Assert.AreEqual("Old name", raisedEvent.OldName);
      Assert.AreEqual("New name", raisedEvent.NewName);
    }

    [Test]
    public void ChangeName_Changes_Aggregate_State()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "Old name");
      target.MarkChangesAsCommitted();

      // ACT
      target.ChangeName("New name");

      // ASSERT
      Assert.AreEqual("New name", target.Name);
    }

    [Test]
    public void ChangeName_Does_Not_Raise_Events_When_New_Name_Equals_Current_Name()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "Old name");
      target.MarkChangesAsCommitted();

      // ACT
      target.ChangeName("Old name");

      // ASSERT
      var uncommittedChanges = target.GetUncommittedChanges().ToArray();
      Assert.IsEmpty(uncommittedChanges);
    }

    [Test]
    public void ChangeName_Does_Not_Change_Aggregate_State_When_New_Name_Equals_Current_Name()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      var target = InventoryItem.Factory.CreateNew(id, "Old name");
      target.MarkChangesAsCommitted();

      // ACT
      target.ChangeName("Old name");

      // ASSERT
      Assert.AreEqual("Old name", target.Name);
    }
  }
}