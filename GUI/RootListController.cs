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
using System.Windows.Forms;

namespace Syncoco
{
  /// <summary>
  /// Summary description for RootListController.
  /// </summary>
  public class RootListController
  {
    MainDocument _document;
    RootList _view;

    public RootList View { get { return _view; }}

    public RootListController(MainDocument document)
    {
      _document = document;

      _document.Align();


      _view = new RootList();
      _view._controller = this;

      _view.InitializeListCaptions(_document.MyRootComputerName,_document.ForeignRootComputerName);
      
      UpdateList();
    
    }

    public void UpdateList()
    {
      const string EmptyRoot = "????????";

      string[] myRoots = new string[_document.Count];
      for(int i=0;i<_document.Count;i++)
        myRoots[i]= _document.MyRoot(i).IsValid ?  _document.MyRoot(i).FilePath : EmptyRoot;


      string[] foRoots = new string[_document.Count];
      for(int i=0;i<_document.Count;i++)
        foRoots[i]=_document.ForeignRoot(i).IsValid ? _document.ForeignRoot(i).FilePath : EmptyRoot;


      _view.InitializeList(myRoots,foRoots);

    }

    public void UpdateList(int[] selectedIndizes)
    {
      UpdateList();
      _view.SelectListItems(selectedIndizes);
    }

    
   

    public void EhView_AddPath()
    {
     
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        _document.AddRoot(PathUtil.NormalizeAbspath(dlg.SelectedPath));
        UpdateList();
      }
    }


    public void EhView_EditPath(int item)
    {
      if(_document.MyRoot(item).IsValid)
      {
        DialogResult dlgResult=MessageBox.Show(Current.MainForm,
          "Changing the path of an already existing root can cause serious malfunction of the synchronization.\r\n"+
          "For instance, all differences between the old path and the new path are treated as changes.\r\n"+
          "You should only do this if you previously moved the old directory root manually to a new "+
          "location, but the directory structure was kept.\r\n"+
          "You are absolutely sure to change the root path?",
          "Confirmation",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button2);

        if(dlgResult==DialogResult.Yes)
        {
          System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
          if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
          {
            _document.SetBasePath(item,PathUtil.NormalizeAbspath(dlg.SelectedPath));
            UpdateList();
          }
        }
      }
      else
      {
        System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
        if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
        {
          _document.SetBasePath(item,PathUtil.NormalizeAbspath(dlg.SelectedPath));
          UpdateList();
        }
      }
    }


    public void EhView_DeletePath(int item)
    {
      DialogResult dlgResult=MessageBox.Show(Current.MainForm,
        "Are you sure to delete the root pair?",
        "Confirmation",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2);
      if(DialogResult.Yes==dlgResult)
      {
        _document.DeleteRoot(item);
        UpdateList();
      }
    }
      
    public void EhView_EditFilterList(int item)
    {
      string title = string.Format("Filterlist (root here: {0})",this._document.RootPair(item).MyRoot.FilePath);
      FilterListItemListController controller = new FilterListItemListController(
        this._document.RootPair(item).PathFilter,
        this._document.RootPair(item).MyRoot.FilePath,
        this._document.RootPair(item).ForeignRoot.FilePath);
      FilterListItemListControl control = new FilterListItemListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control),controller,
        title,true);
      dlgctrl.ShowDialog(this.View.ParentForm);

    }

    public void EhView_MoveUp(int[] selIndices)
    {
      
      if(selIndices.Length==0 || selIndices[0]==0)
        return;

      // Presumption: the first selected index is greater than 0
      for(int i=0;i<selIndices.Length;i++)
      {
        int idx = selIndices[i];
        _document.ExchangeRootPairPositions(idx,idx-1);
        selIndices[i]--; // for new list selection
      }

      UpdateList(selIndices);
    }
   
    public void EhView_MoveDown(int[] selIndices)
    {
      if(selIndices.Length==0 || selIndices[selIndices.Length-1]==(_document.RootCount-1))
        return;

      // Presumption: the first selected index is greater than 0
      for(int i=selIndices.Length-1;i>=0;i--)
      {
        int idx = selIndices[i];
        _document.ExchangeRootPairPositions(idx,idx+1);
        selIndices[i]++; // for new list selection
      }

      UpdateList(selIndices);
    }
   
  }
}
