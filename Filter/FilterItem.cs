using System;

namespace SyncTwoCo.Filter
{
	
  /// <summary>
  /// FilterItem holds a match string along with the information whether to include or to exclude the item.
  /// </summary>
  [Serializable]
  public class FilterItem
  {
    /// <summary>
    /// The action that is used if this filter item matches. If the action is set to ignore, this filter item
    /// will be ignored.
    /// </summary>
    public FilterAction Action;

    /// <summary>
    /// Match string to check the name against. Can contain wildcards.
    /// </summary>
    public string MatchString;

    public FilterItem(FilterAction action, string matchString)
    {
      Action = action;
      MatchString = matchString;
    }

    public FilterItem(FilterItem from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(FilterItem from)
    {
      this.Action = from.Action;
      this.MatchString = from.MatchString;
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      tw.WriteElementString("Action", Action.ToString());
      tw.WriteElementString("Match",MatchString);
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      Action = (FilterAction)System.Enum.Parse(typeof(FilterAction),tr.ReadElementString("Action"));
      MatchString = tr.ReadElementString("Match");
    }

    public FilterItem(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }
  }

}
