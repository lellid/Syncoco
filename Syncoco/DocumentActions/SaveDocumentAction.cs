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

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for OpenDocumentAction.
  /// </summary>
  public class SaveDocumentAction : AbstractDocumentAction
  {
    private string _filename;

    public SaveDocumentAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter, string filename)
      : base(doc, monitor, reporter)
    {
      _filename = filename;
      _monitor = new ExternalDrivenTimeReportMonitor();
    }
    public SaveDocumentAction(MainDocument doc, string filename)
      : this(doc, null, null, filename)
    {
    }



    public override void DirectExecute()

    {

      _doc.Save(_filename);

      CheckParentHierarchyAction checkHierarchy = new CheckParentHierarchyAction(_doc, _monitor, _reporter);
      checkHierarchy.DirectExecute();
    }

  }
}
