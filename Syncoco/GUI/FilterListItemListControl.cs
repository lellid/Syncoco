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
  using Filter;

  /// <summary>
  /// Summary description for FilterListItemListControl.
  /// </summary>
  public class FilterListItemListControl : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox edMyRoot;
    private System.Windows.Forms.TextBox edForeignRoot;
    private System.Windows.Forms.ListView lvFilterListItemList;
    private System.Windows.Forms.Label lblDefaultFilterAction;
    private System.Windows.Forms.Button btDefaultFilter;
    private System.Windows.Forms.ContextMenuStrip listContextMenu;
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.ColumnHeader chSubPath;
    private System.Windows.Forms.ColumnHeader chDefaultAction;
    private System.Windows.Forms.TextBox edDefaultFilterAction;
    private System.Windows.Forms.Button btAddPath;
    private System.Windows.Forms.Button btEditPath;
    private System.Windows.Forms.Button btDeletePath;
    private System.Windows.Forms.Button btEditFilterList;
    private System.Windows.Forms.Button btMoveUp;
    private System.Windows.Forms.Button btMoveDown;

    private FilterListItemListController _controller;

    public FilterListItemListController Controller
    {
      get
      {
        return _controller;
      }
      set
      {
        _controller = value;
      }
    }

    public FilterListItemListControl()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FilterListItemListControl));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.edMyRoot = new System.Windows.Forms.TextBox();
      this.edForeignRoot = new System.Windows.Forms.TextBox();
      this.lvFilterListItemList = new System.Windows.Forms.ListView();
      this.chSubPath = new System.Windows.Forms.ColumnHeader();
      this.chDefaultAction = new System.Windows.Forms.ColumnHeader();
      this.listContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
      this.lblDefaultFilterAction = new System.Windows.Forms.Label();
      this.btDefaultFilter = new System.Windows.Forms.Button();
      this.edDefaultFilterAction = new System.Windows.Forms.TextBox();
      this.btAddPath = new System.Windows.Forms.Button();
      this.btEditPath = new System.Windows.Forms.Button();
      this.btDeletePath = new System.Windows.Forms.Button();
      this.btEditFilterList = new System.Windows.Forms.Button();
      this.btMoveUp = new System.Windows.Forms.Button();
      this.btMoveDown = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(32, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Here:";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(8, 32);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "There:";
      // 
      // edMyRoot
      // 
      this.edMyRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.edMyRoot.Location = new System.Drawing.Point(56, 8);
      this.edMyRoot.Name = "edMyRoot";
      this.edMyRoot.ReadOnly = true;
      this.edMyRoot.Size = new System.Drawing.Size(328, 20);
      this.edMyRoot.TabIndex = 2;
      this.edMyRoot.Text = "textBox1";
      // 
      // edForeignRoot
      // 
      this.edForeignRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.edForeignRoot.Location = new System.Drawing.Point(56, 32);
      this.edForeignRoot.Name = "edForeignRoot";
      this.edForeignRoot.ReadOnly = true;
      this.edForeignRoot.Size = new System.Drawing.Size(328, 20);
      this.edForeignRoot.TabIndex = 3;
      this.edForeignRoot.Text = "textBox2";
      // 
      // lvFilterListItemList
      // 
      this.lvFilterListItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lvFilterListItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                           this.chSubPath,
                                                                                           this.chDefaultAction});
      this.lvFilterListItemList.ContextMenuStrip = this.listContextMenu;
      this.lvFilterListItemList.Location = new System.Drawing.Point(8, 72);
      this.lvFilterListItemList.Name = "lvFilterListItemList";
      this.lvFilterListItemList.Size = new System.Drawing.Size(336, 296);
      this.lvFilterListItemList.TabIndex = 4;
      this.lvFilterListItemList.View = System.Windows.Forms.View.Details;
      // 
      // chSubPath
      // 
      this.chSubPath.Text = "SubPath";
      this.chSubPath.Width = 244;
      // 
      // chDefaultAction
      // 
      this.chDefaultAction.Text = "Default Action";
      this.chDefaultAction.Width = 88;
      // 
      // listContextMenu
      // 
      this.listContextMenu.Opening += this.listContextMenu_Popup;
      // 
      // lblDefaultFilterAction
      // 
      this.lblDefaultFilterAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblDefaultFilterAction.Location = new System.Drawing.Point(8, 376);
      this.lblDefaultFilterAction.Name = "lblDefaultFilterAction";
      this.lblDefaultFilterAction.Size = new System.Drawing.Size(104, 20);
      this.lblDefaultFilterAction.TabIndex = 5;
      this.lblDefaultFilterAction.Text = "default filter action:";
      this.lblDefaultFilterAction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btDefaultFilter
      // 
      this.btDefaultFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btDefaultFilter.Image = ((System.Drawing.Image)(resources.GetObject("btDefaultFilter.Image")));
      this.btDefaultFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btDefaultFilter.Location = new System.Drawing.Point(240, 376);
      this.btDefaultFilter.Name = "btDefaultFilter";
      this.btDefaultFilter.Size = new System.Drawing.Size(144, 23);
      this.btDefaultFilter.TabIndex = 6;
      this.btDefaultFilter.Text = "configure default filter";
      this.btDefaultFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btDefaultFilter.Click += new System.EventHandler(this.btDefaultFilter_Click);
      // 
      // edDefaultFilterAction
      // 
      this.edDefaultFilterAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.edDefaultFilterAction.Location = new System.Drawing.Point(112, 376);
      this.edDefaultFilterAction.Name = "edDefaultFilterAction";
      this.edDefaultFilterAction.ReadOnly = true;
      this.edDefaultFilterAction.Size = new System.Drawing.Size(120, 20);
      this.edDefaultFilterAction.TabIndex = 7;
      this.edDefaultFilterAction.Text = "";
      // 
      // btAddPath
      // 
      this.btAddPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btAddPath.Image = ((System.Drawing.Image)(resources.GetObject("btAddPath.Image")));
      this.btAddPath.Location = new System.Drawing.Point(352, 72);
      this.btAddPath.Name = "btAddPath";
      this.btAddPath.Size = new System.Drawing.Size(32, 32);
      this.btAddPath.TabIndex = 8;
      this.btAddPath.Click += new System.EventHandler(this.EhAddNewPath);
      // 
      // btEditPath
      // 
      this.btEditPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btEditPath.Image = ((System.Drawing.Image)(resources.GetObject("btEditPath.Image")));
      this.btEditPath.Location = new System.Drawing.Point(352, 112);
      this.btEditPath.Name = "btEditPath";
      this.btEditPath.Size = new System.Drawing.Size(32, 32);
      this.btEditPath.TabIndex = 9;
      this.btEditPath.Click += new System.EventHandler(this.EhEditPath);
      // 
      // btDeletePath
      // 
      this.btDeletePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btDeletePath.Image = ((System.Drawing.Image)(resources.GetObject("btDeletePath.Image")));
      this.btDeletePath.Location = new System.Drawing.Point(352, 152);
      this.btDeletePath.Name = "btDeletePath";
      this.btDeletePath.Size = new System.Drawing.Size(32, 32);
      this.btDeletePath.TabIndex = 10;
      this.btDeletePath.Click += new System.EventHandler(this.EhDeletePath);
      // 
      // btEditFilterList
      // 
      this.btEditFilterList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btEditFilterList.Image = ((System.Drawing.Image)(resources.GetObject("btEditFilterList.Image")));
      this.btEditFilterList.Location = new System.Drawing.Point(352, 192);
      this.btEditFilterList.Name = "btEditFilterList";
      this.btEditFilterList.Size = new System.Drawing.Size(32, 32);
      this.btEditFilterList.TabIndex = 11;
      this.btEditFilterList.Click += new System.EventHandler(this.EhEditFilter);
      // 
      // btMoveUp
      // 
      this.btMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btMoveUp.Image")));
      this.btMoveUp.Location = new System.Drawing.Point(352, 296);
      this.btMoveUp.Name = "btMoveUp";
      this.btMoveUp.Size = new System.Drawing.Size(32, 32);
      this.btMoveUp.TabIndex = 12;
      this.btMoveUp.Click += new System.EventHandler(this.EhMoveUp);
      // 
      // btMoveDown
      // 
      this.btMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btMoveDown.Image")));
      this.btMoveDown.Location = new System.Drawing.Point(352, 336);
      this.btMoveDown.Name = "btMoveDown";
      this.btMoveDown.Size = new System.Drawing.Size(32, 32);
      this.btMoveDown.TabIndex = 13;
      this.btMoveDown.Click += new System.EventHandler(this.EhMoveDown);
      // 
      // FilterListItemListControl
      // 
      this.Controls.Add(this.btMoveDown);
      this.Controls.Add(this.btMoveUp);
      this.Controls.Add(this.btEditFilterList);
      this.Controls.Add(this.btDeletePath);
      this.Controls.Add(this.btEditPath);
      this.Controls.Add(this.btAddPath);
      this.Controls.Add(this.edDefaultFilterAction);
      this.Controls.Add(this.btDefaultFilter);
      this.Controls.Add(this.lblDefaultFilterAction);
      this.Controls.Add(this.lvFilterListItemList);
      this.Controls.Add(this.edForeignRoot);
      this.Controls.Add(this.edMyRoot);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "FilterListItemListControl";
      this.Size = new System.Drawing.Size(392, 408);
      this.ResumeLayout(false);

    }
    #endregion

    private void listContextMenu_Popup(object sender, System.ComponentModel.CancelEventArgs e)
    {
      listContextMenu.Items.Clear();

      if (this.lvFilterListItemList.SelectedIndices.Count == 0)
      {
        listContextMenu.Items.Add(new ToolStripMenuItem("Add new Path", null, new EventHandler(EhAddNewPath)));
      }
      if (this.lvFilterListItemList.SelectedIndices.Count == 1)
      {
        listContextMenu.Items.Add(new ToolStripMenuItem("Edit filter", null, new EventHandler(EhEditFilter)));
        listContextMenu.Items.Add(new ToolStripSeparator());
        listContextMenu.Items.Add(new ToolStripMenuItem("Edit path", null, new EventHandler(EhEditPath)));
        listContextMenu.Items.Add(new ToolStripMenuItem("Delete path", null, new EventHandler(EhDeletePath)));
      }
      if (this.lvFilterListItemList.SelectedIndices.Count >= 1)
      {
        listContextMenu.Items.Add(new ToolStripSeparator());
        listContextMenu.Items.Add(new ToolStripMenuItem("Move up", null, new EventHandler(EhMoveUp)));
        listContextMenu.Items.Add(new ToolStripMenuItem("Move down", null, new EventHandler(EhMoveDown)));

      }


    }

    public void UpdateList(FilterListItemList list)
    {
      this.lvFilterListItemList.BeginUpdate();

      this.lvFilterListItemList.Items.Clear();

      foreach (FilterListItem it in list)
      {
        System.Windows.Forms.ListViewItem lvitem = new ListViewItem(it.Path);
        lvitem.SubItems.Add(it.Filter.DefaultAction.ToString());
        lvFilterListItemList.Items.Add(lvitem);
      }

      this.lvFilterListItemList.EndUpdate();
    }

    public void SelectListItems(int[] selectedIndices)
    {
      for (int i = 0; i < selectedIndices.Length; i++)
      {
        lvFilterListItemList.Items[selectedIndices[i]].Selected = true;
      }
    }


    public void UpdatePathInformation(string myRootPath, string foreignRootPath)
    {
      this.edMyRoot.Text = myRootPath;
      this.edForeignRoot.Text = foreignRootPath;
    }

    public void UpdateDefaultFilterAction(FilterAction action)
    {
      this.edDefaultFilterAction.Text = action.ToString();
    }


    private void EhAddNewPath(object sender, EventArgs e)
    {
      if (null != this.Controller)
      {
        Controller.EhView_AddNewPath();
      }
    }

    private void EhEditPath(object sender, EventArgs e)
    {
      if (null != this.Controller)
      {
        Controller.EhView_EditPath(this.lvFilterListItemList.SelectedIndices[0]);
      }
    }

    private void EhDeletePath(object sender, EventArgs e)
    {
      if (null != this.Controller)
      {
        Controller.EhView_DeletePath(GetIndexArray(this.lvFilterListItemList.SelectedIndices));
      }
    }

    private int[] GetIndexArray(System.Windows.Forms.ListView.SelectedIndexCollection coll)
    {
      int[] res = new int[coll.Count];
      coll.CopyTo(res, 0);
      return res;
    }

    private void EhEditFilter(object sender, EventArgs e)
    {
      if (null != this.Controller && this.lvFilterListItemList.SelectedIndices.Count > 0)
      {
        Controller.EhView_EditFilter(this.lvFilterListItemList.SelectedIndices[0]);
      }
    }

    private void btDefaultFilter_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_ShowDefaultFilter();
      }
    }

    private void EhMoveUp(object sender, EventArgs e)
    {
      if (null != this.Controller && lvFilterListItemList.SelectedIndices.Count > 0)
      {
        Controller.EhView_MoveUp(GetIndexArray(this.lvFilterListItemList.SelectedIndices));
        lvFilterListItemList.Focus();
      }
    }

    private void EhMoveDown(object sender, EventArgs e)
    {
      if (null != this.Controller && lvFilterListItemList.SelectedIndices.Count > 0)
      {
        Controller.EhView_MoveDown(GetIndexArray(this.lvFilterListItemList.SelectedIndices));
        lvFilterListItemList.Focus();
      }
    }

  }
}
