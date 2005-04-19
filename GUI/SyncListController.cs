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
using System.Windows.Forms;

namespace Syncoco
{
  using Traversing;

  public enum SyncAction 
  {
    Remove, RemoveRollback, ForcedRemove, 
    RemoveManually, RemoveManuallyRollback, 
    Copy,
    Overwrite, OverwriteRollback, 
    ResolveManually, ResolveManuallyOverwrite, ResolveManuallyIgnore, ResolveManuallyRollback,
    CreateDirectory
  };

  public class SyncItemTag
  {
    public string ShortName;
    public string FileName;
    public string FullDirectoryName;
    public SyncAction Action;
    public int RootListIndex;
    public long myFileLength=-1;
    public long foFileLength=-1;
    public DateTime myWriteTime;
    public DateTime foWriteTime;

    public SyncItemTag(int rootListIndex, SyncAction action, string fullFileName)
    {
      RootListIndex = rootListIndex;
      Action = action;
      FileName = fullFileName;
    }
  }

  public class SyncItemTagList : System.Collections.CollectionBase
  {
    public SyncItemTag this[int i]
    {
      get { return (SyncItemTag)this.InnerList[i]; }
      set { this.InnerList[i] = value; }
    }

    public void Add(SyncItemTag item)
    {
      this.InnerList.Add(item);
    }

    public void Sort()
    {
      this.InnerList.Sort(new Comparer());
    }

    private class Comparer : IComparer
    {
      #region IComparer Members

      public int Compare(object x, object y)
      {
        SyncItemTag xx = (SyncItemTag)x;
        SyncItemTag yy = (SyncItemTag)y;

        if(xx.RootListIndex!=yy.RootListIndex)
          return xx.RootListIndex<yy.RootListIndex ? -1 : 1;

        int sresult = string.Compare(xx.FileName,yy.FileName,true);
        return sresult;
      }

      #endregion
    }
  }


  /// <summary>
  /// Summary description for SyncListController.
  /// </summary>
  public class SyncListController
  {
    FilesToSynchronizeCollector[] _collectors;

    SyncList _view;
    ListViewItemComparer _comparer = new ListViewItemComparer();

    public SyncList View { get { return _view; }}

    public SyncListController()
    {

      _view = new SyncList();
      _view._controller = this;
      _view.InitializeColumnHeaders();
      //      _view.InitializeListCaptions(_document.MyRootComputerName,_document.ForeignRootComputerName);
    }

    public void SetCollectors(FilesToSynchronizeCollector[] collectors)
    {
      _collectors = collectors;

      
      UpdateList();
    
    }

    System.Drawing.Color GetBackColor(SyncAction action)
    {
      switch(action)
      {
        case SyncAction.Remove:
        case SyncAction.ForcedRemove:
          return System.Drawing.Color.OrangeRed;

        case SyncAction.Overwrite:
        case SyncAction.ResolveManuallyOverwrite:
          return System.Drawing.Color.Yellow;

        case SyncAction.RemoveManually:
        case SyncAction.ResolveManually:
          return System.Drawing.Color.LightCyan;

        default:
          return System.Drawing.SystemColors.Window;
      }
    }

    public void AddListViewItems(ArrayList list, int rootListIndex, SyncAction action, System.Collections.Specialized.StringCollection filenames)
    {
      foreach(string fullname in filenames)
      {
#if DEBUG
        PathUtil.Assert_RelpathOrFilename(fullname);
#endif

        string filename, directoryname;
        bool isDirectory = PathUtil.IsDirectoryName(fullname);
        if(isDirectory)
        {
          PathUtil.SplitInto_Relpath_Dirname(fullname,out directoryname, out filename);
          filename += System.IO.Path.DirectorySeparatorChar;
        }
        else
        {
          PathUtil.SplitInto_Relpath_Filename(fullname,out directoryname, out filename);
        }

        ListViewItem item = new ListViewItem(filename);
        SyncItemTag syncItemTag = new SyncItemTag(rootListIndex,action,fullname);
        syncItemTag.ShortName = filename;
        item.Tag = syncItemTag;
        item.UseItemStyleForSubItems=false; // neccessary that the subitems can have different colors
        
        ListViewItem.ListViewSubItem actionItem = item.SubItems.Add(action.ToString());
        actionItem.BackColor = this.GetBackColor(action);

        string fullDirectoryName = PathUtil.Combine_Abspath_RelPath(Current.Document.MyRoot(rootListIndex).FilePath,directoryname);
        item.SubItems.Add(fullDirectoryName);
        syncItemTag.FullDirectoryName = fullDirectoryName;

        if(!isDirectory)
        {
          FileNode myNode = Current.Document.RootPair(rootListIndex).MyRoot.DirectoryNode.GetFileNodeFullPath(fullname);
          FileNode foNode = Current.Document.RootPair(rootListIndex).ForeignRoot.DirectoryNode.GetFileNodeFullPath(fullname);
          
          if(myNode!=null)
          {
            syncItemTag.myFileLength = myNode.FileLength;
            syncItemTag.myWriteTime = myNode.LastWriteTimeUtc;
          }
          if(foNode!=null)
          {
            syncItemTag.foFileLength = foNode.FileLength;
            syncItemTag.foWriteTime = foNode.LastWriteTimeUtc;
          }

          string dat = string.Format("{0} | {1}",myNode==null ? "?" : myNode.LastWriteTimeUtc.ToString(), foNode==null? "?":foNode.LastWriteTimeUtc.ToString());
          string len = string.Format("{0} | {1}",myNode==null ? "?" : myNode.FileLength.ToString(), foNode==null? "?":foNode.FileLength.ToString());
          item.SubItems.Add(len);
          item.SubItems.Add(dat);
        }

        //  if(action==SyncAction.Copy)
        //    item.BackColor = System.Drawing.Color.LightGray;
        list.Add(item);
      }
    }

