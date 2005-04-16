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
    public string FileName;
    public SyncAction Action;
    public int RootListIndex;
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

    public SyncList View { get { return _view; }}

    public SyncListController()
    {

      _view = new SyncList();
      _view._controller = this;
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
        item.Tag = new SyncItemTag(rootListIndex,action,fullname);
        item.UseItemStyleForSubItems=false; // neccessary that the subitems can have different colors
        
        ListViewItem.ListViewSubItem actionItem = item.SubItems.Add(action.ToString());
        actionItem.BackColor = this.GetBackColor(action);

        string fullDirectoryName = PathUtil.Combine_Abspath_RelPath(Current.Document.MyRoot(rootListIndex).FilePath,directoryname);
        item.SubItems.Add(fullDirectoryName);

        if(!isDirectory)
        {
          FileNode myNode = Current.Document.RootPair(rootListIndex).MyRoot.DirectoryNode.GetFileNodeFullPath(fullname);
          FileNode foNode = Current.Document.RootPair(rootListIndex).ForeignRoot.DirectoryNode.GetFileNodeFullPath(fullname);
          

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
