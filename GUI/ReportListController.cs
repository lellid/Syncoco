using System;

namespace Syncoco
{
	/// <summary>
	/// Summary description for ReportListController.
	/// </summary>
	public class ReportListController : IErrorReporter
	{
    ReportListControl _view;

    public ReportListControl View { get { return _view; }}

    public ReportListController()
    {

      _view = new ReportListControl();
      _view.Controller = this;
    }

    #region IErrorReporter Members

    bool _newParagraphPending=true;
    public void ReportBeginNewParagraph()
    {
      _newParagraphPending = true;
    }

    private void WriteNewParagraph()
    {
      if(_newParagraphPending)
      {
        _newParagraphPending = false;
        Write("------- " + DateTime.Now.ToLongTimeString() + " ---------");
      }
    }

    private void Write(string msg)
    {
      _view.AppendText(msg);
    }

    public void ReportError(string msg)
    {
      _numberOfErrors++;

      WriteNewParagraph();
      Write("Error: " + msg);
      Write("\n");
    }

    public void ReportWarning(string msg)
    {
      _numberOfWarnings++;

      WriteNewParagraph();
      Write("Warning: " + msg);
      Write("\n");
    }

    int _numberOfWarnings;
    int _numberOfErrors;
    public int NumberOfErrors
    {
      get
      {
        return _numberOfErrors;
      }
    }

    public int NumberOfWarnings
    {
      get
      {
        return _numberOfWarnings;
      }
    }

    public string ErrorText 
    {
      get { return _view.GetText(); }
    }

    #endregion
  }
}
