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

namespace Syncoco.GUI
{
  using Filter;
  /// <summary>
  /// Summary description for FilterListControl.
  /// </summary>
  public class FilterListControl : System.Windows.Forms.UserControl
  {
    private System.Windows.Forms.ComboBox cbDefaultAction;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btMoveUp;
    private System.Windows.Forms.Button btMoveDown;
    private System.Windows.Forms.Button btChangeAction;
    private System.Windows.Forms.Button btDeletePath;
    private System.Windows.Forms.TextBox edPathToAdd;
    private System.Windows.Forms.ListView lvPathNames;
    private System.Windows.Forms.ColumnHeader chFilterPath;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.ImageList imgListImages;
    private System.Windows.Forms.Button btPathInclude;
    private System.Windows.Forms.Button btPathExclude;
    private System.Windows.Forms.Label label2;

    protected FilterListController _controller;

    public FilterListController Controller
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



    public FilterListControl()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call

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
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FilterListControl));
      this.lvPathNames = new System.Windows.Forms.ListView();
      this.chFilterPath = new System.Windows.Forms.ColumnHeader();
      this.btMoveUp = new System.Windows.Forms.Button();
      this.btMoveDown = new System.Windows.Forms.Button();
      this.btChangeAction = new System.Windows.Forms.Button();
      this.btDeletePath = new System.Windows.Forms.Button();
      this.cbDefaultAction = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.edPathToAdd = new System.Windows.Forms.TextBox();
      this.imgListImages = new System.Windows.Forms.ImageList(this.components);
      this.btPathInclude = new System.Windows.Forms.Button();
      this.btPathExclude = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lvPathNames
      // 
      this.lvPathNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lvPathNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                  this.chFilterPath});
      this.lvPathNames.ForeColor = System.Drawing.SystemColors.WindowText;
      this.lvPathNames.HideSelection = false;
      this.lvPathNames.LabelWrap = false;
      this.lvPathNames.Location = new System.Drawing.Point(8, 56);
      this.lvPathNames.Name = "lvPathNames";
      this.lvPathNames.Size = new System.Drawing.Size(248, 304);
      this.lvPathNames.SmallImageList = this.imgListImages;
      this.lvPathNames.TabIndex = 0;
      this.lvPathNames.View = System.Windows.Forms.View.Details;
      // 
      // chFilterPath
      // 
      this.chFilterPath.Text = "Filter path";
      this.chFilterPath.Width = 244;
      // 
      // btMoveUp
      // 
      this.btMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btMoveUp.Image")));
      this.btMoveUp.Location = new System.Drawing.Point(264, 56);
      this.btMoveUp.Name = "btMoveUp";
      this.btMoveUp.Size = new System.Drawing.Size(32, 32);
      this.btMoveUp.TabIndex = 1;
      this.btMoveUp.Click += new System.EventHandler(this.btMoveUp_Click);
      // 
      // btMoveDown
      // 
      this.btMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btMoveDown.Image")));
      this.btMoveDown.Location = new System.Drawing.Point(264, 96);
      this.btMoveDown.Name = "btMoveDown";
      this.btMoveDown.Size = new System.Drawing.Size(32, 32);
      this.btMoveDown.TabIndex = 2;
      this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
      // 
      // btChangeAction
      // 
      this.btChangeAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btChangeAction.Image = ((System.Drawing.Image)(resources.GetObject("btChangeAction.Image")));
      this.btChangeAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btChangeAction.Location = new System.Drawing.Point(264, 296);
      this.btChangeAction.Name = "btChangeAction";
      this.btChangeAction.Size = new System.Drawing.Size(112, 23);
      this.btChangeAction.TabIndex = 5;
      this.btChangeAction.Text = "Change action";
      this.btChangeAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btChangeAction.Click += new System.EventHandler(this.btChangeAction_Click);
      // 
      // btDeletePath
      // 
      this.btDeletePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btDeletePath.Image = ((System.Drawing.Image)(resources.GetObject("btDeletePath.Image")));
      this.btDeletePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btDeletePath.Location = new System.Drawing.Point(264, 336);
      this.btDeletePath.Name = "btDeletePath";
      this.btDeletePath.Size = new System.Drawing.Size(112, 23);
      this.btDeletePath.TabIndex = 6;
      this.btDeletePath.Text = "Delete";
      this.btDeletePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btDeletePath.Click += new System.EventHandler(this.btDeletePath_Click);
      // 
      // cbDefaultAction
      // 
      this.cbDefaultAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.cbDefaultAction.Location = new System.Drawing.Point(8, 384);
      this.cbDefaultAction.Name = "cbDefaultAction";
      this.cbDefaultAction.Size = new System.Drawing.Size(248, 21);
      this.cbDefaultAction.TabIndex = 7;
      this.cbDefaultAction.SelectedIndexChanged += new System.EventHandler(this.cbDefaultAction_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.Location = new System.Drawing.Point(8, 368);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(136, 16);
      this.label1.TabIndex = 8;
      this.label1.Text = "If not in the list then ...";
      // 
      // edPathToAdd
      // 
      this.edPathToAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.edPathToAdd.Location = new System.Drawing.Point(8, 24);
      this.edPathToAdd.Name = "edPathToAdd";
      this.edPathToAdd.Size = new System.Drawing.Size(248, 20);
      this.edPathToAdd.TabIndex = 9;
      this.edPathToAdd.Text = "";
      // 
      // imgListImages
      // 
      this.imgListImages.ImageSize = new System.Drawing.Size(16, 16);
      this.imgListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListImages.ImageStream")));
      this.imgListImages.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // btPathInclude
      // 
      this.btPathInclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btPathInclude.Image = ((System.Drawing.Image)(resources.GetObject("btPathInclude.Image")));
      this.btPathInclude.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPathInclude.Location = new System.Drawing.Point(264, 24);
      this.btPathInclude.Name = "btPathInclude";
      this.btPathInclude.Size = new System.Drawing.Size(64, 23);
      this.btPathInclude.TabIndex = 10;
      this.btPathInclude.Text = "Include";
      this.btPathInclude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btPathInclude.Click += new System.EventHandler(this.btPathInclude_Click);
      // 
      // btPathExclude
      // 
      this.btPathExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btPathExclude.Image = ((System.Drawing.Image)(resources.GetObject("btPathExclude.Image")));
      this.btPathExclude.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btPathExclude.Location = new System.Drawing.Point(336, 24);
      this.btPathExclude.Name = "btPathExclude";
      this.btPathExclude.Size = new System.Drawing.Size(64, 23);
      this.btPathExclude.TabIndex = 11;
      this.btPathExclude.Text = "Exclude";
      this.btPathExclude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btPathExclude.Click += new System.EventHandler(this.btPathExclude_Click);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(360, 16);
      this.label2.TabIndex = 12;
      this.label2.Text = "Path (may be contain joker chars) :";
      // 
      // FilterListControl
      // 
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btPathExclude);
      this.Controls.Add(this.btPathInclude);
      this.Controls.Add(this.edPathToAdd);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbDefaultAction);
      this.Controls.Add(this.btDeletePath);
      this.Controls.Add(this.btChangeAction);
      this.Controls.Add(this.btMoveDown);
      this.Controls.Add(this.btMoveUp);
      this.Controls.Add(this.lvPathNames);
      this.Name = "FilterListControl";
      this.Size = new System.Drawing.Size(408, 416);
      this.ResumeLayout(false);

    }
    #endregion

    public void InitializePathList(FilterList filterList, int[] selectedIndices)
    {
      this.lvPathNames.BeginUpdate();

      this.lvPathNames.Items.Clear();
      foreach (FilterItem item in filterList)
      {
        lvPathNames.Items.Add(" " + item.MatchString, (int)item.Action);
      }

      if (selectedIndices != null)
      {
        for (int i = 0; i < selectedIndices.Length; i++)
        {
          lvPathNames.Items[selectedIndices[i]].Selected = true;
        }
      }

      this.lvPathNames.EndUpdate();
    }


    public void InitializeDefaultAction(FilterAction action)
    {
      this.cbDefaultAction.Items.Clear();
      this.cbDefaultAction.Items.Add("Include");
      this.cbDefaultAction.Items.Add("Exclude");
      this.cbDefaultAction.Items.Add("Ignore");

      this.cbDefaultAction.SelectedIndex = (int)action;
    }

    private void btMoveDown_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_MoveDown(GetIndexArray(this.lvPathNames.SelectedIndices));
      }
    }

    private void btMoveUp_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_MoveUp(GetIndexArray(this.lvPathNames.SelectedIndices));
      }
    }

    private void rbPathInclude_CheckedChanged(object sender, System.EventArgs e)
    {

    }

    private void rbPathExclude_CheckedChanged(object sender, System.EventArgs e)
    {

    }

    private int[] GetIndexArray(System.Windows.Forms.ListView.SelectedIndexCollection coll)
    {
      int[] res = new int[coll.Count];
      coll.CopyTo(res, 0);
      return res;
    }



    private void btChangeAction_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_ChangeAction(GetIndexArray(this.lvPathNames.SelectedIndices));
      }
    }

    private void btDeletePath_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_DeletePath(GetIndexArray(this.lvPathNames.SelectedIndices));
      }
    }

    private void cbDefaultAction_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_DefaultActionChanged(cbDefaultAction.SelectedIndex);
      }
    }

    private void btPathInclude_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_AddPath(this.edPathToAdd.Text, true);
      }
    }

    private void btPathExclude_Click(object sender, System.EventArgs e)
    {
      if (null != Controller)
      {
        Controller.EhView_AddPath(this.edPathToAdd.Text, false);
      }
    }
  }
}
