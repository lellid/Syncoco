using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace Syncoco
{
  using Filter;
  using Traversing;

  
  /// <summary>
  /// Summary description for RootPair.
  /// </summary>
  [Serializable]
  public class RootPair
  {
    FileSystemRoot _root1;
    FileSystemRoot _root2;
    PathFilter _pathFilter;

    [NonSerialized]
    MainDocument _parent;

    public RootPair(MainDocument parent)
    {
      _parent = parent;
      _root1 = new FileSystemRoot();
      _root2 = new FileSystemRoot();
      _pathFilter = new PathFilter();
    }

    public RootPair(MainDocument parent, System.Xml.XmlTextReader tr)
    {
      _parent = parent;
      Open(tr);
    }

    public void RestoreParentOfChildObjects(MainDocument parent)
    {
      _parent = parent;

      if(null!=_root1)
        _root1.RestoreParentOfChildObjects();
      if(null!=_root2)
        _root2.RestoreParentOfChildObjects();
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      Save(tw,false);
    }

    public void SaveFilterOnly(System.Xml.XmlTextWriter tw)
    {
      Save(tw,true);
    }

    public void Save(System.Xml.XmlTextWriter tw, bool saveFilterOnly)
    {
      tw.WriteStartElement("PathFilter");
      _pathFilter.Save(tw);
      tw.WriteEndElement();

      if(_root1!=null && _root1.IsValid)
      {
        _root1.Save(tw,"Root1",saveFilterOnly);
      }

      if(_root2!=null && _root2.IsValid)
      {
        _root2.Save(tw,"Root2",saveFilterOnly);
      }

    
    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      tr.ReadStartElement("PathFilter");
      this._pathFilter = new PathFilter(tr);
      tr.ReadEndElement();


      if(tr.LocalName=="Root1")
      {
        _root1 = new FileSystemRoot(tr,"Root1");
      }
      else
      {
        _root1 = new FileSystemRoot();
      }
      
      
      if(tr.LocalName=="Root2")
      {
        _root2 = new FileSystemRoot(tr,"Root2");
      }
      else
      {
        _root2 = new FileSystemRoot();
      }

   

    }

    public FileSystemRoot MyRoot
    {
      get 
      { 
        return _parent.RootsExchanged ? _root2 : _root1;
      }
    }

    public FileSystemRoot ForeignRoot
    {
      get
      {
        return _parent.RootsExchanged ? _root1 : _root2;
      }
    }

    public PathFilter PathFilter
    {
      get { return _pathFilter; }
    }

    public FileSystemRoot root1
    {
      get 
      { 
        return _root1;
      }
      set
      {
        _root1 = value;
      }
    }

    public FileSystemRoot root2
    {
      get
      {
        return _root2;
      }
      set
      {
        _root2 = value;
      }
    }



   
  }
}
