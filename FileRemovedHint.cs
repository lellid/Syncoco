using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for FileRemovedHint.
  /// </summary>
  [Serializable]
  public class FileRemovedHint : IFileHint
  {
    DateTime _removedTimeUtc;

    public FileRemovedHint()
    {
      _removedTimeUtc = DateTime.UtcNow;
    }

    public FileRemovedHint(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
     
      tw.WriteElementString("RT",System.Xml.XmlConvert.ToString(_removedTimeUtc));
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
     
      _removedTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("RT"));
    }
  }
}
