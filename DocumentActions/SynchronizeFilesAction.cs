using System;
using System.Collections;
using System.Runtime.InteropServices;

using SyncTwoCo.Filter;
using SyncTwoCo.Traversing;
using System.IO;


namespace SyncTwoCo.DocumentActions
{
  /// <summary>
  /// Summary description for CollectFilesToSynchronizeAction.
  /// </summary>
  public class SynchronizeFilesAction : AbstractDocumentAction
  {
    MD5SumFileNodesHashTable _allFilesHere;

    IList _filesToSynchronize;


    public SynchronizeFilesAction(MainDocument doc, IBackgroundMonitor monitor, IList filesToSynchronize)
      : base(doc,monitor)
    {
      _filesToSynchronize = filesToSynchronize;
      this._allFilesHere = _doc.CachedAllMyFiles;
    }
    public SynchronizeFilesAction(MainDocument doc, IList filesToSynchronize)
      : this(doc,null, filesToSynchronize)
    {
    }

 
   
    
    public FileNode UpdateMyFile(FileSystemRoot fileSystemRoot, FileInfo fileinfo, bool forceUpdateHash)
    {
      DirectoryInfo dirinfo = new DirectoryInfo(fileSystemRoot.FilePath);

      return DirectoryUpdater.UpdateFileNode(fileSystemRoot.DirectoryNode,dirinfo,fileinfo,forceUpdateHash);
    }


    public void CopyWithDirectoryCreation(string sourceFileName, string destFileName, bool overwrite, FileNode foFileNode)
    {
#if DEBUG
      PathUtil.Assert_AbspathFilename(sourceFileName);
      PathUtil.Assert_AbspathFilename(destFileName);
#endif

      string dirname = System.IO.Path.GetDirectoryName(destFileName);
      if(!System.IO.Directory.Exists(dirname))
        System.IO.Directory.CreateDirectory(dirname);

      System.IO.File.Copy(sourceFileName,destFileName,overwrite);
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

    /// <summary>
    /// This functions looks from where to copy the foFileNode from. First we have a look in our own file system.
    /// Then we look at the files on the medium.
    /// </summary>
    /// <param name="foFileNode">This is the file in the foreign file system that should be copied here to destFileName.</param>
    /// <param name="destFileName">The destination file name where to copy to.</param>
    /// <param name="overwrite">True if the destination file can be overwritten. False if not.</param>
    /// <returns>True if the copy was successfull, false otherwise.</returns>
    public bool CopyWithDirectoryCreation(FileNode foFileNode, string destFileName, bool overwrite)
    {
      object o = this._allFilesHere[foFileNode.FileHash];
      if(o is PathAndFileNode)
      {
        PathAndFileNode pfn = (PathAndFileNode)o;
        if(System.IO.File.Exists(pfn.Path) && foFileNode.HasSameHashThan(pfn.Path))
        {
          CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite, foFileNode);
          return true;
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
            CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite,foFileNode);
            return true; // return true if the copy was successfull
          }
        }
       
      }
    

      string sourcefilename = PathUtil.Combine_Abspath_Filename(_doc.MediumDirectoryName,foFileNode.MediumFileName);
      if(foFileNode.HasSameHashThan(sourcefilename))
      {
        CopyWithDirectoryCreation(sourcefilename, destFileName, overwrite, foFileNode);
        return true;
      }
      
      return false;
    }


    void DeleteFileForced(string path)
    {
#if DEBUG
      PathUtil.Assert_AbspathFilename(path);
#endif

      try
      {
        System.IO.File.Delete(path);
        return;
      }
      catch(System.IO.IOException)
      {
      }

      // if this was not successfull, try to remove the read-only attribute
      System.IO.FileInfo info = new System.IO.FileInfo(path);
      info.Attributes &= ~(System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
      // this time we try to delete the file without catching the exception
      System.IO.File.Delete(path);
    }


    public override void DirectExecute()
    {
      System.Text.StringBuilder stb = new System.Text.StringBuilder();
    
      foreach(SyncItemTag tag in _filesToSynchronize)
      {
        if(_monitor.ShouldReport)
          _monitor.Report(string.Format("Perform {0} on {1}",tag.Action.ToString(),tag.FileName));
        try
        {
          PerformAction(tag);
        }
        catch(Exception ex)
        {
          if(stb.Length>0)
            stb.Append(System.Environment.NewLine);
          stb.Append(ex.ToString());
        }


      }

      if(stb.Length>0)
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm,stb.ToString(),"Error report",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Exclamation);
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
          if(PathUtil.IsDirectoryName(myfilename)) // delete subdir
          {
            try 
            {
              System.IO.Directory.Delete(myfilename); 
              myRoot.DeleteSubDirectoryNode(relPathFileName);
              foRoot.DeleteSubDirectoryNode(relPathFileName);

            }
            catch(System.IO.IOException )  { } // Don't care if subdir cannot be deleted, maybe files are still in there!
          }
          else // Delete file
          {
            DeleteFileForced(myfilename);
            myRoot.DeleteFileNode(relPathFileName);
            foRoot.DeleteFileNode(relPathFileName);
          }
          break;
        case SyncAction.RemoveRollback:
          foRoot.DeleteFileNode(relPathFileName);
          break;
        case SyncAction.Copy:
          foFileNode = foRoot.GetFileNode(relPathFileName);
       
          if(CopyWithDirectoryCreation(foFileNode,myfilename,false))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = UpdateMyFile(_doc.MyRoot(tag.RootListIndex),myfileinfo,true);
            if(foFileNode.HasSameHashThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.Overwrite:
        case SyncAction.ResolveManuallyOverwrite:
          foFileNode = foRoot.GetFileNode(relPathFileName);
          if(CopyWithDirectoryCreation(foFileNode,myfilename,true))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = UpdateMyFile(_doc.MyRoot(tag.RootListIndex),myfileinfo,true);
            if(foFileNode.HasSameHashThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.ResolveManuallyRollback:
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
