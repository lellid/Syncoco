#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco:  synchronizing two computers with a data medium
//    Copyright (C) 2004-2005 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.Collections.Specialized;


using Syncoco.Filter;

namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for Collector.
  /// </summary>
  public class FilesToSynchronizeCollector
  {
    string _AbsMediumDirectory;
    string _AbsBaseDirectory;
    MD5SumFileNodesHashTable _allFilesHereOnDisk;
    DirectoryNode _myDirRoot;
    DirectoryNode _foreignDirRoot;
    PathFilter _pathFilter;
    IBackgroundMonitor _monitor = new DummyBackgroundMonitor();
    IErrorReporter _reporter = new DefaultErrorReporter();
    
    StringCollection _ToRemove = new StringCollection();
    StringCollection _ToRemoveButChanged = new StringCollection();
    StringCollection _ToCopy = new StringCollection();
    StringCollection _ToOverwrite = new StringCollection();
    StringCollection _ToResolveManually = new StringCollection();
    StringCollection _ToCreateDir = new StringCollection();


    public StringCollection ToRemove { get { return _ToRemove; }}
    public StringCollection ToRemoveManually { get { return _ToRemoveButChanged; }}
    public StringCollection ToCopy { get { return _ToCopy; }}
    public StringCollection ToOverwrite { get { return _ToOverwrite; }}
    public StringCollection ToResolveManually { get { return _ToResolveManually; }}
    public StringCollection ToCreateDir { get { return _ToCreateDir; }}




    public FilesToSynchronizeCollector(
      string mediumdirectory,
      string absbasedirectory,
      MD5SumFileNodesHashTable  allFilesHereOnDisk,
      DirectoryNode myDirRoot,
      DirectoryNode foreignDirRoot,
      PathFilter pathFilter,
      IBackgroundMonitor monitor,
      IErrorReporter reporter)
    {
#if DEBUG
      PathUtil.Assert_Abspath(mediumdirectory);
      PathUtil.Assert_Abspath(absbasedirectory);
#endif

      _AbsMediumDirectory = mediumdirectory;
      _AbsBaseDirectory = absbasedirectory;
      _allFilesHereOnDisk = allFilesHereOnDisk;
      _myDirRoot = myDirRoot;
      _foreignDirRoot = foreignDirRoot;
      _pathFilter = pathFilter;
      _monitor = monitor;
      _reporter = reporter;
    }

    public string GetFullPath(string relativdir)
    {
      return PathUtil.Combine_Abspath_RelPath(_AbsBaseDirectory,relativdir);
    }

    public void AddRemovedFile(string dirbase,string filename, bool isUnchanged)
    {
      if(isUnchanged)
        _ToRemove.Add(PathUtil.Combine_Relpath_Filename(dirbase,filename));
      else
        _ToRemoveButChanged.Add(PathUtil.Combine_Relpath_Filename(dirbase,filename));

    }

    public void AddRemovedSubdirectory(string dirbase,string subdirname)
    {
      _ToRemove.Add(PathUtil.Combine_Relpath_Dirname(dirbase,subdirname));
    }

    public void AddManuallyResolvedFile(string dirbase,string filename)
    {
      _ToResolveManually.Add(PathUtil.Combine_Relpath_Filename(dirbase,filename));
    }

    public void AddFileToCopy(string dirbase, string filename)
    {
      _ToCopy.Add(PathUtil.Combine_Relpath_Filename(dirbase,filename));
    }
    
    public void AddFileToOverwrite(string dirbase, string filename)
    {
      _ToOverwrite.Add(PathUtil.Combine_Relpath_Filename(dirbase,filename));
    }

    public void AddDirToCreate(string dirbase, string dirname)
    {
      _ToCreateDir.Add(PathUtil.Combine_Relpath_Dirname(dirbase,dirname));
    }

    protected bool IsFileOnMedium(string filename)
    {
      return System.IO.File.Exists(PathUtil.Combine_Abspath_Filename(_AbsMediumDirectory,filename));
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
      return System.IO.File.Exists(PathUtil.Combine_Abspath_Filename(_AbsMediumDirectory,node.MediumFileName));
    }


    public void Traverse()
    {
      _pathFilter.ResetCurrentDirectory();
      VisitDirectory(_myDirRoot,_foreignDirRoot,System.IO.Path.DirectorySeparatorChar.ToString());
    }

    /// <summary>
    /// Collects the files that are supposed for synchronizing our own file system.
    /// </summary>
    /// <param name="myDir">Directory node of our own file system.</param>
    /// <param name="foreignDir">Corresponding directory node of the foreign file system.</param>
    /// <param name="pathFilter">The path filter used to filter files and directories.</param>
    /// <param name="collector">Container that collectes the files that are supposed for synchronization.</param>
    /// <param name="directorybase"></param>
    protected void VisitDirectory(DirectoryNode myDir, DirectoryNode foreignDir, string reldirectorybase)
    {
#if DEBUG
      PathUtil.Assert_Relpath(reldirectorybase);
#endif

      System.Collections.Specialized.StringCollection foreignFilesToRemove=null;
      foreach(FileNode foreignFileNode in foreignDir.Files)
      {
        string   foreignFileName = foreignFileNode.Name;

        if(_monitor.ShouldReport)
          _monitor.Report("Visit file " + foreignFileName);

        if(foreignFileNode.IsUnchanged)
          continue;
        if(!_pathFilter.IsFileIncluded(foreignFileName))
          continue;

        if(myDir!=null)
        {
          System.IO.FileInfo fileinfo = new System.IO.FileInfo(PathUtil.Combine_Abspath_Filename(GetFullPath(reldirectorybase),foreignFileName));
          DirectoryUpdater.UpdateFile(myDir,fileinfo,false,_reporter);
        }

        
        // First handle the error, that both the nodes are unchanged, but nevertheless different in content
        if( myDir!=null &&
          myDir.ContainsFile(foreignFileName) && 
          foreignFileNode.IsUnchanged && 
          myDir.File(foreignFileName).IsUnchanged && 
          !foreignFileNode.HasSameContentThan(myDir.File(foreignFileName)))
        {
          // set both nodes to changed then, since we don't know when this error occurs
          foreignFileNode.SwitchFromUnchangedToChanged();
          myDir.File(foreignFileName).SwitchFromUnchangedToChanged();
        }

        // File removed handling
        if(foreignFileNode.IsRemoved)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName) && !myDir.File(foreignFileName).IsRemoved)
          {
            bool isUnchanged = foreignFileNode.HasSameHashThan(myDir.File(foreignFileName));
            AddRemovedFile(reldirectorybase,foreignFileName,isUnchanged);
          }
          else // is not in the list
          {
            if(null==foreignFilesToRemove) foreignFilesToRemove=new System.Collections.Specialized.StringCollection();
            foreignFilesToRemove.Add(foreignFileName);  // foreignDir._files.Remove(foreignFileName);
            if(myDir.ContainsFile(foreignFileName))
              myDir.Files.Remove(foreignFileName);
          }
        }
        else if(foreignFileNode.IsNew)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName))
          {
            if(foreignFileNode.HasSameContentThan(myDir.File(foreignFileName)))
            {
              foreignFileNode.SetToUnchanged();
              myDir.File(foreignFileName).SetToUnchanged();
            }
            else
            {
              if(IsFileHereAnywhere(foreignFileNode))
                AddManuallyResolvedFile(reldirectorybase,foreignFileName);
             
            }
          }
          else
          {
            if(IsFileHereAnywhere(foreignFileNode))
              AddFileToCopy(reldirectorybase,foreignFileName);
          }
        }
        else if(foreignFileNode.IsChanged)
        {
          if(myDir!=null && myDir.ContainsFile(foreignFileName))
          {
            if(foreignFileNode.HasSameContentThan(myDir.File(foreignFileName)))
            {
              foreignFileNode.SetToUnchanged();
              myDir.File(foreignFileName).SetToUnchanged();
            }
            else if(myDir.File(foreignFileName).IsUnchanged)
            {
              if(IsFileHereAnywhere(foreignFileNode))
                AddFileToOverwrite(reldirectorybase,foreignFileName);
            }
            else if(IsFileHereAnywhere(foreignFileNode))
            {
              AddManuallyResolvedFile(reldirectorybase,foreignFileName);
            }
          }
          else
          {
            if(IsFileHereAnywhere(foreignFileNode))
              AddFileToCopy(reldirectorybase,foreignFileName);
          }
        }
      
      } // foreach file
      // now remove the foreign files that can not be removed during the enumeration
      if(null!=foreignFilesToRemove)
      {
        foreach(string name in foreignFilesToRemove)
          foreignDir.Files.Remove(name);
      }


      // now the directories...
      System.Collections.Specialized.StringCollection foreignSubDirsToRemove=null;
      foreach(DirectoryNode foreignSubDirNode in foreignDir.Directories)
      {
        string foreignSubDirName = foreignSubDirNode.Name;
        


        // If the pathfilter rejects this, we can remove the nodes silently and continue with the next node
        if(!_pathFilter.IsDirectoryIncluded(foreignSubDirName))
        {
          if(null==foreignSubDirsToRemove) foreignSubDirsToRemove=new System.Collections.Specialized.StringCollection();
          foreignSubDirsToRemove.Add(foreignSubDirName);  // foreignDir._files.Remove(foreignFileName);
          if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
            myDir.Directories.Remove(foreignSubDirName);

          continue;
        }

        string newdirectorybase = PathUtil.Combine_Relpath_Dirname(reldirectorybase, foreignSubDirName);
  

        

        if(foreignSubDirNode.IsRemoved)
        {

          // Test if the own directory is removed also now and set it to removed in this case
          if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName) && !System.IO.Directory.Exists(GetFullPath(newdirectorybase)))
          {
            myDir.Directory(foreignSubDirName).SetToRemoved();
          }

          
          if(myDir==null || !myDir.ContainsDirectory(foreignSubDirName) || myDir.Directory(foreignSubDirName).IsRemoved)
          {
            // if myDir also not exist, remove the nodes silently
            if(null==foreignSubDirsToRemove) foreignSubDirsToRemove=new System.Collections.Specialized.StringCollection();
            foreignSubDirsToRemove.Add(foreignSubDirName);  // foreignDir._files.Remove(foreignFileName);
            if(myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
              myDir.Directories.Remove(foreignSubDirName);

            continue;
          }
        }
       

        if(myDir!=null)
        {
          if(!myDir.ContainsDirectory(foreignSubDirName))
          {
            if(System.IO.Directory.Exists(GetFullPath(newdirectorybase)))
              myDir.AddSubDirectory(foreignSubDirName,new DirectoryNode(foreignSubDirName,myDir));
            else if(!foreignSubDirNode.IsRemoved) // directory here do not exist, but create it
              this.AddDirToCreate(reldirectorybase,foreignSubDirName);
          }
          else if(myDir.Directory(foreignSubDirName).IsRemoved)
          {
            if(System.IO.Directory.Exists(GetFullPath(newdirectorybase)))
              myDir.Directory(foreignSubDirName).IsRemoved = false; // obviously it is not removed any longer
          }
        }
        else // myDir is null
        {
          if(!foreignSubDirNode.IsRemoved)
            this.AddDirToCreate(reldirectorybase,foreignSubDirName);
        }

        _pathFilter.EnterSubDirectory(foreignSubDirName);
        VisitDirectory(
          myDir!=null && myDir.ContainsDirectory(foreignSubDirName)? myDir.Directory(foreignSubDirName) : null,
          foreignSubDirNode,
          newdirectorybase);

        _pathFilter.LeaveSubDirectory(foreignSubDirName);


        if(foreignSubDirNode.IsRemoved && myDir!=null && myDir.ContainsDirectory(foreignSubDirName))
          AddRemovedSubdirectory(reldirectorybase,foreignSubDirName);          



      }  // for each sub dir

      // now remove the foreign files that can not be removed during the enumeration
      if(null!=foreignSubDirsToRemove)
      {
        foreach(string name in foreignSubDirsToRemove)
          foreignDir.Directories.Remove(name);
      }
    }



  }
}
