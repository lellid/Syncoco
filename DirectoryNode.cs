using System;
using System.Collections;
using System.IO;

namespace SyncTwoCo
{
    using Filter;
  /// <summary>
  /// Holds information about itself and about all files and subdirectories in this node.
  /// </summary>
  [Serializable]
  public class DirectoryNode : IComparable, IParentDirectory
  {
    #region Member variables

    // Directory name
    string _name;

    /// <summary>
    /// Hashtable of subdirectory name as key and DirectoryNode as value. Note that the
    /// name is stored as it is, i.e. without directory separator char.
    /// </summary>
    DirectoryNodeList _subDirectories = new DirectoryNodeList();

    /// <summary>
    /// Hashtable of file name as key and FileNode as value
    /// </summary>
    FileNodeList _files = new FileNodeList();

    /// <summary>
    /// True if the directory now longer exists and should therefore be also removed on the foreign system.
    /// </summary>
    bool _IsRemoved;

    [NonSerialized]
    IParentDirectory _parent;

    #endregion

    #region Serialization

    public DirectoryNode(System.Xml.XmlTextReader tr, DirectoryNode parent)
    {
      _parent = parent;
      _name = tr.GetAttribute("Name");
      tr.ReadStartElement("Dir");
      Read(tr);
      tr.ReadEndElement(); // Dir
    }

    public DirectoryNode(System.Xml.XmlTextReader tr, FileSystemRoot parent)
    {
      _parent = parent;
      _name = String.Empty;
      tr.ReadStartElement("DirectoryNode");
      Read(tr);
      tr.ReadEndElement();
    }

    public DirectoryNode(string name)
    {
      _name = name;
    }

    void Read(System.Xml.XmlTextReader tr)
    {
      bool isEmptyElement;
      this._IsRemoved = System.Xml.XmlConvert.ToBoolean(tr.ReadElementString("IsRemoved"));
      
      isEmptyElement = tr.IsEmptyElement;
      tr.ReadStartElement("Files");
      if(!isEmptyElement)
      {
        while(tr.LocalName=="File")
        {
          FileNode node = new FileNode(tr,this);
          AddFile(node.Name,node);
        }
        tr.ReadEndElement(); // Files
      }

      isEmptyElement = tr.IsEmptyElement;
      tr.ReadStartElement("Dirs");
      if(!isEmptyElement)
      {
        while(tr.LocalName=="Dir")
        {
          DirectoryNode node = new DirectoryNode(tr,this);
          AddSubDirectory(node.Name,node);
        }
        tr.ReadEndElement(); // Dirs
      }

     
    }


    public void Save(System.Xml.XmlTextWriter tw, string localName)
    {
      tw.WriteStartElement(localName);
      tw.WriteAttributeString("Name",_name);

      tw.WriteElementString("IsRemoved",System.Xml.XmlConvert.ToString(_IsRemoved));
      tw.WriteStartElement("Files");
      foreach(FileNode fnode in _files)
      {
        fnode.Save(tw);
      }
      tw.WriteEndElement(); // Files

      tw.WriteStartElement("Dirs");
      foreach(DirectoryNode dnode in _subDirectories)
      {
        dnode.Save(tw,"Dir");
      }
      tw.WriteEndElement(); // Dirs

      tw.WriteEndElement(); // localName
    }

  
    #endregion

    #region Constructors
  

  
    /// <summary>
    /// Creates a dir node. If pathFilter is set (not null), then the dir node is also updated.
    /// The current directory in pathFilter has to match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">Directory info for the dir node.</param>
    /// <param name="pathFilter">The path filter.</param>
    public DirectoryNode(System.IO.DirectoryInfo dirinfo, PathFilter pathFilter)
    {
      if(dirinfo!=null)
        _name = dirinfo.Name;
           
      if(pathFilter!=null)
        Update(dirinfo,pathFilter,true);
    }

    #endregion

    #region File/Subdirectory access

    /// <summary>
    /// Tests if the directory node contains a file.
    /// </summary>
    /// <param name="name">The name of the file.</param>
    /// <returns>True if the directory node contains the file.</returns>
    public bool ContainsFile(string name) 
    {
      return _files.Contains(name); 
    }


    public IList Files
    {
      get 
      {
        return _files;
      }
    }

