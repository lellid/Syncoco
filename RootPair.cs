using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace SyncTwoCo
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


    public void Update(bool forceUpdateHash, IBackgroundMonitor monitor)
    {
      PathFilter.ResetCurrentDirectory();
      MyRoot.Update(PathFilter, forceUpdateHash, monitor);
    }

   


    public void CopyFilesToMedium(string directoryname, Hashtable copiedFiles, MD5SumHashTable md5Hashes, IBackgroundMonitor monitor)
    {
      FileSystemRoot myRoot = this.MyRoot;
      FileSystemRoot foreignRoot = this.ForeignRoot;
 
      if(monitor.ShouldReport)
        monitor.Report("Look for files to transfer in " + directoryname);

      FilesToTransferCollector ftCollector = new FilesToTransferCollector(myRoot.DirectoryNode,foreignRoot.DirectoryNode);
      ftCollector.Traverse();
      SortedList changedfiles = ftCollector.Result;

      // Here code found to get the free space on the drive
      /*
      ManagementPath path = ManagementPath.DefaultPath;
      path.RelativePath = "Win32_LogicalDisk='c:'";
      System.Management.ManagementObject o = new ManagementObject(path);
      Console.WriteLine("Drive C: size {0}, free space {1}", o["Size"], o["FreeSpace"]);
      */
    
      ulong freeBytesAvailable,totalNumberOfBytes,totalNumberOfFreeBytes;
      GetDiskFreeSpaceEx(directoryname, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
      long maxLength = (long)freeBytesAvailable;
      // im Moment greifen wir uns einfach eine Datei raus, die draufpasst



      bool tryToCopy;
      do
      {
        tryToCopy = false;
        foreach(DictionaryEntry entry in changedfiles)
        {

          FileNode node = (FileNode)entry.Value;
          if(node.FileLength<maxLength 
            && !copiedFiles.Contains(node)
            && (md5Hashes==null || !node.IsContainedIn(md5Hashes))
            )
          {
            

            string sourcefilename = PathUtil.Combine_Abspath_RelpathFilename(myRoot.FilePath, (string)entry.Key);
            string destfilename = PathUtil.Combine_Abspath_Filename(directoryname, node.MediumFileName);

            tryToCopy = true;
            copiedFiles.Add(node,null);

            if(!System.IO.File.Exists(destfilename)) // copy only, if this file not already exists
            {
              if(monitor.ShouldReport)
                monitor.Report("Copy file " + sourcefilename);

              System.IO.File.Copy(sourcefilename,destfilename);
              GetDiskFreeSpaceEx(directoryname, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
              maxLength = (long)freeBytesAvailable;
            }
          }
        }
      } while(tryToCopy==true);
    }


    
    [DllImport("kernel32", CharSet=CharSet.Auto)] 
    static extern int GetDiskFreeSpaceEx( 
      string lpDirectoryName, 
      out ulong lpFreeBytesAvailable, 
      out ulong lpTotalNumberOfBytes, 
      out ulong lpTotalNumberOfFreeBytes);


    public static long GetFreeBytesAvailable(string path)
    {
      ulong freeBytesAvailable,totalNumberOfBytes,totalNumberOfFreeBytes;
      GetDiskFreeSpaceEx(path,out freeBytesAvailable,out totalNumberOfBytes,out totalNumberOfFreeBytes);
      return (long) freeBytesAvailable;
    }
  }
}
