using System;
using System.Collections;
//using System.Management;

using System.IO;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for FileSystemRoot.
  /// </summary>
  [Serializable]
  public class FileSystemRoot
  {

    string _FilePath;

    DirectoryNode _DirectoryNode;



    public FileSystemRoot()
    {
      _FilePath = null;
      _DirectoryNode = null;
    }

    public FileSystemRoot(string path)
    {
      SetFilePath(path);
    }

    public FileSystemRoot(string path, DirectoryNode dirnode)
    {
      _FilePath = NormalizePath(path);
      _DirectoryNode = dirnode;
    }


    public void Save(System.Xml.XmlTextWriter tw)
    {
      tw.WriteAttributeString("Path",_FilePath);
      _DirectoryNode.Save(tw);
    }

    public void Read(System.Xml.XmlTextReader tr)
    {
      _FilePath = NormalizePath(tr.GetAttribute("Path"));
      tr.ReadStartElement("Root");
      _DirectoryNode = new DirectoryNode(tr);
      tr.ReadEndElement();
    }

    public bool IsValid
    {
      get { return _FilePath!=null && _FilePath.Length>0; }
    }

    public string FilePath
    {
      get { return _FilePath; }
    }

    public DirectoryNode DirectoryNode
    {
      get { return _DirectoryNode; }
    }

    public void SetFilePath(string path)
    {
      _FilePath = NormalizePath(path);
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_FilePath);
      if(dirinfo.Exists)
        _DirectoryNode = new DirectoryNode(dirinfo,null);
      else
        _DirectoryNode = null;
    }
    // following the filter items


    // Following the collection of file and directory nodes


    public FileNode GetFileNode(string pathname)
    {
      if(_DirectoryNode==null)
        return null;
      else
        return _DirectoryNode.GetFileNodeFullPath(pathname);
    }

    public void DeleteFileNode(string pathname)
    {
      if(_DirectoryNode!=null)
        _DirectoryNode.DeleteFileNodeFullPath(pathname);
    }

    /// <summary>
    /// Deletes a special subdirectory node. Path must include a trailing DirectorySeparatorChar!
    /// </summary>
    /// <param name="path">The full path name (from the root dir) to the subdirectory including a trailing DirectorySeparatorChar.</param>
    public void DeleteSubDirectoryNode(string path)
    {
      System.Diagnostics.Debug.Assert(path[path.Length-1]==Path.DirectorySeparatorChar);

      if(_DirectoryNode!=null)
        _DirectoryNode.DeleteSubDirectoryNodeFullPath(path);
    }

    public void Update(PathFilter pathFilter)
    {
      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(_FilePath);

      if(null!=_DirectoryNode)
        _DirectoryNode.Update(dirinfo,pathFilter);
      else
        _DirectoryNode = new DirectoryNode(dirinfo,pathFilter);
    }

    public FileNode UpdateMyFile(FileInfo fileinfo, bool forceUpdateHash)
    {
      DirectoryInfo dirinfo = new DirectoryInfo(this._FilePath);

      return DirectoryNode.UpdateFileNode(_DirectoryNode,dirinfo,fileinfo,forceUpdateHash);
    }

    public void FillMd5HashTable(MD5SumHashTable table)
    {
      this._DirectoryNode.FillMd5HashTable(table);
    }

    public void FillMD5SumFileNodesHashTable(MD5SumFileNodesHashTable table)
    {
      this._DirectoryNode.FillMD5SumFileNodesHashTable(table,this.FilePath);
    }
   
    /// <summary>
    /// Returns a path with is forced to end with a DirectorySeparatorChar
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string NormalizePath(string path)
    {
      if(path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
        return path;
      else
        return path+System.IO.Path.DirectorySeparatorChar;
    }

 
  }
}
