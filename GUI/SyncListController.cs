using System;
using System.Collections;
using System.Windows.Forms;

namespace SyncTwoCo
{

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
    Collector[] _collectors;

    SyncList _view;

    public SyncList View { get { return _view; }}

    public SyncListController()
    {

      _view = new SyncList();
      _view._controller = this;
      //      _view.InitializeListCaptions(_document.MyRootComputerName,_document.ForeignRootComputerName);
    }

    public void SetCollectors(Collector[] collectors)
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
        string filename, directoryname;
        if(fullname[fullname.Length-1]==System.IO.Path.DirectorySeparatorChar)
        {
          int split = fullname.LastIndexOf(System.IO.Path.DirectorySeparatorChar,fullname.Length-2);
          filename = fullname.Substring(split+1);
          directoryname = split<0 ? string.Empty : fullname.Substring(0,split);
        }
        else
        {
          filename = System.IO.Path.GetFileName(fullname);
          directoryname = System.IO.Path.GetDirectoryName(fullname);
        }
        ListViewItem item = new ListViewItem(filename);
        item.SubItems.Add(action.ToString());
        item.SubItems.Add(directoryname);
        item.Tag = new SyncItemTag(rootListIndex,action,fullname);

        if(action==SyncAction.Copy)
          item.BackColor = System.Drawing.Color.LightGray;
        list.Add(item);
      }
    }

    public void UpdateList()
    {
      ArrayList list = new ArrayList();

      for(int rootListIndex=0;rootListIndex<_collectors.Length;rootListIndex++)
      {
        Collector coll = _collectors[rootListIndex];

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
      System.Text.StringBuilder stb = new System.Text.StringBuilder();
      
      int count = _view.GetItemCount();
      for(int i=0;i<count;i++)
      {
        if(!_view.IsItemSelected(i))
          continue;

      
        SyncItemTag tag = (SyncItemTag)_view.GetItemTag(i);

        try
        {

          if(false==Synchronize(tag))
            break;
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


    bool Synchronize(SyncItemTag tag)
    {
      Current.Document.PerformAction(tag);

      return true;
    }

  }
}
