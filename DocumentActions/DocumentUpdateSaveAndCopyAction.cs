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
using Syncoco.Filter;
using Syncoco.Traversing;

namespace Syncoco.DocumentActions
{
  
  class DocumentUpdateSaveAndCopyAction : AbstractDocumentAction
  {
 
    public DocumentUpdateSaveAndCopyAction(MainDocument doc)
      : base(doc)
    {
    }

    public override void BackgroundExecute()
    {
      if(!_doc.HasFileName)
        throw new ApplicationException("This operation is possible only if the document has a file name");
    
      base.BackgroundExecute();
    }




    public override void DirectExecute()
    {
      if(!_doc.HasFileName)
        throw new ApplicationException("This operation is possible only if the document has a file name");

      _monitor.Report("Updating file system ...");
      new DocumentUpdateAction(_doc,false,_monitor,_reporter).DirectExecute();
      if(_monitor.CancelledByUser)
        return;


      _monitor.Report("Cleaning transfer medium ...");
      new ClearMediumDirectoryAction(_doc,_monitor,_reporter).DirectExecute();
      if(_monitor.CancelledByUser)
        return;


      _monitor.Report("Saving document ...");
      new SaveDocumentAction(_doc,_monitor,_reporter,_doc.FileName).DirectExecute();
      if(_monitor.CancelledByUser)
        return;


      _monitor.Report("Copy files to medium ...");
      new CopyFilesToMediumAction(_doc,_monitor,_reporter).DirectExecute();
    }
  }
}
