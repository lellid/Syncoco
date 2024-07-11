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
  /// Summary description for CollectFilesToSynchronizeAction.
  /// </summary>
  public class ClearMediumDirectoryAction : AbstractDocumentAction
  {


    public ClearMediumDirectoryAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
      : base(doc, monitor, reporter)
    {
    }
    public ClearMediumDirectoryAction(MainDocument doc)
      : this(doc, null, null)
    {
    }


    public override void BackgroundExecute()
    {
      if (!_doc.HasFileName)
        throw new ApplicationException("Must have a file name to know where the medium directory is");

      base.BackgroundExecute();
    }


    public override void DirectExecute()
    {

      if (!_doc.HasFileName)
        throw new ApplicationException("Must have a file name to know where the medium directory is");

      PathUtil.Assert_Abspath(_doc.MediumDirectoryName);
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_doc.MediumDirectoryName);
      if (!dirinfo.Exists)
        return;

      System.IO.FileInfo[] fileinfos = dirinfo.GetFiles("X*.XXX");

      foreach (System.IO.FileInfo fileinfo in fileinfos)
      {
        if (_monitor.CancelledByUser)
          return;

        if (_monitor.ShouldReport)
          _monitor.Report("Deleting file " + fileinfo.FullName);

        if (0 != (fileinfo.Attributes & System.IO.FileAttributes.ReadOnly))
        {
          fileinfo.Attributes &= ~(System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
        }
        try
        {
          fileinfo.Delete();
        }
        catch (Exception ex)
        {
          _reporter.ReportError(string.Format("deleting file {0} : {1}", fileinfo.FullName, ex.Message));
        }
      }
    }
  }
}
