using System;
using System.Security.Cryptography;


namespace SyncTwoCo
{
  
  /// <summary>
  /// SimpleFileNode holds the information for a single file.
  /// </summary>
  [Serializable]
  public class FileNodeBase : IComparable
  {
    /// <summary>Used to calculate the hash value.</summary>
    protected static System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

    /// <summary>
    /// File name.
    /// </summary>
    protected string   _name;
    /// <summary>Time of last writing to the file.</summary>
    protected DateTime _lastWriteTimeUtc;
    /// <summary>Time of file creation.</summary>
    protected DateTime _creationTimeUtc;
    /// <summary>Length of the file.</summary>
    protected long     _fileLength;
    /// <summary>MD5 hash value.</summary>
    protected FileHash _fileHash;
    /// <summary>File attributes.</summary>
    protected System.IO.FileAttributes _attributes;
    /// <summary>Actual state of the file.</summary>
 

    /// <summary>
    /// Only for deserialization purposes.
    /// </summary>
    protected FileNodeBase()
    {
    }

    public FileNodeBase(FileNodeBase from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(FileNodeBase from)
    {
      this._name = from._name;
      this._lastWriteTimeUtc = from._lastWriteTimeUtc;
      this._creationTimeUtc = from._creationTimeUtc;
      this._fileLength = from._fileLength;
      this._fileHash = from._fileHash;
      this._attributes = from._attributes;
    }
  
    /// <summary>
    /// Constructor. Creates a SimpleFileNode out of a file info for that file.
    /// </summary>
    /// <param name="info"></param>
    public FileNodeBase(System.IO.FileInfo info)
    {
      Update(info,true);
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
    /// <summary>
    /// Length of the file.
    /// </summary>
    public long FileLength 
    {
      get
      {
        return _fileLength; 
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="forceUpdateHash"></param>
    /// <returns>True if the node data changed due to changed file information, otherwise false.</returns>
    public void Update(System.IO.FileInfo info, bool forceUpdateHash)
    {
    
      _name = info.Name;

      FileHash hashCalculatedBefore;
      if(forceUpdateHash && info.Exists)
        hashCalculatedBefore = CalculateHash(info);
      else
        hashCalculatedBefore = new FileHash();
        

      if(IsDifferent(info) || (hashCalculatedBefore.Valid && !this.HasSameHashThan(hashCalculatedBefore)))
      {
        _lastWriteTimeUtc = info.LastWriteTimeUtc;
        _creationTimeUtc = info.CreationTimeUtc;
        _fileLength = info.Length;
        _fileHash = hashCalculatedBefore.Valid ? hashCalculatedBefore : CalculateHash(info);
       
      }

      _attributes = info.Attributes;

      
    }
   

    public bool IsDifferent(System.IO.FileInfo info)
    {
      return info.LastWriteTimeUtc!=_lastWriteTimeUtc
        || info.CreationTimeUtc != _creationTimeUtc
        || info.Length != _fileLength;
    }

    public bool IsDifferent(FileNodeBase from)
    {
      return this._fileLength != from._fileLength ||
        this._creationTimeUtc != from._creationTimeUtc ||
        this._lastWriteTimeUtc != from._lastWriteTimeUtc ||
        (!this.HasSameHashThan(from._fileHash));
    }


    public static FileHash CalculateHash(System.IO.FileInfo fileinfo)
    {
      try
      {
        byte[] result=null;
        using(System.IO.FileStream stream = fileinfo.OpenRead())
        {
          result = md5.ComputeHash(stream);
        }
        return new FileHash(result);
      }
      catch(System.IO.IOException)
      {
        return new FileHash();
      }
    }

    public FileHash FileHash 
    {
      get 
      { 
        return _fileHash; 
      }
    }

    public bool HasSameHashThan(FileHash otherfilehash)
    {
      return this._fileHash == otherfilehash;
    }


    public bool HasSameHashThan(FileNodeBase other)
    {
      return HasSameHashThan(other._fileHash);
    }

    public bool HasSameHashThan(string fullFileName)
    {
      System.IO.FileInfo fileinfo = new System.IO.FileInfo(fullFileName);
      FileHash otherFileHash = CalculateHash(fileinfo);
      return HasSameHashThan(otherFileHash);
      
    }

    public string MediumFileName
    {
      get
      {
        return string.Format("X{0}.XXX", _fileHash.BinHexRepresentation);
      }
    }

    public DateTime CreationTimeUtc { get { return this._creationTimeUtc; }}
    public DateTime LastWriteTimeUtc { get { return this._lastWriteTimeUtc; }}
    public System.IO.FileAttributes Attributes { get { return this._attributes; }}
 
    #region IComparable Members

    public int CompareTo(object obj)
    {
      if(obj is string)
        return string.Compare(this._name,(string)obj,true);
      else if(obj is FileNodeBase)
        return string.Compare(this._name,((FileNodeBase)obj)._name,true);
      else
        throw new ArgumentException("Cannot compare a SimpleFileNode with a object of type " + obj.GetType().ToString());
    }

    public override string ToString()
    {
      return _name==null ? base.ToString() : _name;
    }

    #endregion
  }
}
