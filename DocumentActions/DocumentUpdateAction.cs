#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco:  synchronizing two computers with a data medium
//    Copyright (C) 2004-2005 Dr. Dirk Lellinger
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
using Syncoco.Filter;
using Syncoco.Traversing;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for Update.
  /// </summary>
  public class DocumentUpdateAction : AbstractDocumentAction
  {
    bool _forceUpdateHash=false;
    

    public DocumentUpdateAction(MainDocument doc, bool forceUpdateHash)
      : this(doc,forceUpdateHash,null,null)
    {
    }

    public DocumentUpdateAction(MainDocument doc, bool forceUpdateHash, IBackgroundMonitor monitor,IErrorReporter reporter)
      : base(doc,monitor,reporter)
    {
      _forceUpdateHash = forceUpdateHash;
      
    }

    public override void BackgroundExecute()
    {
      base.BackgroundExecute();
    }

   
    public override void DirectExecute()
    {
      _doc.SetDirty();

      _doc.ClearCachedMyFiles();

      _doc.EnsureAlignment();

      for(int i=0;i<_doc.Count;i++)
      {
        if(_monitor.CancelledByUser)
          return;

        if(_doc.RootPair(i).MyRoot.IsValid)
        {
          if(System.IO.Directory.Exists(_doc.RootPair(i).MyRoot.FilePath))
            Update(_doc.RootPair(i));
          else
            _reporter.ReportWarning(string.Format("RootPair[{0}] not updated because the path is unavailable.", i));
        }
        else
        {
          _reporter.ReportWarning(string.Format("RootPair[{0}] not updated because the path is invalid.",i));
        }
      }
    }

    // RootPair
    public void Update(RootPair rootPair)
    {
      rootPair.PathFilter.ResetCurrentDirectory();
      Update(rootPair.MyRoot, rootPair.PathFilter);
    }

    // FileSystemRoot
    public void Update(FileSystemRoot fileSystemRoot, PathFilter pathFilter)
    {
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(fileSystemRoot.FilePath);

      if(null!=fileSystemRoot.DirectoryNode)
        new DirectoryUpdater(pathFilter,_monitor,_reporter).Update(fileSystemRoot.DirectoryNode, dirinfo, _forceUpdateHash);
      else
        fileSystemRoot.DirectoryNode = new DirectoryUpdater(pathFilter,_monitor,_reporter).NewDirectoryNode(dirinfo,fileSystemRoot);
    }
  }
}
