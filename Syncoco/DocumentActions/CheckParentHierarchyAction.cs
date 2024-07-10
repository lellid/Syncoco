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

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for CheckParentHierarchyAction.
  /// </summary>
  public class CheckParentHierarchyAction : AbstractDocumentAction
  {
    string _filename;

    public CheckParentHierarchyAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
      : base(doc,monitor,reporter)
    {
    }
    public CheckParentHierarchyAction(MainDocument doc)
      : this(doc,null,null)
    {
    }
  
   
    
    public override void DirectExecute()
    {
      for(int i=0;i<_doc.Count;i++)
      {
        Check(_doc.RootPair(i).MyRoot);
        Check(_doc.RootPair(i).ForeignRoot);
      }
    }

    public void Check(FileSystemRoot fsRoot)
    {
      if(fsRoot==null)
        return;
      if(fsRoot.DirectoryNode==null)
        return;

      System.Diagnostics.Debug.Assert(object.ReferenceEquals(fsRoot.DirectoryNode.ParentDirectory,fsRoot));
      Check(fsRoot.DirectoryNode);
    }

    public void Check(DirectoryNode dirNode)
    {
      // Check files
      foreach(FileNode fNode in dirNode.Files)
      {
        System.Diagnostics.Debug.Assert(object.ReferenceEquals(fNode.Parent,dirNode));
      }

      // Check subdirectories (flat)
      foreach(DirectoryNode dNode in dirNode.Directories)
      {
        System.Diagnostics.Debug.Assert(object.ReferenceEquals(dNode.ParentDirectory,dirNode));
      }

      // Check subdirectories (recursive)
      foreach(DirectoryNode dNode in dirNode.Directories)
      {
        Check(dNode);
      }

    }
  }
}
