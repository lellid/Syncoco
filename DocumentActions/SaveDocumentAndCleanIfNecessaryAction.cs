using System;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// 
  /// </summary>
  public class SaveDocumentAndCleanIfNecessaryAction : AbstractDocumentAction
  {
    enum WorkMode { WithoutCleaning, WithCleaning };

    string _filename;
    WorkMode _mode = WorkMode.WithoutCleaning;
    string _saveerror=null;

    public SaveDocumentAndCleanIfNecessaryAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter, string filename)
      : base(doc,monitor,reporter)
    {
      _filename = filename;
      _monitor = new ExternalDrivenTimeReportMonitor();
    }
    public SaveDocumentAndCleanIfNecessaryAction(MainDocument doc,string filename)
      : this(doc,null,null,filename)
    {
    }
  
    public override void BackgroundExecute()
    {
      _mode = WorkMode.WithoutCleaning;
      base.BackgroundExecute ();

      if(_saveerror!=null)
      {
        _mode = WorkMode.WithCleaning;
        base.BackgroundExecute();
      }
    }

    
    public override void DirectExecute()
    {
      if(_mode==WorkMode.WithCleaning)
        new ClearMediumDirectoryAction(_doc,_monitor,_reporter).DirectExecute();

      try
      {
        _saveerror = null;
        _doc.Save(_filename);
      }
      catch(Exception ex)
      {
        _saveerror = ex.Message;
      }

      if(_saveerror!=null && _mode==WorkMode.WithCleaning)
        _reporter.ReportError(string.Format("Error saving the document: {0}", _saveerror));
    }
  }
}
