using System;

namespace SyncTwoCo.Traversing
{
	/// <summary>
	/// Summary description for Md5HashTableCollector.
	/// </summary>
	public class Md5HashTableCollector
	{
    DirectoryNode _rootDir;
    MD5SumHashTable _table;
    string _pathRoot;

		public Md5HashTableCollector(DirectoryNode rootDir, string pathRoot, MD5SumHashTable table)
		{
#if DEBUG
      PathUtil.Assert_Abspath(pathRoot);
#endif
      
      _rootDir = rootDir;
      _table = table;
      _pathRoot = pathRoot;

		}


    public void Traverse()
    {
      VisitDirectory(_rootDir,_pathRoot);
    }



    protected void VisitDirectory(DirectoryNode dir, string currentPath)
    {
#if DEBUG
      PathUtil.Assert_Abspath(currentPath);
#endif

      foreach(FileNode fnode in dir.Files)
      {        
        fnode.FillMd5HashTable(_table,currentPath+fnode.Name);
      }
      foreach(DirectoryNode dnode in dir.Directories)
      {
        VisitDirectory(dnode,PathUtil.Combine_Abspath_Dirname(currentPath, dnode.Name));
      }
    }
	}
}
