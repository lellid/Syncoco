using System;
using System.Security.Cryptography;


namespace SyncTwoCo
{
  /// <summary>
  /// FileNode holds the information for a single file.
  /// </summary>
  [Serializable]
  public class FileNode
  {
    /// <summary>Used to calculate the hash value.</summary>
    static System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

    /// <summary>Time of last writing to the file.</summary>
    DateTime _lastWriteTimeUtc;
    /// <summary>Time of file creation.</summary>
    DateTime _creationTimeUtc;
    /// <summary>Length of the file.</summary>
    long     _fileLength;
    /// <summary>MD5 hash value.</summary>
    byte[]   _fileHash;
    /// <summary>File attributes.</summary>
    System.IO.FileAttributes _attributes;
    /// <summary>Actual state of the file.</summary>
    object _hint;


    public void Save(System.Xml.XmlTextWriter tw)
    {
      tw.WriteElementString("LE",System.Xml.XmlConvert.ToString(_fileLength));
      tw.WriteElementString("FA",System.Xml.XmlConvert.ToString((int)_attributes));
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc));
      tw.WriteStartElement("FH");
      tw.WriteBinHex(_fileHash,0,_fileHash.Length);
      tw.WriteEndElement();

      if(_hint!=null)
      {
        tw.WriteStartElement("HI");
       
        if(_hint is FileNewHint)
        {
          tw.WriteAttributeString("TY","NEW");
          ((FileNewHint)_hint).Save(tw);
        }
        else if(_hint is FileChangedHint)
        {
          tw.WriteAttributeString("TY","CHG");
          ((FileChangedHint)_hint).Save(tw);
        }
        else if(_hint is FileRemovedHint)
        {
          tw.WriteAttributeString("TY","RMV");
          ((FileRemovedHint)_hint).Save(tw);
        }
        else
          throw new ApplicationException("Unexpected type of hint, the type is " + _hint.GetType().ToString());


        tw.WriteEndElement(); // HI
      }
    }

    public FileNode(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

    static byte[] buffer = new byte[32];
    public void Open(System.Xml.XmlTextReader tr)
    {
      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
      _attributes = (System.IO.FileAttributes)System.Xml.XmlConvert.ToInt32(tr.ReadElementString("FA"));
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));

      if(tr.LocalName!="FH")
        throw new System.Xml.XmlException("The expected node <<FH>> was not found, instead we are at node: " + tr.LocalName);

      _fileHash = new byte[32];
      int read = tr.ReadBinHex(buffer,0,32);
      _fileHash = new byte[read];
      Array.Copy(buffer,0,_fileHash,0,read);

      // tr.ReadEndElement();

      if(tr.LocalName=="HI")
      {
        string hinttype = tr.GetAttribute("TY");
     
        bool isEmptyElement = tr.IsEmptyElement;
        tr.ReadStartElement("HI");
     
        switch(hinttype)
        {
          case "NUL":
            break;
          case "NEW":
            _hint = new FileNewHint(tr);
            break;
          case "CHG":
            _hint = new FileChangedHint(tr);
            break;
          case "RMV":
            _hint = new FileRemovedHint(tr);
            break;
          default:
            throw new ApplicationException("Unknown hint type during deserialization, hint type: " + hinttype);
        }

        if(!isEmptyElement)
          tr.ReadEndElement();
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
    /// Constructor. Creates a FileNode out of a file info for that file.
    /// </summary>
    /// <param name="info"></param>
    public FileNode(System.IO.FileInfo info)
    {
      Update(info,false,true);
      _hint = new FileNewHint();
    }

    /// <summary>
    /// True if the file is not longer existent (in the file system).
    /// </summary>
    public bool IsRemoved
    {
      get { return _hint is FileRemovedHint; }
    }
    /// <summary>
    /// Sets the file state. The file is no longer existent in the file system.
    /// </summary>
    public void SetToRemoved()
    {
      _hint = new FileRemovedHint();
    }

    public bool IsNew
    {
      get { return _hint is FileNewHint; }
    }
    
    public bool IsChanged
    {
      get { return _hint is FileChangedHint; }
    }

    /// <summary>
    /// This function is called when the node is unchanged, but differences between the own node and the foreign node had occured.
    /// </summary>
    public void SwitchFromUnchangedToChanged()
    {
      if(_hint==null)
        this._hint = new FileChangedHint(this._creationTimeUtc,this._lastWriteTimeUtc,this._fileLength,this._fileHash);
    }

    public void Update(System.IO.FileInfo info, bool forceUpdateHash)
    {
      Update(info,true, forceUpdateHash);
    }

    protected void Update(System.IO.FileInfo info, bool createHint, bool forceUpdateHash)
    {
      byte[] hashCalculatedBefore=null;
      if(forceUpdateHash && info.Exists)
        hashCalculatedBefore = CalculateHash(info);

      if(IsDifferent(info) || (null!=hashCalculatedBefore && !this.HasSameHashThan(hashCalculatedBefore)))
      {
        if(createHint && !(_hint is FileChangedHint))
          _hint = new FileChangedHint(_creationTimeUtc,_lastWriteTimeUtc,_fileLength,_fileHash);

        _lastWriteTimeUtc = info.LastWriteTimeUtc;
        _creationTimeUtc = info.CreationTimeUtc;
        _fileLength = info.Length;
        _fileHash = null!=hashCalculatedBefore ? hashCalculatedBefore : CalculateHash(info);
      }

      _attributes = info.Attributes;

    }

    public bool IsDifferent(System.IO.FileInfo info)
    {
      return info.LastWriteTimeUtc!=_lastWriteTimeUtc
        || info.CreationTimeUtc != _creationTimeUtc
        || info.Length != _fileLength;
    }

    public bool IsDifferent(FileNode from)
    {
      return this._fileLength != from._fileLength ||
        this._creationTimeUtc != from._creationTimeUtc ||
        this._lastWriteTimeUtc != from._lastWriteTimeUtc ||
        (!this.HasSameHashThan(from._fileHash));
    }

    public bool IsDataContentNewOrChanged
    {
      get
      {
        if(_hint is FileNewHint)
          return true;
        if(_hint is FileChangedHint)
          return true;
        else
          return false;

      }
    }

    public bool IsUnchanged 
    {
      get { return _hint==null; }
    }

    public void SetToUnchanged()
    {
      _hint = null;
    }


    public static byte[] CalculateHash(System.IO.FileInfo fileinfo)
    {
      byte[] result=null;
      using(System.IO.FileStream stream = fileinfo.OpenRead())
      {
        result = md5.ComputeHash(stream);
      }
      return result;
    }

    public byte[] FileHash 
    {
      get 
      { 
        return _fileHash; 
      }
    }

    public bool HasSameHashThan(byte[] otherfilehash)
    {
      if(this._fileHash.Length != otherfilehash.Length)
        return false;
      for(int i=0;i<_fileHash.Length;i++)
        if(_fileHash[i]!=otherfilehash[i])
          return false;
      return true;
    }


    public bool HasSameHashThan(FileNode other)
    {
      return HasSameHashThan(other._fileHash);
    }

    public bool HasSameHashThan(string fullFileName)
    {
      System.IO.FileInfo fileinfo = new System.IO.FileInfo(fullFileName);
      byte[] fileHash;
      using(System.IO.FileStream stream = fileinfo.OpenRead())
      {
        fileHash = md5.ComputeHash(stream);
      }
      return HasSameHashThan(fileHash);
      
    }

    public void FillMd5HashTable(MD5SumHashTable table)
    {
      if(!this.IsRemoved)
        table.Add(this._fileHash,this._fileLength);
    }

    public void FillMD5SumFileNodesHashTable (MD5SumFileNodesHashTable table, string absoluteFileName)
    {
      if(!this.IsRemoved)
        table.Add(this._fileHash,absoluteFileName,this);
    }

    public bool IsContainedIn(MD5SumHashTable table)
    {
      return table.ContainsKey(this._fileHash);
    }

    public string MediumFileName
    {
      get
      {
        //const byte nullByte = 0;

        System.Text.StringBuilder stb = new System.Text.StringBuilder();
        stb.Append('X');

        //byte lenByte = (byte)_fileHash.Length;
        //stb.AppendFormat("{0:X}",lenByte);

        for(int i=0;i<_fileHash.Length;i++)
          stb.AppendFormat("{0:X02}",_fileHash[i]);
        //for(int i=_fileHash.Length;i<16;i++)
        //stb.AppendFormat("{0:X02}",nullByte);

        stb.Append(".XXX");

        return stb.ToString();
      }
    }
  }
}
