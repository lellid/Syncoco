#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco: offline file synchronization
//    Copyright (C) 2004-2099 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.IO;
using Syncoco.Filter;

namespace Syncoco.Online
{
  /// <summary>
  /// Holds information about itself and about all files and subdirectories in this node.
  /// </summary>
  [Serializable]
  public class SimpleDirectoryNode : IComparable
  {
    #region Member variables

    // Directory name
    string _name;

    /// <summary>
    /// Hashtable of subdirectory name as key and SimpleDirectoryNode as value. Note that the
    /// name is stored as it is, i.e. without directory separator char.
    /// </summary>
    SimpleDirectoryNodeList _subDirectories = new SimpleDirectoryNodeList();

    /// <summary>
    /// Hashtable of file name as key and SimpleFileNode as value
    /// </summary>
    SimpleFileNodeList _files = new SimpleFileNodeList();

    [NonSerialized]
    bool _IsUpdated;

    public bool IsUpdated
    {
      get { return _IsUpdated; }
      set { _IsUpdated = value; }
    }

    #endregion

   

    #region Constructors
  

    public SimpleDirectoryNode(string name)
    {
      _name = name;
    }


    /// <summary>
    /// Copy constructor from DirectoryNode
    /// </summary>
    /// <param name="from"></param>
    public SimpleDirectoryNode(DirectoryNode from)
    {
      _name = from.Name;

      foreach(FileNode file in from.Files)
      {
        _files.Add(new SimpleFileNode(file));
      }

      foreach(DirectoryNode dir in from.Directories)
      {
        _subDirectories.Add(new SimpleDirectoryNode(dir));
      }
    }
    

    /// <summary>
    /// Creates a dir node. If pathFilter is set (not null), then the dir node is also updated.
    /// The current directory in pathFilter has to match the directory given by dirinfo.
    /// </summary>
    /// <param name="dirinfo">Directory info for the dir node.</param>
    /// <param name="pathFilter">The path filter.</param>
    public SimpleDirectoryNode(System.IO.DirectoryInfo dirinfo, PathFilter pathFilter)
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

    /// <summary>
    /// Access to the file node by name.
    /// </summary>
    /// <param name="name">File name.</param>
    /// <returns>The file node with name 'name', or null if it doesn't exists.</returns>
    public SimpleFileNode File(string name)
    {
      return (SimpleFileNode)_files[name]; 
    }
    
    public void AddFile(SimpleFileNode node)
    {
      System.Diagnostics.Debug.Assert(node!=null,"File node must not be null!");
      System.Diagnostics.Debug.Assert(node.Name!=null,"File name must not be null!");
      System.Diagnostics.Debug.Assert(node.Name.Length>0,"File name length must greater than zero!");

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

    /// <summary>
    /// Access to the subdirectory node by name.
    /// </summary>
    /// <param name="name">Name of the subdirectory.</param>
    /// <returns>The subdirectory node with name 'name', or null if it doesn't exists.</returns>
    public SimpleDirectoryNode Directory(string name) 
    { 
      return (SimpleDirectoryNode)_subDirectories[name]; 
    }

    public void AddSubDirectory(SimpleDirectoryNode node)
    {
      System.Diagnostics.Debug.Assert(node!=null,"Directory node must not be null!");
      System.Diagnostics.Debug.Assert(node.Name!=null,"Directory name must not be null!");
      System.Diagnostics.Debug.Assert(node.Name.Length>0,"Directory name must not be empty!");
   
      this._subDirectories.Add(node);
     
    }


    #endregion
   
    

    #region Access by full path strings

    public SimpleFileNode GetFileNodeFullPath(string path)
    {
      SimpleDirectoryNode dirnode = GetDirectoryNodeFullPath(path);
      if(dirnode!=null)
        return dirnode.File(System.IO.Path.GetFileName(path));
      else
        return null;
    }

    public void DeleteFileNodeFullPath(string path)
    {
      SimpleDirectoryNode dirnode = GetDirectoryNodeFullPath(path);
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
      
      SimpleDirectoryNode dirnode = GetDirectoryNodeFullPath(path.Substring(0,baseidx+1));
      if(dirnode!=null)
      {
        string name = path.Substring(baseidx+1,path.Length-baseidx-2);
        if(dirnode.ContainsDirectory(name))
          dirnode._subDirectories.Remove(name);
      }
    }

    public SimpleDirectoryNode GetDirectoryNodeFullPath(string path)
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
    public static SimpleFileNode UpdateFileNode(SimpleDirectoryNode dirnode, DirectoryInfo dirinfo, FileInfo fileinfo, bool forceUpdateHash)
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
          dirnode.AddSubDirectory(new SimpleDirectoryNode(subdirs[i]));

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
          this._files.Remove(fileinfo.Name);
      }
      else if(fileinfo.Exists)
      {
        AddFile(new SimpleFileNode(fileinfo));
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
      foreach(SimpleFileNode file in _files)
      {
        if(pathFilter.IsFileIncluded(file.Name))
        {
          if(!actualFiles.ContainsKey(file.Name))
            filesToRemoveSilently.Add(file.Name); // remove it silently from the list
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
          AddFile(new SimpleFileNode((System.IO.FileInfo)actualFiles[file]));
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
      foreach(SimpleDirectoryNode subdir in _subDirectories)
      {
        if(!pathFilter.IsDirectoryIncluded(subdir.Name) || !actualDirs.ContainsKey(subdir.Name))
          subDirsToRemoveSilently.Add(subdir.Name);
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
          AddSubDirectory(new SimpleDirectoryNode((System.IO.DirectoryInfo)actualDirs[name],pathFilter));
        }
        pathFilter.LeaveSubDirectory(name);
      }

      _subDirectories.TrimToSize();
    }

    #endregion

 


   
    #region IComparable Members

    public int CompareTo(object obj)
    {
      if(obj is string)
        return string.Compare(this._name,(string)obj,true);
      else if(obj is SimpleDirectoryNode)
        return string.Compare(this._name,((SimpleDirectoryNode)obj)._name,true);
      else
        throw new ArgumentException("Cannot compare a SimpleFileNode with a object of type " + obj.GetType().ToString());
    }

    #endregion

    #region IParentDirectory Members

   
  

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

    #endregion
  }
}
