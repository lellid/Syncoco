using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for PathUtil.
  /// </summary>
  public class PathUtil
  {
    /// <summary>
    /// The char to separate directories in file names.
    /// </summary>
    public static readonly char DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
    private static readonly char[] directorysplitchars = new char[]{System.IO.Path.DirectorySeparatorChar};

    /// <summary>
    /// Determines wether a file has a parent root path.
    /// </summary>
    /// <param name="rootpath">The root path.</param>
    /// <param name="fullfilename">The full file name.</param>
    /// <returns>True if the file name starts with the root path, i.e. the file is located anywhere into the root path.</returns>
    public static bool HasRootPath(string rootpath, string fullfilename)
    {
      return fullfilename.Substring(0,rootpath.Length)==rootpath;
    }

    /// <summary>
    /// Determines whether a file has a parent root path.
    /// </summary>
    /// <param name="rootpath">The root path (including drive letter, with trailing DirectorySeparatorChar).</param>
    /// <param name="fullfilename">The full file name (including drive letter).</param>
    /// <param name="relativefilename">On output, returns the relative path (no leading DirectorySeparatorChar),
    /// i.e. the remaining path of the file relative to the root path. If the file is not located in the root path, null is returned.</param>
    /// <returns>True if the file name starts with the root path, i.e. the file is located anywhere into the root path.</returns>
    public static bool HasRootPath(string rootpath, string fullfilename, out string relativefilename)
    {
      System.Diagnostics.Debug.Assert(rootpath[rootpath.Length-1]==System.IO.Path.DirectorySeparatorChar);

      if(HasRootPath(rootpath,fullfilename))
      {
        int i;
        for(i=rootpath.Length;i<fullfilename.Length && fullfilename[i]==DirectorySeparatorChar;i++);
        relativefilename = fullfilename.Substring(i);

        System.Diagnostics.Debug.Assert(relativefilename.Length>0);
        System.Diagnostics.Debug.Assert(relativefilename[0]!=System.IO.Path.DirectorySeparatorChar);
        return true;
      }
      else
      {
        relativefilename = null;
        return false;
      }
    }

    /// <summary>
    /// Gets the array of directory names for the relative name (the relativename must not contain drive letters etc.)
    /// If relativename is a directory, it must end with a DirectorySeparatorChar to be distinguished from a file name.
    /// </summary>
    /// <param name="relativename">Name of file or directory. Must not start with a drive letter and must not start
    /// with a directorySeparatorChar.</param>
    /// <returns></returns>
    public static string[] GetDirectories(string relativename)
    {
      System.Diagnostics.Debug.Assert(relativename.Length==0 || relativename[0]!=System.IO.Path.DirectorySeparatorChar);
    
      return System.IO.Path.GetDirectoryName(relativename).Split(directorysplitchars);
    }


  }
}
