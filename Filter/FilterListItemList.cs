using System;

namespace Syncoco.Filter
{
  /// <summary>
  /// Summary description for FilterListItemList.
  /// </summary>
  
  [Serializable]
  public class FilterListItemList : System.Collections.CollectionBase
  {
    public FilterListItem this[int i]
    {
      get { return (FilterListItem)InnerList[i]; }
      set { InnerList[i] = value; }
    }

    public void Add(FilterListItem it)
    {
      InnerList.Add(it);
    }

    public FilterListItemList()
    {
    }

    public FilterListItemList(FilterListItemList from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(FilterListItemList from)
    {
      this.Clear();
      for(int i=0;i<from.Count;i++)
        this.Add(new FilterListItem(from[i]));
    }


    public void Save(System.Xml.XmlTextWriter tw)
    {
     
      for(int i=0;i<Count;i++)
      {
        tw.WriteStartElement("FilterList");
        this[i].Save(tw);
        tw.WriteEndElement();
      }
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      while(tr.LocalName=="FilterList")
      {
        tr.ReadStartElement("FilterList");
        this.Add(new FilterListItem(tr));
        tr.ReadEndElement();
      }
    
    }

    public FilterListItemList(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

    public void ExchangeItemPositions(int index1, int index2)
    {
      if(index1<0 || index2<0)
        throw new ArgumentOutOfRangeException("Indizes have to be >=0");
      if(index1>=Count || index2>=Count)
        throw new ArgumentOutOfRangeException("Indizes have to be <Count");

      object o = InnerList[index1];
      InnerList[index1] = InnerList[index2];
      InnerList[index2] = o;
    }
  }
}
