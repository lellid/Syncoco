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

namespace Syncoco
{
  /// <summary>
  /// Holds the current directory and provides easy methods to enter or leave subdirectories.
  /// </summary>
  [Serializable]
  public class CurrentDirectoryService
  {
    #region Member variables

    /// <summary>Number of array items in <see>_subDirectory</see> that are valid.</summary>
    [NonSerialized]
    protected int      _subDirectoryLevel = 0;

    /// <summary>Current directory stored as single subdirectory names. The number <see>_subDirectoryLevel</see> gives the number of
    /// array items that are valid.</summary>
    [NonSerialized]
    protected string[] _subDirectory = new string[1024];

    /// <summary>Stores the current directory as a string. </summary>
    [NonSerialized]
    protected System.Text.StringBuilder _directoryPath = new System.Text.StringBuilder();

    /// <summary>True if <see>_directoryPath</see> is valid. If false, it must be rebuild from the <see>_subDirectory</see> array.</summary>
    [NonSerialized]
    protected bool _directoryPathValid = false;
    

    /// <summary>
    /// Provides the level, where building the relative subdirectory path starts. This is intended for filters or so,
    /// where base of the filter is not the root directory, but some subdirectory above.
    /// </summary>
    [NonSerialized]
    int _relativeSubDirLevel=int.MinValue;
    /// <summary>True if the <see>_relativeSubDirPath</see> is valid.</summary>
    [NonSerialized]
    bool _relativeSubDirPathValid=false;
    /// <summary>Path from base level subdirectory up to the current directory.</summary>
    [NonSerialized]
    System.Text.StringBuilder _relativeSubDirPath = new System.Text.StringBuilder();

    #endregion

   
    /// <summary>
    /// This sets the current directory to the root directory.
    /// </summary>
    public virtual void ResetCurrentDirectory()
    {
     
      //Invalidate
      _directoryPathValid = false;
      _relativeSubDirLevel=int.MinValue;
      _relativeSubDirPathValid=false;

      _subDirectoryLevel=0;
    }

    /// <summary>
    /// This sets the current path to the path plus the subdirectory name.
    /// </summary>
    /// <param name="name">The name of the subdirectory.</param>
    public virtual void EnterSubDirectory(string name)
    {
#if DEBUG
      PathUtil.Assert_Dirname(name);
#endif

      _subDirectory[_subDirectoryLevel] = name;
      ++_subDirectoryLevel;
   
      // Invalidate
      _directoryPathValid = false;
      _relativeSubDirLevel=int.MinValue;
      _relativeSubDirPathValid=false;

      

    }

    /// <summary>
    /// This leaves the subdirectory name and sets the current directory to the parent directory.
    /// </summary>
    /// <param name="name">The name of the subdirectory to leave.</param>
    public virtual void LeaveSubDirectory(string name)
    {
      System.Diagnostics.Debug.Assert(_subDirectory[_subDirectoryLevel-1]==name);
      --_subDirectoryLevel;

      // Invalidate
      _directoryPathValid = false;
      _relativeSubDirLevel=int.MinValue;
      _relativeSubDirPathValid=false;

       
    }

   


  

    /// <summary>
    /// Gives the relative file name of the current directory, but starting on some subdirectory level. This means that the path name that is returned
    /// does not include the first some subdirectories.
    /// </summary>
    /// <param name="relativeSubDirLevel">Number of first subdirectories that are not included in the full file name.</param>
    /// <param name="filename">The file name.</param>
    /// <returns>The relative full path name, but not with the first <c>relativeSubDirLevel</c> number of subdirectories. The relative full
    /// path name starts always with a DirectorySeparatorChar.</returns>
    public string GetBiasedpathFilename(int relativeSubDirLevel, string filename)
    {
#if DEBUG
      PathUtil.Assert_NameOrNameWithEndSeparator(filename);
#endif

      if(!_relativeSubDirPathValid || relativeSubDirLevel!=_relativeSubDirLevel)
      {
        _relativeSubDirLevel=relativeSubDirLevel;
        _relativeSubDirPathValid=true;
        _relativeSubDirPath.Length=0;
        _relativeSubDirPath.Append(System.IO.Path.DirectorySeparatorChar);
        for(int i=relativeSubDirLevel;i<_subDirectoryLevel;i++)
        {
          _relativeSubDirPath.Append(_subDirectory[i]);
          _relativeSubDirPath.Append(System.IO.Path.DirectorySeparatorChar);
        }
      }    

      string relativesubdirpath = _relativeSubDirPath.ToString();
#if DEBUG
      PathUtil.Assert_Relpath(relativesubdirpath);
#endif
      return relativesubdirpath + filename;
    }



    /// <summary>
    /// Returns the file name for subdirLevel 0, i.e. the whole relative file name, starting with a DirectorySeparatorChar.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetZeroLevelFileName(string name)
    {

#if DEBUG
      PathUtil.Assert_NameOrNameWithEndSeparator(name);
#endif

      return CurrentDirectoryPath+name;
    }

    /// <summary>
    /// Gives the current directory as a string.
    /// </summary>
    public string CurrentDirectoryPath
    {
      get 
      {
        string result;

        if(_directoryPathValid)
        {
          result= _directoryPath.ToString();
        }
        else
        {
          _directoryPath.Length=0;
          _directoryPath.Append(System.IO.Path.DirectorySeparatorChar);
          for(int i=0;i<_subDirectoryLevel;i++)
          {
            _directoryPath.Append(_subDirectory[i]);
            _directoryPath.Append(System.IO.Path.DirectorySeparatorChar);
          }
          _directoryPathValid=true;
          result =  _directoryPath.ToString();
        }

#if DEBUG
        PathUtil.Assert_Relpath(result);
#endif
        return result;
      }
    }
  }
}
