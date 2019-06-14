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
    /// The aggregate version obtained by applying the event to the aggregate. Each event increments the aggregate version by one. 
    /// </summary>
    public int AggregateVersion { get; }
  }
}
