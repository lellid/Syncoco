using System;

namespace Syncoco
{
  /// <summary>
  /// Summary description for FileNewHint.
  /// </summary>
  [Serializable]
  public class FileNewHint : IFileHint
  {
    public FileNewHint()
    {
    }

    public FileNewHint(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      
    }
  }
}
