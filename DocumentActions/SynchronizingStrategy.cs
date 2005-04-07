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
  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingCopyItem
  {
    protected string _destination=null;
    protected FileNode _foFileNode=null;
    protected FileHash _sourceContentHash;
    protected System.Collections.Specialized.StringCollection _sources=null;
  
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
  }

  public class SynchronizingCopyItemList : System.Collections.CollectionBase
  {
    public SynchronizingCopyItem this[int i]
    {
      get { return (SynchronizingCopyItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }
  }

  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingOverwriteItem : SynchronizingCopyItem
  {
    protected FileNode _myFileNode=null;
  }
  public class SynchronizingOverwriteItemList : System.Collections.CollectionBase
  {
    public SynchronizingOverwriteItem this[int i]
    {
      get { return (SynchronizingOverwriteItem)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }
  }

  /// <summary>
  /// Stores the information about how to copy which item from which source.
  /// </summary>
  public class SynchronizingDeleteItem 
  {
    protected string _destination=null;
    protected FileNode _myFileNode=null;
    protected FileNode _foFileNode=null;

    public SynchronizingDeleteItem(string path)
    {
      _destination = path;
    }


    public string Destination
    {
      get { return _destination; }
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

  public class SourceItem
  {
    public string Path;
    public bool Verified;
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
  }


  public class SynchronizingStrategy
  {
    #region Member variables


    SourceItemHash _sourceItemHash = new SourceItemHash();

    public SynchronizingCopyItemList _copyItems;
    public SynchronizingOverwriteItemList _overwriteItems;
    public SynchronizingDeleteItemList _deleteItems;

    #endregion

    #region Questions


    public bool CanBeDeleted(FileHash filehash, string filename)
    {
      SourceItemArray arr = this._sourceItemHash[filehash];
      
      if(arr==null)
      {
        return true;
      }
      else // arr!=null)
      {
        
      }

      return true;
    }


    /// <summary>
    /// Determines whether this file can be deleted. It can be if it is
    /// not needed for copy or overwrite actions anymore.
    /// </summary>
    /// <param name="filename">Absolute path file name to the file.</param>
    /// <returns>True if the file can be deleted.</returns>
    public bool CanBeDeleted(string filename)
    {
      PathUtil.Assert_AbspathFilename(filename);
      filename = PathUtil.NormalizeForComparison(filename);
      
      foreach(SynchronizingOverwriteItem item in _overwriteItems)
      {
        if(item.Sources.Contains(filename))
          return false;
      }

      foreach(SynchronizingCopyItem item in _copyItems)
      {
        if(item.Sources.Contains(filename))
          return false;
      }

      return true;
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
        if(copyitem.Sources.Contains(item.Destination))
          return true;
      }

      foreach(SynchronizingCopyItem item in _copyItems)
      {
        if(copyitem.Sources.Contains(item.Destination))
          return true;
      }

      return false;
    }

    #endregion

    #region Single Actions

    public void ExecuteDeleteOn(SynchronizingDeleteItem item)
    {
      
    }
    public void ExecuteOverwriteOn(SynchronizingOverwriteItem item)
    {
      
    }
    public void ExecuteCopyOn(SynchronizingCopyItem item)
    {
      
    }
    #endregion

    #region Complex Actions
    /// <summary>
    /// Delete all deleteItems which can be deleted now.
    /// </summary>
    /// <returns>False if no item could be deleted. True if at least one item could be deleted.</returns>
    public bool DeletePossibleItems()
    {
      bool hasDeleted = false;
      for(int i=_deleteItems.Count-1;i>=0;i--)
      {
        if(CanBeDeleted(_deleteItems[i].Destination))
        {
          ExecuteDeleteOn(_deleteItems[i]);
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
       bool hasOverwritten = false;
      for(int i=_overwriteItems.Count-1;i>=0;i--)
      {
        if(CanBeDeleted(_overwriteItems[i].Destination))
        {
          ExecuteOverwriteOn(_overwriteItems[i]);
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
      for(int i=_copyItems.Count-1;i>=0;i--)
      {
        if(PreventsDeleteOrOverwrite(_copyItems[i]))
        {
          ExecuteCopyOn(_copyItems[i]);
          _copyItems.RemoveAt(i);
          return true;
        }
      }
      return false;
    }


    public void CopyAllItems()
    {
      for(int i=_copyItems.Count-1;i>=0;i--)
      {
        ExecuteCopyOn(_copyItems[i]);
        _copyItems.RemoveAt(i);
      }
    }
        
    /// <summary>
    /// This will replace originalname by replacename in all source file collections.
    /// </summary>
    /// <param name="originalname">Name to replace.</param>
    /// <param name="replacename">Name which is replacing originalname.</param>
    public void ReplaceInSourceFiles(string originalname, string replacename)
    {
      foreach(SynchronizingOverwriteItem item in _overwriteItems)
      {
        if(item.Sources.Contains(originalname))
        {
          item.Sources.Remove(originalname);
          item.Sources.Add(replacename);
        }
      }
      foreach(SynchronizingCopyItem item in _copyItems)
      {
        if(item.Sources.Contains(originalname))
        {
          item.Sources.Remove(originalname);
          item.Sources.Add(replacename);
        }
      }

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

      System.IO.File.Copy(overwriteitem.Destination,tempfile);

      // now in all source collections which contains the destination file, 
      // the destination file name is replaced by the temp file name
      ReplaceInSourceFiles(overwriteitem.Destination,tempfile);

      // additionally, the temp file name is added to the collection of files that
      // should be deleted
      _deleteItems.Add(new SynchronizingDeleteItem(tempfile));
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

        while(DeletePossibleItems())
          somethingDone = true;

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

    public void DirectExecute()
    {
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
    }
  }

}
