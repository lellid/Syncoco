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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Syncoco.DocumentActions;

namespace Syncoco
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class Syncoco : System.Windows.Forms.Form
  {
    private System.Windows.Forms.MainMenu mainMenu1;

    private System.Windows.Forms.MenuItem menuFileNew;
    private System.Windows.Forms.MenuItem menuFileOpen;
    private System.Windows.Forms.MenuItem menuFileExit;
    private System.Windows.Forms.MenuItem menuFileSave;
    private System.Windows.Forms.MenuItem menuEditUpdate;
    private System.Windows.Forms.MenuItem menuEditCopyFiles;
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
    private System.Windows.Forms.MenuItem menuItem4;
    private System.Windows.Forms.MenuItem menuHelp;
    private System.Windows.Forms.MenuItem menuHelpAbout;
    private System.Windows.Forms.MenuItem menuFile;
    private System.Windows.Forms.MenuItem menuActions;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuEditSynchronize;
    private System.Windows.Forms.MenuItem menuEditCleanTransferDir;
    private System.Windows.Forms.MenuItem menuItem2;
    private System.Windows.Forms.MenuItem menuBeginSaveCleanIfNeccessary;
    private System.Windows.Forms.MenuItem menuHelpSyncoco;
    private System.Windows.Forms.MenuItem menuItem5;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public Syncoco()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      Current.Document.DirtyChanged += new EventHandler(EhDocumentDirtyChanged);
      Current.Document.FileNameChanged += new EventHandler(EhDocumentFileNameChanged);


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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Syncoco));
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.menuFile = new System.Windows.Forms.MenuItem();
      this.menuFileNew = new System.Windows.Forms.MenuItem();
      this.menuFileOpen = new System.Windows.Forms.MenuItem();
      this.menuFileSave = new System.Windows.Forms.MenuItem();
      this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menuItem4 = new System.Windows.Forms.MenuItem();
      this.menuFileExit = new System.Windows.Forms.MenuItem();
      this.menuBeginWork = new System.Windows.Forms.MenuItem();
      this.menuBeginShowSyncFiles = new System.Windows.Forms.MenuItem();
      this.menuBeginSyncSelected = new System.Windows.Forms.MenuItem();
      this.menuItem2 = new System.Windows.Forms.MenuItem();
      this.menuBeginSaveCleanIfNeccessary = new System.Windows.Forms.MenuItem();
      this.menuEndWork = new System.Windows.Forms.MenuItem();
      this.menuEndUpdateSaveCopy = new System.Windows.Forms.MenuItem();
      this.menuActions = new System.Windows.Forms.MenuItem();
      this.menuEditCollect = new System.Windows.Forms.MenuItem();
      this.menuEditSynchronize = new System.Windows.Forms.MenuItem();
      this.menuEditUpdate = new System.Windows.Forms.MenuItem();
      this.menuEditCleanTransferDir = new System.Windows.Forms.MenuItem();
      this.menuEditCopyFiles = new System.Windows.Forms.MenuItem();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuUpdateHash = new System.Windows.Forms.MenuItem();
      this.menuHelp = new System.Windows.Forms.MenuItem();
      this.menuHelpAbout = new System.Windows.Forms.MenuItem();
      this._tabControl = new System.Windows.Forms.TabControl();
      this.menuHelpSyncoco = new System.Windows.Forms.MenuItem();
      this.menuItem5 = new System.Windows.Forms.MenuItem();
      this.SuspendLayout();
      // 
      // mainMenu1
      // 
      this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuFile,
                                                                              this.menuBeginWork,
                                                                              this.menuEndWork,
                                                                              this.menuActions,
                                                                              this.menuHelp});
      // 
      // menuFile
      // 
      this.menuFile.Index = 0;
      this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.menuFileNew,
                                                                             this.menuFileOpen,
                                                                             this.menuFileSave,
                                                                             this.menuFileSaveAs,
                                                                             this.menuItem3,
                                                                             this.menuItem4,
                                                                             this.menuFileExit});
      this.menuFile.Text = "File";
      this.menuFile.Popup += new System.EventHandler(this.menuFile_Popup);
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
      // menuFileSave
      // 
      this.menuFileSave.Index = 2;
      this.menuFileSave.Text = "Save";
      this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
      // 
      // menuFileSaveAs
      // 
      this.menuFileSaveAs.Index = 3;
      this.menuFileSaveAs.Text = "Save As..";
      this.menuFileSaveAs.Click += new System.EventHandler(this.menuFileSaveAs_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Index = 4;
      this.menuItem3.Text = "Save Filter Only As..";
      this.menuItem3.Click += new System.EventHandler(this.menuFileSaveFilterOnly_Click);
      // 
      // menuItem4
      // 
      this.menuItem4.Index = 5;
      this.menuItem4.Text = "-";
      // 
      // menuFileExit
      // 
      this.menuFileExit.Index = 6;
      this.menuFileExit.Text = "Exit";
      this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
      // 
      // menuBeginWork
      // 
      this.menuBeginWork.Index = 1;
      this.menuBeginWork.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this.menuBeginShowSyncFiles,
                                                                                  this.menuBeginSyncSelected,
                                                                                  this.menuItem2,
                                                                                  this.menuBeginSaveCleanIfNeccessary});
      this.menuBeginWork.Text = "Beginning work...";
      this.menuBeginWork.Popup += new System.EventHandler(this.menuBeginWork_Popup);
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
      // menuItem2
      // 
      this.menuItem2.Index = 2;
      this.menuItem2.Text = "-";
      // 
      // menuBeginSaveCleanIfNeccessary
      // 
      this.menuBeginSaveCleanIfNeccessary.Index = 3;
      this.menuBeginSaveCleanIfNeccessary.Text = "SaveAndCleanIfNeccessary";
      this.menuBeginSaveCleanIfNeccessary.Click += new System.EventHandler(this.menuBeginSaveCleanIfNeccessary_Click);
      // 
      // menuEndWork
      // 
      this.menuEndWork.Index = 2;
      this.menuEndWork.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.menuEndUpdateSaveCopy});
      this.menuEndWork.Text = "Ending work...";
      this.menuEndWork.Popup += new System.EventHandler(this.menuEndWork_Popup);
      // 
      // menuEndUpdateSaveCopy
      // 
      this.menuEndUpdateSaveCopy.Index = 0;
      this.menuEndUpdateSaveCopy.Text = "UpdateSaveAndCopyFiles";
      this.menuEndUpdateSaveCopy.Click += new System.EventHandler(this.menuEndUpdateSaveCopy_Click);
      // 
      // menuActions
      // 
      this.menuActions.Index = 3;
      this.menuActions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                this.menuEditCollect,
                                                                                this.menuEditSynchronize,
                                                                                this.menuEditUpdate,
                                                                                this.menuEditCleanTransferDir,
                                                                                this.menuEditCopyFiles,
                                                                                this.menuItem1,
                                                                                this.menuUpdateHash});
      this.menuActions.Text = "Single Actions";
      this.menuActions.Popup += new System.EventHandler(this.menuActions_Popup);
      // 
      // menuEditCollect
      // 
      this.menuEditCollect.Index = 0;
      this.menuEditCollect.Text = "Show files to sync";
      this.menuEditCollect.Click += new System.EventHandler(this.menuEditCollect_Click);
      // 
      // menuEditSynchronize
      // 
      this.menuEditSynchronize.Index = 1;
      this.menuEditSynchronize.Text = "Sync selected files";
      this.menuEditSynchronize.Click += new System.EventHandler(this.menuEditSyncronize_Click);
      // 
      // menuEditUpdate
      // 
      this.menuEditUpdate.Index = 2;
      this.menuEditUpdate.Text = "Update";
      this.menuEditUpdate.Click += new System.EventHandler(this.menuEditUpdate_Click);
      // 
      // menuEditCleanTransferDir
      // 
      this.menuEditCleanTransferDir.Index = 3;
      this.menuEditCleanTransferDir.Text = "Clean transfer directory";
      this.menuEditCleanTransferDir.Click += new System.EventHandler(this.menuEditCleanTransferDir_Click);
      // 
      // menuEditCopyFiles
      // 
      this.menuEditCopyFiles.Index = 4;
      this.menuEditCopyFiles.Text = "Copy files to medium";
      this.menuEditCopyFiles.Click += new System.EventHandler(this.menuEditCopyFiles_Click);
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 5;
      this.menuItem1.Text = "-";
      // 
      // menuUpdateHash
      // 
      this.menuUpdateHash.Index = 6;
      this.menuUpdateHash.Text = "Force update hash sums";
      this.menuUpdateHash.Click += new System.EventHandler(this.menuUpdateHash_Click);
      // 
      // menuHelp
      // 
      this.menuHelp.Index = 4;
      this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.menuHelpSyncoco,
                                                                             this.menuItem5,
                                                                             this.menuHelpAbout});
      this.menuHelp.Text = "Help";
      // 
      // menuHelpAbout
      // 
      this.menuHelpAbout.Index = 2;
      this.menuHelpAbout.Text = "About..";
      this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
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
      // menuHelpSyncoco
      // 
      this.menuHelpSyncoco.Index = 0;
      this.menuHelpSyncoco.Text = "Index..";
      this.menuHelpSyncoco.Click += new System.EventHandler(this.menuHelpSyncoco_Click);
      // 
      // menuItem5
      // 
      this.menuItem5.Index = 1;
      this.menuItem5.Text = "-";
      // 
      // Syncoco
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(520, 398);
      this.Controls.Add(this._tabControl);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Menu = this.mainMenu1;
      this.Name = "Syncoco";
      this.Text = "Syncoco";
      this.Closing += new System.ComponentModel.CancelEventHandler(this.EhFormClosing);
      this.Load += new System.EventHandler(this.Syncoco_Load);
      this.ResumeLayout(false);

    }
    #endregion

  
    RootListController _rootList = new RootListController(Current.Document);
    SyncListController _syncList = new SyncListController();
    ReportListController _reportList = new ReportListController();


    public IErrorReporter ErrorReporter
    {
      get { return _reportList; }
    }


    public void ShowControl(string caption, UserControl ctrl)
    {

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

    public void ShowReportList()
    {
      ShowControl("Report",_reportList.View);
    }

    void UpdateTitle()
    {
      string filename = Current.Document.HasFileName? Current.Document.FileName : "Unnamed";
      string dirty = Current.Document.IsDirty?  "*" : string.Empty;
      this.Text = string.Format("{1}{2} - Syncoco@{0}",Current.ComputerName, filename, dirty);
    }

    public void ExchangeCurrentDocument(MainDocument newDoc)
    {
      if(newDoc == null)
        return;

      this._tabControl.TabPages.Clear();

      Current.Document.DirtyChanged -= new EventHandler(EhDocumentDirtyChanged);
      Current.Document.FileNameChanged -= new EventHandler(EhDocumentFileNameChanged);
      Current.Document = newDoc;
      Current.Document.DirtyChanged += new EventHandler(EhDocumentDirtyChanged);
      Current.Document.FileNameChanged += new EventHandler(EhDocumentFileNameChanged);
      this.UpdateTitle();
      _rootList = new RootListController(Current.Document);
      ShowControl("Root",_rootList.View);
    }

    

    private void SaveCloseDocument(System.ComponentModel.CancelEventArgs e)
    {
      while(Current.Document.IsDirty)
      {
        DialogResult boxresult = MessageBox.Show(this,"Your document is not saved yet. Do you want to save it now?","Document not saved",
          MessageBoxButtons.YesNoCancel,
          MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button1);

        if(boxresult==DialogResult.Cancel)
        {
          e.Cancel=true;
          return; 
        }
        else if(boxresult==DialogResult.No)
        {
          return;
        }
        else
        {
          this.menuFileSave_Click(this,EventArgs.Empty);
        }
      }
    }


  
    #region Menu Handler

    #region File Menu

    private void menuFile_Popup(object sender, System.EventArgs e)
    {
   
    }

    private void menuFileNew_Click(object sender, System.EventArgs e)
    {
      System.ComponentModel.CancelEventArgs ce = new CancelEventArgs();
      SaveCloseDocument(ce);

      if(ce.Cancel==false)
      {
        ExchangeCurrentDocument(new MainDocument());
      }
    }


    private void menuFileOpen_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      dlg.Filter = "Syncoco xml files (*.syncox)|*.syncox|All files (*.*)|*.*" ;
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        DocumentActions.OpenDocumentAction action = new OpenDocumentAction(dlg.FileName);
        action.BackgroundExecute();

        ExchangeCurrentDocument(action.Doc);
      }
    }

  

    private void menuFileSave_Click(object sender, System.EventArgs e)
    {
      if(Current.Document.HasFileName)
      {
        DocumentActions.SaveDocumentAction action = new SaveDocumentAction(Current.Document,Current.Document.FileName);
        action.BackgroundExecute();
        UpdateTitle();
      }
      else
      {
        menuFileSaveAs_Click(sender,e);
      }
      
    
    }

    private void menuFileSaveAs_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      dlg.Filter = "Syncoco xml files (*.syncox)|*.syncox|All files (*.*)|*.*" ;
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        DocumentActions.SaveDocumentAction action = new SaveDocumentAction(Current.Document,dlg.FileName);
        action.BackgroundExecute();
        UpdateTitle();
      }
    }

    private void menuFileSaveFilterOnly_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      dlg.Filter = "Syncoco filter files (*.syncof)|*.syncof|All files (*.*)|*.*" ;
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        Current.Document.SaveFilterOnly(dlg.FileName);
      }
    }

    private void menuFileExit_Click(object sender, System.EventArgs e)
    {
      this.Close();
    }


    #endregion

    #region Action menu
    
    private void menuActions_Popup(object sender, System.EventArgs e)
    {
      bool hasFileName = Current.Document.HasFileName;

      menuEditCollect.Enabled = hasFileName;
      menuEditSynchronize.Enabled = hasFileName;
      menuEditCopyFiles.Enabled = hasFileName;
      menuEditCleanTransferDir.Enabled = hasFileName;
    }

    private void menuEditUpdate_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.DocumentUpdateAction(Current.Document,false).BackgroundExecute();

    }

    private void menuUpdateHash_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.DocumentUpdateAction(Current.Document,true).BackgroundExecute();
    }

    private void menuEditCleanTransferDir_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.ClearMediumDirectoryAction(Current.Document).BackgroundExecute();
    }

    private void menuEditCopyFiles_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.CopyFilesToMediumAction(Current.Document).BackgroundExecute();
    }

    private void menuEditCollect_Click(object sender, System.EventArgs e)
    {
      if(_syncList==null)
        _syncList = new SyncListController();

      DocumentActions.CollectFilesToSynchronizeAction action = new CollectFilesToSynchronizeAction(Current.Document);
      action.BackgroundExecute();
      _syncList.SetCollectors(action.CollectedFiles);

      ShowSyncList();
    
    }


    private void menuEditSyncronize_Click(object sender, System.EventArgs e)
    {
      _syncList.Synchronize();
      menuEditCollect_Click(sender, e);

    }

    #endregion

    #region Begin work menu

    private void menuBeginWork_Popup(object sender, System.EventArgs e)
    {
      this.menuBeginShowSyncFiles.Enabled = Current.Document.HasFileName;
      this.menuBeginSyncSelected.Enabled = Current.Document.HasFileName;
      this.menuBeginSaveCleanIfNeccessary.Enabled = Current.Document.HasFileName;
    }

    private void menuBeginShowSyncFiles_Click(object sender, System.EventArgs e)
    {
      menuEditCollect_Click(sender,e);
    }

    private void menuBeginSyncSelected_Click(object sender, System.EventArgs e)
    {
      menuEditSyncronize_Click(sender,e);
    }

    private void menuBeginSaveCleanIfNeccessary_Click(object sender, System.EventArgs e)
    {
      if(Current.Document.HasFileName)
        new DocumentActions.SaveDocumentAndCleanIfNecessaryAction(Current.Document,Current.Document.FileName).BackgroundExecute();    
    }


    #endregion 

    #region End work menu

    
    private void menuEndWork_Popup(object sender, System.EventArgs e)
    {
      menuEndUpdateSaveCopy.Enabled = Current.Document.HasFileName;
    }

   
    private void menuEndUpdateSaveCopy_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.DocumentUpdateSaveAndCopyAction(Current.Document).BackgroundExecute();

    }

    #endregion

    #region Help menu

    private void menuHelpSyncoco_Click(object sender, System.EventArgs e)
    {
      try
      {
        string name = System.Reflection.Assembly.GetEntryAssembly().Location;
        
        name = System.IO.Path.ChangeExtension(name,".chm");
        
        System.Diagnostics.Process.Start(name);
      }
      catch(Exception ex)
      {
        MessageBox.Show(this,ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
    }


    private void menuHelpAbout_Click(object sender, System.EventArgs e)
    {
      AboutDialog dlg = new AboutDialog();
      dlg.ShowDialog(this);
    }

    #endregion End help


    #endregion


    #region Other event handlers

    private void EhFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveCloseDocument(e);
    }

    private void EhDocumentDirtyChanged(object sender, System.EventArgs e)
    {
      UpdateTitle();
    }
    private void EhDocumentFileNameChanged(object sender, System.EventArgs e)
    {
      UpdateTitle();
    }

  
    #endregion

    private void Syncoco_Load(object sender, System.EventArgs e)
    {
      if(Current.InitialFileName!=null)
      {
        DocumentActions.OpenDocumentAction action = new OpenDocumentAction(Current.InitialFileName);
        action.BackgroundExecute();
        ExchangeCurrentDocument(action.Doc);
        Current.InitialFileName=null;
      }
    
    }


  
  

  

 
   

  }
}
