using System;
using System.Collections;

namespace SyncTwoCo
{
  using Traversing;
  /// <summary>
  /// Summary description for MainDocument.
  /// </summary>
  [Serializable]
  public class MainDocument
  {
    [NonSerialized]
    string _FileName;
    [NonSerialized]
    bool   _IsDirty;
    [NonSerialized]
    MD5SumFileNodesHashTable _allFilesHere;
    

    //ArrayList _root1, _root2;
    string    _root1ComputerName, _root2ComputerName;
    ArrayList _rootPairs;

    [NonSerialized]
    bool _ExchangeRoots;

    public MainDocument()
    {
      //_root1 = new ArrayList();
      //_root2 = new ArrayList();
      _rootPairs = new ArrayList();
      _root1ComputerName = Current.ComputerName;

    }

    public void RestoreParentOfChildObjects()
    {
      for(int i=0;i<_rootPairs.Count;i++)
        this.RootPair(i).RestoreParentOfChildObjects(this);
    }

    public void Save(string filename)
    {
      
      if(!System.IO.Path.HasExtension(filename))
        filename += ".stc";

      if(System.IO.Path.GetExtension(filename).ToLower()==".stcbin")
        SaveBinary(filename);
      else
        SaveXML(filename);
    }


    protected void SaveBinary(string filename)
    {
    

      // first create a directory
      string dirname = System.IO.Path.GetFileNameWithoutExtension(filename);
      System.IO.Directory.CreateDirectory(dirname);

      string tempfilename=null;
      Exception exception=null;
      System.IO.FileStream stream=null;
      try
      {

        if(System.IO.File.Exists(filename))
          tempfilename = System.IO.Path.GetTempFileName();


        stream = new System.IO.FileStream(null!=tempfilename ? tempfilename : filename, System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite,System.IO.FileShare.None);
       
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        formatter.Serialize(stream,Current.Document);

        stream.Close();

        if(null!=tempfilename)
          System.IO.File.Copy(tempfilename,filename,true);

      }
      catch(Exception ex)
      {
        exception = ex;
      }
      finally
      {
        if(null!=stream)
          stream.Close();
        if(null!=tempfilename)
          System.IO.File.Delete(tempfilename);
      }

      if(null!=exception)
        throw exception;

    }

    protected void SaveXML(string filename)
    {
      // first create a directory
      string dirname = System.IO.Path.GetFileNameWithoutExtension(filename);
      System.IO.Directory.CreateDirectory(dirname);

      string tempfilename=null;
      Exception exception=null;
      System.IO.FileStream stream=null;
      try
      {

        if(System.IO.File.Exists(filename))
          tempfilename = System.IO.Path.GetTempFileName();


        stream = new System.IO.FileStream(null!=tempfilename ? tempfilename : filename, System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite,System.IO.FileShare.None);
        System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream,System.Text.Encoding.UTF8);
        tw.WriteStartDocument();
        tw.WriteStartElement("TwoPointSynchronizationByDataMedium");
        Save(tw);
        tw.WriteEndElement();
        tw.WriteEndDocument();
        SetFileSavedFlag(filename);
        tw.Flush();
        tw.Close();

        if(null!=tempfilename)
          System.IO.File.Copy(tempfilename,filename,true);

      }
      catch(Exception ex)
      {
        exception = ex;
      }
      finally
      {
        if(null!=stream)
          stream.Close();
        if(null!=tempfilename)
          System.IO.File.Delete(tempfilename);
      }

      if(null!=exception)
        throw exception;

    }

    public void SaveFilterOnly(string filename)
    {
   
      if(!System.IO.Path.HasExtension(filename))
        filename += ".flt";

      string tempfilename=null;
      Exception exception=null;
      try
      {

        if(System.IO.File.Exists(filename))
          tempfilename = System.IO.Path.GetTempFileName();


        System.IO.FileStream stream = new System.IO.FileStream(null!=tempfilename ? tempfilename : filename,System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite,System.IO.FileShare.None);
        System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream,System.Text.Encoding.UTF8);
        tw.WriteStartDocument();
        tw.WriteStartElement("TwoPointSynchronizationByDataMedium");
        SaveFilterOnly(tw);
        tw.WriteEndElement();
        tw.WriteEndDocument();
        tw.Flush();
        tw.Close();
        
        if(null!=tempfilename)
          System.IO.File.Copy(tempfilename,filename,true);

      }
      catch(Exception ex)
      {
        exception = ex;
      }
      finally
      {
        if(null!=tempfilename)
          System.IO.File.Delete(tempfilename);
      }

