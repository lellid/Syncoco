using System;

namespace Syncoco
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
     
      
#if WRITEDATEASTICKS
      tw.WriteElementString("RT",System.Xml.XmlConvert.ToString(_removedTimeUtc.Ticks));
#else
      tw.WriteElementString("RT",System.Xml.XmlConvert.ToString(_removedTimeUtc));
#endif

    }

    public void Open(System.Xml.XmlTextReader tr)
    {
#if READDATEASTICKS
      _removedTimeUtc = new DateTime(System.Xml.XmlConvert.ToInt64(tr.ReadElementString("RT")));
#else
      _removedTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("RT"));
#endif
    }
  }
}
