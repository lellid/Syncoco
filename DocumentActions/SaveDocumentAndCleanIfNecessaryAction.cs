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

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// 
  /// </summary>
  public class SaveDocumentAndCleanIfNecessaryAction : AbstractDocumentAction
  {
    enum WorkMode { WithoutCleaning, WithCleaning };

    string _filename;
    WorkMode _mode = WorkMode.WithoutCleaning;
    string _saveerror=null;

    public SaveDocumentAndCleanIfNecessaryAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter, string filename)
      : base(doc,monitor,reporter)
    {
      _filename = filename;
      _monitor = new ExternalDrivenTimeReportMonitor();
    }
    public SaveDocumentAndCleanIfNecessaryAction(MainDocument doc,string filename)
      : this(doc,null,null,filename)
    {
    }
  
    public override void BackgroundExecute()
    {
      _mode = WorkMode.WithoutCleaning;
      base.BackgroundExecute ();

      if(_saveerror!=null)
      {
        _mode = WorkMode.WithCleaning;
        base.BackgroundExecute();
      }
    }

    
    public override void DirectExecute()
    {
      if(_mode==WorkMode.WithCleaning)
      {
        new ClearMediumDirectoryAction(_doc,_monitor,_reporter).DirectExecute();
        if(_monitor.CancelledByUser)
          return;
      }

      try
      {
        _saveerror = null;
        _doc.Save(_filename);
      }
      catch(Exception ex)
      {
        _saveerror = ex.Message;
      }

      if(_saveerror!=null && _mode==WorkMode.WithCleaning)
        _reporter.ReportError(string.Format("Error saving the document: {0}", _saveerror));
    }
  }
}