    /// <summary>
    /// Access to the file node by name.
    /// </summary>
    /// <param name="name">File name.</param>
    /// <returns>The file node with name 'name', or null if it doesn't exists.</returns>
    public FileNode File(string name)
    {
      return (FileNode)_files[name]; 
    }
    
    public void AddFile(string name, FileNode node)
    {
      System.Diagnostics.Debug.Assert(name!=null,"File name must not be null!");
      System.Diagnostics.Debug.Assert(name.Length>0,"File name must not be empty!");
      System.Diagnostics.Debug.Assert(node!=null,"File node must not be null!");
      this._files.Add(node);
    }

    /// <summary>
    /// Tests if the directory node contains a subdirectory.
    /// </summary>
    /// <param name="name">The name of the subdirectory.</param>
    /// <returns>True if the directory node contains the subdirectory.</returns>
    public bool ContainsDirectory(string name) 
    {
      return _subDirectories.Contains(name); 
    }

    public IList Directories
    {
      get 
      {
        return _subDirectories;
      }
    }

    /// <summary>
    /// Access to the subdirectory node by name.
    /// </summary>
    /// <param name="name">Name of the subdirectory.</param>
    /// <returns>The subdirectory node with name 'name', or null if it doesn't exists.</returns>
    public DirectoryNode Directory(string name) 
    { 
      return (DirectoryNode)_subDirectories[name]; 
    }

    public void AddSubDirectory(string name, DirectoryNode node)
    {
      System.Diagnostics.Debug.Assert(node!=null,"Directory node must not be null!");
      System.Diagnostics.Debug.Assert(node.Name!=null,"Directory name must not be null!");
      System.Diagnostics.Debug.Assert(node.Name.Length>0,"Directory name must not be empty!");
   
      this._subDirectories.Add(node);
     
    }


    #endregion
   
    #region Set properties

    /// <summary>
    /// Indicates whether this directory no longer exists in the file system.
    /// </summary>
    public bool IsRemoved { get { return _IsRemoved; }}

    /// <summary>
    /// This sets the directory node to the status 'removed'. This means, the directory no longer
    /// exists in the file system. All subdirectories and files are also set to the status 'removed'.
    /// </summary>
    public void SetToRemoved()
    {
      _IsRemoved = true;

      foreach(FileNode node in _files)
        node.SetToRemoved();

      foreach(DirectoryNode node in _subDirectories)
        node.SetToRemoved();
    }

    #endregion

    #region Access by full path strings

    public FileNode GetFileNodeFullPath(string path)
    {
      DirectoryNode dirnode = GetDirectoryNodeFullPath(path);
      if(dirnode!=null)
        return dirnode.File(System.IO.Path.GetFileName(path));
      else
        return null;
    }

    public void DeleteFileNodeFullPath(string path)
    {
      DirectoryNode dirnode = GetDirectoryNodeFullPath(path);
      if(dirnode!=null)
      {
        string filename = System.IO.Path.GetFileName(path);
        if(dirnode.ContainsFile(filename))
          dirnode._files.Remove(filename);
      }
    }

    /// <summary>
    /// Deletes a special subdirectory node.
    /// </summary>
    /// <param name="pathname">The full path name (from the root dir) to the subdirectory including a trailing DirectorySeparatorChar.</param>
    public void DeleteSubDirectoryNodeFullPath(string path)
    {
      System.Diagnostics.Debug.Assert(path[path.Length-1]==Path.DirectorySeparatorChar);

      int baseidx = path.LastIndexOf(Path.DirectorySeparatorChar,path.Length-2);
      if(baseidx<0)
        throw new Exception("This must be a programming error, the variable path was: " + path);
      
      DirectoryNode dirnode = GetDirectoryNodeFullPath(path.Substring(0,baseidx+1));
      if(dirnode!=null)
      {
        string name = path.Substring(baseidx+1,path.Length-baseidx-2);
        if(dirnode.ContainsDirectory(name))
          dirnode._subDirectories.Remove(name);
      }
    }

    public DirectoryNode GetDirectoryNodeFullPath(string path)
    {
      string dirname = System.IO.Path.GetDirectoryName(path);
      string filename = System.IO.Path.GetFileName(path);

      if(dirname==null || dirname==string.Empty)
      {
        return this;
      }
      else
      {
        string[] roots = dirname.Split(new char[]{System.IO.Path.DirectorySeparatorChar},2);
        if(this.ContainsDirectory(roots[0]))
        {
          if(roots.Length==2)
            return this.Directory(roots[0]).GetDirectoryNodeFullPath(path.Substring(roots[0].Length+1));
          else
            return this.Directory(roots[0]);
        }
        else
        {
          return null;
        }
      }
    }

