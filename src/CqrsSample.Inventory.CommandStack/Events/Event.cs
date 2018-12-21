namespace CqrsSample.Inventory.CommandStack.Events
{
  /// <summary>
  /// Base class for domain events
  /// </summary>
  public abstract class Event
  {
    protected Event(int aggregateVersion)
    {
      this.AggregateVersion = aggregateVersion;
    }

    /// <summary>
    /// The aggregate version at the moment when the domain event was raised
    /// </summary>
    public int AggregateVersion { get; }
  }
}