    public void UpdateList()
    {
      ArrayList list = new ArrayList();

      for(int rootListIndex=0;rootListIndex<_collectors.Length;rootListIndex++)
      {
        FilesToSynchronizeCollector coll = _collectors[rootListIndex];
        
        if(null==coll)
          continue;

        AddListViewItems(list, rootListIndex, SyncAction.CreateDirectory, coll.ToCreateDir);
        AddListViewItems(list, rootListIndex, SyncAction.Copy,coll.ToCopy);
        AddListViewItems(list, rootListIndex, SyncAction.Overwrite,coll.ToOverwrite);
        AddListViewItems(list, rootListIndex, SyncAction.ResolveManually,coll.ToResolveManually);
        AddListViewItems(list, rootListIndex, SyncAction.Remove,coll.ToRemove);
        AddListViewItems(list, rootListIndex, SyncAction.RemoveManually,coll.ToRemoveManually);
      }

      _view.InitializeList(list);

      _comparer = new ListViewItemComparer();
      _view.SortList(_comparer,2,false);
    }

    public void ChangeActionOnSelected(SyncAction originalAction, SyncAction newAction)
    {
      System.Drawing.Color newcolor = GetBackColor(newAction);

      int count = _view.GetItemCount();
      for(int i=0;i<count;i++)
      {
        if(!_view.IsItemSelected(i))
          continue;

      

        ListViewItem item = _view.GetItem(i);
        SyncItemTag tag = (SyncItemTag)item.Tag;

        if(tag.Action==originalAction)
        {
          tag.Action = newAction;
          item.BackColor = newcolor;
          item.SubItems[1].Text = tag.Action.ToString();
        }
      }
    }

    public void EhView_Remove_ForcedRemove()
    {
      ChangeActionOnSelected(SyncAction.Remove,SyncAction.ForcedRemove);
    }

    public void EhView_RemoveManually_Remove()
    {
      ChangeActionOnSelected(SyncAction.RemoveManually,SyncAction.Remove);
    }

    public void EhView_Remove_Rollback()
    {
      ChangeActionOnSelected(SyncAction.Remove,SyncAction.RemoveRollback);
    }

    public void EhView_Overwrite_Rollback()
    {
      ChangeActionOnSelected(SyncAction.Overwrite,SyncAction.OverwriteRollback);
    }


    public void EhView_RemoveManually_Rollback()
    {
      ChangeActionOnSelected(SyncAction.RemoveManually,SyncAction.RemoveManuallyRollback);
    }

    public void EhView_ResolveManually_Overwrite()
    {
      ChangeActionOnSelected(SyncAction.ResolveManually,SyncAction.ResolveManuallyOverwrite);
    }

    public void EhView_ResolveManually_Ignore()
    {
      ChangeActionOnSelected(SyncAction.ResolveManually,SyncAction.ResolveManuallyIgnore);
    }
    public void EhView_ResolveManually_Rollback()
    {
      ChangeActionOnSelected(SyncAction.ResolveManually,SyncAction.ResolveManuallyRollback);
    }

    public void EhView_ColumnClick(int nColumn)
    {
      bool reverse=false;
      switch(nColumn)
      {
        case 0:
          reverse = _comparer.ColumnClick(ListViewComparerColumn.Name);
          break;
        case 1:
          reverse = _comparer.ColumnClick(ListViewComparerColumn.Action);
          break;
        case 2:
          reverse = _comparer.ColumnClick(ListViewComparerColumn.Path);
          break;
        case 3:
          reverse = _comparer.ColumnClick(ListViewComparerColumn.FileLength);
          break;
        case 4:
          reverse = _comparer.ColumnClick(ListViewComparerColumn.FileTime);
          break;
      }

    _view.SortList(_comparer,nColumn,reverse);

 
    }

