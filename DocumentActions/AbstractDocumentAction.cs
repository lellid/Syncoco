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
      _reporter.ReportBeginNewParagraph();

      System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.DirectExecute));
      thread.Name = this.GetType().Name;
      thread.IsBackground = true;
      thread.Start();
     
      if(!(_monitor is ExternalDrivenBackgroundMonitor))
        _monitor = new ExternalDrivenBackgroundMonitor();

      GUI.BackgroundCancelDialog dlg = new GUI.BackgroundCancelDialog(thread,(ExternalDrivenBackgroundMonitor)_monitor);
      dlg.ShowDialog(Current.MainForm);

      int nErrors =   _reporter.NumberOfErrors - oldErrors;
      int nWarnings = _reporter.NumberOfWarnings - oldWarnings;

      if(nErrors!=0 || nWarnings!=0)
      {
        System.Windows.Forms.MessageBox.Show(Current.MainForm,
          string.Format("Task {0} finished, {1} error(s), {2} warning(s). Please refer to the report for details!",this.GetType().Name,nErrors,nWarnings),
          "Errors & Warnings",
          System.Windows.Forms.MessageBoxButtons.OK,
          System.Windows.Forms.MessageBoxIcon.Exclamation);

        ((Syncoco)Current.MainForm).ShowReportList();
      }

      
    }

   
    public abstract void DirectExecute();

  }
}
