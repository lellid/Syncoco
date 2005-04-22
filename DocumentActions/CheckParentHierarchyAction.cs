using System;

namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for CheckParentHierarchyAction.
  /// </summary>
  public class CheckParentHierarchyAction : AbstractDocumentAction
  {
    string _filename;

    public CheckParentHierarchyAction(MainDocument doc, IBackgroundMonitor monitor, IErrorReporter reporter)
      : base(doc,monitor,reporter)
    {
    }
    public CheckParentHierarchyAction(MainDocument doc)
      : this(doc,null,null)
    {
    }
  
   
    
    public override void DirectExecute()
    {
      for(int i=0;i<_doc.Count;i++)
      {
        Check(_doc.RootPair(i).MyRoot);
        Check(_doc.RootPair(i).ForeignRoot);
      }
    }

    public void Check(FileSystemRoot fsRoot)
    {
      if(fsRoot==null)
        return;
      if(fsRoot.DirectoryNode==null)
        return;

      System.Diagnostics.Debug.Assert(object.ReferenceEquals(fsRoot.DirectoryNode.ParentDirectory,fsRoot));
      Check(fsRoot.DirectoryNode);
    }

    public void Check(DirectoryNode dirNode)
    {
      // Check files
      foreach(FileNode fNode in dirNode.Files)
      {
        System.Diagnostics.Debug.Assert(object.ReferenceEquals(fNode.Parent,dirNode));
      }

      // Check subdirectories (flat)
      foreach(DirectoryNode dNode in dirNode.Directories)
      {
        System.Diagnostics.Debug.Assert(object.ReferenceEquals(dNode.ParentDirectory,dirNode));
      }

      // Check subdirectories (recursive)
      foreach(DirectoryNode dNode in dirNode.Directories)
      {
        Check(dNode);
      }

    }
  }
}