    #endregion

    #region Node updating

    /// <summary>
    /// This updates a file node for an existing (!) file. The intermediate subdirectory nodes that lie between dirinfo and fileinfo are created if neccessary.
    /// </summary>
    /// <param name="dirnode">Starting directory node.</param>
    /// <param name="dirinfo">The directory info corresponding to the starting directory node.</param>
    /// <param name="fileinfo">The file info of an existing file.</param>
    /// <param name="forceUpdateHash">If true, the MD5 hash for the file is recalculated.</param>
    public static FileNode UpdateFileNode(DirectoryNode dirnode, DirectoryInfo dirinfo, FileInfo fileinfo, bool forceUpdateHash)
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
      dirnode.UpdateFile(fileinfo,forceUpdateHash);

      return dirnode.File(fileinfo.Name);
    }


    /// <summary>
    /// This updates the directory given by dirinfo including all files and all subdirectories in this
    /// directory. The current path in pathFilter has to match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">The directory that has to be updated.</param>
    /// <param name="pathFilter">The path filter.</param>
    public void Update(System.IO.DirectoryInfo dirinfo, PathFilter pathFilter, bool forceUpdateHash)
    {
      if(dirinfo!=null)
        _name = dirinfo.Name;

      UpdateFiles(dirinfo,pathFilter,forceUpdateHash);
      UpdateDirectories(dirinfo,pathFilter,forceUpdateHash);
    }

    /// <summary>
    /// Updates a file in this directory. Make sure that the file really belongs to this directory!
    /// </summary>
    /// <param name="fileinfo">The file info for this file.</param>
    /// <param name="forceUpdateHash">If true, the hash is recalculated if the file exists.</param>
    public void UpdateFile(System.IO.FileInfo fileinfo, bool forceUpdateHash)
    {
      if(this.ContainsFile(fileinfo.Name))
      {
        if(fileinfo.Exists)
          File(fileinfo.Name).Update(fileinfo, forceUpdateHash);
        else
          File(fileinfo.Name).SetToRemoved();
      }
      else if(fileinfo.Exists)
      {
        AddFile(fileinfo.Name,new FileNode(fileinfo,this));
      }
    }

    

    /// <summary>
    /// Updates all files and subdirectory nodes in an existing(!) directory of the own system. 
    /// This function is recursively called for all files and subdirectories in the child nodes.
    /// PathFilter decides, wheter or not a file or subdirectory is included or ignored.
    /// </summary>
    /// <param name="dirinfo">The directory info of the current directory node.</param>
    /// <param name="pathFilter">The path filter.</param>
    public void UpdateFiles(System.IO.DirectoryInfo dirinfo, PathFilter pathFilter, bool forceUpdateHash)
    {
      System.IO.FileInfo[] fileinfos = dirinfo.GetFiles();
      // create a hash table of the actual
      Hashtable actualFiles = new Hashtable();
      foreach(System.IO.FileInfo inf in fileinfos)
        actualFiles.Add(inf.Name,inf);

      // first look for the removed files
      System.Collections.Specialized.StringCollection filesToRemoveSilently = new System.Collections.Specialized.StringCollection();
      foreach(FileNode file in _files)
      {
        if(pathFilter.IsFileIncluded(file.Name))
        {
          if(!actualFiles.ContainsKey(file.Name))
            File(file.Name).SetToRemoved();
        }
        else // pathFilter (now) rejects this file
        {
          filesToRemoveSilently.Add(file.Name); // remove it silently from the list
        }
      }
      foreach(string file in filesToRemoveSilently)
        _files.Remove(file);
      

      // now look for the new or the changed files
      foreach(string file in actualFiles.Keys)
      {
        if(!pathFilter.IsFileIncluded(file))
          continue;

        if(_files.Contains(file))
        {
          File(file).Update((System.IO.FileInfo)actualFiles[file],forceUpdateHash);
          // this file was here before, we look if it was changed
        }
        else
        {
          // this is a new file, we create a new file node for this
          AddFile(file,new FileNode((System.IO.FileInfo)actualFiles[file],this));
        }
      }

      _files.TrimToSize();

    }


