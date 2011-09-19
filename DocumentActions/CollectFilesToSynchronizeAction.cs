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
using System.Collections;
using System.Runtime.InteropServices;

using Syncoco.Filter;
using Syncoco.Traversing;


namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for CollectFilesToSynchronizeAction.
  /// </summary>
  public class CollectFilesToSynchronizeAction : AbstractDocumentAction
  {
    FilesToSynchronizeCollector[] _collectedFiles;

    [NonSerialized]
    MD5SumFileNodesHashTable _allFilesHere;


    public CollectFilesToSynchronizeAction(MainDocument doc, IBackgroundMonitor monitor)
      : base(doc,monitor)
    {
    }
    public CollectFilesToSynchronizeAction(MainDocument doc)
      : this(doc,null)
    {
    }

    public FilesToSynchronizeCollector[] CollectedFiles
    {
      get { return _collectedFiles; }
    }
   

    public override void DirectExecute()
    {
      _doc.EnsureAlignment();

      if(!_doc.HasFileName)
        throw new ApplicationException("The document was not saved yet");


      // before we collect, we save the md5 hashes of all files (not current, but from the XML file)
      // into one hashtable

      _monitor.Report("Hashing file checksums ...");
      this._allFilesHere = new MD5SumFileNodesHashTable();
      for(int i=0;i<_doc.Count;i++)
      {
        if(_doc.MyRoot(i).IsValid)
          FillMD5SumFileNodesHashTable(_doc.RootPair(i).MyRoot,this._allFilesHere);
      }
      _doc.CachedAllMyFiles = _allFilesHere;

      // now that we have all the current files, the information can be used by the collectors to
      // decide if a file can be copied or not

      _collectedFiles = new FilesToSynchronizeCollector[_doc.Count];
      for(int i=0;i<_doc.Count;i++)
      {
          if(_monitor.CancelledByUser)
                 return;

        if(_doc.ForeignRoot(i).IsValid && _doc.MyRoot(i).IsValid)
        {
          _collectedFiles[i] = new FilesToSynchronizeCollector(
            _doc.MediumDirectoryName,
            _doc.MyRoot(i).FilePath,
            this._allFilesHere,
            _doc.RootPair(i).MyRoot.DirectoryNode,
            _doc.RootPair(i).ForeignRoot.DirectoryNode,
            _doc.RootPair(i).PathFilter,
            _monitor,
            _reporter);
        
          _collectedFiles[i].Traverse();
        }
      }
    }

      

    public void FillMD5SumFileNodesHashTable(FileSystemRoot fileSystemRoot, MD5SumFileNodesHashTable table)
    {
      Traversing.MD5SumFileNodesHashTableCollector coll = new Traversing.MD5SumFileNodesHashTableCollector(fileSystemRoot.DirectoryNode,fileSystemRoot.FilePath,table);
      coll.Traverse();
    }

  }
}
