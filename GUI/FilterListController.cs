using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for FilterListController.
  /// </summary>
  public class FilterListController : IApplyController
  {
    FilterList _doc;
    FilterListControl _view;

    FilterList _tempdoc;


    public FilterListController(FilterList doc)
    {
      _doc = doc;
      _tempdoc = new FilterList(_doc);
    }


    public FilterList Doc
    {
      get
      {
        return _doc;
      }
      set
      {
        _doc = value;
      }
    }

    public FilterListControl View
    {
      get
      {
        return _view;
      }
      set
      {
        _view = value;
        if(_view!=null)
        {
          _view.Controller = this;
        
          View.InitializeDefaultAction(_tempdoc.DefaultAction);
          UpdateList();
        }

      }
    }

    public void UpdateList()
    {
      UpdateList(null);
    }

    public void UpdateList(int[] selectedIndices)
    {
      if(null!=View)
        View.InitializePathList(_tempdoc,selectedIndices);
    }

    public void EhView_MoveUp(int[] selIndices)
    {

      if(selIndices.Length==0 || selIndices[0]==0)
        return;

      // Presumption: the first selected index is greater than 0
      for(int i=0;i<selIndices.Length;i++)
      {
        int idx = selIndices[i];
        FilterItem temp = _tempdoc[idx];
        _tempdoc[idx] = _tempdoc[idx-1];
        _tempdoc[idx-1]   = temp;

        selIndices[i]--; // for new list selection
      }

      UpdateList(selIndices);

    }

    public void EhView_MoveDown(int[] selIndices)
    {
      if(selIndices.Length==0 || selIndices[selIndices.Length-1]==(_tempdoc.Count-1))
        return;

      // Presumption: the first selected index is greater than 0
      for(int i=selIndices.Length-1;i>=0;i--)
      {
        int idx = selIndices[i];
        FilterItem temp = _tempdoc[idx];
        _tempdoc[idx] = _tempdoc[idx+1];
        _tempdoc[idx+1]   = temp;

        selIndices[i]++; // for new list selection
      }

      UpdateList(selIndices);
    }

    public void EhView_AddPath(string path, bool bIncludePath)
    {
      this._tempdoc.Add(new FilterItem(bIncludePath?FilterAction.Include:FilterAction.Exclude,path));
      this.UpdateList();
    }

    public void EhView_DefaultActionChanged(int selindex)
    {
      switch(selindex)
      {
        case 0:
          _tempdoc.DefaultAction = FilterAction.Include;
          break;
        case 1:
          _tempdoc.DefaultAction = FilterAction.Exclude;
          break;
        case 2:
          _tempdoc.DefaultAction = FilterAction.Ignore;
          break;
      }
    }

    public void EhView_ChangeAction(int[] selIndices)
    {
      for(int i=0;i<selIndices.Length;i++)
      {
        int idx = selIndices[i];
        if(_tempdoc[idx].Action == FilterAction.Include)
          _tempdoc[idx].Action = FilterAction.Exclude;
        else if(_tempdoc[idx].Action == FilterAction.Exclude)
          _tempdoc[idx].Action = FilterAction.Include;
      }

      this.UpdateList(selIndices);
    }


    public void EhView_DeletePath(int[] selIndices)
    {
      for(int i=selIndices.Length-1;i>=0;i--)
      {
        int idx = selIndices[i];
        _tempdoc.RemoveAt(idx);
      }

      this.UpdateList();

    }
    #region IApplyController Members

    public bool Apply()
    {
      _doc.CopyFrom(_tempdoc);      
      return true; // Success
    }

    #endregion
  }
}
