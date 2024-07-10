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

using System;
using System.Windows.Forms;

namespace Syncoco.GUI
{
  /// <summary>
  /// Summary description for RootList.
  /// </summary>
  public class RootList : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.ListView lvRootList;
    private System.Windows.Forms.ColumnHeader chComputer1;
    private System.Windows.Forms.ColumnHeader chComputer2;
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.ContextMenu listContextMenu;
    private System.Windows.Forms.Button btMoveDown;
    private System.Windows.Forms.Button btAddPath;
    private System.Windows.Forms.Button btEditPath;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button btMoveUp;
    private System.Windows.Forms.Button btDeletePath;
    private System.Windows.Forms.Button btEditFilterList;
    public RootListController _controller;

    public RootList()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call

    }

    public void InitializeListCaptions(string name1, string name2)
    {
      lvRootList.Columns[0].Text = name1;
      lvRootList.Columns[1].Text = name2;
    }

    public void InitializeList(string[] list0, string[] list1)
    {
      lvRootList.Items.Clear();
      System.Diagnostics.Debug.Assert(list0.Length == list1.Length);
      int len = Math.Max(list0.Length, list1.Length);

      for (int i = 0; i < len; i++)
      {
        ListViewItem item = new ListViewItem(new string[] { list0[i], list1[i] });
        lvRootList.Items.Add(item);
      }
    }

    public void SelectListItems(int[] selectedIndices)
    {
      for (int i = 0; i < selectedIndices.Length; i++)
      {
        lvRootList.Items[selectedIndices[i]].Selected = true;
      }
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RootList));
      this.lvRootList = new System.Windows.Forms.ListView();
      this.chComputer1 = new System.Windows.Forms.ColumnHeader();
      this.chComputer2 = new System.Windows.Forms.ColumnHeader();
      this.listContextMenu = new System.Windows.Forms.ContextMenu();
      this.btMoveDown = new System.Windows.Forms.Button();
      this.btEditFilterList = new System.Windows.Forms.Button();
      this.btAddPath = new System.Windows.Forms.Button();
      this.btEditPath = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.btMoveUp = new System.Windows.Forms.Button();
      this.btDeletePath = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lvRootList
      // 
      this.lvRootList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lvRootList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                 this.chComputer1,
                                                                                 this.chComputer2});
      this.lvRootList.ContextMenu = this.listContextMenu;
      this.lvRootList.Location = new System.Drawing.Point(8, 8);
      this.lvRootList.Name = "lvRootList";
      this.lvRootList.Size = new System.Drawing.Size(432, 360);
      this.lvRootList.TabIndex = 0;
      this.lvRootList.View = System.Windows.Forms.View.Details;
      this.lvRootList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvRootList_MouseDown);
      // 
      // chComputer1
      // 
      this.chComputer1.Text = "Computer1";
      this.chComputer1.Width = 160;
      // 
      // chComputer2
      // 
      this.chComputer2.Text = "Computer2";
      this.chComputer2.Width = 184;
      // 
      // listContextMenu
      // 
      this.listContextMenu.Popup += new System.EventHandler(this.listContextMenu_Popup);
      // 
      // btMoveDown
      // 
      this.btMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btMoveDown.Image")));
      this.btMoveDown.Location = new System.Drawing.Point(448, 336);
      this.btMoveDown.Name = "btMoveDown";
      this.btMoveDown.Size = new System.Drawing.Size(32, 32);
      this.btMoveDown.TabIndex = 4;
      this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
      // 
      // btEditFilterList
      // 
      this.btEditFilterList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btEditFilterList.Image = ((System.Drawing.Image)(resources.GetObject("btEditFilterList.Image")));
      this.btEditFilterList.Location = new System.Drawing.Point(448, 160);
      this.btEditFilterList.Name = "btEditFilterList";
      this.btEditFilterList.Size = new System.Drawing.Size(32, 32);
      this.btEditFilterList.TabIndex = 3;
      this.btEditFilterList.Click += new System.EventHandler(this.btEditFilterList_Click);
      // 
      // btAddPath
      // 
      this.btAddPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btAddPath.Image = ((System.Drawing.Image)(resources.GetObject("btAddPath.Image")));
      this.btAddPath.Location = new System.Drawing.Point(448, 8);
      this.btAddPath.Name = "btAddPath";
      this.btAddPath.Size = new System.Drawing.Size(32, 32);
      this.btAddPath.TabIndex = 5;
      this.btAddPath.Click += new System.EventHandler(this.btAddPath_Click);
      // 
      // btEditPath
      // 
      this.btEditPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btEditPath.Image = ((System.Drawing.Image)(resources.GetObject("btEditPath.Image")));
      this.btEditPath.Location = new System.Drawing.Point(448, 48);
      this.btEditPath.Name = "btEditPath";
      this.btEditPath.Size = new System.Drawing.Size(32, 32);
      this.btEditPath.TabIndex = 6;
      this.btEditPath.Click += new System.EventHandler(this.btEditPath_Click);
      // 
      // button3
      // 
      this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(232, 176);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(32, 32);
      this.button3.TabIndex = 6;
      // 
      // btMoveUp
      // 
      this.btMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btMoveUp.Image")));
      this.btMoveUp.Location = new System.Drawing.Point(448, 296);
      this.btMoveUp.Name = "btMoveUp";
      this.btMoveUp.Size = new System.Drawing.Size(32, 32);
      this.btMoveUp.TabIndex = 3;
      this.btMoveUp.Click += new System.EventHandler(this.bt_MoveUp_Click);
      // 
      // btDeletePath
      // 
      this.btDeletePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btDeletePath.Image = ((System.Drawing.Image)(resources.GetObject("btDeletePath.Image")));
      this.btDeletePath.Location = new System.Drawing.Point(448, 88);
      this.btDeletePath.Name = "btDeletePath";
      this.btDeletePath.Size = new System.Drawing.Size(32, 32);
      this.btDeletePath.TabIndex = 7;
      this.btDeletePath.Click += new System.EventHandler(this.btDeletePath_Click);
      // 
      // RootList
      // 
      this.Controls.Add(this.btDeletePath);
      this.Controls.Add(this.btEditPath);
      this.Controls.Add(this.btAddPath);
      this.Controls.Add(this.btMoveDown);
      this.Controls.Add(this.btEditFilterList);
      this.Controls.Add(this.lvRootList);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.btMoveUp);
      this.Name = "RootList";
      this.Size = new System.Drawing.Size(488, 376);
      this.ResumeLayout(false);

    }
    #endregion



    private ListViewItem _lastMouseDownItem;
    private void lvRootList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      _lastMouseDownItem = this.lvRootList.GetItemAt(e.X, e.Y);
    }

    private void btAddPath_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        _controller.EhView_AddPath();
      }
    }


    private void btEditPath_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        if (this.lvRootList.SelectedIndices.Count == 1)
        {
          _controller.EhView_EditPath(this.lvRootList.SelectedIndices[0]);
        }
      }
    }

    private void btDeletePath_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        if (this.lvRootList.SelectedIndices.Count == 1)
        {
          _controller.EhView_DeletePath(this.lvRootList.SelectedIndices[0]);
        }
      }
    }

    private void btEditFilterList_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        if (this.lvRootList.SelectedIndices.Count == 1)
        {
          _controller.EhView_EditFilterList(this.lvRootList.SelectedIndices[0]);
        }
      }
    }

    private int[] GetIndexArray(System.Windows.Forms.ListView.SelectedIndexCollection coll)
    {
      int[] res = new int[coll.Count];
      coll.CopyTo(res, 0);
      return res;
    }

    private void bt_MoveUp_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        if (this.lvRootList.SelectedIndices.Count >= 1)
        {
          _controller.EhView_MoveUp(GetIndexArray(this.lvRootList.SelectedIndices));
          lvRootList.Focus();
        }
      }

    }

    private void btMoveDown_Click(object sender, System.EventArgs e)
    {
      if (null != _controller)
      {
        if (this.lvRootList.SelectedIndices.Count >= 1)
        {
          _controller.EhView_MoveDown(GetIndexArray(this.lvRootList.SelectedIndices));
          lvRootList.Focus();
        }
      }

    }
    private void listContextMenu_Popup(object sender, System.EventArgs e)
    {
      listContextMenu.MenuItems.Clear();

      if (this.lvRootList.SelectedIndices.Count == 0)
      {
        listContextMenu.MenuItems.Add(new MenuItem("Add new Path", new EventHandler(btAddPath_Click)));
      }
      if (this.lvRootList.SelectedIndices.Count == 1)
      {
        listContextMenu.MenuItems.Add(new MenuItem("Edit filter", new EventHandler(btEditFilterList_Click)));
        listContextMenu.MenuItems.Add(new MenuItem("-"));
        listContextMenu.MenuItems.Add(new MenuItem("Edit path", new EventHandler(btEditPath_Click)));
        listContextMenu.MenuItems.Add(new MenuItem("Delete root pair", new EventHandler(btDeletePath_Click)));
      }

      if (this.lvRootList.SelectedIndices.Count >= 1)
      {
        listContextMenu.MenuItems.Add(new MenuItem("-"));
        listContextMenu.MenuItems.Add(new MenuItem("Move up", new EventHandler(bt_MoveUp_Click)));
        listContextMenu.MenuItems.Add(new MenuItem("Move down", new EventHandler(btMoveDown_Click)));

      }

    }





  }
}