    /// <summary>
    /// Updates all directories in the directory given by dirinfo. The current path in pathFilter must
    /// match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">The directory which is to be updated.</param>
    /// <param name="pathFilter">The path filter.</param>
    public void UpdateDirectories(System.IO.DirectoryInfo dirinfo, PathFilter pathFilter, bool forceUpdateHash)
    {
      System.IO.DirectoryInfo[] dirinfos = dirinfo.GetDirectories();
      // create a hash table of the actual
      Hashtable actualDirs = new Hashtable();
      foreach(System.IO.DirectoryInfo inf in dirinfos)
        actualDirs.Add(inf.Name,inf);

      // first look for the removed dirs
      System.Collections.Specialized.StringCollection subDirsToRemoveSilently = new System.Collections.Specialized.StringCollection();
      foreach(DirectoryNode subdir in _subDirectories)
      {
        if(pathFilter.IsDirectoryIncluded(subdir.Name))
        {
          if(!actualDirs.ContainsKey(subdir.Name))
            Directory(subdir.Name).SetToRemoved();
          else
            Directory(subdir.Name)._IsRemoved = false;
        }
        else  // pathFilter now rejects this directory, so remove it silently (but not set it to removed - this would cause the
        {     // directory on the foreign system to be removed also)
          subDirsToRemoveSilently.Add(subdir.Name);
        }
      }
      foreach(string name in subDirsToRemoveSilently)
        _subDirectories.Remove(name);


      // now look for the new or the changed files
      foreach(string name in actualDirs.Keys)
      {
        if(!pathFilter.IsDirectoryIncluded(name))
          continue;

        pathFilter.EnterSubDirectory(name);
        if(_subDirectories.Contains(name))
        {
          Directory(name).Update((System.IO.DirectoryInfo)actualDirs[name],pathFilter,forceUpdateHash);
          // this file was here before, we look if it was changed
        }
        else
        {
          // this is a new file, we create a new file node for this
          AddSubDirectory(name,new DirectoryNode((System.IO.DirectoryInfo)actualDirs[name],pathFilter));
        }
        pathFilter.LeaveSubDirectory(name);
      }

      _subDirectories.TrimToSize();
    }

    #endregion

    #region Own synchronization

