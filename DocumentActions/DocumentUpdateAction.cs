using System;
using Syncoco.Filter;
using Syncoco.Traversing;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for Update.
  /// </summary>
  public class DocumentUpdateAction : AbstractDocumentAction
  {
    bool _forceUpdateHash=false;
    

    public DocumentUpdateAction(MainDocument doc, bool forceUpdateHash)
      : this(doc,forceUpdateHash,null,null)
    {
    }

    public DocumentUpdateAction(MainDocument doc, bool forceUpdateHash, IBackgroundMonitor monitor,IErrorReporter reporter)
      : base(doc,monitor,reporter)
    {
      _forceUpdateHash = forceUpdateHash;
      
    }

    public override void BackgroundExecute()
    {
      base.BackgroundExecute();
    }

   
    public override void DirectExecute()
    {
      _doc.SetDirty();

      _doc.ClearCachedMyFiles();

      _doc.EnsureAlignment();

      for(int i=0;i<_doc.Count;i++)
      {
        if(_doc.RootPair(i).MyRoot.IsValid)
        {
          Update(_doc.RootPair(i));
        }
        else
        {
          _reporter.ReportWarning(string.Format("RootPair[{0}] not updated because the path is invalid.",i));
        }
      }
    }

    // RootPair
    public void Update(RootPair rootPair)
    {
      rootPair.PathFilter.ResetCurrentDirectory();
      Update(rootPair.MyRoot, rootPair.PathFilter);
    }

    // FileSystemRoot
    public void Update(FileSystemRoot fileSystemRoot, PathFilter pathFilter)
    {
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(fileSystemRoot.FilePath);

      if(null!=fileSystemRoot.DirectoryNode)
        new DirectoryUpdater(pathFilter,_monitor,_reporter).Update(fileSystemRoot.DirectoryNode, dirinfo, _forceUpdateHash);
      else
        fileSystemRoot.DirectoryNode = new DirectoryUpdater(pathFilter,_monitor,_reporter).NewDirectoryNode(dirinfo);
    }
  }
}
