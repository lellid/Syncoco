using System;

namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for MD5SumFileNodesHashTableCollector.
  /// </summary>
  public class MD5SumFileNodesHashTableCollector
  {
    MD5SumFileNodesHashTable _table;
    DirectoryNode _rootDir;
    string _pathRoot;


    public MD5SumFileNodesHashTableCollector(DirectoryNode rootDir, string pathRoot, MD5SumFileNodesHashTable table)
    {
      System.Diagnostics.Debug.Assert(rootDir!=null);
      System.Diagnostics.Debug.Assert(table!=null);
      PathUtil.Assert_Abspath(pathRoot);

      _rootDir = rootDir;
      _table   = table;

      _pathRoot = pathRoot;

      
    }

    public void Traverse()
    {
      VisitDirectory(_rootDir,_pathRoot);
    }

    protected void VisitDirectory( DirectoryNode dir, string currentPath)
    {
#if DEBUG
      PathUtil.Assert_Abspath(currentPath);
#endif

      foreach(FileNode fnode in dir.Files)
      {
        fnode.FillMD5SumFileNodesHashTable(_table,PathUtil.Combine_Abspath_Filename(currentPath,fnode.Name));
      }
      foreach(DirectoryNode dnode in dir.Directories)
      {
        VisitDirectory(dnode,PathUtil.Combine_Abspath_Dirname(currentPath, dnode.Name));
      }
    }
  }
}
