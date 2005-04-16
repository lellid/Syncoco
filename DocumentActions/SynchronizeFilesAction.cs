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
using System.Collections;
using System.Runtime.InteropServices;

using Syncoco.Filter;
using Syncoco.Traversing;
using System.IO;


namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for CollectFilesToSynchronizeAction.
  /// </summary>
  public class SynchronizeFilesAction : AbstractDocumentAction
  {
    MD5SumFileNodesHashTable _allFilesHere;

    IList _filesToSynchronize;


    public SynchronizeFilesAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter, IList filesToSynchronize)
      : base(doc,monitor,reporter)
    {
      _filesToSynchronize = filesToSynchronize;
      this._allFilesHere = _doc.CachedAllMyFiles;
    }
    public SynchronizeFilesAction(MainDocument doc, IList filesToSynchronize)
      : this(doc,null, null, filesToSynchronize)
    {
    }

 
   
    
    public FileNode UpdateMyFile(FileSystemRoot fileSystemRoot, FileInfo fileinfo, bool forceUpdateHash)
    {
      DirectoryInfo dirinfo = new DirectoryInfo(fileSystemRoot.FilePath);

      return DirectoryUpdater.UpdateFileNode(fileSystemRoot.DirectoryNode,dirinfo,fileinfo,forceUpdateHash,_reporter);
    }

    public FunctionResult CreateDirectory(string dirname)
    {
#if DEBUG
      PathUtil.Assert_Abspath(dirname);
#endif

      if(!System.IO.Directory.Exists(dirname))
      {
        try { System.IO.Directory.CreateDirectory(dirname); }
        catch( Exception ex)
        {
          _reporter.ReportError(string.Format("unable to create directory {0} : {1}",dirname,ex.Message));
          return FunctionResult.Failure;
        }
      }
    
      return FunctionResult.Success;
    }

    public FunctionResult CopyWithDirectoryCreation(string sourceFileName, string destFileName, bool overwrite, FileNode foFileNode)
    {
#if DEBUG
      PathUtil.Assert_AbspathFilename(sourceFileName);
      PathUtil.Assert_AbspathFilename(destFileName);
#endif

      string dirname = System.IO.Path.GetDirectoryName(destFileName);
      if(!System.IO.Directory.Exists(dirname))
      {
        try { System.IO.Directory.CreateDirectory(dirname); }
        catch( Exception ex)
        {
          _reporter.ReportError(string.Format("unable to create directory {0} : {1}",dirname,ex.Message));
          return FunctionResult.Failure;
        }
      }

      

      try 
      {
        // before we overwrite an existing file, make sure the read-only attribute of that file is not set
        if(overwrite && System.IO.File.Exists(destFileName))
        {
          System.IO.FileInfo info = new System.IO.FileInfo(destFileName);
          if(info.Exists)
          {
            // first clear the readonly attribute
            info.Attributes &= ~(System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.System | System.IO.FileAttributes.Hidden);       
          }
        }

        // now we can perform a copy
        System.IO.File.Copy(sourceFileName,destFileName,overwrite); 
      }
      catch(Exception ex)
      {
        _reporter.ReportError(string.Format("unable to copy from {0} to {1} : {2}",sourceFileName,destFileName,ex.Message));
        return FunctionResult.Failure;
      }

      try
      {
        System.IO.FileInfo info = new System.IO.FileInfo(destFileName);
        if(info.Exists)
        {
          // first clear the readonly attribute
          info.Attributes &= ~System.IO.FileAttributes.ReadOnly;
       
          info.CreationTimeUtc = foFileNode.CreationTimeUtc;
          info.LastWriteTimeUtc = foFileNode.LastWriteTimeUtc;
          info.Attributes = foFileNode.Attributes;
        }
      }
      catch(Exception ex)
      {
        _reporter.ReportError(string.Format("setting attributes for file {0} : {1}",destFileName,ex.Message));
        return FunctionResult.Failure;
      }

      return FunctionResult.Success;
    }

    /// <summary>
    /// This functions looks from where to copy the foFileNode from. First we have a look in our own file system.
    /// Then we look at the files on the medium.
    /// </summary>
    /// <param name="foFileNode">This is the file in the foreign file system that should be copied here to destFileName.</param>
    /// <param name="destFileName">The destination file name where to copy to.</param>
    /// <param name="overwrite">True if the destination file can be overwritten. False if not.</param>
    /// <returns>True if the copy was successfull, false otherwise.</returns>
    public FunctionResult CopyWithDirectoryCreation(FileNode foFileNode, string destFileName, bool overwrite)
    {
      object o = this._allFilesHere[foFileNode.FileHash];
      if(o is PathAndFileNode)
      {
        PathAndFileNode pfn = (PathAndFileNode)o;
        if(System.IO.File.Exists(pfn.Path) && foFileNode.HasSameHashThan(pfn.Path))
        {
          return CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite, foFileNode);
        }
      }
      else if(o is ArrayList) // there is more than one possibility where to copy from
      {
        ArrayList arr = (ArrayList)o;
        foreach(PathAndFileNode pfn in arr) // use the first successfull possibility where to copy from
        {
#if DEBUG
          PathUtil.Assert_AbspathFilename(pfn.Path);
#endif
          if(System.IO.File.Exists(pfn.Path) && foFileNode.HasSameHashThan(pfn.Path))
          {
            return CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite,foFileNode);
          }
        }
       
      }
    

      string sourcefilename = PathUtil.Combine_Abspath_Filename(_doc.MediumDirectoryName,foFileNode.MediumFileName);
      if(foFileNode.HasSameHashThan(sourcefilename))
      {
        return CopyWithDirectoryCreation(sourcefilename, destFileName, overwrite, foFileNode);
      }
      
      return FunctionResult.Failure;
    }


    FunctionResult DeleteFileForced(string path)
    {
#if DEBUG
      PathUtil.Assert_AbspathFilename(path);
#endif

      try
      {
        System.IO.File.Delete(path);
        return FunctionResult.Success;
      }
      catch(Exception)
      {
      }

      // if this was not successfull, try to remove the read-only attribute
      System.IO.FileInfo info = new System.IO.FileInfo(path);
      info.Attributes &= ~(System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
      // this time we try to delete the file without catching the exception
      
      try 
      {
        System.IO.File.Delete(path);
      }
      catch(Exception ex)
      {
        _reporter.ReportError(string.Format("deleting file {0} : {1}",path,ex.Message));
        return FunctionResult.Failure;
      }

      return FunctionResult.Success;
    }


    public FunctionResult DeleteDirectory(string name, bool deleteForced)
    {
      DirectoryInfo dirInfo = new DirectoryInfo(name);
      if(!dirInfo.Exists)
        return FunctionResult.Success;

      try 
      {
        dirInfo.Delete(deleteForced); 
        return FunctionResult.Success;
      }
      catch(System.IO.IOException exa ) 
      {
        if(!deleteForced)
          _reporter.ReportWarning(string.Format("directory {0} could not be removed: {1}",dirInfo.FullName, exa.Message));
      }
      catch(System.UnauthorizedAccessException exb)
      {
        if(!deleteForced)
          _reporter.ReportWarning(string.Format("directory {0} could not be removed: {1}",dirInfo.FullName, exb.Message));

      }

      if(!deleteForced)
        return FunctionResult.Failure;

      // if forced then try to reset all attributes
      try 
      {
        ResetAttributes(dirInfo);
        dirInfo.Delete(deleteForced); 
        return FunctionResult.Success;
      }
      catch(System.IO.IOException exa) 
      {
        _reporter.ReportWarning(string.Format("directory {0} could not be removed: {1}",dirInfo.FullName, exa.Message));
      }
      catch(System.UnauthorizedAccessException exb)
      {
        _reporter.ReportWarning(string.Format("directory {0} could not be removed: {1}",dirInfo.FullName, exb.Message));

      }

      return FunctionResult.Failure;

    
    }


    void ResetAttributes(DirectoryInfo dirInfo)
    {
      dirInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);

      FileInfo[] files = dirInfo.GetFiles();
      foreach(FileInfo file in files)
      {
        file.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
      }

      DirectoryInfo[] dirs = dirInfo.GetDirectories();
      foreach(DirectoryInfo dir in dirs)
      {
        ResetAttributes(dir);
      }

    }

    public override void DirectExecute()
    {
      _doc.SetDirty();
    
      foreach(SyncItemTag tag in _filesToSynchronize)
      {
        if(_monitor.ShouldReport)
          _monitor.Report(string.Format("Perform {0} on {1}",tag.Action.ToString(),tag.FileName));

        PerformAction(tag);
      }
      
    }

    public void PerformAction(SyncItemTag tag)
    {
      System.Diagnostics.Debug.Assert(tag!=null);

      string relPathFileName = tag.FileName;
      string absrootdir = _doc.MyRoot(tag.RootListIndex).FilePath;


#if DEBUG
      PathUtil.Assert_RelpathOrFilename(relPathFileName);
      PathUtil.Assert_Abspath(absrootdir);
#endif

      
      string myfilename = PathUtil.Combine_Abspath_RelpathOrFilename(absrootdir,relPathFileName);

      FileSystemRoot myRoot = _doc.MyRoot(tag.RootListIndex);
      FileSystemRoot foRoot = _doc.ForeignRoot(tag.RootListIndex);
      FileNode foFileNode;
     
      System.IO.FileInfo myfileinfo;
      FileNode myfilenode;

      switch(tag.Action)
      {
        case SyncAction.Remove:
        case SyncAction.ForcedRemove:
          if(PathUtil.IsDirectoryName(myfilename)) // delete subdir
          {
            if(FunctionResult.Success==DeleteDirectory(myfilename,SyncAction.ForcedRemove==tag.Action)) 
            {
              myRoot.DeleteSubDirectoryNode(relPathFileName);
              foRoot.DeleteSubDirectoryNode(relPathFileName);
            }
          }
          else // Delete file
          {
            if(FunctionResult.Success==DeleteFileForced(myfilename))
            {
              myRoot.DeleteFileNode(relPathFileName);
              foRoot.DeleteFileNode(relPathFileName);
            }
          }
          break;
        case SyncAction.RemoveRollback:
        case SyncAction.RemoveManuallyRollback:
          foRoot.DeleteFileNode(relPathFileName);
          break;
        case SyncAction.Copy:
          foFileNode = foRoot.GetFileNode(relPathFileName);
       
          if(FunctionResult.Success==CopyWithDirectoryCreation(foFileNode,myfilename,false))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = UpdateMyFile(_doc.MyRoot(tag.RootListIndex),myfileinfo,true);
            if(foFileNode.HasSameContentThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.CreateDirectory:
          CreateDirectory(myfilename);          
          break;
        case SyncAction.Overwrite:
        case SyncAction.ResolveManuallyOverwrite:
          foFileNode = foRoot.GetFileNode(relPathFileName);
          if(FunctionResult.Success==CopyWithDirectoryCreation(foFileNode,myfilename,true))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = UpdateMyFile(_doc.MyRoot(tag.RootListIndex),myfileinfo,true);
            if(foFileNode.HasSameContentThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.ResolveManuallyRollback:
        case SyncAction.OverwriteRollback:
          foFileNode = foRoot.GetFileNode(relPathFileName);
          foFileNode.SetToUnchanged();
          myfileinfo = new System.IO.FileInfo(myfilename);
          myfilenode = UpdateMyFile(_doc.MyRoot(tag.RootListIndex),myfileinfo,true);
          if(myfilenode.IsUnchanged)
            myfilenode.SwitchFromUnchangedToChanged();
          break;
       

      }
    }
  }
}
