using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class SyncTwoCo : System.Windows.Forms.Form
  {
    private System.Windows.Forms.MainMenu mainMenu1;
    private System.Windows.Forms.MenuItem menuItem1;

    public System.Windows.Forms.UserControl _control;
    private System.Windows.Forms.MenuItem menuFileNew;
    private System.Windows.Forms.MenuItem menuFileOpen;
    private System.Windows.Forms.MenuItem menuFileExit;
    private System.Windows.Forms.MenuItem menuFileSave;
    private System.Windows.Forms.MenuItem menuItem2;
    private System.Windows.Forms.MenuItem menuEditUpdate;
    private System.Windows.Forms.MenuItem menuEditCopyFiles;
    private System.Windows.Forms.MenuItem menuEditSyncronize;
    private System.Windows.Forms.MenuItem menuEditCollect;
    private System.Windows.Forms.MenuItem menuFileSaveAs;
    private System.Windows.Forms.MenuItem menuBeginWork;
    private System.Windows.Forms.MenuItem menuEndWork;
    private System.Windows.Forms.MenuItem menuBeginShowSyncFiles;
    private System.Windows.Forms.MenuItem menuBeginSyncSelected;
    private System.Windows.Forms.MenuItem menuEndUpdateSaveCopy;
    private System.Windows.Forms.MenuItem menuItem3;
    private System.Windows.Forms.MenuItem menuUpdateHash;
    private System.Windows.Forms.TabControl _tabControl;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public SyncTwoCo()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null) 
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuFileNew = new System.Windows.Forms.MenuItem();
      this.menuFileOpen = new System.Windows.Forms.MenuItem();
      this.menuFileExit = new System.Windows.Forms.MenuItem();
      this.menuFileSave = new System.Windows.Forms.MenuItem();
      this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menuItem2 = new System.Windows.Forms.MenuItem();
      this.menuEditUpdate = new System.Windows.Forms.MenuItem();
      this.menuEditCopyFiles = new System.Windows.Forms.MenuItem();
      this.menuEditCollect = new System.Windows.Forms.MenuItem();
      this.menuEditSyncronize = new System.Windows.Forms.MenuItem();
      this.menuUpdateHash = new System.Windows.Forms.MenuItem();
      this.menuBeginWork = new System.Windows.Forms.MenuItem();
      this.menuBeginShowSyncFiles = new System.Windows.Forms.MenuItem();
      this.menuBeginSyncSelected = new System.Windows.Forms.MenuItem();
      this.menuEndWork = new System.Windows.Forms.MenuItem();
      this.menuEndUpdateSaveCopy = new System.Windows.Forms.MenuItem();
      this._tabControl = new System.Windows.Forms.TabControl();
      this.SuspendLayout();
      // 
      // mainMenu1
      // 
      this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuItem1,
                                                                              this.menuItem2,
                                                                              this.menuBeginWork,
                                                                              this.menuEndWork});
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuFileNew,
                                                                              this.menuFileOpen,
                                                                              this.menuFileExit,
                                                                              this.menuFileSave,
                                                                              this.menuFileSaveAs,
                                                                              this.menuItem3});
      this.menuItem1.Text = "File";
      // 
      // menuFileNew
      // 
      this.menuFileNew.Index = 0;
      this.menuFileNew.Text = "New";
      this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
      // 
      // menuFileOpen
      // 
      this.menuFileOpen.Index = 1;
      this.menuFileOpen.Text = "Open";
      this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
      // 
      // menuFileExit
      // 
      this.menuFileExit.Index = 2;
      this.menuFileExit.Text = "Exit";
      // 
      // menuFileSave
      // 
      this.menuFileSave.Index = 3;
      this.menuFileSave.Text = "Save";
      this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
      // 
      // menuFileSaveAs
      // 
      this.menuFileSaveAs.Index = 4;
      this.menuFileSaveAs.Text = "Save As..";
      this.menuFileSaveAs.Click += new System.EventHandler(this.menuFileSaveAs_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Index = 5;
      this.menuItem3.Text = "Save Filter Only As..";
      this.menuItem3.Click += new System.EventHandler(this.menuFileSaveFilterOnly_Click);
      // 
      // menuItem2
      // 
      this.menuItem2.Index = 1;
      this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuEditUpdate,
                                                                              this.menuEditCopyFiles,
                                                                              this.menuEditCollect,
                                                                              this.menuEditSyncronize,
                                                                              this.menuUpdateHash});
      this.menuItem2.Text = "Edit";
      // 
      // menuEditUpdate
      // 
      this.menuEditUpdate.Index = 0;
      this.menuEditUpdate.Text = "Update";
      this.menuEditUpdate.Click += new System.EventHandler(this.menuEditUpdate_Click);
      // 
      // menuEditCopyFiles
      // 
      this.menuEditCopyFiles.Index = 1;
      this.menuEditCopyFiles.Text = "CopyFiles";
      this.menuEditCopyFiles.Click += new System.EventHandler(this.menuEditCopyFiles_Click);
      // 
      // menuEditCollect
      // 
      this.menuEditCollect.Index = 2;
      this.menuEditCollect.Text = "Collect";
      this.menuEditCollect.Click += new System.EventHandler(this.menuEditCollect_Click);
      // 
      // menuEditSyncronize
      // 
      this.menuEditSyncronize.Index = 3;
      this.menuEditSyncronize.Text = "Synchronize";
      this.menuEditSyncronize.Click += new System.EventHandler(this.menuEditSyncronize_Click);
      // 
      // menuUpdateHash
      // 
      this.menuUpdateHash.Index = 4;
      this.menuUpdateHash.Text = "UpdateHash";
      this.menuUpdateHash.Click += new System.EventHandler(this.menuUpdateHash_Click);
      // 
      // menuBeginWork
      // 
      this.menuBeginWork.Index = 2;
      this.menuBeginWork.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this.menuBeginShowSyncFiles,
                                                                                  this.menuBeginSyncSelected});
      this.menuBeginWork.Text = "Beginning work...";
      // 
      // menuBeginShowSyncFiles
      // 
      this.menuBeginShowSyncFiles.Index = 0;
      this.menuBeginShowSyncFiles.Text = "Show files to sync";
      this.menuBeginShowSyncFiles.Click += new System.EventHandler(this.menuBeginShowSyncFiles_Click);
      // 
      // menuBeginSyncSelected
      // 
      this.menuBeginSyncSelected.Index = 1;
      this.menuBeginSyncSelected.Text = "Sync selected files";
      this.menuBeginSyncSelected.Click += new System.EventHandler(this.menuBeginSyncSelected_Click);
      // 
      // menuEndWork
      // 
      this.menuEndWork.Index = 3;
      this.menuEndWork.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.menuEndUpdateSaveCopy});
      this.menuEndWork.Text = "Ending work...";
      // 
      // menuEndUpdateSaveCopy
      // 
      this.menuEndUpdateSaveCopy.Index = 0;
      this.menuEndUpdateSaveCopy.Text = "UpdateSaveAndCopyFiles";
      this.menuEndUpdateSaveCopy.Click += new System.EventHandler(this.menuEndUpdateSaveCopy_Click);
      // 
      // _tabControl
      // 
      this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tabControl.Location = new System.Drawing.Point(0, 0);
      this._tabControl.Name = "_tabControl";
      this._tabControl.SelectedIndex = 0;
      this._tabControl.Size = new System.Drawing.Size(520, 398);
      this._tabControl.TabIndex = 0;
      // 
      // SyncTwoCo
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(520, 398);
      this.Controls.Add(this._tabControl);
      this.Menu = this.mainMenu1;
      this.Name = "SyncTwoCo";
      this.Text = "SyncTwoCo";
      this.ResumeLayout(false);

    }
    #endregion

  
    RootListController _rootList = new RootListController(Current.Document);
    SyncListController _syncList = new SyncListController();

    public void ShowControl(string caption, UserControl ctrl)
    {
      if(this._control!=null)
        this.Controls.Remove(_control);


      // try to find appropriate tab page

      TabPage tabpage =null;
      for(int i=0;i<_tabControl.TabPages.Count;i++)
      {
        if(_tabControl.TabPages[i].Controls[0]==ctrl)
        {
          tabpage = _tabControl.TabPages[i];
          break;
        }
      }

      if(tabpage==null)
      {
        tabpage = new TabPage(caption);
        tabpage.Controls.Add(ctrl);
        ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
        _tabControl.TabPages.Add(tabpage);
      }

      _tabControl.SelectedTab = tabpage;
    }

    public void ShowSyncList()
    {
      ShowControl("FilesToSync",_syncList.View);
    }

    private void menuFileNew_Click(object sender, System.EventArgs e)
    {
      if(_rootList==null)
        _rootList = new RootListController(Current.Document);
      ShowControl("Roots",_rootList.View);
    }

    private void menuFileSave_Click(object sender, System.EventArgs e)
    {
      if(Current.Document.HasFileName)
      {
        Current.Document.Save();
      }
      else
      {
        menuFileSaveAs_Click(sender,e);
      }
    
    }

    private void menuFileSaveAs_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        Current.Document.Save(dlg.FileName);
      }
    }

    private void menuFileSaveFilterOnly_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        Current.Document.SaveFilterOnly(dlg.FileName);
      }
    }


    private void menuFileOpen_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        MainDocument doc = MainDocument.Open(dlg.FileName);
        if(doc != null)
        {
          if(this._control!=null)
          {
            this.Controls.Remove(_control);
            this._control=null;
          }

          Current.Document = doc;
          _rootList = new RootListController(Current.Document);
          ShowControl("Root",_rootList.View);
        }
      }
    }

    private void menuFileOpenAsXML_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        MainDocument doc = new MainDocument();
        doc.OpenAsXML(dlg.FileName);

        Current.Document = (MainDocument)doc;
        _rootList = new RootListController(Current.Document);
        ShowControl("Root",_rootList.View);
      }
    }
    
    

    private void menuEditUpdate_Click(object sender, System.EventArgs e)
    {
      Current.Document.Update(false, new DummyBackgroundMonitor());
    }

    private void menuUpdateHash_Click(object sender, System.EventArgs e)
    {
      Current.Document.Update(true,new DummyBackgroundMonitor());
    }

    private void menuEditCopyFiles_Click(object sender, System.EventArgs e)
    {
      Current.Document.CopyFilesToMedium(new DummyBackgroundMonitor());
    }

    private void menuEditCollect_Click(object sender, System.EventArgs e)
    {
      if(_syncList==null)
        _syncList = new SyncListController();

      _syncList.SetCollectors(Current.Document.Collect());

      ShowSyncList();
    
    }


    private void menuEditSyncronize_Click(object sender, System.EventArgs e)
    {
      _syncList.Synchronize();
    }

    private void menuBeginShowSyncFiles_Click(object sender, System.EventArgs e)
    {
      menuEditCollect_Click(sender,e);
    }

    private void menuBeginSyncSelected_Click(object sender, System.EventArgs e)
    {
      _syncList.Synchronize();
    }

    private void menuEndUpdateSaveCopy_Click(object sender, System.EventArgs e)
    {
      Current.Document.UpdateAndSaveAndCopyFilesToMedium();
    }

    

   
 
   

  }
}
