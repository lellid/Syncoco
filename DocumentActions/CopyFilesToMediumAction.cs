using System;
using System.Collections;
using System.Runtime.InteropServices;

using SyncTwoCo.Filter;
using SyncTwoCo.Traversing;

namespace SyncTwoCo.DocumentActions
{
  /// <summary>
  /// Summary description for CopyFilesToMedium.
  /// </summary>
  public class CopyFilesToMediumAction : AbstractDocumentAction
  {
    public CopyFilesToMediumAction(MainDocument doc, IBackgroundMonitor monitor)
      : base(doc,monitor)
    {
    }

    public CopyFilesToMediumAction(MainDocument doc)
      : this(doc,null)
    {
    }

    public override void DirectExecute()
    {
      _doc.EnsureAlignment();

      if(!_doc.HasFileName || _doc.IsDirty)
        throw new ApplicationException("The document was not saved yet");
    

      PathUtil.Assert_Abspath(_doc.MediumDirectoryName);
      if(!System.IO.Directory.Exists(_doc.MediumDirectoryName))
        throw new ApplicationException(string.Format("The directory {0} does not exist!",_doc.MediumDirectoryName));

      if(_monitor.ShouldReport)
        _monitor.Report("Filling MD5 hashtable ...");

      // create a table with all md5 hash sums of the foreign(!) file system
      MD5SumHashTable md5HashTable = new MD5SumHashTable();
      for(int i=0;i<_doc.Count;i++)
      {
        if(_doc.MyRoot(i).IsValid)
        {
          if(_monitor.ShouldReport)
            _monitor.Report("Filling MD5 hashtable from " + _doc.RootPair(i).ForeignRoot.FilePath);

          FillMd5HashTable(_doc.RootPair(i).ForeignRoot,md5HashTable);
        }
      }


      Hashtable copiedFiles = new Hashtable();

      // now copy first with exclusion of the already existing files 
      for(int i=0;i<_doc.Count;i++)
      {
        if(_doc.MyRoot(i).IsValid)
          CopyFilesToMedium(_doc.RootPair(i),_doc.MediumDirectoryName,copiedFiles,md5HashTable,_monitor);
      }

      // and now copy all other files
      for(int i=0;i<_doc.Count;i++)
      {
        if(_doc.MyRoot(i).IsValid)
          CopyFilesToMedium(_doc.RootPair(i),_doc.MediumDirectoryName,copiedFiles,null,_monitor);
      }
    }


    public void FillMd5HashTable(FileSystemRoot fileSystemRoot, MD5SumHashTable table)
    {
      if(null!=fileSystemRoot.DirectoryNode && null!=fileSystemRoot.FilePath)
      {
        Traversing.Md5HashTableCollector coll = new Traversing.Md5HashTableCollector(fileSystemRoot.DirectoryNode,fileSystemRoot.FilePath,table);
        coll.Traverse();
      }
    }

    


    public void CopyFilesToMedium(RootPair rootPair, string directoryname, Hashtable copiedFiles, MD5SumHashTable md5Hashes, IBackgroundMonitor monitor)
    {
      FileSystemRoot myRoot = rootPair.MyRoot;
      FileSystemRoot foreignRoot = rootPair.ForeignRoot;
 
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