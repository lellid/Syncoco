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
    public FilterAction _action;

    /// <summary>
    /// Match string to check the name against. Can contain wildcards.
    /// </summary>
    protected string _matchString;


    [NonSerialized]
    private IMatch _matcher;

    /// <summary>
    /// The action that is used if this filter item matches. If the action is set to ignore, this filter item
    /// will be ignored.
    /// </summary>
      public FilterAction Action
      {
        get
        {
          return _action;
        }
        set
        {
          _action = value;
        }
      }

  

    /// <summary>
    /// Match string to check the name against. Can contain wildcards.
    /// </summary>
    public string MatchString
    {
      get
      {
        return _matchString;
      }
      set
      {
        _matchString = value;
        _matcher = GetMatcher(value);
      }
    }

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

    public FilterAction Match(string name)
    {
      return _action==FilterAction.Ignore ? _action : _matcher.Match(name) ? _action : FilterAction.Ignore;
    }


    private IMatch GetMatcher(string matchstring)
    {
      string sub, sub1;
      if(ExactMatchCondition(matchstring))
        return new ExactMatcher(matchstring.ToLower());
      else if(StartMatchCondition(matchstring, out sub))
        return new StartMatcher(sub.ToLower());
      else if(EndMatchCondition(matchstring, out sub))
        return new EndMatcher(sub.ToLower());
      else if(StartAndEndMatchCondition(matchstring, out sub, out sub1))
        return new StartAndEndMatcher(sub,sub1);
      else
        return new FullMatch(matchstring.ToLower());
    }

    private static bool ExactMatchCondition(string name)
    {
      if(name.IndexOf('*')>=0)
        return false;
      if(name.IndexOf('?')>=0)
        return false;

      return true;
    }

    private static bool EndMatchCondition(string name, out string endstring)
    {
      endstring = null;
      int idx = name.LastIndexOf('*');
      if(idx<0)
        return false;

      // test that the first char till idx only contain stars
      for(int i=0;i<=idx;i++)
        if(name[i]!='*')
          return false;

      // test that the rest of the string does not contain other jokers
      if(name.IndexOf('?',idx+1)>=0)
        return false;

      endstring = name.Substring(idx+1);
      return true;
    }

    private static bool StartMatchCondition(string name, out string startstring)
    {
      startstring = null;
      int idx = name.IndexOf('*');
      if(idx<0)
        return false;

      // test that the last chars till idx only contain stars
      for(int i=name.Length-1;i>=idx;i--)
        if(name[i]!='*')
          return false;

      // test that the rest of the string does not contain other jokers
      if(name.IndexOf('?',0,idx)>=0)
        return false;

      startstring = name.Substring(0,idx);
      return true;

    }

    private static bool StartAndEndMatchCondition(string name, out string startstring, out string endstring)
    {
      startstring = null;
      endstring = null;
      
      int idxs = name.IndexOf('*');
      int idxe = name.LastIndexOf('*');
      if(idxs<0)
        return false; // there has to be at last one joker-free char at the beginning


      // test that the chars from idxa till idxe only contain stars
      for(int i=idxs;i<=idxe;i++)
        if(name[i]!='*')
          return false;

      // test that the rest of the string does not contain other jokers
      startstring = name.Substring(0,idxs);
      if(startstring.Length==0 || startstring.IndexOf('?')>=0)
        return false;

      endstring = name.Substring(idxe+1);
      if(endstring.Length==0 || endstring.IndexOf('?')>=0)
        return false;

      return true;

    }

    #region inner classes

    public interface IMatch
    {
      bool Match(string name);
    }


    class FullMatch : IMatch
    {
      private string _matchString;

      public FullMatch(string matchstring)
      {
        _matchString = matchstring;
      }

      public bool Match(string name)
      {
        return Match(_matchString,0,name,0);
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
        if(ms[msi]!='*') return (ms[msi]==fn[fni]) && Match(ms,msi+1,fn,fni+1); // stimmen Buchstaben und Reststring
        if((msi+1)<ms.Length && ms[msi+1]=='*') return Match(ms,msi+1,fn,fni); // Fix gegen mehrere Stern-Joker hintereinander
        return Match(ms,msi+1,fn,fni) || Match(ms,msi,fn,fni+1); // Stern ersetzt kein Zeichen oder  Stern ersetzt zunächst ein Zeichen
      }
    }

    class ExactMatcher : IMatch
    {
      private string _matchstring;

      public ExactMatcher(string matchstring)
      {
        _matchstring = matchstring;
      }

      public bool Match(string name)
      {
        return name==_matchstring;
      }
    }

    class StartMatcher : IMatch
    {
      private string _startstring;

      public StartMatcher(string startstring)
      {
        _startstring = startstring;
      }

      public bool Match(string name)
      {
        return name.StartsWith(_startstring);
      }
    }

    class EndMatcher : IMatch
    {
      private string _endstring;

      public EndMatcher(string endstring)
      {
        _endstring = endstring;
      }

      public bool Match(string name)
      {
        return name.EndsWith(_endstring);
      }
    }

    class StartAndEndMatcher : IMatch
    {
      private string _startstring;
      private string _endstring;

      public StartAndEndMatcher(string startstring, string endstring)
      {
        _startstring = startstring;
        _endstring = endstring;
      }

      public bool Match(string name)
      {
        return name.StartsWith(_startstring) && name.EndsWith(_endstring);
      }
    }


    #endregion

  }

}
