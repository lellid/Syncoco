using System;
using System.IO;
using System.Collections;
using Syncoco.Filter;

namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for DirectoryUpdater.
  /// </summary>
  public class DirectoryUpdater
  {
    PathFilter pathFilter;
    IBackgroundMonitor _monitor = new DummyBackgroundMonitor();
    IErrorReporter _reporter = new DefaultErrorReporter();

    public DirectoryUpdater(PathFilter filt)
    {
      pathFilter = filt;
    }

    public DirectoryUpdater(PathFilter filt, IBackgroundMonitor monitor, IErrorReporter reporter)
    {
      pathFilter = filt;
      _monitor = monitor;
      _reporter = reporter;
    }

   

    /// <summary>
    /// This updates a file node for an existing (!) file. The intermediate subdirectory nodes that lie between dirinfo and fileinfo are created if neccessary.
    /// </summary>
    /// <param name="dirnode">Starting directory node.</param>
    /// <param name="dirinfo">The directory info corresponding to the starting directory node.</param>
    /// <param name="fileinfo">The file info of an existing file.</param>
    /// <param name="forceUpdateHash">If true, the MD5 hash for the file is recalculated.</param>
    public static FileNode UpdateFileNode(DirectoryNode dirnode, DirectoryInfo dirinfo, FileInfo fileinfo, bool forceUpdateHash, IErrorReporter reporter)
    {
      System.Diagnostics.Debug.Assert(fileinfo.Exists,"This function is only intended for existing files after copy operations");
      System.Diagnostics.Debug.Assert(dirinfo.Exists,"The root directory must exist");

      string relativefullname;
      bool isRooted = PathUtil.HasRootPath(dirinfo.FullName,fileinfo.FullName,out relativefullname);

      string[] subdirs = PathUtil.GetDirectories(relativefullname);

      for(int i=0;i<subdirs.Length;i++)
      {
        if(subdirs[i]==string.Empty)
          continue; // if path accidentally contains more than one DirectorySeparatorChar consecutively
        
        dirinfo = new DirectoryInfo(Path.Combine(dirinfo.FullName,subdirs[i]));
        if(!dirinfo.Exists)
          throw new System.IO.IOException(string.Format("The directory {0} should exist, since it should be a root directory of the file {1}", dirinfo.FullName,fileinfo.FullName));

        if(!dirnode.ContainsDirectory(subdirs[i]))
          dirnode.AddSubDirectory(subdirs[i],new DirectoryNode(subdirs[i]));

        dirnode = dirnode.Directory(subdirs[i]);
      }

      // now we have the final directory node to which the file belongs
      UpdateFile(dirnode,fileinfo,forceUpdateHash,reporter);

      return dirnode.File(fileinfo.Name);
    }

    /// <summary>
    /// Updates a file in this directory. Make sure that the file really belongs to this directory!
    /// </summary>
    /// <param name="fileinfo">The file info for this file.</param>
    /// <param name="forceUpdateHash">If true, the hash is recalculated if the file exists.</param>
    public static void UpdateFile(DirectoryNode dirNode, System.IO.FileInfo fileinfo, bool forceUpdateHash, IErrorReporter reporter)
    {
      try
      {
        if(dirNode.ContainsFile(fileinfo.Name))
        {
          if(fileinfo.Exists)
            dirNode.File(fileinfo.Name).Update(fileinfo, forceUpdateHash);
          else
            dirNode.File(fileinfo.Name).SetToRemoved();
        }
        else if(fileinfo.Exists) // if file was not in the database yet
        {
          dirNode.AddFile(fileinfo.Name,new FileNode(fileinfo,dirNode));
        }

      }
      catch(HashCalculationException ex)
      {
        reporter.ReportError(ex.Message);
        return;
      }
    }

    /// <summary>
    /// This updates the directory given by dirinfo including all files and all subdirectories in this
    /// directory. The current path in pathFilter has to match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">The directory that has to be updated.</param>
    /// <param name="pathFilter">The path filter.</param>
    public void Update(DirectoryNode dirNode, System.IO.DirectoryInfo dirinfo, bool forceUpdateHash)
    {
      System.Diagnostics.Debug.Assert(dirinfo!=null);
      System.Diagnostics.Debug.Assert(dirNode.ParentDirectory==null || dirNode.ParentDirectory.IsFileSystemRoot || dirNode.Name==dirinfo.Name);

      //if(dirinfo!=null)
      //  dirNode.SetName( dirinfo.Name );

      UpdateFiles(dirNode, dirinfo,forceUpdateHash);
      UpdateDirectories(dirNode, dirinfo,forceUpdateHash);
    }

   

    

    /// <summary>
    /// Updates all files and subdirectory nodes in an existing(!) directory of the own system. 
    /// This function is recursively called for all files and subdirectories in the child nodes.
    /// PathFilter decides, wheter or not a file or subdirectory is included or ignored.
    /// </summary>
    /// <param name="dirinfo">The directory info of the current directory node.</param>
    /// <param name="pathFilter">The path filter.</param>
    protected void UpdateFiles(DirectoryNode dirNode, System.IO.DirectoryInfo dirinfo, bool forceUpdateHash)
    {
      if(_monitor.ShouldReport)
        _monitor.Report("Updating directory " + dirinfo.FullName);

      System.IO.FileInfo[] fileinfos = dirinfo.GetFiles();
      // create a hash table of the actual
      Hashtable actualFiles = new Hashtable();
      foreach(System.IO.FileInfo inf in fileinfos)
        actualFiles.Add(inf.Name,inf);

      // first look for the removed files
      System.Collections.Specialized.StringCollection filesToRemoveSilently = new System.Collections.Specialized.StringCollection();
      foreach(FileNode file in dirNode.Files)
      {
        if(pathFilter.IsFileIncluded(file.Name))
        {
          if(!actualFiles.ContainsKey(file.Name))
            dirNode.File(file.Name).SetToRemoved();
        }
        else // pathFilter (now) rejects this file
        {
          filesToRemoveSilently.Add(file.Name); // remove it silently from the list
        }
      }
      foreach(string file in filesToRemoveSilently)
        dirNode.Files.Remove(file);
      

      // now look for the new or the changed files
      foreach(string file in actualFiles.Keys)
      {
        if(!pathFilter.IsFileIncluded(file))
          continue;

        if(_monitor.CancelledByUser)
          break;

        System.IO.FileInfo fileinfo = (System.IO.FileInfo)actualFiles[file];

        if(_monitor.ShouldReport)
          _monitor.Report("Updating file " + fileinfo.FullName);

        if(dirNode.ContainsFile(file))
        {
          dirNode.File(file).Update(fileinfo,forceUpdateHash);
          // this file was here before, we look if it was changed
        }
        else
        {
          // this is a new file, we create a new file node for this
          dirNode.AddFile(file,new FileNode(fileinfo,dirNode));
        }
      }
    }


    /// <summary>
    /// Updates all directories in the directory given by dirinfo. The current path in pathFilter must
    /// match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">The directory which is to be updated.</param>
    /// <param name="pathFilter">The path filter.</param>
    protected void UpdateDirectories(DirectoryNode dirNode, System.IO.DirectoryInfo dirinfo, bool forceUpdateHash)
    {
      System.IO.DirectoryInfo[] dirinfos = dirinfo.GetDirectories();
      // create a hash table of the actual
      Hashtable actualDirs = new Hashtable();
      foreach(System.IO.DirectoryInfo inf in dirinfos)
        actualDirs.Add(inf.Name,inf);

      // first look for the removed dirs
      System.Collections.Specialized.StringCollection subDirsToRemoveSilently = new System.Collections.Specialized.StringCollection();
      foreach(DirectoryNode subdir in dirNode.Directories)
      {
        if(pathFilter.IsDirectoryIncluded(subdir.Name))
        {
          if(!actualDirs.ContainsKey(subdir.Name))
            dirNode.Directory(subdir.Name).SetToRemoved();
          else
            dirNode.Directory(subdir.Name).IsRemoved = false;
        }
        else  // pathFilter now rejects this directory, so remove it silently (but not set it to removed - this would cause the
        {     // directory on the foreign system to be removed also)
          subDirsToRemoveSilently.Add(subdir.Name);
        }
      }
      foreach(string name in subDirsToRemoveSilently)
        dirNode.Directories.Remove(name);


      // now look for the new or the changed files
      foreach(string name in actualDirs.Keys)
      {
        if(!pathFilter.IsDirectoryIncluded(name))
          continue;

        if(_monitor.CancelledByUser)
          break;

        pathFilter.EnterSubDirectory(name);
        if(dirNode.Directories.Contains(name))
        {
          Update(dirNode.Directory(name),(System.IO.DirectoryInfo)actualDirs[name],forceUpdateHash);
          // this file was here before, we look if it was changed
        }
        else
        {
          // this is a new file, we create a new file node for this
          dirNode.AddSubDirectory(name, NewDirectoryNode((System.IO.DirectoryInfo)actualDirs[name]));
        }
        pathFilter.LeaveSubDirectory(name);
      }
    }

 
    /// <summary>
    /// Creates a dir node. Tthe dir node is also updated.
    /// The current directory in pathFilter has to match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">Directory info for the dir node.</param>
    /// <param name="pathFilter">The path filter.</param>
    public  DirectoryNode NewDirectoryNode(System.IO.DirectoryInfo dirinfo)
    {
      System.Diagnostics.Debug.Assert(null!=pathFilter);
      DirectoryNode dirNode = new DirectoryNode(dirinfo.Name);
      Update(dirNode,dirinfo,true);

      return dirNode;
    }

    /// <summary>
    /// Creates a dir node with the right name. Files and subdirectories in this dir are not (!) created.
    /// </summary>
    /// <param name="dirinfo">Directory info for the dir node.</param>
    public static DirectoryNode NewEmptyDirectoryNode(System.IO.DirectoryInfo dirinfo)
    {
      DirectoryNode dirNode = new DirectoryNode(dirinfo.Name);
      return dirNode;
    }
  }
}
