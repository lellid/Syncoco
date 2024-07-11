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

using Syncoco.DocumentActions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Syncoco.GUI
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class MainForm : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TabControl _tabControl;

    private System.Windows.Forms.MenuStrip mainMenu1;
    private System.Windows.Forms.ToolStripMenuItem menuFileNew;
    private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
    private System.Windows.Forms.ToolStripMenuItem menuFileExit;
    private System.Windows.Forms.ToolStripMenuItem menuFileSave;
    private System.Windows.Forms.ToolStripMenuItem menuEditUpdate;
    private System.Windows.Forms.ToolStripMenuItem menuEditCopyFiles;
    private System.Windows.Forms.ToolStripMenuItem menuEditCollect;
    private System.Windows.Forms.ToolStripMenuItem menuFileSaveAs;
    private System.Windows.Forms.ToolStripMenuItem menuBeginWork;
    private System.Windows.Forms.ToolStripMenuItem menuEndWork;
    private System.Windows.Forms.ToolStripMenuItem menuBeginShowSyncFiles;
    private System.Windows.Forms.ToolStripMenuItem menuBeginSyncSelected;
    private System.Windows.Forms.ToolStripMenuItem menuEndUpdateSaveCopy;
    private System.Windows.Forms.ToolStripMenuItem menuItem3;
    private System.Windows.Forms.ToolStripMenuItem menuUpdateHash;
    private System.Windows.Forms.ToolStripSeparator menuItem4;
    private System.Windows.Forms.ToolStripMenuItem menuHelp;
    private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
    private System.Windows.Forms.ToolStripMenuItem menuFile;
    private System.Windows.Forms.ToolStripMenuItem menuActions;
    private System.Windows.Forms.ToolStripSeparator menuItem1;
    private System.Windows.Forms.ToolStripMenuItem menuEditSynchronize;
    private System.Windows.Forms.ToolStripMenuItem menuEditCleanTransferDir;
    private System.Windows.Forms.ToolStripSeparator menuItem2;
    private System.Windows.Forms.ToolStripMenuItem menuBeginSaveCleanIfNeccessary;
    private System.Windows.Forms.ToolStripMenuItem menuHelpSyncoco;
    private System.Windows.Forms.ToolStripSeparator menuItem5;
    private System.Windows.Forms.ToolStripMenuItem menuEditDifferenceReport;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public MainForm()
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

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.mainMenu1 = new System.Windows.Forms.MenuStrip();
      this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
      this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
      this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
      this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
      this.menuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
      this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
      this.menuBeginWork = new System.Windows.Forms.ToolStripMenuItem();
      this.menuBeginShowSyncFiles = new System.Windows.Forms.ToolStripMenuItem();
      this.menuBeginSyncSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.menuBeginSaveCleanIfNeccessary = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEndWork = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEndUpdateSaveCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.menuActions = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditCollect = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditSynchronize = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditUpdate = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditCleanTransferDir = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditCopyFiles = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuUpdateHash = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEditDifferenceReport = new System.Windows.Forms.ToolStripMenuItem();
      this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
      this.menuHelpSyncoco = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem5 = new System.Windows.Forms.ToolStripSeparator();
      this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
      this._tabControl = new System.Windows.Forms.TabControl();
      this.mainMenu1.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenu1
      // 
      this.mainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuBeginWork,
            this.menuEndWork,
            this.menuActions,
            this.menuHelp});
      this.mainMenu1.Location = new System.Drawing.Point(0, 0);
      this.mainMenu1.Name = "mainMenu1";
      this.mainMenu1.Size = new System.Drawing.Size(200, 24);
      this.mainMenu1.TabIndex = 0;
      // 
      // menuFile
      // 
      this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveAs,
            this.menuItem3,
            this.menuItem4,
            this.menuFileExit});
      this.menuFile.Name = "menuFile";
      this.menuFile.Size = new System.Drawing.Size(37, 20);
      this.menuFile.Text = "File";
      this.menuFile.DropDownOpened += new System.EventHandler(this.menuFile_Popup);
      // 
      // menuFileNew
      // 
      this.menuFileNew.Name = "menuFileNew";
      this.menuFileNew.Size = new System.Drawing.Size(177, 22);
      this.menuFileNew.Text = "New";
      this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
      // 
      // menuFileOpen
      // 
      this.menuFileOpen.Name = "menuFileOpen";
      this.menuFileOpen.Size = new System.Drawing.Size(177, 22);
      this.menuFileOpen.Text = "Open";
      this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
      // 
      // menuFileSave
      // 
      this.menuFileSave.Name = "menuFileSave";
      this.menuFileSave.Size = new System.Drawing.Size(177, 22);
      this.menuFileSave.Text = "Save";
      this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
      // 
      // menuFileSaveAs
      // 
      this.menuFileSaveAs.Name = "menuFileSaveAs";
      this.menuFileSaveAs.Size = new System.Drawing.Size(177, 22);
      this.menuFileSaveAs.Text = "Save As..";
      this.menuFileSaveAs.Click += new System.EventHandler(this.menuFileSaveAs_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Name = "menuItem3";
      this.menuItem3.Size = new System.Drawing.Size(177, 22);
      this.menuItem3.Text = "Save Filter Only As..";
      this.menuItem3.Click += new System.EventHandler(this.menuFileSaveFilterOnly_Click);

      // 
      // menuFileExit
      // 
      this.menuFileExit.Name = "menuFileExit";
      this.menuFileExit.Size = new System.Drawing.Size(177, 22);
      this.menuFileExit.Text = "Exit";
      this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
      // 
      // menuBeginWork
      // 
      this.menuBeginWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBeginShowSyncFiles,
            this.menuBeginSyncSelected,
            this.menuItem2,
            this.menuBeginSaveCleanIfNeccessary});
      this.menuBeginWork.Name = "menuBeginWork";
      this.menuBeginWork.Size = new System.Drawing.Size(111, 20);
      this.menuBeginWork.Text = "Beginning work...";
      this.menuBeginWork.DropDownOpened += new System.EventHandler(this.menuBeginWork_Popup);
      // 
      // menuBeginShowSyncFiles
      // 
      this.menuBeginShowSyncFiles.Name = "menuBeginShowSyncFiles";
      this.menuBeginShowSyncFiles.Size = new System.Drawing.Size(216, 22);
      this.menuBeginShowSyncFiles.Text = "Show files to sync";
      this.menuBeginShowSyncFiles.Click += new System.EventHandler(this.menuBeginShowSyncFiles_Click);
      // 
      // menuBeginSyncSelected
      // 
      this.menuBeginSyncSelected.Name = "menuBeginSyncSelected";
      this.menuBeginSyncSelected.Size = new System.Drawing.Size(216, 22);
      this.menuBeginSyncSelected.Text = "Sync selected files";
      this.menuBeginSyncSelected.Click += new System.EventHandler(this.menuBeginSyncSelected_Click);
      // 
      // menuBeginSaveCleanIfNeccessary
      // 
      this.menuBeginSaveCleanIfNeccessary.Name = "menuBeginSaveCleanIfNeccessary";
      this.menuBeginSaveCleanIfNeccessary.Size = new System.Drawing.Size(216, 22);
      this.menuBeginSaveCleanIfNeccessary.Text = "SaveAndCleanIfNeccessary";
      this.menuBeginSaveCleanIfNeccessary.Click += new System.EventHandler(this.menuBeginSaveCleanIfNeccessary_Click);
      // 
      // menuEndWork
      // 
      this.menuEndWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEndUpdateSaveCopy});
      this.menuEndWork.Name = "menuEndWork";
      this.menuEndWork.Size = new System.Drawing.Size(94, 20);
      this.menuEndWork.Text = "Ending work...";
      this.menuEndWork.DropDownOpened += new System.EventHandler(this.menuEndWork_Popup);
      // 
      // menuEndUpdateSaveCopy
      // 
      this.menuEndUpdateSaveCopy.Name = "menuEndUpdateSaveCopy";
      this.menuEndUpdateSaveCopy.Size = new System.Drawing.Size(209, 22);
      this.menuEndUpdateSaveCopy.Text = "UpdateSaveAndCopyFiles";
      this.menuEndUpdateSaveCopy.Click += new System.EventHandler(this.menuEndUpdateSaveCopy_Click);
      // 
      // menuActions
      // 
      this.menuActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditCollect,
            this.menuEditSynchronize,
            this.menuEditUpdate,
            this.menuEditCleanTransferDir,
            this.menuEditCopyFiles,
            this.menuItem1,
            this.menuUpdateHash,
            this.menuEditDifferenceReport});
      this.menuActions.Name = "menuActions";
      this.menuActions.Size = new System.Drawing.Size(94, 20);
      this.menuActions.Text = "Single Actions";
      this.menuActions.DropDownOpened += new System.EventHandler(this.menuActions_Popup);
      // 
      // menuEditCollect
      // 
      this.menuEditCollect.Name = "menuEditCollect";
      this.menuEditCollect.Size = new System.Drawing.Size(202, 22);
      this.menuEditCollect.Text = "Show files to sync";
      this.menuEditCollect.Click += new System.EventHandler(this.menuEditCollect_Click);
      // 
      // menuEditSynchronize
      // 
      this.menuEditSynchronize.Name = "menuEditSynchronize";
      this.menuEditSynchronize.Size = new System.Drawing.Size(202, 22);
      this.menuEditSynchronize.Text = "Sync selected files";
      this.menuEditSynchronize.Click += new System.EventHandler(this.menuEditSyncronize_Click);
      // 
      // menuEditUpdate
      // 
      this.menuEditUpdate.Name = "menuEditUpdate";
      this.menuEditUpdate.Size = new System.Drawing.Size(202, 22);
      this.menuEditUpdate.Text = "Update";
      this.menuEditUpdate.Click += new System.EventHandler(this.menuEditUpdate_Click);
      // 
      // menuEditCleanTransferDir
      // 
      this.menuEditCleanTransferDir.Name = "menuEditCleanTransferDir";
      this.menuEditCleanTransferDir.Size = new System.Drawing.Size(202, 22);
      this.menuEditCleanTransferDir.Text = "Clean transfer directory";
      this.menuEditCleanTransferDir.Click += new System.EventHandler(this.menuEditCleanTransferDir_Click);
      // 
      // menuEditCopyFiles
      // 
      this.menuEditCopyFiles.Name = "menuEditCopyFiles";
      this.menuEditCopyFiles.Size = new System.Drawing.Size(202, 22);
      this.menuEditCopyFiles.Text = "Copy files to medium";
      this.menuEditCopyFiles.Click += new System.EventHandler(this.menuEditCopyFiles_Click);
      // 
      // menuUpdateHash
      // 
      this.menuUpdateHash.Name = "menuUpdateHash";
      this.menuUpdateHash.Size = new System.Drawing.Size(202, 22);
      this.menuUpdateHash.Text = "Force update hash sums";
      this.menuUpdateHash.Click += new System.EventHandler(this.menuUpdateHash_Click);
      // 
      // menuEditDifferenceReport
      // 
      this.menuEditDifferenceReport.Name = "menuEditDifferenceReport";
      this.menuEditDifferenceReport.Size = new System.Drawing.Size(202, 22);
      this.menuEditDifferenceReport.Text = "Difference report";
      this.menuEditDifferenceReport.Click += new System.EventHandler(this.menuEditDifferenceReport_Click);
      // 
      // menuHelp
      // 
      this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpSyncoco,
            this.menuItem5,
            this.menuHelpAbout});
      this.menuHelp.Name = "menuHelp";
      this.menuHelp.Size = new System.Drawing.Size(44, 20);
      this.menuHelp.Text = "Help";
      // 
      // menuHelpSyncoco
      // 
      this.menuHelpSyncoco.Name = "menuHelpSyncoco";
      this.menuHelpSyncoco.Size = new System.Drawing.Size(113, 22);
      this.menuHelpSyncoco.Text = "Index..";
      this.menuHelpSyncoco.Click += new System.EventHandler(this.menuHelpSyncoco_Click);

      // 
      // menuHelpAbout
      // 
      this.menuHelpAbout.Name = "menuHelpAbout";
      this.menuHelpAbout.Size = new System.Drawing.Size(113, 22);
      this.menuHelpAbout.Text = "About..";
      this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
      // 
      // _tabControl
      // 
      this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this._tabControl.Location = new System.Drawing.Point(0, 0);
      this._tabControl.Name = "_tabControl";
      this._tabControl.SelectedIndex = 0;
      this._tabControl.Size = new System.Drawing.Size(725, 514);
      this._tabControl.TabIndex = 0;
      this._tabControl.SelectedIndexChanged += new System.EventHandler(this._tabControl_SelectedIndexChanged);
      // 
      // MainForm
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(725, 514);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Controls.Add(this._tabControl);
      this.Controls.Add(this.mainMenu1);
      this.MainMenuStrip = this.mainMenu1;
      this.Name = "MainForm";
      this.Text = "Syncoco";
      this.Closing += new System.ComponentModel.CancelEventHandler(this.EhFormClosing);
      this.Load += new System.EventHandler(this.Syncoco_Load);
      this.mainMenu1.ResumeLayout(false);
      this.mainMenu1.PerformLayout();
      this.ResumeLayout(false);

    }
    #endregion


    private RootListController _rootList = new RootListController(Current.Document);
    private SyncListController _syncList = new SyncListController();
    private ReportListController _reportList = new ReportListController();


    public IErrorReporter ErrorReporter
    {
      get { return _reportList; }
    }


    public void ShowControl(string caption, UserControl ctrl)
    {

      // try to find appropriate tab page

      TabPage tabpage = null;
      for (int i = 0; i < _tabControl.TabPages.Count; i++)
      {
        if (_tabControl.TabPages[i].Controls[0] == ctrl)
        {
          tabpage = _tabControl.TabPages[i];
          break;
        }
      }

      if (tabpage == null)
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
      ShowControl("FilesToSync", _syncList.View);
    }

    public void ShowReportList()
    {
      ShowControl("Report", _reportList.View);
    }

    private delegate void StringSetter(string val);

    private void UpdateTitle()
    {
      string filename = Current.Document.HasFileName ? Current.Document.FileName : "Unnamed";
      string dirty = Current.Document.IsDirty ? "*" : string.Empty;
      string newTitle = string.Format("{1}{2} - Syncoco@{0}", Current.ComputerName, filename, dirty);

      if (InvokeRequired)
      {
        this.Invoke((MethodInvoker)delegate { this.Text = newTitle; });
      }
      else
      {
        this.Text = newTitle;
      }
    }

    public void ExchangeCurrentDocument(MainDocument newDoc)
    {
      if (newDoc == null)
      {
        return;
      }

      this._tabControl.TabPages.Clear();

      Current.Document.DirtyChanged -= new EventHandler(EhDocumentDirtyChanged);
      Current.Document.FileNameChanged -= new EventHandler(EhDocumentFileNameChanged);
      Current.Document = newDoc;
      Current.Document.DirtyChanged += new EventHandler(EhDocumentDirtyChanged);
      Current.Document.FileNameChanged += new EventHandler(EhDocumentFileNameChanged);
      this.UpdateTitle();
      _rootList = new RootListController(Current.Document);
      ShowControl("RootPairs", _rootList.View);

      if (this._reportList.ErrorText.Length > 0)
      {
        this.ShowReportList();
      }
    }



    private void SaveCloseDocument(System.ComponentModel.CancelEventArgs e)
    {
      while (Current.Document.IsDirty)
      {
        DialogResult boxresult = MessageBox.Show(this, "Your document is not saved yet. Do you want to save it now?", "Document not saved",
          MessageBoxButtons.YesNoCancel,
          MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button1);

        if (boxresult == DialogResult.Cancel)
        {
          e.Cancel = true;
          return;
        }
        else if (boxresult == DialogResult.No)
        {
          return;
        }
        else
        {
          this.menuFileSave_Click(this, EventArgs.Empty);
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

      if (ce.Cancel == false)
      {
        ExchangeCurrentDocument(new MainDocument());
      }
    }


    private void menuFileOpen_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
      dlg.Filter = "Syncoco xml files (*.syncox)|*.syncox|All files (*.*)|*.*";
      if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog(Current.MainForm))
      {
        DocumentActions.OpenDocumentAction action = new OpenDocumentAction(dlg.FileName);
        action.BackgroundExecute();

        ExchangeCurrentDocument(action.Doc);
      }
    }



    private void menuFileSave_Click(object sender, System.EventArgs e)
    {
      if (Current.Document.HasFileName)
      {
        DocumentActions.SaveDocumentAction action = new SaveDocumentAction(Current.Document, Current.Document.FileName);
        action.BackgroundExecute();
        UpdateTitle();
      }
      else
      {
        menuFileSaveAs_Click(sender, e);
      }


    }

    private void menuFileSaveAs_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      dlg.Filter = "Syncoco xml files (*.syncox)|*.syncox|All files (*.*)|*.*";
      if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog(Current.MainForm))
      {
        DocumentActions.SaveDocumentAction action = new SaveDocumentAction(Current.Document, dlg.FileName);
        action.BackgroundExecute();
        UpdateTitle();
      }
    }

    private void menuFileSaveFilterOnly_Click(object sender, System.EventArgs e)
    {
      System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
      dlg.Filter = "Syncoco filter files (*.syncof)|*.syncof|All files (*.*)|*.*";
      if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog(Current.MainForm))
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
      new DocumentActions.DocumentUpdateAction(Current.Document, false).BackgroundExecute();

    }

    private void menuUpdateHash_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.DocumentUpdateAction(Current.Document, true).BackgroundExecute();
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
      if (_syncList == null)
      {
        _syncList = new SyncListController();
      }

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

    private void menuEditDifferenceReport_Click(object sender, System.EventArgs e)
    {
      new DocumentActions.DifferenceReportAction(Current.Document).BackgroundExecute();
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
      menuEditCollect_Click(sender, e);
    }

    private void menuBeginSyncSelected_Click(object sender, System.EventArgs e)
    {
      menuEditSyncronize_Click(sender, e);
    }

    private void menuBeginSaveCleanIfNeccessary_Click(object sender, System.EventArgs e)
    {
      if (Current.Document.HasFileName)
      {
        new DocumentActions.SaveDocumentAndCleanIfNecessaryAction(Current.Document, Current.Document.FileName).BackgroundExecute();
      }
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

        name = System.IO.Path.ChangeExtension(name, ".chm");

        System.Diagnostics.Process.Start(name);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
      if (Current.InitialFileName != null)
      {
        DocumentActions.OpenDocumentAction action = new OpenDocumentAction(Current.InitialFileName);
        action.BackgroundExecute();
        ExchangeCurrentDocument(action.Doc);
        Current.InitialFileName = null;
      }

    }

    private void _tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      TabPage selected = _tabControl.SelectedTab;
      if (selected != null && selected.Controls.Count > 0)
      {
        selected.Controls[0].Focus();
      }
    }

  }
}
