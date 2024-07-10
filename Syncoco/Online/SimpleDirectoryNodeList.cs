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

namespace Syncoco.Online
{
  [Serializable]
  public class SimpleDirectoryNodeList : SortedArrayList
  {
    public override int Add(object value)
    {
      if(value is SimpleDirectoryNode)
        return base.Add(value);
      else
        throw new ArgumentException("Item to add is not of expected type");
    }

    public new SimpleDirectoryNode this[int i]
    {
      get { return (SimpleDirectoryNode)base[i]; }
      set { base[i] = value; }
    }

    public SimpleDirectoryNode this[string name]
    {
      get 
      {
        int idx = Array.BinarySearch(_items,0,_size,name); 
        if(idx>=0)
          return (SimpleDirectoryNode)base[idx];
        else
          return null;
      }
    }
  
  }
}
