using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Holds the current directory and provides easy methods to enter or leave subdirectories.
  /// </summary>
  public class CurrentDirectoryService
  {
    #region Member variables

    /// <summary>Number of array items in <see>_subDirectory</see> that are valid.</summary>
    protected int      _subDirectoryLevel = 0;

    /// <summary>Current directory stored as single subdirectory names. The number <see>_subDirectoryLevel</see> gives the number of
    /// array items that are valid.</summary>
    protected string[] _subDirectory = new string[1024];

    /// <summary>Stores the current directory as a string. </summary>
    protected System.Text.StringBuilder _directoryPath = new System.Text.StringBuilder();

    /// <summary>True if <see>_directoryPath</see> is valid. If false, it must be rebuild from the <see>_subDirectory</see> array.</summary>
    protected bool _directoryPathValid = false;
    

    /// <summary>
    /// Provides the level, where building the relative subdirectory path starts. This is intended for filters or so,
    /// where base of the filter is not the root directory, but some subdirectory above.
    /// </summary>
    int _relativeSubDirLevel=int.MinValue;
    /// <summary>True if the <see>_relativeSubDirPath</see> is valid.</summary>
    bool _relativeSubDirPathValid=false;
    /// <summary>Path from base level subdirectory up to the current directory.</summary>
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
    public string GetRelativeFileName(int relativeSubDirLevel, string filename)
    {
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

      return _relativeSubDirPath.ToString() + filename;
    }

    /// <summary>
    /// Returns the file name for subdirLevel 0, i.e. the whole relative file name, starting with a DirectorySeparatorChar.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetZeroLevelFileName(string filename)
    {
      return CurrentDirectoryPath+filename;
    }

    /// <summary>
    /// Gives the current directory as a string.
    /// </summary>
    public string CurrentDirectoryPath
    {
      get 
      {
        if(_directoryPathValid)
          return _directoryPath.ToString();
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
          return _directoryPath.ToString();
        }
      }
    }
  }
}
