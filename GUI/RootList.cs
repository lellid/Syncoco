using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Syncoco
{
  /// <summary>
  /// Summary description for RootList.
  /// </summary>
  public class RootList : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.ListView lvRootList;
    private System.Windows.Forms.ColumnHeader chComputer1;
    private System.Windows.Forms.ColumnHeader chComputer2;
    private System.Windows.Forms.Button btAddPath;
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.Button btEditFilterList;
    private System.Windows.Forms.ContextMenu listContextMenu;
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
      int len = Math.Max(list0.Length,list1.Length);

      for(int i=0;i<len;i++)
      {
        string txt0 = i<list0.Length ? list0[i] : string.Empty;
        string txt1 = i<list1.Length ? list1[i] : string.Empty;
        ListViewItem item = new ListViewItem(new string[]{txt0,txt1});

        lvRootList.Items.Add(item);
      }

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
      this.lvRootList = new System.Windows.Forms.ListView();
      this.chComputer1 = new System.Windows.Forms.ColumnHeader();
      this.chComputer2 = new System.Windows.Forms.ColumnHeader();
      this.btAddPath = new System.Windows.Forms.Button();
      this.btEditFilterList = new System.Windows.Forms.Button();
      this.listContextMenu = new System.Windows.Forms.ContextMenu();
      this.SuspendLayout();
      // 
      // lvRootList
      // 
      this.lvRootList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                 this.chComputer1,
                                                                                 this.chComputer2});
      this.lvRootList.ContextMenu = this.listContextMenu;
      this.lvRootList.LabelEdit = true;
      this.lvRootList.Location = new System.Drawing.Point(8, 56);
      this.lvRootList.Name = "lvRootList";
      this.lvRootList.Size = new System.Drawing.Size(416, 312);
      this.lvRootList.TabIndex = 0;
      this.lvRootList.View = System.Windows.Forms.View.Details;
      this.lvRootList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvRootList_MouseDown);
      this.lvRootList.Click += new System.EventHandler(this.lvRootList_Click);
      this.lvRootList.DoubleClick += new System.EventHandler(this.lvRootList_DoubleClick);
      this.lvRootList.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvRootList_BeforeLabelEdit);
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
      // btAddPath
      // 
      this.btAddPath.Location = new System.Drawing.Point(48, 24);
      this.btAddPath.Name = "btAddPath";
      this.btAddPath.Size = new System.Drawing.Size(72, 24);
      this.btAddPath.TabIndex = 1;
      this.btAddPath.Text = "btAddPath";
      this.btAddPath.Click += new System.EventHandler(this.btAddPath_Click);
      // 
      // btEditFilterList
      // 
      this.btEditFilterList.Location = new System.Drawing.Point(336, 24);
      this.btEditFilterList.Name = "btEditFilterList";
      this.btEditFilterList.Size = new System.Drawing.Size(88, 24);
      this.btEditFilterList.TabIndex = 2;
      this.btEditFilterList.Text = "Edit FilterList";
      this.btEditFilterList.Click += new System.EventHandler(this.btEditFilterList_Click);
      // 
      // listContextMenu
      // 
      this.listContextMenu.Popup += new System.EventHandler(this.listContextMenu_Popup);
      // 
      // RootList
      // 
      this.Controls.Add(this.btEditFilterList);
      this.Controls.Add(this.btAddPath);
      this.Controls.Add(this.lvRootList);
      this.Name = "RootList";
      this.Size = new System.Drawing.Size(432, 376);
      this.ResumeLayout(false);

    }
    #endregion

    private void lvRootList_DoubleClick(object sender, System.EventArgs e)
    {
      if(_lastMouseDownItem!=null)
      {
        if(null!=_controller)
          _controller.EhView_ItemDoubleClick(_lastMouseDownItem.Index);
      }
    }

    private void lvRootList_BeforeLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
    {
      if(null!=_controller)
        _controller.EhView_BeforeLabelEdit(e.Item);
    
    }

    private void lvRootList_Click(object sender, System.EventArgs e)
    {
    
    }

    ListViewItem _lastMouseDownItem;
    private void lvRootList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      _lastMouseDownItem = this.lvRootList.GetItemAt(e.X,e.Y);
    }

    private void btAddPath_Click(object sender, System.EventArgs e)
    {
      if(null!=_controller)
        _controller.EhView_AddPath();
    }

    private void btEditFilterList_Click(object sender, System.EventArgs e)
    {
      if(null!=_controller)
      {
        if(this.lvRootList.SelectedIndices.Count==1)
        {
          _controller.EhView_EditFilterList(this.lvRootList.SelectedIndices[0]);
        }
      }
    }

    private void btDeletePath_Click(object sender, System.EventArgs e)
    {
      if(null!=_controller)
      {
        if(this.lvRootList.SelectedIndices.Count==1)
        {
          _controller.EhView_DeletePath(this.lvRootList.SelectedIndices[0]);
        }
      }
    }
    private void listContextMenu_Popup(object sender, System.EventArgs e)
    {
      listContextMenu.MenuItems.Clear();

      if(this.lvRootList.SelectedIndices.Count==0)
      {
        listContextMenu.MenuItems.Add(new MenuItem("Add new Path", new EventHandler(btAddPath_Click)));
      }
        if(this.lvRootList.SelectedIndices.Count==1)
        {
          listContextMenu.MenuItems.Add(new MenuItem("Edit filter", new EventHandler(btEditFilterList_Click)));
          listContextMenu.MenuItems.Add(new MenuItem("-"));
          listContextMenu.MenuItems.Add(new MenuItem("Delete path", new EventHandler(btDeletePath_Click)));
        }

      if(this.lvRootList.SelectedIndices.Count>=1)
      {
      }
    
    }

   
  }
}
