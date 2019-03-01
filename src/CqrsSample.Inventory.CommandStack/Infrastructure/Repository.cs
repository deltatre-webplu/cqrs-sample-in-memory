using System;
using System.Collections.Generic;
using System.Text;
using CqrsSample.Inventory.CommandStack.Model;

namespace CqrsSample.Inventory.CommandStack.Infrastructure
{
  public class Repository : IRepository
  {
    private readonly IEventStore eventStore;

    public Repository(IEventStore eventStore)
    {
      this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : IAggregateRoot
    {
      throw new NotImplementedException();
    }

    public void Save(IAggregateRoot aggregate, Guid aggregateId)
    {
      throw new NotImplementedException();
    }
  }
}
