using System;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for OpenDocumentAction.
  /// </summary>
  public class SaveDocumentAction : AbstractDocumentAction
  {
    string _filename;

    public SaveDocumentAction(MainDocument doc, IBackgroundMonitor monitor, string filename)
      : base(doc,monitor)
    {
      _filename = filename;
    }
    public SaveDocumentAction(MainDocument doc,string filename)
      : this(doc,null,filename)
    {
    }
  
    public override void BackgroundExecute()
    {
      _monitor = new ExternalDrivenTimeReportMonitor();
      base.BackgroundExecute ();
    }

    
    public override void DirectExecute()
    
    {
      _doc.Save(_filename);
    }

  }
}
