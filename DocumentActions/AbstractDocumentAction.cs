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


using System;
using Syncoco.Filter;
using Syncoco.Traversing;
namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for AbstractAction.
  /// </summary>
  public abstract class AbstractDocumentAction
  {
    protected IBackgroundMonitor _monitor;
    protected IErrorReporter _reporter;
    protected MainDocument _doc;

    public AbstractDocumentAction(MainDocument doc)
      : this(doc,null,null)
    {
    }

    public AbstractDocumentAction(MainDocument doc, IBackgroundMonitor monitor)
      : this(doc,monitor,null)
    {
    }


    public AbstractDocumentAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
    {
      _doc = doc;
      _monitor = monitor!=null ? monitor : new DummyBackgroundMonitor();
      _reporter = reporter!=null ? reporter : Current.ErrorReporter;
    }


    public string Errors
    {
      get
      {
        return _reporter.ErrorText;
      }
    }

    public virtual void BackgroundExecute()
    {
      int oldErrors = _reporter.NumberOfErrors;
      int oldWarnings = _reporter.NumberOfWarnings;
      int oldTextLength = _reporter.TextLength;

      _reporter.ReportBeginNewParagraph();

      System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.CatchedDirectExecute));
      thread.Name = this.GetType().Name;
      thread.IsBackground = true;
      thread.Start();
     
      if(!(_monitor is ExternalDrivenBackgroundMonitor))
        _monitor = new ExternalDrivenBackgroundMonitor();

      GUI.BackgroundCancelDialog dlg = new GUI.BackgroundCancelDialog(thread,(ExternalDrivenBackgroundMonitor)_monitor);
      dlg.ShowDialog(Current.MainForm);

      int nErrors =   _reporter.NumberOfErrors - oldErrors;
      int nWarnings = _reporter.NumberOfWarnings - oldWarnings;

      if(_reporter.TextLength != oldTextLength)
        ShowErrorsAndWarnings(nErrors,nWarnings);
    }


    protected virtual void ShowErrorsAndWarnings(int nErrors, int nWarnings)
    {
      if(nErrors!=0 || nWarnings!=0)
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm,
          string.Format("Task {0} finished, {1} error(s), {2} warning(s). Please refer to the report for details!",this.GetType().Name,nErrors,nWarnings),
          "Errors & Warnings",
          System.Windows.Forms.MessageBoxButtons.OK,
          System.Windows.Forms.MessageBoxIcon.Exclamation);
      }
      ((Syncoco)Current.MainForm).ShowReportList();
    }

    protected virtual void CatchedDirectExecute()
    {
      try
      {
        DirectExecute();
      }
      catch(Exception ex)
      {
        _reporter.ReportError(string.Format("Unexpeced exception occured during action {0}: {1}",this.GetType().ToString(),ex.ToString()));
      }
    }
   
    public abstract void DirectExecute();

  }
}
