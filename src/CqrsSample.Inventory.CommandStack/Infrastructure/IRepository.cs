using CqrsSample.Inventory.CommandStack.Model;
using System;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  public interface IRepository
  {
    TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : IAggregateRoot;
    void Save(IAggregateRoot aggregate, Guid aggregateId);
  }
}
