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
    [Test]
    public void CreateNew_Throws_ArgumenException_When_Id_Is_Empty_Guid()
    {
      // ACT
      Assert.Throws<ArgumentException>(
        () => InventoryItem.Factory.CreateNew(Guid.Empty, "shoes")
      );
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("    ")]
    public void CreateNew_Throws_ArgumenException_When_Name_Is_Null_Or_White_Space(string name)
    {
      // ACT
      Assert.Throws<ArgumentException>(
        () => InventoryItem.Factory.CreateNew(Guid.NewGuid(), name)
      );
    }

    [Test]
    public void CreateNew_Returns_New_Instance_With_Expected_Properties()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      const string name = "tennis shoes";

      // ACT
      var result = InventoryItem.Factory.CreateNew(id, name);

      // ASSERT
      Assert.IsNotNull(result);
      Assert.AreEqual(id, result.Id);
      Assert.AreEqual(name, result.Name);
      Assert.IsTrue(result.IsActive);
      Assert.AreEqual(0, result.Count);
    }

    [Test]
    public void CreateNew_Raises_InventoryItemCreated_Event()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      const string name = "tennis shoes";

      // ACT
      var result = InventoryItem.Factory.CreateNew(id, name);

      // ASSERT
      var events = ((IAggregateRoot)result).GetUncommittedChanges().ToArray();
      Assert.AreEqual(1, events.Length);

      var @event = events[0] as InventoryItemCreated;
      Assert.IsNotNull(@event);
      Assert.AreEqual(id, @event.Id);
      Assert.AreEqual(name, @event.Name);
    }

    [Test]
    public void CreateNew_Creates_An_Aggregate_Having_Version_Equal_To_One()
    {
      // ARRANGE
      var id = Guid.NewGuid();
      const string name = "tennis shoes";

      // ACT
      var result = InventoryItem.Factory.CreateNew(id, name);

      // ASSERT
      Assert.IsNotNull(result);
      Assert.AreEqual(1, result.Version);
    }
  }
}