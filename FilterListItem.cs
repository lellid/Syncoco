using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for FilterListItem.
  /// </summary>
  public class FilterListItem
  {
    /// <summary>The relative path that this FilterListItem belongs to. The path must <b>not</b> contain wildcards.</summary>
    public string _path;
    public FilterList Filter = new FilterList();

    /// <summary>Subdirectories that this path includes.</summary>
    string[] _subDirs = new string[0];
    

    public FilterListItem(FilterListItem from)
    {
      this.CopyFrom(from);
    }

    public FilterListItem(string path)
    {
      this.Path = path;
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
     
      tw.WriteElementString("Path",this._path);
      tw.WriteStartElement("Filter");
      this.Filter.Save(tw);
      tw.WriteEndElement();
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      Path = tr.ReadElementString("Path");
      tr.ReadStartElement("Filter");
      this.Filter = new FilterList(tr);
      tr.ReadEndElement();
    }

    public FilterListItem(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }


    /// <summary>The relative path that this FilterListItem belongs to. The path must <b>not</b> contain wildcards.</summary>
    public string Path
    {
      get { return _path; }
      set 
      {
        _path = NormalizePath(value);
        _subDirs = _path.Trim(System.IO.Path.DirectorySeparatorChar).Split(new char[]{System.IO.Path.DirectorySeparatorChar});
      }
    }
    
    public void CopyFrom(FilterListItem from)
    {
      this.Path = from.Path;
      this.Filter.CopyFrom(from.Filter);
    }

    /// <summary>
    /// Gets the array of subdirectories of the relative path that this item applies to.
    /// </summary>
    public string[] PathSubDirectory
    {
      get { return _subDirs; }
    }
    
    /// <summary>
    /// The depth of the path, i.e. the number of subdirectories that the path includes.
    /// </summary>
    public int PathDepth
    {
      get { return _subDirs.Length; }
    }

    /// <summary>
    /// Returns a path with is forced to end with a DirectorySeparatorChar
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string NormalizePath(string path)
    {
      if(path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
        return path;
      else
        return path+System.IO.Path.DirectorySeparatorChar;
    }
  }
}
