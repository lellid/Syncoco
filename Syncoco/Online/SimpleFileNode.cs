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
  /// <summary>
  /// Summary description for SimpleFileNode.
  /// </summary>
  public class SimpleFileNode : FileNodeBase
  {
    [NonSerialized]
    private bool _IsUpdated;

    public bool IsUpdated
    {
      get { return _IsUpdated; }
      set { _IsUpdated = value; }
    }


    /// <summary>
    /// Only for deserialization purposes.
    /// </summary>
    protected SimpleFileNode()
    {
    }

    public SimpleFileNode(FileNodeBase from)
      : base(from)
    {
    }

    /// <summary>
    /// Constructor. Creates a SimpleFileNode out of a file info for that file.
    /// </summary>
    /// <param name="info"></param>
    public SimpleFileNode(System.IO.FileInfo info)
      : base(info)
    {
    }



  }
}