    #region Sorting class
    public enum ListViewComparerColumn
    {
      Name,
      Path,
      Action,
      FileLength,
      FileTime
    }
    class ListViewItemComparer : IComparer 
    {
      protected delegate int CompareFunction(SyncItemTag x, SyncItemTag y);

      CompareFunction[] _comparers = new CompareFunction[3];
      bool[] _reverse = new bool[3];

    
      public ListViewItemComparer() 
      {
        _comparers[0] = new CompareFunction( ComparePath );
        _comparers[1] = new CompareFunction( CompareName );
      }
      
      public bool ColumnClick(ListViewComparerColumn col)
      {
        CompareFunction func = GetCompareFunction(col);
        if(func==_comparers[0])
        {
          _reverse[0] = !_reverse[0];
        }
        else
        {
          for(int i=_comparers.Length-1;i>0;i--)
          {
            _comparers[i] = _comparers[i-1];
            _reverse[i] = _reverse[i-1];
          }
          _comparers[0] = func;
          _reverse[0] = false;
        }

        return _reverse[0];
      }

      private CompareFunction GetCompareFunction(ListViewComparerColumn col)
      {
        switch(col)
        {
          case ListViewComparerColumn.Name:
            return new CompareFunction( CompareName);
           
          case ListViewComparerColumn.Path:
            return new CompareFunction( ComparePath);
           
          case ListViewComparerColumn.Action:
            return new CompareFunction( CompareAction);
           
          case ListViewComparerColumn.FileLength:
            return new CompareFunction( CompareFileLength);
           
          case ListViewComparerColumn.FileTime:
            return new CompareFunction( CompareFileTime);
         
          default:
            throw new ApplicationException("Oops, probably I have forgotten to implement the case " + col.ToString());
        }
      }

      public int Compare(object x, object y) 
      {
        //return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        SyncItemTag xx = (SyncItemTag)(((ListViewItem)x).Tag);
        SyncItemTag yy = (SyncItemTag)(((ListViewItem)y).Tag);
        for(int i=0;i<_comparers.Length;i++)
        {
          CompareFunction func = _comparers[i];
          if(null==func)
            break;

          int result = func(xx,yy);
          if(result!=0) 
            return _reverse[i] ? -result : result;
        }

        return 0;
      }

      protected int CompareName(SyncItemTag x, SyncItemTag y)
      {
        return string.Compare(x.ShortName,y.ShortName,true);
      }

      protected int ComparePath(SyncItemTag x, SyncItemTag y)
      {
        return string.Compare(x.FullDirectoryName,y.FullDirectoryName);
      }

      protected int CompareAction(SyncItemTag x, SyncItemTag y)
      {
        return x.Action==y.Action ? 0 : ((int)x.Action)<((int)y.Action) ? -1 : 1;
      }

      protected int CompareFileLength(SyncItemTag x, SyncItemTag y)
      {
        long xx = Math.Max(x.myFileLength,x.foFileLength);
        long yy = Math.Max(y.myFileLength,y.foFileLength);
        return xx==yy ? 0 : (xx<yy ? -1 : 1);
      }
      protected int CompareFileTime(SyncItemTag x, SyncItemTag y)
      {
        DateTime xx = Max(x.myWriteTime,x.foWriteTime);
        DateTime yy = Max(y.myWriteTime,y.foWriteTime);
        return xx==yy ? 0 : (xx<yy ? -1 : 1);
      }

      private DateTime Max(DateTime x, DateTime y)
      {
        return x>y ? x : y;
      }


    
    }

    #endregion

    public void Synchronize()
    {
      SyncItemTagList list = new SyncItemTagList();


      int count = _view.GetItemCount();
      for(int i=0;i<count;i++)
      {
        if(_view.IsItemSelected(i))
          list.Add((SyncItemTag)_view.GetItem(i).Tag);
      }

      if(count>0 && list.Count==0)
      {
        DialogResult dlgres = MessageBox.Show(Current.MainForm, "Nothing selected! Do you want to synchronize all items in the list?",
          "Nothing selected!",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);

        if(DialogResult.Yes==dlgres)
        {
          for(int i=0;i<count;i++)
            list.Add((SyncItemTag)_view.GetItem(i).Tag);
        }
      }


      //    DocumentActions.SynchronizeFilesAction action = new DocumentActions.SynchronizeFilesAction(Current.Document,list);
      DocumentActions.SynchronizingStrategy action = new DocumentActions.SynchronizingStrategy(Current.Document,list);
      action.BackgroundExecute();
    }


 

  }
}
