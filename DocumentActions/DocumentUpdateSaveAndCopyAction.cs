
using System;
using SyncTwoCo.Filter;
using SyncTwoCo.Traversing;

namespace SyncTwoCo.DocumentActions
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
      new DocumentUpdateAction(_doc,false,_monitor).DirectExecute();

      _monitor.Report("Cleaning transfer medium ...");
      new ClearMediumDirectoryAction(_doc,_monitor).DirectExecute();

      _monitor.Report("Saving document ...");
      _doc.Save();

      _monitor.Report("Copy files to medium ...");
      new CopyFilesToMediumAction(_doc,_monitor).DirectExecute();
    }
  }
}
