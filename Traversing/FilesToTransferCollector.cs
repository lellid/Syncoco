using System;
using System.Collections;
using SyncTwoCo;
namespace SyncTwoCo.Traversing
{
	/// <summary>
	/// Summary description for FilesToTransferCollector.
	/// </summary>
	public class FilesToTransferCollector
	{
    DirectoryNode _myDirRoot;
    DirectoryNode _foreignDirRoot;

    SortedList list;

		public FilesToTransferCollector(DirectoryNode myDir, DirectoryNode foreignDir)
		{
			_myDirRoot = myDir;
      _foreignDirRoot = foreignDir;
		}

    public void Traverse()
    {
      list = new SortedList();
      GetNewOrChangedFiles(_myDirRoot,_foreignDirRoot,System.IO.Path.DirectorySeparatorChar.ToString());
    }

    public SortedList Result
    {
      get { return list; }
    }

    /// <summary>
    /// Collects all files that should be copied to the transfermedium. We presume that the own file system was
    /// updated before. That is why no PathFilter is neccessary here.
    /// </summary>
    /// <param name="myDir">Own directory node.</param>
    /// <param name="foreignDir">Corresponding directory node of the foreign system.</param>
    /// <param name="list">List in which the files are collected that should be copied to the transfer medium.</param>
    /// <param name="nameroot"></param>
    protected void GetNewOrChangedFiles(DirectoryNode myDir, DirectoryNode foreignDir, string nameroot)
    {
#if DEBUG
      PathUtil.Assert_Relpath(nameroot);
#endif

      foreach(FileNode myFileNode in myDir.Files)
      {
        string   myFileName = myFileNode.Name;
        
        if(myFileNode.IsDataContentNewOrChanged)
        {
          // before adding them to the list, make sure that our node don't
          // have the same content as the foreign node
          if(foreignDir!=null && foreignDir.ContainsFile(myFileName) && myFileNode.HasSameHashThan(foreignDir.File(myFileName)))
          {
            // Do nothing here, don't change the nodes, since this is done during the sync stage
          }
          else
          {
            list.Add(PathUtil.Combine_Relpath_Filename(nameroot,myFileName),myFileNode);
          }
        }
        else if(myFileNode.IsUnchanged && (foreignDir==null || !foreignDir.ContainsFile(myFileName)))
        {
          list.Add(PathUtil.Combine_Relpath_Filename(nameroot,myFileName),myFileNode);
        }
      }

      foreach(DirectoryNode mySubDirNode in myDir.Directories)
      {
        string mySubDirName = mySubDirNode.Name;

        GetNewOrChangedFiles(
          mySubDirNode,
          foreignDir==null ? null : foreignDir.Directory(mySubDirName),
          PathUtil.Combine_Relpath_Dirname(nameroot,mySubDirName));
      }
    }
	}
}
