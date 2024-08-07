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
  /// Summary description for IErrorReporter.
  /// </summary>
  public interface IErrorReporter
  {
    void ReportBeginNewParagraph();
    void ReportError(string msg);
    void ReportWarning(string msg);
    void ReportText(string msg);
    int NumberOfErrors { get; }
    int NumberOfWarnings { get; }
    string ErrorText { get; }
    int TextLength { get; }
  }

  internal class DefaultErrorReporter : IErrorReporter
  {
    private System.Text.StringBuilder _text = new System.Text.StringBuilder();
    private bool _newParagraphPending = true;
    private int _numberOfErrors;
    private int _numberOfWarnings;

    #region IErrorReporter Members

    public void ReportBeginNewParagraph()
    {
      if (_newParagraphPending)
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

    public void ReportText(string msg)
    {
      _text.Append(msg);
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


    public int TextLength
    {
      get
      {
        return _text.Length;
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
