using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SyncTwoCo
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
      contextMenu.MenuItems.Add("Set to remove",new EventHandler(this.EhSetToRemove));
      contextMenu.MenuItems.Add("Set to rollback remove",new EventHandler(this.EhSetToRollbackRemove));
      contextMenu.MenuItems.Add("Set resolve manually to overwrite",new EventHandler(this.EhSetResolveManuallyToOverwrite));
      contextMenu.MenuItems.Add("Set resolve manually to ignore",new EventHandler(this.EhSetResolveManuallyToIgnore));
      contextMenu.MenuItems.Add("Set resolve manually to rollback",new EventHandler(this.EhSetResolveManuallyToRollback));
      this.ContextMenu = contextMenu;

    }

    public SyncListController _controller;

    public void InitializeList(ArrayList list)
    {
      lvSyncList.Clear();
      lvSyncList.Columns.Add("Name",150,System.Windows.Forms.HorizontalAlignment.Center);
      lvSyncList.Columns.Add("Action",50,System.Windows.Forms.HorizontalAlignment.Center);
      lvSyncList.Columns.Add("Path",50,System.Windows.Forms.HorizontalAlignment.Center);


      lvSyncList.Items.AddRange((ListViewItem[])list.ToArray(typeof(ListViewItem)));
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

    public void EhSetToRemove(object sender, EventArgs e)
    {
      _controller.EhView_SetToRemove();
    }

    public void EhSetToRollbackRemove(object sender, EventArgs e)
    {
      _controller.EhView_SetToRollbackRemove();
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

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
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
      this.lvSyncList.Location = new System.Drawing.Point(8, 8);
      this.lvSyncList.Name = "lvSyncList";
      this.lvSyncList.Size = new System.Drawing.Size(360, 320);
      this.lvSyncList.TabIndex = 0;
      this.lvSyncList.View = System.Windows.Forms.View.Details;
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
