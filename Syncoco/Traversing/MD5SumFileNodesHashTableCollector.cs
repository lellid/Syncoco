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

namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for MD5SumFileNodesHashTableCollector.
  /// </summary>
  public class MD5SumFileNodesHashTableCollector
  {
    MD5SumFileNodesHashTable _table;
    DirectoryNode _rootDir;
    string _pathRoot;


    public MD5SumFileNodesHashTableCollector(DirectoryNode rootDir, string pathRoot, MD5SumFileNodesHashTable table)
    {
      System.Diagnostics.Debug.Assert(rootDir!=null);
      System.Diagnostics.Debug.Assert(table!=null);
      PathUtil.Assert_Abspath(pathRoot);

      _rootDir = rootDir;
      _table   = table;

      _pathRoot = pathRoot;

      
    }

    public void Traverse()
    {
      VisitDirectory(_rootDir,_pathRoot);
    }

    protected void VisitDirectory( DirectoryNode dir, string currentPath)
    {
#if DEBUG
      PathUtil.Assert_Abspath(currentPath);
#endif

      foreach(FileNode fnode in dir.Files)
      {
        fnode.FillMD5SumFileNodesHashTable(_table,PathUtil.Combine_Abspath_Filename(currentPath,fnode.Name));
      }
      foreach(DirectoryNode dnode in dir.Directories)
      {
        VisitDirectory(dnode,PathUtil.Combine_Abspath_Dirname(currentPath, dnode.Name));
      }
    }
  }
}
