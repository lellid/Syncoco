using System;
using System.Collections;
using System.Collections.Specialized;



namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for Collector.
  /// </summary>
  public class Collector
  {
    string _MediumDirectory;
    string _BaseDirectory;
    MD5SumFileNodesHashTable _allFilesHereOnDisk;
    
    StringCollection _ToRemove = new StringCollection();
    StringCollection _ToRemoveButChanged = new StringCollection();
    StringCollection _ToCopy = new StringCollection();
    StringCollection _ToOverwrite = new StringCollection();
    StringCollection _ToResolveManually = new StringCollection();


    public StringCollection ToRemove { get { return _ToRemove; }}
    public StringCollection ToRemoveManually { get { return _ToRemoveButChanged; }}
    public StringCollection ToCopy { get { return _ToCopy; }}
    public StringCollection ToOverwrite { get { return _ToOverwrite; }}
    public StringCollection ToResolveManually { get { return _ToResolveManually; }}




    public Collector(string mediumdirectory, string basedirectory, MD5SumFileNodesHashTable  allFilesHereOnDisk)
    {
      _MediumDirectory = mediumdirectory;
      _BaseDirectory = basedirectory;
      _allFilesHereOnDisk = allFilesHereOnDisk;
    }

    public string GetFullPath(string relativdir)
    {
      return System.IO.Path.Combine(_BaseDirectory,relativdir);
    }

    public void AddRemovedFile(string dirbase,string filename, bool isUnchanged)
    {
      if(isUnchanged)
        _ToRemove.Add(System.IO.Path.Combine(dirbase,filename));
      else
        _ToRemoveButChanged.Add(System.IO.Path.Combine(dirbase,filename));

    }

    public void AddRemovedSubdirectory(string dirbase,string subdirname)
    {
        _ToRemove.Add(System.IO.Path.Combine(dirbase,subdirname+System.IO.Path.DirectorySeparatorChar));
    }

    public void AddManuallyResolvedFile(string dirbase,string filename)
    {
      _ToResolveManually.Add(System.IO.Path.Combine(dirbase,filename));
    }

    public void AddFileToCopy(string dirbase, string filename)
    {
      _ToCopy.Add(System.IO.Path.Combine(dirbase,filename));
    }
    public void AddFileToOverwrite(string dirbase, string filename)
    {
      _ToOverwrite.Add(System.IO.Path.Combine(dirbase,filename));
    }

    protected bool IsFileOnMedium(string filename)
    {
      return System.IO.File.Exists(System.IO.Path.Combine(_MediumDirectory,filename));
    }

    public bool IsFileHereAnywhere(FileNode node)
    {
      return IsFileHereOnDisk(node) || IsFileOnMedium(node);
    }


    protected bool IsFileHereOnDisk(PathAndFileNode node)
    {
      return System.IO.File.Exists(node.Path);
    }

    protected bool IsFileHereOnDisk(FileNode node)
    {
      object o = _allFilesHereOnDisk[node.FileHash];
      if(o==null)
        return false;
      else if(o is PathAndFileNode)
      {
        PathAndFileNode pfn = (PathAndFileNode)o;
        return IsFileHereOnDisk(pfn);
      }
      else if( o is System.Collections.ArrayList)
      {
        System.Collections.ArrayList arr = (System.Collections.ArrayList)o;
        foreach(PathAndFileNode pfn in arr)
        {
          if(IsFileHereOnDisk(pfn))
            return true;
        }
      }
      else
        throw new ApplicationException("Unexpected object in hashtable: " + o.ToString());

      return false;
    }
    protected bool IsFileOnMedium(FileNode node)
    {
      return System.IO.File.Exists(System.IO.Path.Combine(_MediumDirectory,node.MediumFileName));
    }
  }
}
