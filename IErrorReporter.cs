using System;

namespace Syncoco
{
	/// <summary>
	/// Summary description for IErrorReporter.
	/// </summary>
	public interface IErrorReporter
	{
    void ReportBeginNewParagraph();
    void ReportError(string msg);
    void ReportWarning(string msg);
    int  NumberOfErrors { get; }
    int  NumberOfWarnings { get; }
    string ErrorText { get; }
	}

  class DefaultErrorReporter : IErrorReporter
  {
    System.Text.StringBuilder _text = new System.Text.StringBuilder();

    bool _newParagraphPending=true;
    int _numberOfErrors;
    int _numberOfWarnings;

    #region IErrorReporter Members

    public void ReportBeginNewParagraph()
    {
      if(_newParagraphPending)
      {
        _newParagraphPending = false;
        _text.Append("-------- " + DateTime.Now.ToLongTimeString() + " --------\n");
      }
    }

    public void ReportError(string msg)
    {
      _numberOfErrors++;
      _text.Append("Error: ");
      _text.Append(msg);
      _text.Append("\n");
    }

    public void ReportWarning(string msg)
    {
      _numberOfWarnings++;
      _text.Append("Error: ");
      _text.Append(msg);
      _text.Append("\n");
    }

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
      get
      {
        return _text.ToString();
      }
    }

    #endregion

  }

}
