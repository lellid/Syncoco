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
    byte[]   _fileHash;

    public FileChangedHint(DateTime creationTimeUtc, DateTime lastWriteTimeUtc, long fileLength, byte[] fileHash)
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
      tw.WriteStartElement("FH");
      tw.WriteBinHex(_fileHash,0,_fileHash.Length);
      tw.WriteEndElement();
    }

    static byte[] buffer = new byte[32];
    public void Open(System.Xml.XmlTextReader tr)
    {
      
      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));
      _fileHash = new byte[32];
      int read = tr.ReadBinHex(buffer,0,32);
      _fileHash = new byte[read];
      Array.Copy(buffer,0,_fileHash,0,read);
    }
    
  }

 
}
