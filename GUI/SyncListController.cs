using System;
using System.Collections;
using System.Windows.Forms;

namespace Syncoco
{
  using Traversing;

  public enum SyncAction 
  {
    Remove, RemoveManually, RemoveRollback, Copy, Overwrite, ResolveManually, ResolveManuallyOverwrite,
    ResolveManuallyIgnore, ResolveManuallyRollback
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
      if(action==SyncAction.RemoveManually || action==SyncAction.ResolveManually)
        return System.Drawing.Color.LightGray;
      else
        return System.Drawing.Color.White;
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
        item.SubItems.Add(action.ToString());
        item.SubItems.Add(directoryname);

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

    public void EhView_SetToRemove()
    {
      ChangeActionOnSelected(SyncAction.RemoveManually,SyncAction.Remove);
    }

    public void EhView_SetToRollbackRemove()
    {
      ChangeActionOnSelected(SyncAction.RemoveManually,SyncAction.RemoveRollback);
      ChangeActionOnSelected(SyncAction.Remove,SyncAction.RemoveRollback);
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
      ArrayList list = new ArrayList();


      int count = _view.GetItemCount();
      for(int i=0;i<count;i++)
      {
        if(_view.IsItemSelected(i))
          list.Add(_view.GetItem(i).Tag);
      }

      if(count>0 && list.Count==0)
      {
        DialogResult dlgres = MessageBox.Show(Current.MainForm, "Nothing selected! Do you want to synchronize all items in the list?",
          "Nothing selected!",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);

        if(DialogResult.Yes==dlgres)
        {
          for(int i=0;i<count;i++)
            list.Add(_view.GetItem(i).Tag);
        }
      }


      DocumentActions.SynchronizeFilesAction action = new DocumentActions.SynchronizeFilesAction(Current.Document,list);
      action.BackgroundExecute();
    }


 

  }
}
