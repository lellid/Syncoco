using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SyncTwoCo
{
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
    private System.Windows.Forms.ContextMenu listContextMenu;
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.ColumnHeader chSubPath;
    private System.Windows.Forms.ColumnHeader chDefaultAction;
    private System.Windows.Forms.TextBox edDefaultFilterAction;

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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.edMyRoot = new System.Windows.Forms.TextBox();
      this.edForeignRoot = new System.Windows.Forms.TextBox();
      this.lvFilterListItemList = new System.Windows.Forms.ListView();
      this.chSubPath = new System.Windows.Forms.ColumnHeader();
      this.chDefaultAction = new System.Windows.Forms.ColumnHeader();
      this.listContextMenu = new System.Windows.Forms.ContextMenu();
      this.lblDefaultFilterAction = new System.Windows.Forms.Label();
      this.btDefaultFilter = new System.Windows.Forms.Button();
      this.edDefaultFilterAction = new System.Windows.Forms.TextBox();
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
      this.lvFilterListItemList.ContextMenu = this.listContextMenu;
      this.lvFilterListItemList.Location = new System.Drawing.Point(8, 72);
      this.lvFilterListItemList.Name = "lvFilterListItemList";
      this.lvFilterListItemList.Size = new System.Drawing.Size(376, 296);
      this.lvFilterListItemList.TabIndex = 4;
      this.lvFilterListItemList.View = System.Windows.Forms.View.Details;
      // 
      // chSubPath
      // 
      this.chSubPath.Text = "SubPath";
      this.chSubPath.Width = 235;
      // 
      // chDefaultAction
      // 
      this.chDefaultAction.Text = "Default Action";
      this.chDefaultAction.Width = 92;
      // 
      // listContextMenu
      // 
      this.listContextMenu.Popup += new System.EventHandler(this.listContextMenu_Popup);
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
      this.btDefaultFilter.Location = new System.Drawing.Point(240, 376);
      this.btDefaultFilter.Name = "btDefaultFilter";
      this.btDefaultFilter.Size = new System.Drawing.Size(144, 23);
      this.btDefaultFilter.TabIndex = 6;
      this.btDefaultFilter.Text = "configure default filter";
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
      // FilterListItemListControl
      // 
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

    private void listContextMenu_Popup(object sender, System.EventArgs e)
    {
      listContextMenu.MenuItems.Clear();

      if(this.lvFilterListItemList.SelectedIndices.Count==0)
      {
        listContextMenu.MenuItems.Add(new MenuItem("Add new Path", new EventHandler(EhAddNewPath)));
      }
      else
      {
        if(this.lvFilterListItemList.SelectedIndices.Count==1)
        {
          listContextMenu.MenuItems.Add(new MenuItem("Edit filter", new EventHandler(EhEditFilter)));
          listContextMenu.MenuItems.Add(new MenuItem("-"));
        }

        listContextMenu.MenuItems.Add(new MenuItem("Delete path", new EventHandler(EhDeletePath)));
      }

    
    }

    public void UpdateList(FilterListItemList list)
    {
      this.lvFilterListItemList.BeginUpdate();

      this.lvFilterListItemList.Items.Clear();

      foreach(FilterListItem it in list)
      {
        System.Windows.Forms.ListViewItem lvitem = new ListViewItem(it.Path);
        lvitem.SubItems.Add(it.Filter.DefaultAction.ToString());
        lvFilterListItemList.Items.Add(lvitem);
      }

      this.lvFilterListItemList.EndUpdate();
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
      if(null!=this.Controller)
        Controller.EhView_AddNewPath();
    }

    private int[] GetIndexArray(System.Windows.Forms.ListView.SelectedIndexCollection coll)
    {
      int[] res = new int[coll.Count];
      coll.CopyTo(res,0);
      return res;
    }

    private void EhDeletePath(object sender, EventArgs e)
    {
      if(null!=this.Controller)
        Controller.EhView_DeletePath(GetIndexArray(this.lvFilterListItemList.SelectedIndices));
    }

    private void EhEditFilter(object sender, EventArgs e)
    {
      if(null!=this.Controller)
        Controller.EhView_EditFilter(this.lvFilterListItemList.SelectedIndices[0]);
    }

    private void btDefaultFilter_Click(object sender, System.EventArgs e)
    {
      if(null!=Controller)
        Controller.EhView_ShowDefaultFilter();

    }
  }
}
