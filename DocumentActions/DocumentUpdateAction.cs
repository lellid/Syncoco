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
      : this(doc,forceUpdateHash,null)
    {
    }

    public DocumentUpdateAction(MainDocument doc, bool forceUpdateHash, IBackgroundMonitor monitor)
      : base(doc,monitor)
    {
      _forceUpdateHash = forceUpdateHash;
      
    }

    public override void BackgroundExecute()
    {
      if(!_doc.HasFileName)
        throw new ApplicationException("This operation is possible only if the document has a file name");
    
      base.BackgroundExecute();
    }

   
    public override void DirectExecute()
    {
      _doc.ClearCachedMyFiles();

      _doc.EnsureAlignment();

      for(int i=0;i<_doc.Count;i++)
        Update(_doc.RootPair(i));
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
        new DirectoryUpdater(pathFilter,_monitor).Update(fileSystemRoot.DirectoryNode, dirinfo, _forceUpdateHash);
      else
        fileSystemRoot.DirectoryNode = new DirectoryUpdater(pathFilter,_monitor).NewDirectoryNode(dirinfo);
    }
  }
}
