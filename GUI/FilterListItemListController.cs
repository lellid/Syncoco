#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco: offline file synchronization
//    Copyright (C) 2004-2099 Dr. Dirk Lellinger
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

namespace Syncoco.GUI
{
  using Filter;

  /// <summary>
  /// Summary description for FilterListItemListController.
  /// </summary>
  public class FilterListItemListController : IApplyController
  {
    private PathFilter _doc;
    private PathFilter _tempdoc;
    private FilterListItemListControl _view;
    private string _myRootPath, _foreignRootPath;
    private string _folderBrowserPath;

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
      if (View != null)
      {
        View.UpdateList(this._tempdoc.FilterList);
      }
    }

    public void UpdateList(int[] selIndices)
    {
      UpdateList();
      if (View != null)
      {
        View.SelectListItems(selIndices);
      }
    }
    public void UpdatePathInformation()
    {
      if (View != null)
      {
        View.UpdatePathInformation(_myRootPath, _foreignRootPath);
      }
    }

    public void EhView_ShowDefaultFilter()
    {
      string title = string.Format("Default filter (root: {0})", this._myRootPath);

      FilterListController controller = new FilterListController(_tempdoc.DefaultList);
      FilterListControl control = new FilterListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control), controller,
        title,
        true);
      dlgctrl.ShowDialog(View.ParentForm);

      if (View != null)
      {
        View.UpdateDefaultFilterAction(_tempdoc.DefaultList.DefaultAction);
      }
    }

    public void EhView_AddNewPath()
    {
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      dlg.SelectedPath = this._folderBrowserPath;
      if (System.Windows.Forms.DialogResult.OK != dlg.ShowDialog(Current.MainForm))
      {
        return;
      }

      if (!dlg.SelectedPath.StartsWith(_myRootPath))
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm, string.Format("The path you select must be a subdirectory of the base path <<{0}>>", _myRootPath));
        EhView_AddNewPath();
      }
      else
      {
        this._folderBrowserPath = dlg.SelectedPath;

        string subpath = dlg.SelectedPath.Substring(_myRootPath.Length);
        this._tempdoc.FilterList.Add(new FilterListItem(PathUtil.NormalizeRelpath(subpath)));

        this.UpdateList();
      }
    }


    public void EhView_EditPath(int index)
    {
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      dlg.SelectedPath = this._folderBrowserPath;
      if (System.Windows.Forms.DialogResult.OK != dlg.ShowDialog(Current.MainForm))
      {
        return;
      }

      if (!dlg.SelectedPath.StartsWith(_myRootPath))
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm, string.Format("The path you select must be a subdirectory of the base path <<{0}>>", _myRootPath));
        EhView_EditPath(index);
      }
      else
      {
        this._folderBrowserPath = dlg.SelectedPath;

        string subpath = dlg.SelectedPath.Substring(_myRootPath.Length);
        this._tempdoc.FilterList[index].Path = PathUtil.NormalizeRelpath(subpath);

        this.UpdateList();
      }

    }


    public void EhView_DeletePath(int[] indices)
    {
      System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(this.View,
        string.Format("Are you sure to delete the selected {0} path(s)?", indices.Length),
        "Confirmation",
        System.Windows.Forms.MessageBoxButtons.YesNo,
        System.Windows.Forms.MessageBoxIcon.Question,
        System.Windows.Forms.MessageBoxDefaultButton.Button2);

      if (System.Windows.Forms.DialogResult.Yes == result)
      {
        for (int i = indices.Length - 1; i >= 0; i--)
        {
          int idx = indices[i];
          _tempdoc.FilterList.RemoveAt(idx);
        }

        UpdateList();
      }
    }


    public void EhView_EditFilter(int idx)
    {
      string title = string.Format("Filter for subpath: {0} (root: {1})", _tempdoc.FilterList[idx].Path, this._myRootPath);
      FilterListController controller = new FilterListController(_tempdoc.FilterList[idx].Filter);
      FilterListControl control = new FilterListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control), controller,
        title,
        true);
      dlgctrl.ShowDialog(View.ParentForm);

      UpdateList();
    }

    public void EhView_MoveUp(int[] selIndices)
    {
      if (selIndices.Length == 0 || selIndices[0] == 0)
      {
        return;
      }

      // Presumption: the first selected index is greater than 0
      for (int i = 0; i < selIndices.Length; i++)
      {
        int idx = selIndices[i];
        _tempdoc.FilterList.ExchangeItemPositions(idx, idx - 1);
        selIndices[i]--; // for new list selection
      }

      UpdateList(selIndices);
    }

    public void EhView_MoveDown(int[] selIndices)
    {
      if (selIndices.Length == 0 || selIndices[selIndices.Length - 1] == (_tempdoc.FilterList.Count - 1))
      {
        return;
      }

      // Presumption: the first selected index is greater than 0
      for (int i = selIndices.Length - 1; i >= 0; i--)
      {
        int idx = selIndices[i];
        _tempdoc.FilterList.ExchangeItemPositions(idx, idx + 1);
        selIndices[i]++; // for new list selection
      }

      UpdateList(selIndices);
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
