using System;
using System.Collections;
using System.Runtime.InteropServices;

using SyncTwoCo.Filter;
using SyncTwoCo.Traversing;


namespace SyncTwoCo.DocumentActions
{
  /// <summary>
  /// Summary description for CollectFilesToSynchronizeAction.
  /// </summary>
  public class ClearMediumDirectoryAction : AbstractDocumentAction
  {
   

    public ClearMediumDirectoryAction(MainDocument doc, IBackgroundMonitor monitor)
      : base(doc,monitor)
    {
    }
    public ClearMediumDirectoryAction(MainDocument doc)
      : this(doc,null)
    {
    }


    public override void BackgroundExecute()
    {
      if(!_doc.HasFileName)
        throw new ApplicationException("Must have a file name to know where the medium directory is");

      base.BackgroundExecute ();
    }


    public override void DirectExecute()
    {
   
      if(!_doc.HasFileName)
        throw new ApplicationException("Must have a file name to know where the medium directory is");

      PathUtil.Assert_Abspath(_doc.MediumDirectoryName);
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_doc.MediumDirectoryName);
      if(!dirinfo.Exists)
        return;

      System.IO.FileInfo[] fileinfos = dirinfo.GetFiles("X*.XXX");

      foreach(System.IO.FileInfo fileinfo in fileinfos)
      {
        if(_monitor.ShouldReport)
          _monitor.Report("Deleting file " + fileinfo.FullName);


        if(0!=(fileinfo.Attributes & System.IO.FileAttributes.ReadOnly))
        {
          fileinfo.Attributes &= ~(System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System);
        }
        fileinfo.Delete();
      }
    }

      

    
  }
}