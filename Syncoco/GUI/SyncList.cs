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
using System.Collections;
using System.Windows.Forms;

namespace Syncoco.GUI
{
  /// <summary>
  /// Summary description for SyncList.
  /// </summary>
  public class SyncList : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.ListView lvSyncList;
    private System.Windows.Forms.ColumnHeader _chName;

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public SyncList()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call
      ContextMenu contextMenu = new ContextMenu();
      contextMenu.MenuItems.Add("Set remove manually to remove", new EventHandler(this.EhSetRemoveManuallyToRemove));
      contextMenu.MenuItems.Add("Set remove to forced remove", new EventHandler(this.EhSetRemoveToForcedRemove));
      contextMenu.MenuItems.Add("Set remove manually to rollback remove", new EventHandler(this.EhSetRemoveManuallyToRollback));
      contextMenu.MenuItems.Add("-");

      contextMenu.MenuItems.Add("Set resolve manually to overwrite", new EventHandler(this.EhSetResolveManuallyToOverwrite));
      contextMenu.MenuItems.Add("Set resolve manually to ignore", new EventHandler(this.EhSetResolveManuallyToIgnore));
      contextMenu.MenuItems.Add("Set resolve manually to rollback", new EventHandler(this.EhSetResolveManuallyToRollback));

      contextMenu.MenuItems.Add("-");
      contextMenu.MenuItems.Add("Set remove to rollback remove", new EventHandler(this.EhSetRemoveToRollbackRemove));
      contextMenu.MenuItems.Add("Set overwrite to rollback overwrite", new EventHandler(this.EhSetOverwriteToRollbackOverwrite));

      this.ContextMenu = contextMenu;

    }

    public SyncListController _controller;


    public void InitializeColumnHeaders()
    {
      lvSyncList.Clear();
      lvSyncList.Columns.Add("Name", 170, System.Windows.Forms.HorizontalAlignment.Center);
      lvSyncList.Columns.Add("Action", 70, System.Windows.Forms.HorizontalAlignment.Center);
      lvSyncList.Columns.Add("Path", 70, System.Windows.Forms.HorizontalAlignment.Center);

      lvSyncList.Columns.Add("Length", 70, System.Windows.Forms.HorizontalAlignment.Center);
      lvSyncList.Columns.Add("WrTime", 90, System.Windows.Forms.HorizontalAlignment.Center);
    }

    public void InitializeList(ArrayList list)
    {
      lvSyncList.BeginUpdate();
      lvSyncList.Items.Clear();
      lvSyncList.Items.AddRange((ListViewItem[])list.ToArray(typeof(ListViewItem)));
      lvSyncList.EndUpdate();
    }

    public int GetItemCount()
    {
      return lvSyncList.Items.Count;
    }

    public bool IsItemSelected(int idx)
    {
      return lvSyncList.Items[idx].Selected;
    }

    public object GetItemTag(int idx)
    {
      return lvSyncList.Items[idx].Tag;
    }

    public ListViewItem GetItem(int idx)
    {
      return lvSyncList.Items[idx];
    }

    public void SortList(IComparer comparer, int nColumn, bool reverse)
    {
      char upArrow = '\u2191';
      char dnArrow = '\u2193';

      lvSyncList.ListViewItemSorter = comparer;
      lvSyncList.Sort();
      // and now indicate in the column name that this column is sorted now
      lvSyncList.Columns[0].Text = "Name";
      lvSyncList.Columns[1].Text = "Action";
      lvSyncList.Columns[2].Text = "Path";
      lvSyncList.Columns[3].Text = "Length";
      lvSyncList.Columns[4].Text = "WrTime";

      lvSyncList.Columns[nColumn].Text += reverse ? dnArrow : upArrow;
    }

    public void EhSetRemoveManuallyToRemove(object sender, EventArgs e)
    {
      _controller.EhView_RemoveManually_Remove();
    }

    public void EhSetRemoveToRollbackRemove(object sender, EventArgs e)
    {
      _controller.EhView_Remove_Rollback();
    }

    public void EhSetRemoveManuallyToRollback(object sender, EventArgs e)
    {
      _controller.EhView_RemoveManually_Rollback();
    }

    public void EhSetRemoveToForcedRemove(object sender, EventArgs e)
    {
      _controller.EhView_Remove_ForcedRemove();
    }

    public void EhSetResolveManuallyToOverwrite(object sender, EventArgs e)
    {
      _controller.EhView_ResolveManually_Overwrite();
    }
    public void EhSetResolveManuallyToIgnore(object sender, EventArgs e)
    {
      _controller.EhView_ResolveManually_Ignore();
    }
    public void EhSetResolveManuallyToRollback(object sender, EventArgs e)
    {
      _controller.EhView_ResolveManually_Rollback();
    }

    public void EhSetOverwriteToRollbackOverwrite(object sender, EventArgs e)
    {
      _controller.EhView_Overwrite_Rollback();
    }

    private void EhSyncList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
    {
      _controller.EhView_ColumnClick(e.Column);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if ((Keys.A | Keys.Control) == keyData)
      {
        this.lvSyncList.BeginUpdate();
        for (int i = 0; i < lvSyncList.Items.Count; i++)
        {
          this.lvSyncList.Items[i].Selected = true;
        }

        this.lvSyncList.EndUpdate();


      }

      return base.ProcessCmdKey(ref msg, keyData);
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
      this.lvSyncList = new System.Windows.Forms.ListView();
      this._chName = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // lvSyncList
      // 
      this.lvSyncList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lvSyncList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                 this._chName});
      this.lvSyncList.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.lvSyncList.Location = new System.Drawing.Point(8, 8);
      this.lvSyncList.Name = "lvSyncList";
      this.lvSyncList.Size = new System.Drawing.Size(360, 320);
      this.lvSyncList.TabIndex = 0;
      this.lvSyncList.View = System.Windows.Forms.View.Details;
      this.lvSyncList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.EhSyncList_ColumnClick);
      // 
      // _chName
      // 
      this._chName.Text = "Name";
      this._chName.Width = 191;
      // 
      // SyncList
      // 
      this.Controls.Add(this.lvSyncList);
      this.Name = "SyncList";
      this.Size = new System.Drawing.Size(376, 336);
      this.ResumeLayout(false);

    }
    #endregion


  }
}
