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
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ReportListControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

    public ReportListController Controller
    {
      get { return _controller; }
      set { _controller = value; }
    }

    public void AppendText(string msg)
    {
      textBox1.AppendText(msg);
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
      this.textBox1 = new System.Windows.Forms.TextBox();
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
      // ReportListControl
      // 
      this.Controls.Add(this.textBox1);
      this.Name = "ReportListControl";
      this.Size = new System.Drawing.Size(408, 440);
      this.ResumeLayout(false);

    }
		#endregion
	}
}
