#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco: offline file synchronization
//    Copyright (C) 2004-2099 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

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
