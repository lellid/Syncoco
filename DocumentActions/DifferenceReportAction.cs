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
using System.Collections;

namespace Syncoco.DocumentActions
{
	/// <summary>
	/// Summary description for DifferenceReportAction.
	/// </summary>
	public class DifferenceReportAction  : AbstractDocumentAction
	{
    public DifferenceReportAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
    : base(doc,monitor,reporter)
		{
		}

    public DifferenceReportAction(MainDocument doc)
      : this(doc,null,null)
    {
    }


    public override void DirectExecute()
    {
      _reporter.ReportBeginNewParagraph();
      for(int i=0;i<_doc.Count;i++)
      {
        Traversing.DifferenceCollector coll = 
          new Traversing.DifferenceCollector(_doc.RootPair(i).MyRoot.DirectoryNode,
          _doc.RootPair(i).ForeignRoot.DirectoryNode);

        coll.Traverse();

        _reporter.ReportText(coll.GetReport());
      }
      _reporter.ReportText("End of difference report\n");
    }

	}
}
