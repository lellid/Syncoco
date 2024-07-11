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
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.ContextMenuStrip listContextMenu;
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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RootList));
      lvRootList = new ListView();
      chComputer1 = new ColumnHeader();
      chComputer2 = new ColumnHeader();
      listContextMenu = new ContextMenuStrip(components);
      btMoveDown = new Button();
      btEditFilterList = new Button();
      btAddPath = new Button();
      btEditPath = new Button();
      button3 = new Button();
      btMoveUp = new Button();
      btDeletePath = new Button();
      SuspendLayout();
      // 
      // lvRootList
      // 
      lvRootList.ContextMenuStrip = listContextMenu;
      lvRootList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lvRootList.Columns.AddRange(new ColumnHeader[] { chComputer1, chComputer2 });
      lvRootList.Location = new System.Drawing.Point(8, 8);
      lvRootList.Name = "lvRootList";
      lvRootList.Size = new System.Drawing.Size(432, 360);
      lvRootList.TabIndex = 0;
      lvRootList.UseCompatibleStateImageBehavior = false;
      lvRootList.View = View.Details;
      lvRootList.MouseDown += lvRootList_MouseDown;
      // 
      // chComputer1
      // 
      chComputer1.Text = "Computer1";
      chComputer1.Width = 160;
      // 
      // chComputer2
      // 
      chComputer2.Text = "Computer2";
      chComputer2.Width = 184;
      // 
      // listContextMenu
      // 
      listContextMenu.Name = "listContextMenu";
      listContextMenu.Size = new System.Drawing.Size(181, 26);
      listContextMenu.Opening += EhContextMenuOpening;
      // 
      // btMoveDown
      // 
      btMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      btMoveDown.Image = (System.Drawing.Image)resources.GetObject("btMoveDown.Image");
      btMoveDown.Location = new System.Drawing.Point(448, 336);
      btMoveDown.Name = "btMoveDown";
      btMoveDown.Size = new System.Drawing.Size(32, 32);
      btMoveDown.TabIndex = 4;
      btMoveDown.Click += btMoveDown_Click;
      // 
      // btEditFilterList
      // 
      btEditFilterList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btEditFilterList.Image = (System.Drawing.Image)resources.GetObject("btEditFilterList.Image");
      btEditFilterList.Location = new System.Drawing.Point(448, 160);
      btEditFilterList.Name = "btEditFilterList";
      btEditFilterList.Size = new System.Drawing.Size(32, 32);
      btEditFilterList.TabIndex = 3;
      btEditFilterList.Click += btEditFilterList_Click;
      // 
      // btAddPath
      // 
      btAddPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btAddPath.Image = (System.Drawing.Image)resources.GetObject("btAddPath.Image");
      btAddPath.Location = new System.Drawing.Point(448, 8);
      btAddPath.Name = "btAddPath";
      btAddPath.Size = new System.Drawing.Size(32, 32);
      btAddPath.TabIndex = 5;
      btAddPath.Click += btAddPath_Click;
      // 
      // btEditPath
      // 
      btEditPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btEditPath.Image = (System.Drawing.Image)resources.GetObject("btEditPath.Image");
      btEditPath.Location = new System.Drawing.Point(448, 48);
      btEditPath.Name = "btEditPath";
      btEditPath.Size = new System.Drawing.Size(32, 32);
      btEditPath.TabIndex = 6;
      btEditPath.Click += btEditPath_Click;
      // 
      // button3
      // 
      button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
      button3.Location = new System.Drawing.Point(232, 176);
      button3.Name = "button3";
      button3.Size = new System.Drawing.Size(32, 32);
      button3.TabIndex = 6;
      // 
      // btMoveUp
      // 
      btMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      btMoveUp.Image = (System.Drawing.Image)resources.GetObject("btMoveUp.Image");
      btMoveUp.Location = new System.Drawing.Point(448, 296);
      btMoveUp.Name = "btMoveUp";
      btMoveUp.Size = new System.Drawing.Size(32, 32);
      btMoveUp.TabIndex = 3;
      btMoveUp.Click += bt_MoveUp_Click;
      // 
      // btDeletePath
      // 
      btDeletePath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btDeletePath.Image = (System.Drawing.Image)resources.GetObject("btDeletePath.Image");
      btDeletePath.Location = new System.Drawing.Point(448, 88);
      btDeletePath.Name = "btDeletePath";
      btDeletePath.Size = new System.Drawing.Size(32, 32);
      btDeletePath.TabIndex = 7;
      btDeletePath.Click += btDeletePath_Click;
      // 
      // RootList
      // 
      Controls.Add(btDeletePath);
      Controls.Add(btEditPath);
      Controls.Add(btAddPath);
      Controls.Add(btMoveDown);
      Controls.Add(btEditFilterList);
      Controls.Add(lvRootList);
      Controls.Add(button3);
      Controls.Add(btMoveUp);
      Name = "RootList";
      Size = new System.Drawing.Size(488, 376);
      ResumeLayout(false);
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

    private void EhContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      listContextMenu.Items.Clear();

      if (this.lvRootList.SelectedIndices.Count == 0)
      {
        listContextMenu.Items.Add(new ToolStripMenuItem("Add new Path", null, new EventHandler(btAddPath_Click)));
      }
      if (this.lvRootList.SelectedIndices.Count == 1)
      {
        listContextMenu.Items.Add(new ToolStripMenuItem("Edit filter", null, new EventHandler(btEditFilterList_Click)));
        listContextMenu.Items.Add(new ToolStripSeparator());
        listContextMenu.Items.Add(new ToolStripMenuItem("Edit path", null, new EventHandler(btEditPath_Click)));
        listContextMenu.Items.Add(new ToolStripMenuItem("Delete root pair", null, new EventHandler(btDeletePath_Click)));
      }

      if (this.lvRootList.SelectedIndices.Count >= 1)
      {
        listContextMenu.Items.Add(new ToolStripSeparator());
        listContextMenu.Items.Add(new ToolStripMenuItem("Move up", null, new EventHandler(bt_MoveUp_Click)));
        listContextMenu.Items.Add(new ToolStripMenuItem("Move down", null, new EventHandler(btMoveDown_Click)));

      }
    }
  }
}
