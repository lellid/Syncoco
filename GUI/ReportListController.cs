using System;

namespace Syncoco
{
	/// <summary>
	/// Summary description for ReportListController.
	/// </summary>
	public class ReportListController : IErrorReporter
	{
    ReportListControl _view;
    System.Text.StringBuilder _text = new System.Text.StringBuilder();

    public ReportListControl View { get { return _view; }}

    public ReportListController()
    {

      _view = new ReportListControl();
      _view.Controller = this;
    }


    int _oldNumberOfErrors;
    int _oldNumberOfWarnings;
    public void EhView_TimerTick()
    {
      if(this._numberOfErrors!=_oldNumberOfErrors || this._numberOfWarnings!=_oldNumberOfWarnings)
      {
        _oldNumberOfErrors=_numberOfErrors;
        _oldNumberOfWarnings=_numberOfWarnings;
        _view.SetText(_text.ToString());
      }
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
        Write("------- " + DateTime.Now.ToLongTimeString() + " ---------\r\n");
      }
    }

    private void Write(string msg)
    {
      _text.Append(msg);
    }

    public void ReportError(string msg)
    {
      _numberOfErrors++;

      WriteNewParagraph();
      Write("Error: " + msg);
      Write("\r\n");
    }

    public void ReportWarning(string msg)
    {
      _numberOfWarnings++;

      WriteNewParagraph();
      Write("Warning: " + msg);
      Write("\r\n");
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
      get { return _text.ToString(); }
    }

    #endregion
  }
}
