using System;

namespace Syncoco
{
  
  public class PathAndFileNode
  {
    string _Path;
    FileNode _Node;

    public PathAndFileNode(string path, FileNode node)
    {
      _Path = path;
      _Node = node;
    }

    public string Path { get { return _Path; }}
    public FileNode Node { get { return _Node; }}
  
  }


  public class MD5SumHashTable : System.Collections.Hashtable
  {
    public void Add(FileHash arr, string path, FileNode node)
    {
      if(base.ContainsKey(arr))
      {
        PathAndFileNode existingNode = this[arr];
        if(node.FileLength!=existingNode.Node.FileLength)
          throw new ApplicationException(
            string.Format("it should not happen, that two files with different length have the same hash, so rethink this" +
            "The two nodes here are: {0}(length={1}) and {2}(length={3})",existingNode.Path,existingNode.Node.FileLength,
            path,node.FileLength));
      }
      else
      {
        base.Add(arr,new PathAndFileNode(path,node));
      }
    }

    public PathAndFileNode this[FileHash arr]
    {
      get { return (PathAndFileNode)base[arr]; }
    }
  }

  

  public class MD5SumFileNodesHashTable : System.Collections.Hashtable
  {
   

    public void Add(FileHash arr, string path , FileNode node )
    {
      PathAndFileNode pan = new PathAndFileNode(path,node);
      if(base.ContainsKey(arr))
      {
        object item = base[arr];
        if(item is PathAndFileNode)
        {
          System.Collections.ArrayList list = new System.Collections.ArrayList();
          list.Add(item);
          list.Add(pan);
          base[arr] = list;
        }
        else
        {
          ((System.Collections.ArrayList)item).Add(pan);
        }
      }
      else
      {
        base.Add(arr,pan);
      }
    }

   
  }




}
