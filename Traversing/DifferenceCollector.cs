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

namespace Syncoco.Traversing
{
  /// <summary>
  /// Summary description for DifferenceCollector.
  /// </summary>
  public class DifferenceCollector
  {
    DirectoryNode _myDirRoot;
    DirectoryNode _foreignDirRoot;

    bool _reportCreationTimeDifferences= false;

   
    bool _reportWriteTimeDifferences= false;


    System.IO.StringWriter report = new System.IO.StringWriter();

    public DifferenceCollector(DirectoryNode myDir, DirectoryNode foreignDir)
    {
      _myDirRoot = myDir;
      _foreignDirRoot = foreignDir;
    }

    public string GetReport()
    {
      return report.ToString();
    }

    public bool ReportCreationTimeDifferences
    {
      get
      {
        return _reportCreationTimeDifferences;
      }
      set
      {
        _reportCreationTimeDifferences = value;
      }
    }
    public bool ReportWriteTimeDifferences
    {
      get
      {
        return _reportWriteTimeDifferences;
      }
      set
      {
        _reportWriteTimeDifferences = value;
      }
    }


    public void Traverse()
    {
      VisitDirectory(_myDirRoot,_foreignDirRoot);
    }


    protected void VisitDirectory(DirectoryNode myDir, DirectoryNode foDir)
    {
      System.Collections.SortedList names = new SortedList(new CaseInsensitiveComparer());

      names.Clear();

      // add all filenames of myDir
      if(myDir!=null)
      {
        foreach(FileNode node in myDir.Files)
          if(!names.ContainsKey(node.Name))
            names.Add(node.Name,null);
      }
      // and add all filenames of foDir
      if(foDir!=null)
      {
        foreach(FileNode node in foDir.Files)
          if(!names.ContainsKey(node.Name))
            names.Add(node.Name,null);
      }

      ReportOnFiles(myDir,foDir,names);

      // now the same for all directories
      names.Clear();
      // add all dirnames of myDir
      if(myDir!=null)
      {
        foreach(DirectoryNode node in myDir.Directories)
          if(!names.ContainsKey(node.Name))
            names.Add(node.Name,null);
      }
      // and add all dirnames of foDir
      if(foDir!=null)
      {
        foreach(DirectoryNode node in foDir.Directories)
          if(!names.ContainsKey(node.Name))
            names.Add(node.Name,null);
      }

      ReportOnSubdirectories(myDir,foDir,names);


      // and now Repeat it for all subdirectories
      foreach(string subdirname in names.Keys)
      {
        DirectoryNode mySub = myDir==null ? null : myDir.Directory(subdirname);
        DirectoryNode foSub = foDir==null ? null : foDir.Directory(subdirname);
        VisitDirectory(mySub,foSub);
      }
    }


    protected void ReportOnFiles(DirectoryNode myDir, DirectoryNode foDir, SortedList names)
    {
      foreach(string name in names.Keys)
      {
        FileNode myFile = myDir==null ? null : myDir.File(name);
        FileNode foFile = foDir==null ? null : foDir.File(name);
        ReportOnSingleFile(myFile,foFile);
      }
    }
    protected void ReportOnSubdirectories(DirectoryNode myDir, DirectoryNode foDir, SortedList names)
    {
      foreach(string name in names.Keys)
      {
        DirectoryNode mySub = myDir==null ? null : myDir.Directory(name);
        DirectoryNode foSub = foDir==null ? null : foDir.Directory(name);
        ReportOnSingleDirectory(mySub,foSub);
      }
    }


    protected void ReportOnSingleFile(FileNode myFile, FileNode foFile)
    {
      if(myFile!=null && foFile==null)
      {
        report.WriteLine("File {0} here only",GetFullName(myFile));
      }
      else if(myFile==null && foFile!=null)
      {
        report.WriteLine("File {0} there only",GetFullName(foFile));
      }
      else if (myFile!=null && foFile!=null)
      {
        bool isLengthDifferent = myFile.FileLength != foFile.FileLength;
        bool isHashDifferent = !(myFile.HasSameHashThan(foFile));
        bool isCreationTimeDifferent = IsTimeDifferent(myFile.CreationTimeUtc,foFile.CreationTimeUtc);
        bool isWriteTimeDifferent = IsTimeDifferent(myFile.LastWriteTimeUtc, foFile.LastWriteTimeUtc);
        bool isUnchanged = myFile.IsUnchanged && foFile.IsUnchanged;

        if(isLengthDifferent ||
          isHashDifferent || 
          (isCreationTimeDifferent && _reportCreationTimeDifferences) || 
          (isWriteTimeDifferent && _reportWriteTimeDifferences) ||
          (!isUnchanged))
        {
          report.Write("File {0}: ", GetFullName(myFile));
          if(isLengthDifferent)
            report.Write("differs in length ({0}::{1}); ",myFile.FileLength,foFile.FileLength);
          if(isHashDifferent)
            report.Write("differs in hash; ");
          if(isCreationTimeDifferent && _reportCreationTimeDifferences)
            report.Write("differs in creation time ({0}::{1}); ",myFile.CreationTimeUtc,foFile.CreationTimeUtc);
          if(isWriteTimeDifferent && _reportWriteTimeDifferences)
            report.Write("differs in last write time ({0}::{1}); ",myFile.LastWriteTimeUtc,foFile.LastWriteTimeUtc);
          if(!isUnchanged)
            report.Write("has at least a hint set ({0}::{1}); ", myFile.HintAsString,foFile.HintAsString);
          report.WriteLine();
        }

      }
      else
      {
        throw new ApplicationException("Programming error. Please report the error to the bug forum");
      }
    }

    protected void ReportOnSingleDirectory(DirectoryNode myDir, DirectoryNode foDir)
    {
      if(myDir!=null && foDir==null)
      {
        report.WriteLine("Dir {0} here only",GetFullName(myDir));
      }
      else if(myDir==null && foDir!=null)
      {
        report.WriteLine("Dir {0} there only",GetFullName(foDir));
      }
      else if (myDir!=null && foDir!=null)
      {
        bool isRemoved = myDir.IsRemoved || foDir.IsRemoved;

        if(isRemoved)
        {
          report.Write("Dir {0}: ", GetFullName(myDir));
          if(isRemoved)
            report.Write("at least one is removed ({0}::{1}); ",myDir.IsRemoved,foDir.IsRemoved);
          
          report.WriteLine();
        }
      }
      else
      {
        throw new ApplicationException("Programming error. Please report the error to the bug forum");
      }
    }

    #region Utility functions


    bool IsTimeDifferent(DateTime time1, DateTime time2)
    {
      TimeSpan span = (time1-time2);
      return span.TotalSeconds>1;
    }

    string GetFullName(DirectoryNode node)
    {
      IParentDirectory parent;
      for(parent = node;parent.ParentDirectory!=null;parent=parent.ParentDirectory)
      {
      }

      return PathUtil.Combine_Abspath_RelPath(parent.Name,node.FullRelativePath);
    }

    string GetFullName(FileNode node)
    {
      return GetFullName(node.Parent)+node.Name;
    }
    #endregion
  }
}
