using System;

namespace SyncTwoCo.Filter
{
  


  /// <summary>
  /// FilterList holds MatchStrings along with the information wheter to exlucde or to include the item.
  /// </summary>
  [Serializable]
  public class FilterList : System.Collections.CollectionBase
  {

    /// <summary>
    /// Get/sets the default action. The default action is used if none of the filter items match.
    /// </summary>
    public FilterAction _DefaultAction = FilterAction.Include;


    /// <summary>
    /// Get/sets the i'th filter item.
    /// </summary>
    public FilterItem this[int i]
    {
      get { return (FilterItem)InnerList[i]; }
      set { this.InnerList[i] = value; }
    }

    /// <summary>
    /// Adds a filter item to the end of the list
    /// </summary>
    /// <param name="item"></param>
    public void Add(FilterItem item)
    {
      this.InnerList.Add(item);
    }

    /// <summary>
    /// Get/sets the default action. The default action is used if none of the filter items match.
    /// </summary>
    public FilterAction DefaultAction
    {
      get { return _DefaultAction; }
      set { _DefaultAction = value; }
    }

    public FilterList()
    {
    }

    public FilterList(FilterList from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(FilterList from)
    {
      this.Clear();
      for(int i=0;i<from.Count;i++)
        this.InnerList.Add(new FilterItem(from[i]));

      this.DefaultAction = from.DefaultAction;
    }

    /// <summary>
    /// Match the name against the match string of all filter items in the list. If one of the filter string matches,
    /// and the action is Include or Exclude, the search stops and this action is returned then. If no string matches,
    /// the default action of the list is returned.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FilterAction Match(string name)
    {
      name = name.ToLower();

      for(int i=0;i<Count;i++)
      {
        FilterAction action = this[i].Match(name);
        if(FilterAction.Ignore!=action)
        {
          return action;
        }
      }

      return DefaultAction;
    }


    public void Save(System.Xml.XmlTextWriter tw)
    {
      tw.WriteElementString("DefaultAction",DefaultAction.ToString());
      for(int i=0;i<Count;i++)
      {
        tw.WriteStartElement("FilterItem");
        this[i].Save(tw);
        tw.WriteEndElement();
      }
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      DefaultAction = (FilterAction)System.Enum.Parse(typeof(FilterAction),tr.ReadElementString("DefaultAction"));

      while(tr.Name=="FilterItem")
      {
        tr.ReadStartElement("FilterItem");
        this.Add(new FilterItem(tr));
        tr.ReadEndElement();
      }
    
    }

    public FilterList(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }


    /// <summary>
    /// Compares a matchstring (which can contain joker chars) with a filename (which must not
    /// contain joker chars).
    /// </summary>
    /// <param name="ms">The match string.</param>
    /// <param name="msi">First index into the match string that is used.</param>
    /// <param name="fn">The file name.</param>
    /// <param name="fni">First index into the file name that is used for comparism.</param>
    /// <returns>True if the two strings match, false otherwise.</returns>
    static bool Match( string ms, int msi,  string fn, int fni) // ms==Matchstring, fn=filename;
    {
      if(msi==ms.Length) return fni==fn.Length;
      if(fni==fn.Length) return (ms[msi]=='*')&&(Match(ms,msi+1,fn,fni));  // Matchfrage wenn ein String am Ende
      if(ms[msi]=='?') return Match(ms,msi+1,fn,fni+1); // bei ? tut ein Zeichen nichts zur Sache
      if(ms[msi]!='*') return (char.ToUpper(ms[msi])==char.ToUpper(fn[fni])) && Match(ms,msi+1,fn,fni+1); // stimmen Buchstaben und Reststring
      if((msi+1)<ms.Length && ms[msi+1]=='*') return Match(ms,msi+1,fn,fni); // Fix gegen mehrere Stern-Joker hintereinander
      return Match(ms,msi+1,fn,fni) || Match(ms,msi,fn,fni+1); // Stern ersetzt kein Zeichen oder  Stern ersetzt zunächst ein Zeichen
      
    }
  }
}
