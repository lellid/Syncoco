using System;

namespace SyncTwoCo
{
  using Filter;

  /// <summary>
  /// Summary description for FilterListItemListController.
  /// </summary>
  public class FilterListItemListController : IApplyController
  {
    PathFilter _doc;
    PathFilter _tempdoc;
    FilterListItemListControl _view;
    string _myRootPath, _foreignRootPath;
    string _folderBrowserPath;

    public FilterListItemListControl View
    {
      get
      {
        return _view;
      }
      set
      {
        _view = value;
        _view.Controller = this;
        UpdatePathInformation();
        UpdateList();
        View.UpdateDefaultFilterAction(_tempdoc.DefaultList.DefaultAction);
      }
    }


    public FilterListItemListController(PathFilter doc, string myRootPath, string foreignRootPath)
    {
      _doc = doc;
      _myRootPath = myRootPath;
      _foreignRootPath = foreignRootPath;
      _tempdoc = new PathFilter(_doc);

      _folderBrowserPath = _myRootPath;
    }


    public void UpdateList()
    {
      if(View!=null)
        View.UpdateList(this._tempdoc.FilterList);
    }

    public void UpdatePathInformation()
    {
      if(View!=null)
        View.UpdatePathInformation(_myRootPath,_foreignRootPath);
    }

    public void EhView_ShowDefaultFilter()
    {
      FilterListController controller = new FilterListController(_tempdoc.DefaultList);
      FilterListControl    control = new FilterListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control),controller);
      dlgctrl.ShowDialog(View.ParentForm);

      if(View!=null)
        View.UpdateDefaultFilterAction(_tempdoc.DefaultList.DefaultAction);
    }

    public void EhView_AddNewPath()
    {
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      dlg.SelectedPath = this._folderBrowserPath;
      if(System.Windows.Forms.DialogResult.OK!=dlg.ShowDialog(Current.MainForm))
        return;

      if(!dlg.SelectedPath.StartsWith(_myRootPath))
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm,string.Format("The path you select must be a subdirectory of the base path <<{0}>>",_myRootPath));
        EhView_AddNewPath();
      }
      else
      {
        this._folderBrowserPath = dlg.SelectedPath;

        string subpath = dlg.SelectedPath.Substring(_myRootPath.Length);
        this._tempdoc.FilterList.Add(new FilterListItem(subpath));
        
        this.UpdateList();
      }
    }

    public void EhView_DeletePath(int[] indices)
    {
      System.Windows.Forms.DialogResult result=System.Windows.Forms.MessageBox.Show(this.View,
        string.Format("Are you sure to delete the selected {0} path(s)?",indices.Length),
        "Confirmation",
        System.Windows.Forms.MessageBoxButtons.YesNo,
        System.Windows.Forms.MessageBoxIcon.Question);

      if(System.Windows.Forms.DialogResult.Yes==result)
      {
        for(int i=indices.Length-1;i>=0;i--)
        {
          int idx = indices[i];
          _tempdoc.FilterList.RemoveAt(idx);
        }

        UpdateList();
      }
    }


    public void EhView_EditFilter(int idx)
    {
      FilterListController controller = new FilterListController(_tempdoc.FilterList[idx].Filter);
      FilterListControl    control = new FilterListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control),controller);
      dlgctrl.ShowDialog(View.ParentForm);

      UpdateList();
    }

    #region IApplyController Members

    public bool Apply()
    {
      _doc.CopyFrom(_tempdoc);
      return true;
    }

    #endregion
  }
}
