using System;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for OpenDocumentAction.
  /// </summary>
  public class OpenDocumentAction : AbstractDocumentAction
  {
    string _filename;

    public OpenDocumentAction(string filename, IBackgroundMonitor monitor)
      : base(null,monitor)
    {
      _filename = filename;
    }
    public OpenDocumentAction(string filename)
      : this(filename,null)
    {
    }

    public MainDocument Doc
    {
      get { return _doc; }
    }

    public override void BackgroundExecute()
    {
      _monitor = new ExternalDrivenTimeReportMonitor();
      base.BackgroundExecute ();
    }

    
    public override void DirectExecute()
    
    {
      if(System.IO.Path.GetExtension(_filename).ToLower()==".stcbin")
      {
        _doc = MainDocument.OpenAsBinary(_filename);
      }
      else
      {
        _doc = new MainDocument();
        _doc.OpenAsXML(_filename);
      }
    }

  }
}
