using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Syncoco
{
	/// <summary>
	/// Summary description for ReportListControl.
	/// </summary>
	public class ReportListControl : System.Windows.Forms.UserControl
	{
    private System.Windows.Forms.TextBox textBox1;
    ReportListController _controller;
    private System.Windows.Forms.Timer timer1;
    private System.ComponentModel.IContainer components;

		public ReportListControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

    public ReportListController Controller
    {
      get { return _controller; }
      set
      { 
        _controller = value;
        if(_controller!=null)
          timer1.Start();
      }
    }

    public void SetText(string msg)
    {
      textBox1.Text= msg;
    }

    public string GetText()
    {
      return textBox1.Text;
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
      this.components = new System.ComponentModel.Container();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(8, 8);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(392, 424);
      this.textBox1.TabIndex = 0;
      this.textBox1.Text = "";
      // 
      // timer1
      // 
      this.timer1.Interval = 1000;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // ReportListControl
      // 
      this.Controls.Add(this.textBox1);
      this.Name = "ReportListControl";
      this.Size = new System.Drawing.Size(408, 440);
      this.ResumeLayout(false);

    }
		#endregion

    private void timer1_Tick(object sender, System.EventArgs e)
    {
      if(null!=_controller)
        _controller.EhView_TimerTick();
    }
	}
}
