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
//using System.Management;

using System.IO;
using Syncoco.Filter;

namespace Syncoco.Online
{

  /// <summary>
  /// Summary description for FileSystemRoot.
  /// </summary>
  [Serializable]
  public class SimpleFileSystemRoot : IParentDirectory
  {
    private string _FilePath;
    private SimpleDirectoryNode _DirectoryNode;
    private PathFilter _pathFilter;

    [NonSerialized]
    private FileSystemWatcher _watcher;

    [NonSerialized]
    private Hashtable _changedFiles = new Hashtable();




    public SimpleFileSystemRoot(string path)
    {
      SetFilePath(path);
    }

    public SimpleFileSystemRoot(FileSystemRoot fsroot, PathFilter filter)
    {
      this._FilePath = fsroot.FilePath;
      this._DirectoryNode = new SimpleDirectoryNode(fsroot.DirectoryNode);
      this._pathFilter = new PathFilter();
      this._pathFilter.CopyFrom(filter);

      SetFilePath(fsroot.FilePath);
    }




    public bool IsValid
    {
      get { return _FilePath != null && _FilePath.Length > 0; }
    }

    public string FilePath
    {
      get { return _FilePath; }
    }

    public SimpleDirectoryNode DirectoryNode
    {
      get { return _DirectoryNode; }
    }

    public void SetFilePath(string path)
    {
      _FilePath = NormalizePath(path);
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_FilePath);
      if (dirinfo.Exists)
      {
        _DirectoryNode = new SimpleDirectoryNode(dirinfo, null);
        _watcher = new FileSystemWatcher(dirinfo.FullName);
        _watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
        _watcher.IncludeSubdirectories = true;

        // Add event handlers.
        _watcher.Changed += new FileSystemEventHandler(OnChanged);
        _watcher.Created += new FileSystemEventHandler(OnChanged);
        _watcher.Deleted += new FileSystemEventHandler(OnChanged);
        _watcher.Renamed += new RenamedEventHandler(OnRenamed);

        // Begin watching.
        _watcher.EnableRaisingEvents = true;

      }
      else
      {
        if (_watcher != null)
        {
          // Begin watching.
          _watcher.EnableRaisingEvents = false;

          // remove event handlers.
          _watcher.Changed -= new FileSystemEventHandler(OnChanged);
          _watcher.Created -= new FileSystemEventHandler(OnChanged);
          _watcher.Deleted -= new FileSystemEventHandler(OnChanged);
          _watcher.Renamed -= new RenamedEventHandler(OnRenamed);



        }
        _DirectoryNode = null;
      }
    }
    // following the filter items


    // Following the collection of file and directory nodes


    public SimpleFileNode GetFileNode(string pathname)
    {
      if (_DirectoryNode == null)
        return null;
      else
        return _DirectoryNode.GetFileNodeFullPath(pathname);
    }

    public void DeleteFileNode(string pathname)
    {
      if (_DirectoryNode != null)
        _DirectoryNode.DeleteFileNodeFullPath(pathname);
    }

    /// <summary>
    /// Deletes a special subdirectory node. Path must include a trailing DirectorySeparatorChar!
    /// </summary>
    /// <param name="path">The full path name (from the root dir) to the subdirectory including a trailing DirectorySeparatorChar.</param>
    public void DeleteSubDirectoryNode(string path)
    {
      System.Diagnostics.Debug.Assert(path[path.Length - 1] == Path.DirectorySeparatorChar);

      if (_DirectoryNode != null)
        _DirectoryNode.DeleteSubDirectoryNodeFullPath(path);
    }

    public void Update(PathFilter pathFilter, bool forceUpdateHash)
    {
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_FilePath);

      if (null != _DirectoryNode)
        _DirectoryNode.Update(dirinfo, pathFilter, forceUpdateHash);
      else
        _DirectoryNode = new SimpleDirectoryNode(dirinfo, pathFilter);
    }

    public SimpleFileNode UpdateMyFile(FileInfo fileinfo, bool forceUpdateHash)
    {
      DirectoryInfo dirinfo = new DirectoryInfo(this._FilePath);

      return SimpleDirectoryNode.UpdateFileNode(_DirectoryNode, dirinfo, fileinfo, forceUpdateHash);
    }



    /// <summary>
    /// Returns a path with is forced to end with a DirectorySeparatorChar
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string NormalizePath(string path)
    {
      if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
        return path;
      else
        return path + System.IO.Path.DirectorySeparatorChar;
    }
    #region IParentDirectory Members

    public bool IsFileSystemRoot
    {
      get
      {
        return true;
      }
    }

    public string Name
    {
      get
      {
        return _FilePath;
      }
    }

    public IParentDirectory ParentDirectory
    {
      get
      {
        return null;
      }
    }

    #endregion

    // Define the event handlers.
    private void OnChanged(object source, FileSystemEventArgs e)
    {
      string path = e.FullPath.Substring(this._FilePath.Length);
      _changedFiles[path] = DateTime.Now;
    }

    private void OnRenamed(object source, RenamedEventArgs e)
    {
      string path;

      path = e.OldFullPath.Substring(this._FilePath.Length);
      _changedFiles[path] = DateTime.Now;

      path = e.FullPath.Substring(this._FilePath.Length);
      _changedFiles[path] = DateTime.Now;

    }

  }
}
