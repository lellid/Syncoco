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


namespace Syncoco.GUI
{
  /// <summary>
  /// Summary description for BackgroundCancelDialog.
  /// </summary>
  public class BackgroundCancelDialog : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.Button btCancel;
    private System.Timers.Timer _timer;
    System.Threading.Thread _thread;
    ExternalDrivenBackgroundMonitor _monitor;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public BackgroundCancelDialog(  System.Threading.Thread thread, ExternalDrivenBackgroundMonitor monitor)
    {
      _thread = thread;
      _monitor = monitor;
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
        if(components != null)
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
      this.lblText = new System.Windows.Forms.Label();
      this.btCancel = new System.Windows.Forms.Button();
      this._timer = new System.Timers.Timer();
      ((System.ComponentModel.ISupportInitialize)(this._timer)).BeginInit();
      this.SuspendLayout();
      // 
      // lblText
      // 
      this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lblText.Location = new System.Drawing.Point(8, 8);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(368, 56);
      this.lblText.TabIndex = 0;
      // 
      // btCancel
      // 
      this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btCancel.Location = new System.Drawing.Point(152, 72);
      this.btCancel.Name = "btCancel";
      this.btCancel.TabIndex = 1;
      this.btCancel.Text = "Cancel";
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // _timer
      // 
      this._timer.Enabled = true;
      this._timer.SynchronizingObject = this;
      this._timer.Elapsed += new System.Timers.ElapsedEventHandler(this._timer_Elapsed);
      // 
      // BackgroundCancelDialog
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(384, 98);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.lblText);
      this.Name = "BackgroundCancelDialog";
      this.Text = "Working...";
      this.Closing += new System.ComponentModel.CancelEventHandler(this.BackgroundCancelDialog_Closing);
      ((System.ComponentModel.ISupportInitialize)(this._timer)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

   
  
    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      if(!_thread.IsAlive)
      {
        this.Close();
      }

      this.lblText.Text = _monitor.ReportText;

      _monitor.ShouldReport = true;
    }


  
    private void btCancel_Click(object sender, System.EventArgs e)
    {
      _monitor.CancelledByUser = true;
    }

    private void BackgroundCancelDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(_thread.IsAlive)
        e.Cancel = true;
    }
  }
}
