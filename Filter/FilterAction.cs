using System;

namespace Syncoco.Filter
{
  /// <summary>
  /// Enumerates the action for a filter item.
  /// </summary>
  [Serializable]
  public enum  FilterAction
  {
    /// <summary>
    /// Include the item.
    /// </summary>
    Include=0,
    /// <summary>
    /// Exclude the item.
    /// </summary>
    Exclude=1,

    /// <summary>Ignore the item or take no action</summary>
    Ignore=2
  }
}
