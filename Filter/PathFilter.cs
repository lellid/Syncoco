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

namespace Syncoco.Filter
{
  /// <summary>
  /// Responsible for determination whether a file or path should be included 
  /// or not.
  /// </summary>
  [Serializable]
  public class PathFilter : CurrentDirectoryService
  {
    
    /// <summary>The collection of filter lists.</summary>
    FilterListItemList _filterList = new FilterListItemList();
    
    /// <summary>This is the default list that is applied if no other list matches.</summary>
    FilterList         _defaultList = new FilterList();

    /// <summary>
    /// Cached index to this lists which are active.
    /// </summary>
    [NonSerialized]
    int[] _activeList=new int[0];
    [NonSerialized]
    int   _activeListCount;
    /// <summary>Indicates if the active list is valid. After a directory change, this list
    /// can become invalid.</summary>
    [NonSerialized]
    bool  _activeListValid;



    public override void ResetCurrentDirectory()
    {
      base.ResetCurrentDirectory ();
      this._activeListValid = false;
    }

    public override void EnterSubDirectory(string name)
    {
#if DEBUG
      PathUtil.Assert_Dirname(name);
#endif

      base.EnterSubDirectory (name);
      this._activeListValid = false;
    }

    public override void LeaveSubDirectory(string name)
    {
#if DEBUG
      PathUtil.Assert_Dirname(name);
#endif

      base.LeaveSubDirectory (name);
      this._activeListValid = false;
    }

    public PathFilter()
    {
    }

    public PathFilter(PathFilter from)
    {
      CopyFrom(from);
    }

    public void CopyFrom(PathFilter from)
    {
      this._filterList.CopyFrom(from._filterList);
      this._defaultList.CopyFrom(from._defaultList);
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      tw.WriteStartElement("FilterListItemList");
      _filterList.Save(tw);
      tw.WriteEndElement();

      tw.WriteStartElement("DefaultList");
      _defaultList.Save(tw);
      tw.WriteEndElement();

    }

    public void Open(System.Xml.XmlTextReader tr)
    {
      bool isEmpty = tr.IsEmptyElement;
      tr.ReadStartElement("FilterListItemList");
      this._filterList = new FilterListItemList(tr);
      if(!isEmpty)
        tr.ReadEndElement();
    
      tr.ReadStartElement("DefaultList");
      this._defaultList = new FilterList(tr);
      tr.ReadEndElement();

    }

    public PathFilter(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

   



    public FilterList DefaultList { get { return _defaultList; }}
    public FilterListItemList FilterList { get { return _filterList; }}

    /// <summary>
    /// Match the filter list path the current subdirectory? The filterListPath must have lesser or equal elements
    /// to the currentSubDirectoryPath.
    /// </summary>
    /// <param name="filterListPath">Array of subdirectories of the filter list path.</param>
    /// <param name="currentSubDirectoryPath">Array of subdirectories of the actual path.</param>
    /// <returns>True if there is a match, otherwise false.</returns>
    protected static bool FilterListPathMatch(string[] filterListPath, string[] currentSubDirectoryPath, int currentSubDirectoryLevel)
    {
      if(filterListPath.Length>currentSubDirectoryLevel)
        return false;

      for(int i=0;i<filterListPath.Length;i++)
      {
        if(filterListPath[i].ToLower()!=currentSubDirectoryPath[i].ToLower())
          return false;
      }

      return true;
    }

   

    protected void UpdateActiveList()
    {
      // presumption: the length of the active list shold be at least 1 item longer as the _filterList.
      if(_activeList.Length<_filterList.Count)
        _activeList = new int[_filterList.Count];

      int i,j;
      for(i=0,j=0;i<_filterList.Count;i++)
      {
        if(FilterListPathMatch(_filterList[i].PathSubDirectory,this._subDirectory,this._subDirectoryLevel))
        {
          _activeList[j++] = i;
        }
      }
      _activeListCount = j;
      _activeListValid = true;
    }


    /// <summary>
    /// Determines if a name should be included or not.
    /// </summary>
    /// <param name="filename">Name of the file.</param>
    /// <returns>True if the file should be included.</returns>
    private bool IsNameIncluded(string name)
    {
      
      if(!_activeListValid)
        UpdateActiveList();

      string fileWithPath;
      FilterAction action;
      
      
      for(int i=0;i<_activeListCount;i++)
      {
        FilterListItem filterListItem = _filterList[_activeList[i]];

        // now create the relative file path (relative to the filter item)
        fileWithPath = this.GetBiasedpathFilename(filterListItem.PathDepth,name);

        action = filterListItem.Filter.Match(fileWithPath);

        if(action!=FilterAction.Ignore)
          return action==FilterAction.Include;
  
      }

      fileWithPath = this.GetZeroLevelFileName(name);
      action = this.DefaultList.Match(fileWithPath);

      return action!=FilterAction.Exclude; // per default, all files are included
    }

    /// <summary>
    /// Determines if a file should be included or not.
    /// </summary>
    /// <param name="filename">Name of the file.</param>
    /// <returns>True if the file should be included.</returns>
    public bool IsFileIncluded(string filename)
    {
#if DEBUG
      PathUtil.Assert_Filename(filename);
#endif
      return IsNameIncluded(filename);
    }


    /// <summary>
    /// Determines if a directory should be included or not.
    /// </summary>
    /// <param name="directoryname">Name of the directory.</param>
    /// <returns>True if the file should be included.</returns>
    public bool IsDirectoryIncluded(string directoryname)
    {
#if DEBUG
      PathUtil.Assert_Dirname(directoryname);
#endif
      return IsNameIncluded(directoryname + System.IO.Path.DirectorySeparatorChar);
    }

 

  
  }
}
