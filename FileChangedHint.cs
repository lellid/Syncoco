using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for FileChangedHint.
  /// </summary>
  [Serializable]
  public class FileChangedHint : IFileHint
  {
    DateTime _lastWriteTimeUtc;
    DateTime _creationTimeUtc;
    long     _fileLength;
    FileHash   _fileHash;

    public FileChangedHint(DateTime creationTimeUtc, DateTime lastWriteTimeUtc, long fileLength, FileHash fileHash)
    {
      _creationTimeUtc = creationTimeUtc;
      _lastWriteTimeUtc = lastWriteTimeUtc;
      _fileLength = fileLength;
      _fileHash = fileHash;
    }

    public FileChangedHint(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }


    public void Save(System.Xml.XmlTextWriter tw)
    {
      
      tw.WriteElementString("LE",System.Xml.XmlConvert.ToString(_fileLength));
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc));
      tw.WriteElementString("FH",_fileHash.BinHexRepresentation);
    }

    
    public void Open(System.Xml.XmlTextReader tr)
    {
      
      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));
      _fileHash = FileHash.FromBinHexRepresentation(tr.ReadElementString("FH"));
    }
    
  }

 
}
