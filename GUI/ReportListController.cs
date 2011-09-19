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


    int _oldTextLength;
    public void EhView_TimerTick()
    {
      if(this.TextLength != _oldTextLength)
      {
        _oldTextLength = this.TextLength;
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

    public void ReportText(string msg)
    {
      WriteNewParagraph();
      Write(msg);
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

    public int TextLength
    {
      get { return _text.Length; }
    }

    public string ErrorText 
    {
      get { return _text.ToString(); }
    }

    #endregion
  }
}
