using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Designed especially for fast comparism and hash code creation of
  /// MD5 hash sums (byte arrays of 16 elements). Do not use it for byte arrays longer than 24 elements.
  /// </summary>
  class MD5SumComparer 
    : 
    System.Collections.IHashCodeProvider,
    System.Collections.IComparer
  {
    #region IHashCodeProvider Members

    public int GetHashCode(object obj)
    {
      byte[] arr = (byte[])obj;
      int result=0;
      for(int i=0;i<arr.Length;i++)
        result ^= ((int)arr[i])<<i;
      return result;
    }

    #endregion

    #region IComparer Members

    public int Compare(object x, object y)
    {
      byte[] arrx = (byte[])x;
      byte[] arry = (byte[])y;
      
      if(arrx.Length!=arry.Length)
        return arrx.Length<arry.Length ? -1 : 1;

      for(int i=0;i<arrx.Length;i++)
        if(arrx[i]!=arry[i])
          return arrx[i]<arry[i] ? -1 : 1;
      
      return 0;
    }

    #endregion
  }
  
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
    static MD5SumComparer comp = new MD5SumComparer();
    public MD5SumHashTable()
      : base(comp,comp)
    {
    }

    public void Add(byte[] arr, string path, FileNode node)
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

    public PathAndFileNode this[byte[] arr]
    {
      get { return (PathAndFileNode)base[arr]; }
    }
  }

  

  public class MD5SumFileNodesHashTable : System.Collections.Hashtable
  {
    static MD5SumComparer comp = new MD5SumComparer();

   

    public MD5SumFileNodesHashTable()
      : base(comp,comp)
    {
    }

    public void Add(byte[] arr, string path , FileNode node )
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
          base[arr] = arr;
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