      if(null!=exception)
        throw exception;
    }

    public void Save(System.Xml.XmlTextWriter tw, bool saveFilterOnly)
    {

      tw.WriteElementString("ComputerName1",_root1ComputerName);
      tw.WriteElementString("ComputerName2",_root2ComputerName);
      tw.WriteStartElement("RootPairs");

      for(int i=0;i<_rootPairs.Count;i++)
      {
        tw.WriteStartElement("RootPair");
        RootPair(i).Save(tw, saveFilterOnly);
        tw.WriteEndElement(); // RootPair
      }

      tw.WriteEndElement(); // RootPairs
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      Save(tw,false);
    }
    public void SaveFilterOnly(System.Xml.XmlTextWriter tw)
    {
      Save(tw,true);
    }
    public void Save()
    {
      if(!HasFileName)
        throw new ApplicationException("No file name for saving document (programming error)");

      Save(_FileName);
    }

    


    public void Read(System.Xml.XmlTextReader tr)
    {
      _root1ComputerName = tr.ReadElementString("ComputerName1");
      _root2ComputerName = tr.ReadElementString("ComputerName2");

      tr.ReadStartElement("RootPairs");
      while(tr.LocalName=="RootPair")
      {
        tr.ReadStartElement("RootPair");
        _rootPairs.Add(new RootPair(this,tr));
        tr.ReadEndElement(); // RootPair
      }
      tr.ReadEndElement(); // RootPairs
    }

    

    public void OpenAsXML(string filename)
    {
      System.IO.FileStream stream = new System.IO.FileStream(filename,System.IO.FileMode.Open,System.IO.FileAccess.Read,System.IO.FileShare.None);
      System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(stream);
      tr.WhitespaceHandling = System.Xml.WhitespaceHandling.None;

      tr.MoveToContent();

      tr.ReadStartElement("TwoPointSynchronizationByDataMedium");
      Read(tr);
      tr.ReadEndElement();

      tr.Close();
      stream.Close();

      SetFileSavedFlag(filename);
    }


    public static MainDocument OpenAsBinary(string filename)
    {
      System.IO.FileStream stream = new System.IO.FileStream(filename,System.IO.FileMode.Open,System.IO.FileAccess.Read,System.IO.FileShare.None);
   
      System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
      object o = formatter.Deserialize(stream);
      MainDocument doc = (MainDocument)o;
      stream.Close();

      doc.RestoreParentOfChildObjects();
      doc.SetFileSavedFlag(filename);

    return doc;
    }

  
   

    public void SetFileSavedFlag(string filename)
    {
      _FileName = filename;
      _IsDirty = false;
    }


    public bool IsDirty
    {
      get { return _IsDirty; }
    }

    public bool  HasFileName 
    { 
      get 
      { 
        return _FileName != null && _FileName != string.Empty;
      }
    }

    public string FileName
    {
      get { return _FileName; }
    }

    protected void ExchangeRoots()
    {
      _ExchangeRoots = !_ExchangeRoots;
    }

    /// <summary>
    /// Change the roots so that the actual computer gets the first root
    /// </summary>
    public void Align()
    {
      if((_root1ComputerName==null || _root1ComputerName==string.Empty) && _root2ComputerName != Current.ComputerName)
        _root1ComputerName = Current.ComputerName;
      else if((_root2ComputerName==null || _root2ComputerName==string.Empty) && _root1ComputerName != Current.ComputerName)
        _root2ComputerName = Current.ComputerName;

      if(_root2ComputerName == Current.ComputerName)
        _ExchangeRoots = true;

    }

    public FileSystemRoot MyRoot(int i)
    {
      return ((RootPair)_rootPairs[i]).MyRoot; 
    }

    public FileSystemRoot ForeignRoot(int i)
    {
      return ((RootPair)_rootPairs[i]).ForeignRoot; 
    }

    public RootPair RootPair(int i)
    {
      return (RootPair)_rootPairs[i];
    }
    public string MyRootComputerName 
    { 
      get
      { 
        return _ExchangeRoots ? _root2ComputerName : _root1ComputerName;
      }
    }
    public string ForeignRootComputerName 
    { 
      get 
      { 
        return _ExchangeRoots ? _root1ComputerName : _root2ComputerName; 
      }
    }

    public bool RootsExchanged
    {
      get { return _ExchangeRoots; }
    }

    public static string MediumDirectoryNameFromFileName(string fileName)
    {
      string result = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName) , System.IO.Path.GetFileNameWithoutExtension(fileName));
      result = PathUtil.NormalizeAbspath(result);
#if DEBUG
      PathUtil.Assert_Abspath(result);
#endif

      return result;
    }

    public string MediumDirectoryName
    {
      get 
      {
        string result = MediumDirectoryNameFromFileName(this._FileName);
#if DEBUG
        PathUtil.Assert_Abspath(result);
#endif
        return result;
      }
    }

    public void EnsureAlignment()
    {
      if(Current.ComputerName==null || Current.ComputerName==string.Empty)
        throw new ApplicationException("The current computer name is null or empty");

      Align();
 

      if(this.MyRootComputerName!=Current.ComputerName)
        throw new ApplicationException("The current computer name is non of the computer names in this document");
    }


    public int Count
    {
      get 
      {
        return _rootPairs.Count;
      }
    }

    public void ClearCachedMyFiles()
    {
       this._allFilesHere=null;
    }

    public MD5SumFileNodesHashTable CachedAllMyFiles
    {
      get { return _allFilesHere; }
      set { _allFilesHere = value; }
    }

    public void AddRoot(string basename)
    {
      PathUtil.Assert_Abspath(basename);

      ClearCachedMyFiles();

      EnsureAlignment();

      RootPair added = new RootPair(this);
      _rootPairs.Add(added);
      added.MyRoot.SetFilePath(basename);
    }

    public void SetBasePath(int item, string path)
    {
      PathUtil.Assert_Abspath(path);
      
      EnsureAlignment();
      ((RootPair)_rootPairs[item]).MyRoot.SetFilePath(path);
    }

  

   


  

  
  }
}
