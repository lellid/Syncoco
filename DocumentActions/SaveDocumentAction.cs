using System;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for OpenDocumentAction.
  /// </summary>
  public class SaveDocumentAction : AbstractDocumentAction
  {
    string _filename;

    public SaveDocumentAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter, string filename)
      : base(doc,monitor,reporter)
    {
      _filename = filename;
      _monitor = new ExternalDrivenTimeReportMonitor();
    }
    public SaveDocumentAction(MainDocument doc,string filename)
      : this(doc,null,null,filename)
    {
    }
  
   
    
    public override void DirectExecute()
    
    {
      _doc.Save(_filename);
    }

  }
}
