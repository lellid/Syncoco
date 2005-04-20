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
  #region Items classes

  #region Copy item
  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingCopyItem
  {
    protected string _destination=null;
    protected FileNode _foFileNode=null;
    protected FileHash _sourceContentHash;
    protected int _rootListIndex;
    protected System.Collections.Specialized.StringCollection _sources=null;
  
    protected SynchronizingCopyItem()
    {
    }

    public SynchronizingCopyItem(SyncItemTag tag, MainDocument doc)
    {
      System.Diagnostics.Debug.Assert(tag.Action==SyncAction.Copy);
      System.Diagnostics.Debug.Assert(tag!=null);

      string relPathFileName = tag.FileName;
      string absrootdir = doc.MyRoot(tag.RootListIndex).FilePath;


#if DEBUG
      PathUtil.Assert_RelpathOrFilename(relPathFileName);
      PathUtil.Assert_Abspath(absrootdir);
#endif

      
      _destination = PathUtil.Combine_Abspath_RelpathOrFilename(absrootdir,relPathFileName);

      FileSystemRoot foRoot = doc.ForeignRoot(tag.RootListIndex);
      _foFileNode = foRoot.GetFileNode(relPathFileName);
      _sourceContentHash = _foFileNode.FileHash;
      _rootListIndex = tag.RootListIndex;

    }

    public string Destination
    {
      get { return _destination; }
    }
  
    public System.Collections.Specialized.StringCollection Sources
    {
      get { return _sources; }
    }

    public FileHash SourceContentHash
    {
      get { return _sourceContentHash; }
    }

    public FileNode ForeignFileNode
    {
      get { return _foFileNode; }
    }

    public int RootListIndex
    {
      get { return _rootListIndex; }
    }
  }

  public class SynchronizingCopyItemList : System.Collections.CollectionBase
  {
    public SynchronizingCopyItem this[int i]
    {
      get { return (SynchronizingCopyItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }

    public void Add(SynchronizingCopyItem item)
    {
      this.InnerList.Add(item);
    }
  }

  #endregion

  #region Overwrite item

  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingOverwriteItem : SynchronizingCopyItem
  {
    protected FileNode _myFileNode=null;
    protected FileHash _destinationContentHash;


    public SynchronizingOverwriteItem(SyncItemTag tag, MainDocument doc)
    {
      System.Diagnostics.Debug.Assert(tag.Action==SyncAction.Overwrite || tag.Action==SyncAction.ResolveManuallyOverwrite);
      System.Diagnostics.Debug.Assert(tag!=null);

      string relPathFileName = tag.FileName;
      string absrootdir = doc.MyRoot(tag.RootListIndex).FilePath;


#if DEBUG
      PathUtil.Assert_RelpathOrFilename(relPathFileName);
      PathUtil.Assert_Abspath(absrootdir);
#endif

      
      _destination = PathUtil.Combine_Abspath_RelpathOrFilename(absrootdir,relPathFileName);

      FileSystemRoot foRoot = doc.ForeignRoot(tag.RootListIndex);
      _foFileNode = foRoot.GetFileNode(relPathFileName);
      _sourceContentHash = _foFileNode.FileHash;

      FileSystemRoot myRoot = doc.MyRoot(tag.RootListIndex);
      _myFileNode = myRoot.GetFileNode(relPathFileName);
      _destinationContentHash = _myFileNode.FileHash;

      _rootListIndex = tag.RootListIndex;

    }

    public FileHash DestinationContentHash
    {
      get { return _destinationContentHash; }
    }
  }
  public class SynchronizingOverwriteItemList : System.Collections.CollectionBase
  {
    public SynchronizingOverwriteItem this[int i]
    {
      get { return (SynchronizingOverwriteItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }

    public void Add(SynchronizingOverwriteItem item)
    {
      this.InnerList.Add(item);
    }

    
  }

  #endregion

  #region Delete item
  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingDeleteItem 
  {
    protected string _destination=null;
    protected FileHash _destinationContentHash;
    protected FileNode _myFileNode=null;
    protected FileNode _foFileNode=null;
    int _rootListIndex=-1;

    public SynchronizingDeleteItem(string path, FileHash hash)
    {
      _destination = path;
      _destinationContentHash = hash;
    }

    public SynchronizingDeleteItem(SyncItemTag tag, MainDocument doc)
    {
      System.Diagnostics.Debug.Assert(tag.Action==SyncAction.Remove || tag.Action==SyncAction.RemoveManually);
      System.Diagnostics.Debug.Assert(tag!=null);

      string relPathFileName = tag.FileName;
      string absrootdir = doc.MyRoot(tag.RootListIndex).FilePath;


#if DEBUG
      PathUtil.Assert_RelpathOrFilename(relPathFileName);
      PathUtil.Assert_Abspath(absrootdir);
#endif

      
      _destination = PathUtil.Combine_Abspath_RelpathOrFilename(absrootdir,relPathFileName);

      FileSystemRoot foRoot = doc.ForeignRoot(tag.RootListIndex);
      _foFileNode = foRoot.GetFileNode(relPathFileName);

      FileSystemRoot myRoot = doc.MyRoot(tag.RootListIndex);
      _myFileNode = myRoot.GetFileNode(relPathFileName);
      _destinationContentHash = _myFileNode.FileHash;

      _rootListIndex = tag.RootListIndex;

    }



    public string Destination
    {
      get { return _destination; }
    }

    public FileHash DestinationContentHash
    {
      get { return _destinationContentHash; }
    }

    public int RootListIndex
    {
      get { return _rootListIndex; }
    }
  }

  public class SynchronizingDeleteItemList : System.Collections.CollectionBase
  {
    public SynchronizingDeleteItem this[int i]
    {
      get { return (SynchronizingDeleteItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }
    public void Add(SynchronizingDeleteItem item)
    {
      this.InnerList.Add(item);
    }
  }
  #endregion

  #region Source item

  public class SourceItem
  {
    public string Path;
    public bool Verified;

    public SourceItem(string fullpath)
    {
      Path = fullpath;
      Verified = false;
    }
  }

  public class SourceItemArray : System.Collections.CollectionBase
  {
    public SourceItem this[int i]
    {
      get { return (SourceItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }

    public void Add(SourceItem item)
    {
      this.InnerList.Add(item);
    }

    public bool Contains(string fullpath)
    {
      for(int i=Count-1;i>=0;i--)
        if(PathUtil.ArePathsEqual(this[i].Path,fullpath))
          return true;

      return false;
    }
  }

  public class SourceItemHash : System.Collections.DictionaryBase
  {
    public SourceItemArray this[FileHash key]
    {
      get  
      {
        return( (SourceItemArray)Dictionary[key] );
      }
      set  
      {
        Dictionary[key] = value;
      }
    }

    protected void Add(FileHash key, string fullpath, SourceItemArray existingvalue)
    {
      if(existingvalue==null)
      {
        existingvalue = new SourceItemArray();
        Dictionary.Add(key,existingvalue);
      }
       
      if(!existingvalue.Contains(fullpath))
        existingvalue.Add(new SourceItem(fullpath));
    }

    /// <summary>
    /// Adds a file to the list.
    /// </summary>
    /// <param name="key">The hash of the file. It is not neccessary to be verified before, since it is verified before the item is used.</param>
    /// <param name="fullfilename">The full name of the file.</param>
    public void Add(FileHash key, string fullfilename)
    {
      SourceItemArray existingvalue = this[key];
      Add(key,fullfilename,existingvalue);
    }
    /// <summary>
    /// Adds a entry to this hash.
    /// </summary>
    /// <param name="key">The file hash of the files to add.</param>
    /// <param name="val">The value must either be a <see>PathAndFileNode</see> or an <see>ArrayList</see> of <see>PathAndFileNode</see>s.</param>
    public void Add(FileHash key, object val)
    {
      SourceItemArray existingvalue = this[key];
      if(val is PathAndFileNode)
      {
        Add(key,((PathAndFileNode)val).Path,existingvalue);
      }
      else if(val is ArrayList)
      {
        ArrayList list = (ArrayList)val;
        foreach(PathAndFileNode pafn in list)
        {
          Add(key,pafn.Path,existingvalue);
          existingvalue = this[key];
        }
      }
    }

    public void Remove(FileHash key, string path)
    {
      SourceItemArray existingvalue = this[key];
      if(existingvalue!=null)
      {
        for(int i=existingvalue.Count-1;i>=0;i--)
        {
          if(PathUtil.ArePathsEqual(existingvalue[i].Path,path))
            existingvalue.RemoveAt(i);
        }
      }

    }
  }


  #endregion

  #region Source reference count

  public class SourceHashReferenceCounter : System.Collections.DictionaryBase
  {
    public void Increment(FileHash hash)
    {
      object o = Dictionary[hash];
      if(o!=null)
        Dictionary[hash] = 1+(int)o;
      else
        Dictionary.Add(hash,1);
    }

    public void Decrement(FileHash hash)
    {
      object o = Dictionary[hash];
      System.Diagnostics.Debug.Assert(o!=null);
      System.Diagnostics.Debug.Assert(o is int);
      System.Diagnostics.Debug.Assert(((int)o)>0);
      int oo = (int)o - 1;
      if(oo>0)
        Dictionary[hash] = oo;
      else
        Dictionary.Remove(hash);
    }

    public bool IsReferenced(FileHash hash)
    {
      return Dictionary.Contains(hash);
    }

  
  }

  #endregion

  #endregion

 
  public class SynchronizingStrategy : AbstractDocumentAction
  {
    #region Member variables


    SourceItemHash _sourceItemHash = new SourceItemHash();
    SourceHashReferenceCounter _sourceReferenceConter = new SourceHashReferenceCounter();

    public SynchronizingCopyItemList _copyItems = new SynchronizingCopyItemList();
    public SynchronizingOverwriteItemList _overwriteItems = new SynchronizingOverwriteItemList();
    public SynchronizingDeleteItemList _deleteItems = new SynchronizingDeleteItemList();

    SyncItemTagList _syncItemTagList;

    bool _verbose=true;

    #endregion

    #region Constructors

    public SynchronizingStrategy(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
      : base(doc,monitor,reporter)
    {
    }


    public SynchronizingStrategy(MainDocument doc, SyncItemTagList list)
      : this(doc,null,null)
    {
      _syncItemTagList = list;
      for(int i=_syncItemTagList.Count-1;i>=0;i--)
      {
        SyncItemTag item = _syncItemTagList[i];
        switch(item.Action)
        {
          case SyncAction.Copy:
            _copyItems.Add(new SynchronizingCopyItem(item,_doc));
            _syncItemTagList.RemoveAt(i);
            break;
          case SyncAction.Overwrite:
          case SyncAction.ResolveManuallyOverwrite:
            _overwriteItems.Add(new SynchronizingOverwriteItem(item,_doc));
            _syncItemTagList.RemoveAt(i);
            break;
          case SyncAction.Remove:
            if(!PathUtil.IsDirectoryName(item.FileName))
            {
              _deleteItems.Add(new SynchronizingDeleteItem(item,_doc));
              _syncItemTagList.RemoveAt(i);
            }
            break;
        }
      } // end for



      // sort the rest of the items so that directories with the longer paths will be deleted
      // first
      _syncItemTagList.Sort();

      // now we must collect all files that can be the sources to the copy and overwrite
      // items
      for(int i=0;i<_copyItems.Count;i++)
      {
        FileHash sourceContentHash = _copyItems[i].SourceContentHash;
        object o = doc.CachedAllMyFiles[sourceContentHash];
        this._sourceItemHash.Add(sourceContentHash,o);
        this._sourceReferenceConter.Increment(sourceContentHash);
        string mediumfilename = PathUtil.Combine_Abspath_Filename(_doc.MediumDirectoryName,sourceContentHash.MediumFileName);
        if(System.IO.File.Exists(mediumfilename))
          this._sourceItemHash.Add(sourceContentHash,mediumfilename);
      }
      for(int i=0;i<_overwriteItems.Count;i++)
      {
        FileHash sourceContentHash = _overwriteItems[i].SourceContentHash;
        object o = doc.CachedAllMyFiles[sourceContentHash];
        this._sourceItemHash.Add(sourceContentHash,o);
        this._sourceReferenceConter.Increment(sourceContentHash);
        string mediumfilename = PathUtil.Combine_Abspath_Filename(_doc.MediumDirectoryName,sourceContentHash.MediumFileName);
        if(System.IO.File.Exists(mediumfilename))
          this._sourceItemHash.Add(sourceContentHash,mediumfilename);
      }
    }

    #endregion

    #region Questions

    /// <summary>
    /// Verifies that the source file exists and that it has a FileHash equal to filehash.
    /// </summary>
    /// <param name="item">The source item.</param>
    /// <param name="filehash">The expected file hash.</param>
    /// <returns>True if the source file was verified sucessfully.</returns>
    /// <remarks>If the verify was successfull, the items Verify property will be set to true.</remarks>
    bool VerifySourceItem(SourceItem item, FileHash filehash)
    {
      bool result = false;
      System.IO.FileInfo info = new System.IO.FileInfo(item.Path);
      if(info.Exists)
      {
        try
        {
          result = filehash == FileNode.CalculateHash(info);
        }
        catch(HashCalculationException)
        {
        }
      }
      item.Verified = result;
      return result;
    }
        

    /// <summary>
    /// Determines whether or not the given content (hash) is still needed for a copy or overwrite action.
    /// </summary>
    /// <param name="hash">The content hash.</param>
    /// <returns>True if this content, given by the hash, is needed in a copy or overwrite item.</returns>
    bool IsReferencedSourceHash(FileHash hash)
    {
      return this._sourceReferenceConter.IsReferenced(hash);
    }

  

    /// <summary>
    /// Determines whether this file can be deleted. It can be if it is
    /// not needed for copy or overwrite actions anymore.
    /// </summary>
    /// <param name="filename">Absolute path file name to the file.</param>
    /// <param name="filehash">The filehash of the file.</param>
    /// <param name="testReference">If true, it is tested, whether or not the filehash of this item is still needed in another copy or overwrite operation. If this
    /// is not the case, the item can be safely deleted (the return value is true then).</param>
    /// <returns>True if the file can be deleted.</returns>
    public bool CanBeDeleted( string filename, FileHash filehash, bool testReference )
    {
      SourceItemArray arr = this._sourceItemHash[filehash];
      
      if(arr==null)
        return true;
      if(testReference && !IsReferencedSourceHash(filehash))  // make sure that this item is needed somewhere for a copy or overwrite
        return true;
      else // arr!=null)
      {
        // first look if there is already a file which is verified and is not the file
        // we consider here
        foreach(SourceItem item in arr)
        {
          if(item.Verified && !PathUtil.ArePathsEqual(item.Path,filename))
            return true;
        }

        // if this is not the case, then verify the first unverified item
        // which is not the file we consider here
        for(int i=arr.Count-1;i>=0;i--)
        {
          SourceItem item = arr[i];
          if(!PathUtil.ArePathsEqual(item.Path,filename))
          {
            System.Diagnostics.Debug.Assert(item.Verified==false);
            VerifySourceItem(item, filehash);
            if(item.Verified==false) // if not verified delete this from the list of possible items afterwards
              arr.RemoveAt(i);
            else
              return true; // item is verified
          }
        }
      }

      return false;
    }

    /// <summary>
    /// Determines if a copyitem prevents any deleteItem to be deleted or any overwriteItem
    /// to be overwritten.
    /// </summary>
    /// <param name="copyitem">The copy item to examine.</param>
    /// <returns>True if that copy item prevents another item to be deleted or overwritten. Otherwise false.</returns>
    public bool PreventsDeleteOrOverwrite(SynchronizingCopyItem copyitem)
    {

      foreach(SynchronizingOverwriteItem item in _overwriteItems)
      {
        if(copyitem.SourceContentHash==item.DestinationContentHash && 
          !CanBeDeleted(item.Destination, item.DestinationContentHash, false))
          return true;
      }

      foreach(SynchronizingDeleteItem item in _deleteItems)
      {
        if(copyitem.SourceContentHash==item.DestinationContentHash && 
          !CanBeDeleted(item.Destination, item.DestinationContentHash, false))
          return true;
      }

      return false;
    }

    /// <summary>
    /// Copies the first overwrite item (the destination, i.e. the file to overwrite (!)) to a
    /// temporary location. This is because the destination file is neccessary elsewhere either
    /// for a overwrite or copy operation.
    /// </summary>
    public void MoveSingleOverwriteItemToTemporaryLocation()
    {
      SynchronizingOverwriteItem overwriteitem = _overwriteItems[0]; 

      string tempfile = System.IO.Path.GetTempFileName();

      System.IO.File.Copy(overwriteitem.Destination,tempfile,true);

      // now replace that file in the sourceItemHash by the newly created temporary file
      SourceItemArray arr = this._sourceItemHash[overwriteitem.DestinationContentHash];
      System.Diagnostics.Debug.Assert(arr!=null);
      foreach(SourceItem srcitem in arr)
      {
        if(PathUtil.ArePathsEqual(srcitem.Path,overwriteitem.Destination))
        {
          srcitem.Path = tempfile;
        }
      }

      // additionally, the temp file name is added to the collection of files that
      // should be deleted
      _deleteItems.Add(new SynchronizingDeleteItem(tempfile,overwriteitem.DestinationContentHash));
    }



   

    #endregion

    #region Single Actions

    #region Deletion

    public void ExecuteDeleteOn(SynchronizingDeleteItem item)
    {
      if(_monitor.ShouldReport)
        _monitor.Report("Deleting file " + item.Destination);
      
      FunctionResult functionResult = DeleteFileForced(item.Destination);

      if(_verbose)
        _reporter.ReportText(string.Format("Deleting file {0}, result: {1}\r\n",item.Destination,functionResult.ToString()));

          
      if(FunctionResult.Success==functionResult)
      {
        if(item.RootListIndex>=0) // special case if RootListIndex<0 then that is a temporary item to delete
        {
          FileSystemRoot myRoot = _doc.MyRoot(item.RootListIndex);
          FileSystemRoot foRoot = _doc.ForeignRoot(item.RootListIndex);
          string relPathFileName = PathUtil.GetRelpathFromAbspath(item.Destination, myRoot.FilePath);
          myRoot.DeleteFileNode(relPathFileName);
          foRoot.DeleteFileNode(relPathFileName);
        }
      }
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

    #endregion

    #region Overwrite

    public void ExecuteOverwriteOn(SynchronizingOverwriteItem item)
    {
      if(_monitor.ShouldReport)
        _monitor.Report("Overwriting file " + item.Destination);

      
      SourceItemArray arr = this._sourceItemHash[item.SourceContentHash];
      if(arr==null)
      {
        // Whats this? There are no sources we can use.
        // TODO report an error here
        return;
      }

      for(int i=arr.Count-1;i>=0;i--)
      {
        FunctionResult functionResult = CopyWithDirectoryCreation(arr[i].Path,item.Destination,true,item.ForeignFileNode);

        if(_verbose)
          _reporter.ReportText(string.Format("Overwriting file {0} with {1}, result: {2}\r\n",item.Destination,arr[i].Path,functionResult.ToString()));

        if(FunctionResult.Success==functionResult)
        {
          FileSystemRoot myRoot = _doc.MyRoot(item.RootListIndex);
          FileSystemRoot foRoot = _doc.ForeignRoot(item.RootListIndex);
          string relPathFileName = PathUtil.GetRelpathFromAbspath(item.Destination, myRoot.FilePath);
        
          // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
          // file node to unchanged
          System.IO.FileInfo myfileinfo = new System.IO.FileInfo(item.Destination);
          FileNode myfilenode = UpdateMyFile(_doc.MyRoot(item.RootListIndex),myfileinfo,true);
          FileNode foFileNode = foRoot.GetFileNode(relPathFileName);
          if(foFileNode.HasSameContentThan(myfilenode))
          {
            foFileNode.SetToUnchanged();
            myfilenode.SetToUnchanged();
          }
          return;
        }
        else if(!VerifySourceItem(arr[i],item.SourceContentHash))
        {
          arr.RemoveAt(i);
        }
      }
      // TODO report an error here

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
    #endregion

    #region Copy

    public void ExecuteCopyOn(SynchronizingCopyItem item)
    {
      if(_monitor.ShouldReport)
        _monitor.Report("Copying to file " + item.Destination);

      SourceItemArray arr = this._sourceItemHash[item.SourceContentHash];
      if(arr==null)
      {
        // TODO report an error here
        return;
      }

      for(int i=arr.Count-1;i>=0;i--)
      {
        FunctionResult functionResult = CopyWithDirectoryCreation(arr[i].Path,item.Destination,false,item.ForeignFileNode);
        if(_verbose)
          _reporter.ReportText(string.Format("Copying file {0} from {1}, result: {2}\r\n",item.Destination,arr[i].Path,functionResult.ToString()));


        if(FunctionResult.Success==functionResult)
        {
          FileSystemRoot myRoot = _doc.MyRoot(item.RootListIndex);
          FileSystemRoot foRoot = _doc.ForeignRoot(item.RootListIndex);
          string relPathFileName = PathUtil.GetRelpathFromAbspath(item.Destination, myRoot.FilePath);

          // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
          // file node to unchanged
          System.IO.FileInfo myfileinfo = new System.IO.FileInfo(item.Destination);
          FileNode myfilenode = UpdateMyFile(_doc.MyRoot(item.RootListIndex),myfileinfo,true);
          FileNode foFileNode = foRoot.GetFileNode(relPathFileName);

          if(foFileNode.HasSameContentThan(myfilenode))
          {
            foFileNode.SetToUnchanged();
            myfilenode.SetToUnchanged();
          }
          return;
        }
        else if(!VerifySourceItem(arr[i],item.SourceContentHash))
        {
          arr.RemoveAt(i);
        }
      }
      // TODO report an error here
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

   
   
    #endregion

    #endregion

    #region Complex Actions
    /// <summary>
    /// Delete all deleteItems which can be deleted now.
    /// </summary>
    /// <returns>False if no item could be deleted. True if at least one item could be deleted.</returns>
    public bool DeletePossibleItems()
    {
      if(_verbose)
        _reporter.ReportText("Delete possible items\r\n");

      bool hasDeleted = false;
      for(int i=_deleteItems.Count-1;i>=0;i--)
      {
        if(CanBeDeleted(_deleteItems[i].Destination,_deleteItems[i].DestinationContentHash, true))
        {
          ExecuteDeleteOn(_deleteItems[i]);
          _sourceItemHash.Remove(_deleteItems[i].DestinationContentHash,_deleteItems[i].Destination);
          _deleteItems.RemoveAt(i);
          hasDeleted = true;
        }
      }
      return hasDeleted;
    }



    /// <summary>
    /// Overwrites all overwriteItems which can be overwritten now.
    /// </summary>
    /// <returns>False if no item could be overwritten. True if at least one item could be overwritten.</returns>
    public bool OverwritePossibleItems()
    {
      if(_verbose)
        _reporter.ReportText("Overwrite possible items\r\n");
      
      bool hasOverwritten = false;
      for(int i=_overwriteItems.Count-1;i>=0;i--)
      {
        if(CanBeDeleted(_overwriteItems[i].Destination,_overwriteItems[i].DestinationContentHash, true))
        {
          ExecuteOverwriteOn(_overwriteItems[i]);
          _sourceItemHash.Remove(_overwriteItems[i].DestinationContentHash,_overwriteItems[i].Destination);
          _sourceReferenceConter.Decrement(_overwriteItems[i].SourceContentHash);
          _overwriteItems.RemoveAt(i);
          hasOverwritten = true;
        }
      }

      return hasOverwritten;
    }

    /// <summary>
    /// Searches for a item to copy that prevents another file from being deleted or overwritten.
    /// If such an item is found, the copy operation will be performed and the return value is true.
    /// </summary>
    /// <returns>False if no such item could be found, true if an item was found.</returns>
    public bool CopySingleLockedItem()
    {
      if(_verbose)
        _reporter.ReportText("Copy single locked item\r\n");

      for(int i=_copyItems.Count-1;i>=0;i--)
      {
        if(PreventsDeleteOrOverwrite(_copyItems[i]))
        {
          ExecuteCopyOn(_copyItems[i]);
          _sourceReferenceConter.Decrement(_copyItems[i].SourceContentHash);
          _copyItems.RemoveAt(i);
          return true;
        }
      }
      return false;
    }


    /// <summary>
    /// Perform the copy actions of all residual copy items.
    /// </summary>
    public void CopyAllItems()
    {
      if(_verbose)
        _reporter.ReportText("Copy all items\r\n");

      for(int i=_copyItems.Count-1;i>=0;i--)
      {
        ExecuteCopyOn(_copyItems[i]);
        _sourceReferenceConter.Decrement(_copyItems[i].SourceContentHash);
        _copyItems.RemoveAt(i);
      }
    }
        
      
    /// <summary>
    /// Try to delete items and then to overwriteitems. Then to copy files that prevent other files from
    /// being deleted or overwritten. This is repeated, until no more actions can be done.
    /// </summary>
    public void ExecuteUntilAllLockedUp()
    {
      bool somethingDone=false;
      do
      {
        do
        {
          somethingDone = false;
          
          DeletePossibleItems();

        while(OverwritePossibleItems())
          somethingDone = true;

        } while(somethingDone);

        // if neither delete or overwrite can be done, try to copy those files, which are
        // destination files for delete or overwrite (which prevents deleteItems to be deleted)
        if(CopySingleLockedItem())
          somethingDone = true;

      } while(somethingDone);
    }
    #endregion

    #region Action not requiring order


    /// <summary>
    /// This performs an action on an item that does not requires order. Thus, this item must not be
    /// a copy, overwrite or delete file item.
    /// </summary>
    /// <param name="tag">The item for which to perform the action.</param>
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
            if(FunctionResult.Success==DeleteDirectory(myfilename,SyncAction.ForcedRemove==tag.Action)) 
            {
              myRoot.DeleteSubDirectoryNode(relPathFileName);
              foRoot.DeleteSubDirectoryNode(relPathFileName);
            }
          }
          else // Delete file
          {
            throw new ApplicationException("Please report this exception to the bug list");
          }
          break;
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
          throw new ApplicationException("Please report this exception to the bug list");
          /*
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
          */
        case SyncAction.CreateDirectory:
          CreateDirectory(myfilename);          
          break;
        case SyncAction.Overwrite:
        case SyncAction.ResolveManuallyOverwrite:
          throw new ApplicationException("Please report this exception to the bug list");
          /*
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
          */
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

    public FileNode UpdateMyFile(FileSystemRoot fileSystemRoot, FileInfo fileinfo, bool forceUpdateHash)
    {
      DirectoryInfo dirinfo = new DirectoryInfo(fileSystemRoot.FilePath);

      return DirectoryUpdater.UpdateFileNode(fileSystemRoot.DirectoryNode,dirinfo,fileinfo,forceUpdateHash,_reporter);
    }



    #endregion

    public override void DirectExecute()
    {
      _doc.SetDirty();

      ExecuteUntilAllLockedUp();

      // if nothing more can be done and there are still items to overwrite,
      // we have a locked situation, from which we can only recover by saving some files
      // to a temporary location

      while(_overwriteItems.Count>0)
      {
        MoveSingleOverwriteItemToTemporaryLocation();
        ExecuteUntilAllLockedUp();
      }

      // now all files should be deleted
      System.Diagnostics.Debug.Assert(_deleteItems.Count==0);

      // the remaining are those files to copy, that have not locked up other files.
      CopyAllItems();


      // now perform the rest of the actions
      for(int i=_syncItemTagList.Count-1;i>=0;i--)
        PerformAction(_syncItemTagList[i]);
    }
  }

}
