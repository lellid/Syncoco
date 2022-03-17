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
using System.Security.Cryptography;


namespace Syncoco
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

      HashResult? hashCalculatedBefore;
            if (forceUpdateHash && info.Exists)
                hashCalculatedBefore = CalculateHash(info);
            else
                hashCalculatedBefore = null;
        

      if(IsDifferent(info) || (hashCalculatedBefore.HasValue && !this.HasSameHashThan(hashCalculatedBefore.Value.Hash)))
      {
                if(hashCalculatedBefore is null)
                {
                    hashCalculatedBefore = CalculateHash(info);
                }

                    _fileHash = hashCalculatedBefore.Value.Hash;
                    _fileLength = hashCalculatedBefore.Value.Length;
                    _creationTimeUtc = hashCalculatedBefore.Value.CreationTimeUtc;
                    _lastWriteTimeUtc = hashCalculatedBefore.Value.LastWriteTimeUtc;
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

    /// <summary>
    /// Only tests the two nodes if they have the same content,
    /// i.e. have same length and file hash.
    /// This is needed if we synchronize different file systems, when
    /// we can't rely on the file times.
    /// </summary>
    /// <param name="from">Another file node.</param>
    /// <returns>True if both files are equal in in file length and  in file hash.</returns>
    public bool HasSameContentThan(FileNodeBase from)
    {
      return this._fileLength == from._fileLength &&
        this.HasSameHashThan(from._fileHash);
    }

        public struct HashResult
        {
            public FileHash Hash { get; }
            public long Length { get; }
            public DateTime CreationTimeUtc { get; }
            public DateTime LastWriteTimeUtc { get; }
            public HashResult(byte[] hash, long length, DateTime creationTime, DateTime lastWriteTime)
            {
                Hash =new FileHash(hash); 
                Length =length;
                CreationTimeUtc =creationTime;
                LastWriteTimeUtc =lastWriteTime;
            }
        }


        public static HashResult CalculateHash(System.IO.FileInfo fileinfo)
        {
            try
            {
                byte[] result = null;
                using (System.IO.FileStream stream = fileinfo.OpenRead())
                //using (System.IO.FileStream stream = new System.IO.FileStream(fileinfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    var lwtBefore = fileinfo.LastWriteTimeUtc;
                    var ctBefore = fileinfo.CreationTimeUtc;
                    var lengthBefore = fileinfo.Length;

                    result = md5.ComputeHash(stream);

                    var lengthStream = stream.Position;
                    var lengthAfter = fileinfo.Length;
                    var lwtAfter = fileinfo.LastWriteTimeUtc;
                    var ctAfter = fileinfo.CreationTimeUtc;

                    if (lengthBefore == lengthAfter && lengthBefore == lengthStream && lwtBefore == lwtAfter && ctBefore == ctAfter)
                    {
                        return new HashResult(result, lengthStream, ctAfter, lwtAfter);
                    }
                    else
                    {
                        if(lengthBefore == lengthAfter && lengthAfter != lengthStream)
                            throw new HashCalculationException(fileinfo.FullName, "File corrupt! (stream length differs from length in file information)");
                        else
                            throw new HashCalculationException(fileinfo.FullName, "File changed during hash calculation!");
                    }
                }
            }
            catch (System.IO.IOException ex2)
            {
                throw new HashCalculationException(fileinfo.FullName, ex2.Message);
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
      var otherFileHash = CalculateHash(fileinfo);
      return HasSameHashThan(otherFileHash.Hash);
      
    }

    public string MediumFileName
    {
      get
      {
        return _fileHash.MediumFileName;
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
