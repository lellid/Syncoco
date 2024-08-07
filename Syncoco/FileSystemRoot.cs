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

#endregion Copyright

using System;

//using System.Management;


namespace Syncoco
{
  using Traversing;

  /// <summary>
  /// Summary description for FileSystemRoot.
  /// </summary>
  [Serializable]
  public class FileSystemRoot : IParentDirectory
  {
    private string _FilePath;

    private DirectoryNode _DirectoryNode;

    public FileSystemRoot()
    {
      _FilePath = null;
      _DirectoryNode = null;
    }

    public FileSystemRoot(string path)
    {
      PathUtil.Assert_Abspath(path);
      SetFilePath(path);
    }

    public FileSystemRoot(string path, DirectoryNode dirnode)
    {
      PathUtil.Assert_Abspath(path);
      _FilePath = path;
      _DirectoryNode = dirnode;
    }

    public FileSystemRoot(System.Xml.XmlTextReader tr, string localName)
    {
      tr.ReadStartElement(localName);
      Read(tr);
      tr.ReadEndElement();
    }

    public void Save(System.Xml.XmlTextWriter tw, string localName, bool saveFilterOnly)
    {
      tw.WriteStartElement(localName);
      tw.WriteElementString("Path", this._FilePath);
      if (!saveFilterOnly && null != this._DirectoryNode)
      {
        this._DirectoryNode.Save(tw, "DirectoryNode");
      }
      tw.WriteEndElement(); // Root1
    }

    public void Read(System.Xml.XmlTextReader tr)
    {
      _FilePath = tr.ReadElementString("Path");

      if (tr.LocalName == "DirectoryNode")
      {
        _DirectoryNode = new DirectoryNode(tr, this);
      }
      else
      {
        SetFilePath(_FilePath);
      }
    }

    public bool IsValid
    {
      get { return _FilePath != null && _FilePath.Length > 0; }
    }

    public string FilePath
    {
      get { return _FilePath; }
    }

    public DirectoryNode DirectoryNode
    {
      get { return _DirectoryNode; }
      set
      {
        if (null == _DirectoryNode)
          _DirectoryNode = value;
        else
          throw new ApplicationException("Try to set an already existing DirectoryNode");
      }
    }

    public void SetFilePath(string path)
    {
      this._FilePath = path;

      PathUtil.Assert_Abspath(_FilePath);
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_FilePath);
      if (dirinfo.Exists)
        _DirectoryNode = DirectoryUpdater.NewEmptyDirectoryNode(dirinfo, this);
      else
        _DirectoryNode = null;
    }

    // following the filter items

    // Following the collection of file and directory nodes

    public FileNode GetFileNode(string pathname)
    {
#if DEBUG
      PathUtil.Assert_RelpathFilename(pathname);
#endif
      if (_DirectoryNode == null)
        return null;
      else
        return _DirectoryNode.GetFileNodeFullPath(pathname);
    }

    public DirectoryNode GetDirectoryNode(string pathname)
    {
#if DEBUG
      PathUtil.Assert_Relpath(pathname);
#endif
      if (_DirectoryNode == null)
        return null;
      else
        return _DirectoryNode.GetDirectoryNodeFullPath(pathname);
    }

    public void DeleteFileNode(string pathname)
    {
#if DEBUG
      PathUtil.Assert_RelpathFilename(pathname);
#endif
      if (_DirectoryNode != null)
        _DirectoryNode.DeleteFileNodeFullPath(pathname);
    }

    /// <summary>
    /// Deletes a special subdirectory node. Path must include a trailing DirectorySeparatorChar!
    /// </summary>
    /// <param name="path">The full path name (from the root dir) to the subdirectory including a trailing DirectorySeparatorChar.</param>
    public void DeleteSubDirectoryNode(string path)
    {
#if DEBUG
      PathUtil.Assert_Relpath(path);
#endif
      if (_DirectoryNode != null)
        _DirectoryNode.DeleteSubDirectoryNodeFullPath(path);
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

    public void RestoreParentOfChildObjects()
    {
      if (_DirectoryNode != null)
        _DirectoryNode.RestoreParentOfChildObjects(this);
    }

    #endregion IParentDirectory Members
  }
}