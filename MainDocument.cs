using System;
using System.Collections;

namespace SyncTwoCo
{
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

    public static MainDocument Open(string filename)
    {
      MainDocument doc = new MainDocument();
      doc.OpenAsXML(filename);
      return doc;
    }

    public void Save(string filename)
    {
   
      if(!System.IO.Path.HasExtension(filename))
        filename += ".stc";

      // first create a directory
      string dirname = System.IO.Path.GetFileNameWithoutExtension(filename);
      System.IO.Directory.CreateDirectory(dirname);
      System.IO.FileStream stream = new System.IO.FileStream(filename,System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite,System.IO.FileShare.None);
      System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream,System.Text.Encoding.UTF8);
      tw.WriteStartDocument();
      tw.WriteStartElement("TwoPointSynchronizationByDataMedium");
      Save(tw);
      tw.WriteEndElement();
      tw.WriteEndDocument();
      SetFileSavedFlag(filename);
      tw.Flush();
      tw.Close();
    }

    public void SaveFilterOnly(string filename)
    {
   
      if(!System.IO.Path.HasExtension(filename))
        filename += ".flt";

      System.IO.FileStream stream = new System.IO.FileStream(filename,System.IO.FileMode.Create,System.IO.FileAccess.ReadWrite,System.IO.FileShare.None);
      System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream,System.Text.Encoding.UTF8);
      tw.WriteStartDocument();
      tw.WriteStartElement("TwoPointSynchronizationByDataMedium");
      SaveFilterOnly(tw);
      tw.WriteEndElement();
      tw.WriteEndDocument();
      tw.Flush();
      tw.Close();
    }


    public void SetFileSavedFlag(string filename)
    {
      _FileName = filename;
      _IsDirty = false;
    }

    public bool  HasFileName 
    { 
      get 
      { 
        return _FileName != null && _FileName != string.Empty;
      }
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
      return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName) , System.IO.Path.GetFileNameWithoutExtension(fileName));
    }

    public string MediumDirectoryName
    {
      get 
      {
        return MediumDirectoryNameFromFileName(this._FileName);
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

    public void AddRoot(string basename)
    {
      this._allFilesHere=null;

      EnsureAlignment();

      RootPair added = new RootPair(this);
      _rootPairs.Add(added);
      added.MyRoot.SetFilePath(basename);
    }

    public void SetBasePath(int item, string path)
    {
      EnsureAlignment();

      ((RootPair)_rootPairs[item]).MyRoot.SetFilePath(path);
    }

    public void Update()
    {
      this._allFilesHere=null;

      EnsureAlignment();

      for(int i=0;i<Count;i++)
        RootPair(i).Update();
    }

    public void CopyFilesToMedium()
    {
      EnsureAlignment();

      if(_FileName==null || _FileName==string.Empty || _IsDirty)
        throw new ApplicationException("The document was not saved yet");
    
      if(!System.IO.Directory.Exists(MediumDirectoryName))
        throw new ApplicationException(string.Format("The directory {0} does not exist!",MediumDirectoryName));


      // create a table with all md5 hash sums of the foreign(!) file system
      MD5SumHashTable md5HashTable = new MD5SumHashTable();
      for(int i=0;i<Count;i++)
      {
        if(MyRoot(i).IsValid)
          RootPair(i).ForeignRoot.FillMd5HashTable(md5HashTable);
      }


      Hashtable copiedFiles = new Hashtable();

      // now copy first with exclusion of the already existing files 
      for(int i=0;i<Count;i++)
      {
        if(MyRoot(i).IsValid)
          RootPair(i).CopyFilesToMedium(MediumDirectoryName,copiedFiles,md5HashTable);
      }

      // and now copy all other files
      for(int i=0;i<Count;i++)
      {
        if(MyRoot(i).IsValid)
          RootPair(i).CopyFilesToMedium(MediumDirectoryName,copiedFiles,null);
      }
    }


    public void ClearMediumDirectory()
    {
      if(!this.HasFileName)
        throw new ApplicationException("Must have a file name to know where the medium directory is");

      System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(this.MediumDirectoryName);
      if(!dirinfo.Exists)
        return;

      System.IO.FileInfo[] fileinfos = dirinfo.GetFiles("X*.XXX");

      foreach(System.IO.FileInfo fileinfo in fileinfos)
        fileinfo.Delete();
    }

    public void Save()
    {
      if(!HasFileName)
        throw new ApplicationException("No file name for saving document (programming error)");

      Save(_FileName);
    }

   

    public void UpdateAndSaveAndCopyFilesToMedium()
    {
      if(!this.HasFileName)
        throw new ApplicationException("This operation is possible only if the document has a file name");

      this.Update();
      this.ClearMediumDirectory();
      this.Save();
      this.CopyFilesToMedium();
    }

    public Collector[] Collect()
    {
      EnsureAlignment();

      if(_FileName==null || _FileName==string.Empty)
        throw new ApplicationException("The document was not saved yet");


      // before we collect, we save the md5 hashes of all files (not current, but from the XML file)
      // into one hashtable

      this._allFilesHere = new MD5SumFileNodesHashTable();
      for(int i=0;i<Count;i++)
      {
        if(MyRoot(i).IsValid)
          RootPair(i).MyRoot.FillMD5SumFileNodesHashTable(this._allFilesHere);
      }

      // now that we have all the current files, the information can be used by the collectors to
      // decide if a file can be copied or not

      Collector[] collectors = new Collector[Count];
      for(int i=0;i<Count;i++)
      {
        collectors[i] = new Collector(this.MediumDirectoryName,MyRoot(i).FilePath,this._allFilesHere);
        if(ForeignRoot(i).IsValid && MyRoot(i).IsValid)
          RootPair(i).Collect(collectors[i]);
      }
      return collectors;
    }

    public void CopyWithDirectoryCreation(string sourceFileName, string destFileName, bool overwrite)
    {
      string dirname = System.IO.Path.GetDirectoryName(destFileName);
      if(!System.IO.Directory.Exists(dirname))
        System.IO.Directory.CreateDirectory(dirname);

      System.IO.File.Copy(sourceFileName,destFileName,overwrite);
    }

    /// <summary>
    /// This functions looks from where to copy the foFileNode from. First we have a look in our own file system.
    /// Then we look at the files on the medium.
    /// </summary>
    /// <param name="foFileNode">This is the file in the foreign file system that should be copied here to destFileName.</param>
    /// <param name="destFileName">The destination file name where to copy to.</param>
    /// <param name="overwrite">True if the destination file can be overwritten. False if not.</param>
    /// <returns>True if the copy was successfull, false otherwise.</returns>
    public bool CopyWithDirectoryCreation(FileNode foFileNode, string destFileName, bool overwrite)
    {
      object o = this._allFilesHere[foFileNode.FileHash];
      if(o is PathAndFileNode)
      {
        PathAndFileNode pfn = (PathAndFileNode)o;
        if(System.IO.File.Exists(pfn.Path) && foFileNode.HasSameHashThan(pfn.Path))
        {
          CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite);
          return true;
        }
      }
      else if(o is ArrayList)
      {
        ArrayList arr = (ArrayList)o;
        foreach(PathAndFileNode pfn in arr)
        {
          if(System.IO.File.Exists(pfn.Path) && foFileNode.HasSameHashThan(pfn.Path))
          {
            CopyWithDirectoryCreation(pfn.Path, destFileName, overwrite);
            return true;
          }
        }
      }
    

      string sourcefilename = System.IO.Path.Combine(this.MediumDirectoryName,foFileNode.MediumFileName);
      if(foFileNode.HasSameHashThan(sourcefilename))
      {
        CopyWithDirectoryCreation(sourcefilename, destFileName, overwrite);
        return true;
      }
      
      return false;
      }


    public void PerformAction(SyncItemTag tag)
    {
      string myfilename = System.IO.Path.Combine(MyRoot(tag.RootListIndex).FilePath,tag.FileName);
      FileSystemRoot myRoot = MyRoot(tag.RootListIndex);
      FileSystemRoot foRoot = ForeignRoot(tag.RootListIndex);
      FileNode foFileNode;
     
      System.IO.FileInfo myfileinfo;
      FileNode myfilenode;

      switch(tag.Action)
      {
        case SyncAction.Remove:
          if(myfilename[myfilename.Length-1]==System.IO.Path.DirectorySeparatorChar) // delete subdir
          {
            try 
            {
              System.IO.Directory.Delete(myfilename); 
              myRoot.DeleteSubDirectoryNode(tag.FileName);
              foRoot.DeleteSubDirectoryNode(tag.FileName);

            }
            catch(System.IO.IOException )  { } // Don't care if subdir cannot be deleted, maybe files are still in there!
          }
          else // Delete file
          {
            System.IO.File.Delete(myfilename);
            myRoot.DeleteFileNode(tag.FileName);
            foRoot.DeleteFileNode(tag.FileName);
          }
          break;
        case SyncAction.RemoveRollback:
          foRoot.DeleteFileNode(tag.FileName);
          break;
        case SyncAction.Copy:
          foFileNode = foRoot.GetFileNode(tag.FileName);
       
          if(CopyWithDirectoryCreation(foFileNode,myfilename,false))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = MyRoot(tag.RootListIndex).UpdateMyFile(myfileinfo,true);
            if(foFileNode.HasSameHashThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.Overwrite:
        case SyncAction.ResolveManuallyOverwrite:
          foFileNode = foRoot.GetFileNode(tag.FileName);
          if(CopyWithDirectoryCreation(foFileNode,myfilename,true))
          {
            // update the own FileNode (enforce (!) update the hash of this node also), only if the hash match, then set the own and the foreign
            // file node to unchanged
            myfileinfo = new System.IO.FileInfo(myfilename);
            myfilenode = MyRoot(tag.RootListIndex).UpdateMyFile(myfileinfo,true);
            if(foFileNode.HasSameHashThan(myfilenode))
            {
              foFileNode.SetToUnchanged();
              myfilenode.SetToUnchanged();
            }
          }
          break;
        case SyncAction.ResolveManuallyRollback:
          foFileNode = foRoot.GetFileNode(tag.FileName);
          foFileNode.SetToUnchanged();
          myfileinfo = new System.IO.FileInfo(myfilename);
          myfilenode = MyRoot(tag.RootListIndex).UpdateMyFile(myfileinfo,true);
          if(myfilenode.IsUnchanged)
            myfilenode.SwitchFromUnchangedToChanged();
          break;
      }
    }
  }
}