    /// <summary>
    /// Collects the files that are supposed for synchronizing our own file system.
    /// </summary>
    /// <param name="myDir">Directory node of our own file system.</param>
    /// <param name="foreignDir">Corresponding directory node of the foreign file system.</param>
    /// <param name="pathFilter">The path filter used to filter files and directories.</param>
    /// <param name="collector">Container that collectes the files that are supposed for synchronization.</param>
    /// <param name="directorybase"></param>
    public static void Collect(DirectoryNode myDir, DirectoryNode foreignDir, PathFilter pathFilter, Collector collector, string directorybase)
    {
      System.Collections.Specialized.StringCollection foreignFilesToRemove=null;
      foreach(FileNode foreignFileNode in foreignDir._files)
      {
        string   foreignFileName = foreignFileNode.Name;

        if(foreignFileNode.IsUnchanged)
          continue;
        if(!pathFilter.IsFileIncluded(foreignFileName))
          continue;

        if(myDir!=null)
        {
          System.IO.FileInfo fileinfo = new System.IO.FileInfo(System.IO.Path.Combine(collector.GetFullPath(directorybase),foreignFileName));
          myDir.UpdateFile(fileinfo,false);
        }

        
        // First handle the error, that both the nodes are unchanged, but nevertheless different from each other
        if(myDir!=null && myDir.ContainsFile(foreignFileName) && foreignFileNode.IsUnchanged && myDir.File(foreignFileName).IsUnchanged && foreignFileNode.IsDifferent(myDir.File(foreignFileName)))
        {
          // set both nodes to changed then, since we don't know when this error occurs
          foreignFileNode.SwitchFromUnchangedToChanged();
          myDir.File(foreignFileName).SwitchFromUnchangedToChanged();
        }

        // File removed handling
        if(foreignFileNode.IsRemoved)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName) && !myDir.File(foreignFileName).IsRemoved)
          {
            bool isUnchanged = foreignFileNode.HasSameHashThan(myDir.File(foreignFileName));
            collector.AddRemovedFile(directorybase,foreignFileName,isUnchanged);
          }
          else // is not in the list
          {
            if(null==foreignFilesToRemove) foreignFilesToRemove=new System.Collections.Specialized.StringCollection();
            foreignFilesToRemove.Add(foreignFileName);  // foreignDir._files.Remove(foreignFileName);
            if(myDir._files.Contains(foreignFileName))
              myDir._files.Remove(foreignFileName);
          }
        }
        else if(foreignFileNode.IsNew)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName))
          {
            if(foreignFileNode.HasSameHashThan(myDir.File(foreignFileName)))
            {
              foreignFileNode.SetToUnchanged();
              myDir.File(foreignFileName).SetToUnchanged();
            }
            else
            {
              if(collector.IsFileHereAnywhere(foreignFileNode))
                collector.AddManuallyResolvedFile(directorybase,foreignFileName);
             
            }
          }
          else
          {
            if(collector.IsFileHereAnywhere(foreignFileNode))
              collector.AddFileToCopy(directorybase,foreignFileName);
          }
        }
        else if(foreignFileNode.IsChanged)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName))
          {
            if(foreignFileNode.HasSameHashThan(myDir.File(foreignFileName)))
            {
              foreignFileNode.SetToUnchanged();
              myDir.File(foreignFileName).SetToUnchanged();
            }
            else if(myDir.File(foreignFileName).IsUnchanged)
            {
              if(collector.IsFileHereAnywhere(foreignFileNode))
                collector.AddFileToOverwrite(directorybase,foreignFileName);
            }
            else if(collector.IsFileHereAnywhere(foreignFileNode))
            {
              collector.AddManuallyResolvedFile(directorybase,foreignFileName);
            }
          }
          else
          {
            if(collector.IsFileHereAnywhere(foreignFileNode))
              collector.AddFileToCopy(directorybase,foreignFileName);
          }
        }
      
      } // foreach file
      // now remove the foreign files that can not be removed during the enumeration
      if(null!=foreignFilesToRemove)
      {
        foreach(string name in foreignFilesToRemove)
          foreignDir._files.Remove(name);
      }


      // now the directories...
      System.Collections.Specialized.StringCollection foreignSubDirsToRemove=null;
      foreach(DirectoryNode foreignSubDirNode in foreignDir._subDirectories)
      {
        string foreignSubDirName = foreignSubDirNode.Name;
        


        // If the pathfilter rejects this, we can remove the nodes silently and continue with the next node
        if(!pathFilter.IsDirectoryIncluded(foreignSubDirName))
        {
          if(null==foreignSubDirsToRemove) foreignSubDirsToRemove=new System.Collections.Specialized.StringCollection();
          foreignSubDirsToRemove.Add(foreignSubDirName);  // foreignDir._files.Remove(foreignFileName);
          if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
            myDir._subDirectories.Remove(foreignSubDirName);

          continue;
        }

        string newdirectorybase = System.IO.Path.Combine(directorybase, foreignSubDirName + System.IO.Path.DirectorySeparatorChar);
  

        // Test if the directory is removed now and set it to removed in this case
        if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName) && !System.IO.Directory.Exists(collector.GetFullPath(newdirectorybase)))
        {
          myDir.Directory(foreignSubDirName).SetToRemoved();
        }
        

        if(foreignSubDirNode.IsRemoved)
        {
          if(myDir==null || !myDir.ContainsDirectory(foreignSubDirName) || myDir.Directory(foreignSubDirName).IsRemoved)
          {
            // if myDir also not exist, remove the nodes silently
            if(null==foreignSubDirsToRemove) foreignSubDirsToRemove=new System.Collections.Specialized.StringCollection();
            foreignSubDirsToRemove.Add(foreignSubDirName);  // foreignDir._files.Remove(foreignFileName);
            if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
              myDir._subDirectories.Remove(foreignSubDirName);

            continue;
          }
        }

        if(myDir!=null && System.IO.Directory.Exists(collector.GetFullPath(newdirectorybase)))
        {
          if(!myDir.ContainsDirectory(foreignSubDirName))
            myDir.AddSubDirectory(foreignSubDirName,new DirectoryNode(foreignSubDirName));
          else
            myDir.Directory(foreignSubDirName)._IsRemoved = false; // this is neccessary if it was renamed externally
        }

        pathFilter.EnterSubDirectory(foreignSubDirName);
        Collect(
          myDir!=null && myDir.ContainsDirectory(foreignSubDirName)? myDir.Directory(foreignSubDirName) : null,
          foreignSubDirNode,
          pathFilter,
          collector,newdirectorybase);
        pathFilter.LeaveSubDirectory(foreignSubDirName);


        if(foreignSubDirNode.IsRemoved && myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
          collector.AddRemovedSubdirectory(directorybase,foreignSubDirName);          



      }  // for each sub dir

      // now remove the foreign files that can not be removed during the enumeration
      if(null!=foreignSubDirsToRemove)
      {
        foreach(string name in foreignSubDirsToRemove)
          foreignDir._subDirectories.Remove(name);
      }
    }

    #endregion

    #region Transfer to medium for foreign system

    /// <summary>
    /// Collects all files that should be copied to the transfermedium. We presume that the own file system was
    /// updated before. That is why no PathFilter is neccessary here.
    /// </summary>
    /// <param name="myDir">Own directory node.</param>
    /// <param name="foreignDir">Corresponding directory node of the foreign system.</param>
    /// <param name="list">List in which the files are collected that should be copied to the transfer medium.</param>
    /// <param name="nameroot"></param>
    public static void GetNewOrChangedFiles(DirectoryNode myDir, DirectoryNode foreignDir, SortedList list, string nameroot)
    {
      foreach(FileNode myFileNode in myDir._files)
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
            list.Add(nameroot+myFileNode.Name,myFileNode);
          }
        }
      }

      foreach(DirectoryNode mySubDirNode in myDir._subDirectories)
      {
        string mySubDirName = mySubDirNode.Name;

        GetNewOrChangedFiles(
          mySubDirNode,
          foreignDir==null ? null : foreignDir.Directory(mySubDirName),
          list,
          System.IO.Path.Combine(nameroot,mySubDirName+System.IO.Path.DirectorySeparatorChar));
      }
    }

    #endregion

    #region MD5 Hash sum
    public void FillMd5HashTable(MD5SumHashTable table, string currentPath)
    {
      foreach(FileNode fnode in this._files)
      {        
        fnode.FillMd5HashTable(table,currentPath+fnode.Name);
      }
      foreach(DirectoryNode dnode in this._subDirectories)
      {
        string subPath = currentPath + dnode.Name + System.IO.Path.DirectorySeparatorChar;
        dnode.FillMd5HashTable(table,subPath);
      }
    }

    public void FillMD5SumFileNodesHashTable(MD5SumFileNodesHashTable table, string currentPath)
    {
      System.Diagnostics.Debug.Assert(currentPath!=null);
      System.Diagnostics.Debug.Assert(currentPath.Length>0); 
      System.Diagnostics.Debug.Assert(currentPath[currentPath.Length-1]==System.IO.Path.DirectorySeparatorChar);

      foreach(FileNode fnode in this._files)
      {
        fnode.FillMD5SumFileNodesHashTable(table,currentPath+fnode.Name);
      }
      foreach(DirectoryNode dnode in this._subDirectories)
      {
        string subPath = currentPath + dnode.Name + System.IO.Path.DirectorySeparatorChar;
        dnode.FillMD5SumFileNodesHashTable(table,subPath);
      }
    }

    #endregion

   
    #region IComparable Members

    public int CompareTo(object obj)
    {
      if(obj is string)
        return string.Compare(this._name,(string)obj,true);
      else if(obj is DirectoryNode)
        return string.Compare(this._name,((DirectoryNode)obj)._name,true);
      else
        throw new ArgumentException("Cannot compare a FileNode with a object of type " + obj.GetType().ToString());
    }

    #endregion

    #region IParentDirectory Members

    public bool IsFileSystemRoot
    {
      get
      {
        return false;
      }
    }

    public IParentDirectory ParentDirectory
    {
      get
      {
        return _parent;
      }
    }

    /// <summary>
    /// File name.
    /// </summary>
    public string Name
    {
      get 
      {
        return _name;
      }
    }

    public override string ToString()
    {
      return _name==null || _name==string.Empty ? base.ToString() : _name;
    }

    public void SetParent(IParentDirectory parent)
    {
      _parent = parent;
    }


    public void RestoreParentOfChildObjects(IParentDirectory parent)
    {
      _parent = parent;

      foreach(FileNode file in this._files)
        file.RestoreParentOfChildObjects(this);
      foreach(DirectoryNode dir in this._subDirectories)
        dir.RestoreParentOfChildObjects(this);
    }

    #endregion
  }
}
