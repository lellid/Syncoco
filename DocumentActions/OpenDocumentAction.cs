#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco:  synchronizing two computers with a data medium
//    Copyright (C) 2004-2005 Dr. Dirk Lellinger
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
      _monitor = new ExternalDrivenTimeReportMonitor();
    }
    public OpenDocumentAction(string filename)
      : this(filename,null)
    {
    }

    public MainDocument Doc
    {
      get { return _doc; }
    }
    
    public override void DirectExecute()
    
    {
      if(System.IO.Path.GetExtension(_filename).ToLower()==".sccbin")
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
