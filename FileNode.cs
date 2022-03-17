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
  /// FileNode holds the information for a single file.
  /// </summary>
  [Serializable]
  public class FileNode : FileNodeBase
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
#if WRITEDATEASTICKS
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc.Ticks));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc.Ticks));
#else
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc));
#endif
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

    public DirectoryNode Parent
    {
      get
      {
        return _parent;
      }
    }
    public void RestoreParentOfChildObjects(DirectoryNode parent)
    {
      _parent = parent;
    }

    
    void Open(System.Xml.XmlTextReader tr)
    {
      _name = tr.GetAttribute("Name");
      tr.ReadStartElement("File");

      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
      _attributes = (System.IO.FileAttributes)System.Xml.XmlConvert.ToInt32(tr.ReadElementString("FA"));
#if READDATEASTICKS
      _creationTimeUtc = new DateTime(System.Xml.XmlConvert.ToInt64(tr.ReadElementString("CT")));
      _lastWriteTimeUtc = new DateTime(System.Xml.XmlConvert.ToInt64(tr.ReadElementString("WT")));
#else
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));
#endif
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
			Update(info, false, true);
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

    public string HintAsString
    {
      get
      {
        if(_hint==null)
          return string.Empty;
        else if(_hint is FileNewHint)
          return "FileNew";
        else if(_hint is FileChangedHint)
          return "FileChanged";
        else if(_hint is FileRemovedHint)
          return "FileRemoved";
        else
          return _hint.ToString();
      }
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

    /// <summary>
    /// This will update the node. In case an IO error occur, the node is not updated, and no error occurs.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="createHint"></param>
    /// <param name="forceUpdateHash"></param>
    protected void Update(System.IO.FileInfo info, bool createHint, bool forceUpdateHash)
    {
      //if(info.Name=="IG004040.JPG")
      //System.Diagnostics.Debug.WriteLine("Remove this line");

      
      _name = info.Name;

     HashResult? hashCalculatedBefore;
            if (forceUpdateHash && info.Exists)
                hashCalculatedBefore = CalculateHash(info);
            else
                hashCalculatedBefore = null;
        

      if(IsDifferent(info) || (hashCalculatedBefore.HasValue && !this.HasSameHashThan(hashCalculatedBefore.Value.Hash)))
      {
        if(hashCalculatedBefore is null)
          hashCalculatedBefore = CalculateHash(info);

        // only update the node when the hash is now valid
        if(hashCalculatedBefore.HasValue)
        {
          if(createHint && !(_hint is FileChangedHint))
            _hint = new FileChangedHint(_creationTimeUtc,_lastWriteTimeUtc,_fileLength,_fileHash);

                    _lastWriteTimeUtc = hashCalculatedBefore.Value.LastWriteTimeUtc;
                    _creationTimeUtc = hashCalculatedBefore.Value.CreationTimeUtc;
                    _fileLength = hashCalculatedBefore.Value.Length;
          _fileHash = hashCalculatedBefore.Value.Hash;
        }
      }
      else // it is not different
      {
        if(createHint && (_hint is FileRemovedHint))
          _hint = null; // remove outstanding file remove hints if the file now exists
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
#if DEBUG
      PathUtil.Assert_AbspathFilename(absoluteFileName);
#endif

      if(!this.IsRemoved)
        table.Add(this._fileHash, absoluteFileName, this);
    }

    public void FillMD5SumFileNodesHashTable (MD5SumFileNodesHashTable table, string absoluteFileName)
    {
#if DEBUG
      PathUtil.Assert_AbspathFilename(absoluteFileName);
#endif

      if(!this.IsRemoved)
        table.Add(this._fileHash,absoluteFileName,this);
    }

    public bool IsContainedIn(MD5SumHashTable table)
    {
      return table.ContainsKey(this._fileHash);
    }

 
    
  }
}
