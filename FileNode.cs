using System;
using System.Security.Cryptography;


namespace SyncTwoCo
{
  
  /// <summary>
  /// FileNode holds the information for a single file.
  /// </summary>
  [Serializable]
  public class FileNode : SimpleFileNode
  {
    object _hint;

    [NonSerialized]
    DirectoryNode _parent;

    public void Save(System.Xml.XmlTextWriter tw)
    {
      System.Diagnostics.Debug.Assert(_name!=null);
      System.Diagnostics.Debug.Assert(_name!=string.Empty);

      tw.WriteStartElement("File");
      
      tw.WriteAttributeString("Name",_name);

      tw.WriteElementString("LE",System.Xml.XmlConvert.ToString(_fileLength));
      tw.WriteElementString("FA",System.Xml.XmlConvert.ToString((int)_attributes));
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc));
      tw.WriteElementString("FH",_fileHash.BinHexRepresentation);

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

      tw.WriteEndElement(); // File
    }

   
    public FileNode(System.Xml.XmlTextReader tr, DirectoryNode parent)
    {
      System.Diagnostics.Debug.Assert(null!=parent);
      _parent = parent;
      Open(tr);
    }

    
    void Open(System.Xml.XmlTextReader tr)
    {
      _name = tr.GetAttribute("Name");
      tr.ReadStartElement("File");

      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
      _attributes = (System.IO.FileAttributes)System.Xml.XmlConvert.ToInt32(tr.ReadElementString("FA"));
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));

      if(tr.LocalName!="FH")
        throw new System.Xml.XmlException("The expected node <<FH>> was not found, instead we are at node: " + tr.LocalName);

       _fileHash = FileHash.FromBinHexRepresentation(tr.ReadElementString("FH"));
      
      

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

      tr.ReadEndElement(); // File
     
    }

   

    /// <summary>
    /// Constructor. Creates a FileNode out of a file info for that file.
    /// </summary>
    /// <param name="info"></param>
    public FileNode(System.IO.FileInfo info, DirectoryNode parent)
    {
      System.Diagnostics.Debug.Assert(null!=parent);
      _parent = parent;
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

    public new void Update(System.IO.FileInfo info, bool forceUpdateHash)
    {
      Update(info,true, forceUpdateHash);
    }

    protected void Update(System.IO.FileInfo info, bool createHint, bool forceUpdateHash)
    {
      

      _name = info.Name;

      FileHash hashCalculatedBefore;
      if(forceUpdateHash && info.Exists)
        hashCalculatedBefore = CalculateHash(info);
      else
        hashCalculatedBefore = new FileHash();
        

      if(IsDifferent(info) || (hashCalculatedBefore.Valid && !this.HasSameHashThan(hashCalculatedBefore)))
      {
        if(createHint && !(_hint is FileChangedHint))
          _hint = new FileChangedHint(_creationTimeUtc,_lastWriteTimeUtc,_fileLength,_fileHash);

        _lastWriteTimeUtc = info.LastWriteTimeUtc;
        _creationTimeUtc = info.CreationTimeUtc;
        _fileLength = info.Length;
        _fileHash = hashCalculatedBefore.Valid ? hashCalculatedBefore : CalculateHash(info);
      }

      _attributes = info.Attributes;

     
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



    public void FillMd5HashTable(MD5SumHashTable table, string absoluteFileName)
    {
      if(!this.IsRemoved)
        table.Add(this._fileHash, absoluteFileName, this);
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

 
    
  }
}
