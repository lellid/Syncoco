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

namespace Syncoco
{
  /// <summary>
  /// Holds information about itself and about all files and subdirectories in this node.
  /// </summary>
  [Serializable]
  public class DirectoryNode : IComparable, IParentDirectory
  {
    #region Member variables

    // Directory name
    private string _name;

    /// <summary>
    /// Hashtable of subdirectory name as key and DirectoryNode as value. Note that the
    /// name is stored as it is, i.e. without directory separator char.
    /// </summary>
    private DirectoryNodeList _subDirectories = new DirectoryNodeList();

    /// <summary>
    /// Hashtable of file name as key and FileNode as value
    /// </summary>
    private FileNodeList _files = new FileNodeList();

    /// <summary>
    /// True if the directory now longer exists and should therefore be also removed on the foreign system.
    /// </summary>
    private bool _IsRemoved;

    [NonSerialized]
    private IParentDirectory _parent;

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

    private void Read(System.Xml.XmlTextReader tr)
    {
      bool isEmptyElement;
      this._IsRemoved = System.Xml.XmlConvert.ToBoolean(tr.ReadElementString("IsRemoved"));

      isEmptyElement = tr.IsEmptyElement;
      tr.ReadStartElement("Files");
      if (!isEmptyElement)
      {
        while (tr.LocalName == "File")
        {
          FileNode node = new FileNode(tr, this);
          AddFile(node.Name, node);
        }
        tr.ReadEndElement(); // Files
      }

      isEmptyElement = tr.IsEmptyElement;
      tr.ReadStartElement("Dirs");
      if (!isEmptyElement)
      {
        while (tr.LocalName == "Dir")
        {
          DirectoryNode node = new DirectoryNode(tr, this);
          AddSubDirectory(node.Name, node);
        }
        tr.ReadEndElement(); // Dirs
      }


    }


    public void Save(System.Xml.XmlTextWriter tw, string localName)
    {
      tw.WriteStartElement(localName);
      tw.WriteAttributeString("Name", _name);

      tw.WriteElementString("IsRemoved", System.Xml.XmlConvert.ToString(_IsRemoved));
      tw.WriteStartElement("Files");
      foreach (FileNode fnode in _files)
      {
        fnode.Save(tw);
      }
      tw.WriteEndElement(); // Files

      tw.WriteStartElement("Dirs");
      foreach (DirectoryNode dnode in _subDirectories)
      {
        dnode.Save(tw, "Dir");
      }
      tw.WriteEndElement(); // Dirs

      tw.WriteEndElement(); // localName
    }


    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new empty directory node with a name and a parent. This node is not (!) updated here.
    /// </summary>
    /// <param name="name">The name of this node. Must not contain DirectorySeparatorChars.</param>
    /// <param name="parent">The parent node of this node.</param>
    public DirectoryNode(string name, IParentDirectory parent)
    {
      _name = name;
      _parent = parent;
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
      System.Diagnostics.Debug.Assert(name != null, "File name must not be null!");
      System.Diagnostics.Debug.Assert(name.Length > 0, "File name must not be empty!");
      System.Diagnostics.Debug.Assert(node != null, "File node must not be null!");
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
      System.Diagnostics.Debug.Assert(node != null, "Directory node must not be null!");
      System.Diagnostics.Debug.Assert(node.Name != null, "Directory name must not be null!");
      System.Diagnostics.Debug.Assert(node.Name.Length > 0, "Directory name must not be empty!");

      this._subDirectories.Add(node);

    }


    #endregion

    #region Set properties

    /// <summary>
    /// Indicates whether this directory no longer exists in the file system.
    /// </summary>
    public bool IsRemoved
    {
      get { return _IsRemoved; }
      set { _IsRemoved = value; }
    }

    /// <summary>
    /// This sets the directory node to the status 'removed'. This means, the directory no longer
    /// exists in the file system. All subdirectories and files are also set to the status 'removed'.
    /// </summary>
    public void SetToRemoved()
    {
      _IsRemoved = true;

      foreach (FileNode node in _files)
        node.SetToRemoved();

      foreach (DirectoryNode node in _subDirectories)
        node.SetToRemoved();
    }

    #endregion

    #region Access by full path strings



    public FileNode GetFileNodeFullPath(string path)
    {
      string relpath, filename;
      PathUtil.SplitInto_Relpath_Filename(path, out relpath, out filename);

      DirectoryNode dirnode = GetDirectoryNodeFullPath(relpath);
      if (dirnode != null)
        return dirnode.File(filename);
      else
        return null;
    }

    public void DeleteFileNodeFullPath(string path)
    {
      string relpath, filename;
      PathUtil.SplitInto_Relpath_Filename(path, out relpath, out filename);

      DirectoryNode dirnode = GetDirectoryNodeFullPath(relpath);
      if (dirnode != null)
      {
        if (dirnode.ContainsFile(filename))
          dirnode._files.Remove(filename);
      }
    }

    /// <summary>
    /// Deletes a special subdirectory node.
    /// </summary>
    /// <param name="pathname">The full path name (from the root dir) to the subdirectory including a trailing DirectorySeparatorChar.</param>
    public void DeleteSubDirectoryNodeFullPath(string path)
    {
#if DEBUG
      PathUtil.Assert_Relpath(path);
#endif 

      DirectoryNode node = GetDirectoryNodeFullPath(path);

      if (node != null)
      {
        DirectoryNode parent = (DirectoryNode)node._parent;
        parent._subDirectories.Remove(node._name);
      }
    }

    /// <summary>
    /// Gets a directory node.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public DirectoryNode GetDirectoryNodeFullPath(string relpath)
    {
#if DEBUG
      PathUtil.Assert_Relpath(relpath);
#endif
      if (relpath.Length == 1)
        return this;

      int pos = relpath.IndexOf(System.IO.Path.DirectorySeparatorChar, 1);
      string dirnamehere = relpath.Substring(1, pos - 1);
#if DEBUG
      PathUtil.Assert_Dirname(dirnamehere);
#endif

      if (this.ContainsDirectory(dirnamehere))
        return this.Directory(dirnamehere).GetDirectoryNodeFullPath(relpath.Substring(pos));
      else
        return null;

    }

    #endregion






    #region IComparable Members

    public int CompareTo(object obj)
    {
      if (obj is string)
        return string.Compare(this._name, (string)obj, true);
      else if (obj is DirectoryNode)
        return string.Compare(this._name, ((DirectoryNode)obj)._name, true);
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

    public string FullRelativePath
    {
      get
      {
        if (_parent is DirectoryNode)
          return ((DirectoryNode)_parent).FullRelativePath + _name + System.IO.Path.DirectorySeparatorChar;
        else
          return System.IO.Path.DirectorySeparatorChar.ToString();
      }
    }

    public override string ToString()
    {
      return _name == null || _name == string.Empty ? base.ToString() : _name;
    }

    public void SetParent(IParentDirectory parent)
    {
      _parent = parent;
    }


    public void RestoreParentOfChildObjects(IParentDirectory parent)
    {
      _parent = parent;

      foreach (FileNode file in this._files)
        file.RestoreParentOfChildObjects(this);
      foreach (DirectoryNode dir in this._subDirectories)
        dir.RestoreParentOfChildObjects(this);
    }

    #endregion

    /// <summary>
    /// Does a compacting of the used memory of this dir and all subdirs (trims the arrays to size).
    /// </summary>
    public void Compact()
    {
      this._files.TrimToSize();
      foreach (DirectoryNode fnode in _subDirectories)
        fnode.Compact();
    }
  }
}
