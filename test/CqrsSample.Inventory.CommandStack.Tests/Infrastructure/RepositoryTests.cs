using System;
using System.Collections.Generic;
using System.Text;
using CqrsSample.Inventory.CommandStack.Model;
using NUnit.Framework;

namespace CqrsSample.Inventory.CommandStack.Tests.Infrastructure
{
  [TestFixture]
  public class RepositoryTest
  {

    public class FakeAggregate : AggregateRoot
    {
      private FakeAggregate()
      {
      }

      public string Name { get; private set; }
      public int Age { get; private set; }
    }
  }
}
