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

using System.Collections;
namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for FilesToTransferCollector.
  /// </summary>
  public class FilesToTransferCollector
  {
    private DirectoryNode _myDirRoot;
    private DirectoryNode _foreignDirRoot;
    private SortedList list;

    public FilesToTransferCollector(DirectoryNode myDir, DirectoryNode foreignDir)
    {
      _myDirRoot = myDir;
      _foreignDirRoot = foreignDir;
    }

    public void Traverse()
    {
      list = new SortedList();
      GetNewOrChangedFiles(_myDirRoot, _foreignDirRoot, System.IO.Path.DirectorySeparatorChar.ToString());
    }

    public SortedList Result
    {
      get { return list; }
    }

    /// <summary>
    /// Collects all files that should be copied to the transfermedium. We presume that the own file system was
    /// updated before. That is why no PathFilter is neccessary here.
    /// </summary>
    /// <param name="myDir">Own directory node.</param>
    /// <param name="foreignDir">Corresponding directory node of the foreign system.</param>
    /// <param name="list">List in which the files are collected that should be copied to the transfer medium.</param>
    /// <param name="nameroot"></param>
    protected void GetNewOrChangedFiles(DirectoryNode myDir, DirectoryNode foreignDir, string nameroot)
    {
#if DEBUG
      PathUtil.Assert_Relpath(nameroot);
#endif

      foreach (FileNode myFileNode in myDir.Files)
      {
        string myFileName = myFileNode.Name;

        if (myFileNode.IsDataContentNewOrChanged)
        {
          // before adding them to the list, make sure that our node don't
          // have the same content as the foreign node
          if (foreignDir != null && foreignDir.ContainsFile(myFileName) && myFileNode.HasSameHashThan(foreignDir.File(myFileName)))
          {
            // Do nothing here, don't change the nodes, since this is done during the sync stage
          }
          else
          {
            list.Add(PathUtil.Combine_Relpath_Filename(nameroot, myFileName), myFileNode);
          }
        }
        else if (myFileNode.IsUnchanged && (foreignDir == null || !foreignDir.ContainsFile(myFileName)))
        {
          list.Add(PathUtil.Combine_Relpath_Filename(nameroot, myFileName), myFileNode);
        }
      }

      foreach (DirectoryNode mySubDirNode in myDir.Directories)
      {
        string mySubDirName = mySubDirNode.Name;

        GetNewOrChangedFiles(
          mySubDirNode,
          foreignDir == null ? null : foreignDir.Directory(mySubDirName),
          PathUtil.Combine_Relpath_Dirname(nameroot, mySubDirName));
      }
    }
  }
}
