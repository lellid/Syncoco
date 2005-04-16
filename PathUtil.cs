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
    /// Extracts the relative file name from the full name and the root path.
    /// </summary>
    /// <param name="fullpath">The full file name. Has to start with root path.</param>
    /// <param name="rootpath">The root path of the file.</param>
    /// <returns>The relative file name.</returns>
    public static string GetRelpathFromAbspath(string fullpath, string rootpath)
    {
      string result;
      if(HasRootPath(rootpath,fullpath,out result))
        return System.IO.Path.DirectorySeparatorChar.ToString()+result;
      else
        throw new ArgumentException(string.Format("fullpath {0} does not start with rootpath {1}",fullpath,rootpath));
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

    public static string Combine_Abspath_RelPath(string abspath, string relpath)
    {
#if DEBUG
      Assert_Abspath(abspath);
      Assert_Relpath(relpath);
#endif

      string result = abspath+relpath.Substring(1);
#if DEBUG
      Assert_Abspath(result);
#endif
      return result;
    }

    /// <summary>
    /// Determines from the name if this is a directory name (directorie names end with a directory separator).
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>True if the name ends with a directory separator.</returns>
    public static bool IsDirectoryName(string name)
    {
      return name!=null && name.Length>0 && name[name.Length-1]==System.IO.Path.DirectorySeparatorChar;
    }
    

    /// <summary>
    /// Combine a relative path (starts and ends with DirectorySeparatorChar) with a file name (must not contain DirectorySeparatorChar) to a new name.
    /// </summary>
    /// <param name="relpath">The path (starts and ends with DirectorySeparatorChar)</param>
    /// <param name="filename">The file name (must not contain DirectorySeparatorChar's)</param>
    /// <returns>The new path/filename name (not ending with a DirectorySeparatorChar).</returns>
    public static string Combine_Relpath_Filename(string relpath, string filename)
    {
#if DEBUG
      Assert_Relpath(relpath);
      Assert_Filename(filename);
#endif
      return relpath+filename;
    }

    /// <summary>
    /// Combine a absolue path (starts with either driveletter: or with \\ and ends with a DirectorySeparatorChar) with a file name (must not contain DirectorySeparatorChar) to a new name.
    /// </summary>
    /// <param name="relpath">The path (starts and ends with DirectorySeparatorChar)</param>
    /// <param name="filename">The file name (must not contain DirectorySeparatorChar's)</param>
    /// <returns>The new path/filename name (not ending with a DirectorySeparatorChar).</returns>
    public static string Combine_Abspath_Filename(string abspath, string filename)
    {
#if DEBUG
      Assert_Abspath(abspath);
      Assert_Filename(filename);
#endif
      return abspath+filename;
    }

    /// <summary>
    /// Combine a absolue path (starts with either driveletter: or with \\ and ends with a DirectorySeparatorChar) with a relative file name (must start with DirectorySeparatorChar) to a absolute file name.
    /// </summary>
    /// <param name="relpath">The path (starts and ends with DirectorySeparatorChar)</param>
    /// <param name="relpathFilename">A relative path the plus file name.</param>
    /// <returns>The new path/filename name.</returns>
    public static string Combine_Abspath_RelpathFilename(string abspath, string relpathFilename)
    {
#if DEBUG
      Assert_Abspath(abspath);
      Assert_RelpathFilename(relpathFilename);
#endif
      return abspath+relpathFilename.Substring(1);
    }
   
    /// <summary>
    /// Combine a absolue path (starts with either driveletter: or with \\ and ends with a DirectorySeparatorChar) with a file name (must not contain DirectorySeparatorChar) to a new name.
    /// </summary>
    /// <param name="relpath">The path (starts and ends with DirectorySeparatorChar)</param>
    /// <param name="filename">Either a relative path or a relative path plus file name.</param>
    /// <returns>The new path/filename name.</returns>
    public static string Combine_Abspath_RelpathOrFilename(string abspath, string filename)
    {
#if DEBUG
      Assert_Abspath(abspath);
      Assert_RelpathOrFilename(filename);
#endif
      return abspath+filename.Substring(1);
    }

    /// <summary>
    /// Combine a relative path (starts and ends with DirectorySeparatorChar) with a directory name (must not contain DirectorySeparatorChar) to a new relative path that end
    /// with a DirectorySeparatorChar.
    /// </summary>
    /// <param name="relpath">The relative path (starts and ends with DirectorySeparatorChar)</param>
    /// <param name="dirname">The directory name (must not contain DirectorySeparatorChar's)</param>
    /// <returns>The new relative path name starting and ending with a DirectorySeparatorChar.</returns>
    public static string Combine_Relpath_Dirname(string relpath, string dirname)
    {
#if DEBUG
      Assert_Relpath(relpath);
      Assert_Dirname(dirname);
#endif
      string result = relpath+dirname+System.IO.Path.DirectorySeparatorChar;

#if DEBUG
      Assert_Relpath(result);
#endif

      return result;
    }

    /// <summary>
    /// Combine a absolute path (ends with DirectorySeparatorChar) with a directory name (must not contain DirectorySeparatorChar) to a new path that end
    /// with a DirectorySeparatorChar.
    /// </summary>
    /// <param name="abspath">The absolute path (starts with driveletter: or \\ and ends with DirectorySeparatorChar)</param>
    /// <param name="dirname">The directory name (must not contain DirectorySeparatorChar's)</param>
    /// <returns>The new path name ending with a DirectorySeparatorChar.</returns>
    public static string Combine_Abspath_Dirname(string abspath, string dirname)
    {
#if DEBUG
      Assert_Abspath(abspath);
      Assert_Dirname(dirname);
#endif

      string result = abspath+dirname+System.IO.Path.DirectorySeparatorChar;

#if DEBUG
      Assert_Abspath(result);
#endif

      return result;
    }

    public static void Assert_NameOrNameWithEndSeparator(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      System.Diagnostics.Debug.Assert(name.Length>=1);
      int pos = name.IndexOf(System.IO.Path.DirectorySeparatorChar);
      System.Diagnostics.Debug.Assert(pos<0 || (name.Length>=2 && pos==name.Length-1));
    }


    public static void Assert_Filename(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      System.Diagnostics.Debug.Assert(name.Length>0);
      System.Diagnostics.Debug.Assert(name.IndexOf(System.IO.Path.DirectorySeparatorChar)<0);
    }

    public static void Assert_Dirname(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      System.Diagnostics.Debug.Assert(name.Length>0);
      System.Diagnostics.Debug.Assert(name.IndexOf(System.IO.Path.DirectorySeparatorChar)<0);
    }

    public static void Assert_RelpathOrFilename(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      System.Diagnostics.Debug.Assert(name.Length>0);
      System.Diagnostics.Debug.Assert(name[0]==System.IO.Path.DirectorySeparatorChar);
      System.Diagnostics.Debug.Assert(name.Length==1 || name[1]!=System.IO.Path.DirectorySeparatorChar);
    }

    public static void Assert_RelpathFilename(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      System.Diagnostics.Debug.Assert(name.Length>=2); // at least Separator and a letter
      System.Diagnostics.Debug.Assert(name[0]==System.IO.Path.DirectorySeparatorChar);
      System.Diagnostics.Debug.Assert(name[1]!=System.IO.Path.DirectorySeparatorChar);
      System.Diagnostics.Debug.Assert(name[name.Length-1]!=System.IO.Path.DirectorySeparatorChar);
    }


    public static void Assert_Relpath(string name)
    {
      Assert_RelpathOrFilename(name);
      System.Diagnostics.Debug.Assert(name[name.Length-1]==System.IO.Path.DirectorySeparatorChar);
    }

    public static void Assert_Abspath(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      if('\\'==System.IO.Path.DirectorySeparatorChar) // WINDOWS
      {
        System.Diagnostics.Debug.Assert(name.Length>=3);
        System.Diagnostics.Debug.Assert((name[1]==':'  && name[2]==System.IO.Path.DirectorySeparatorChar) || (name[0]==System.IO.Path.DirectorySeparatorChar && name[1]==System.IO.Path.DirectorySeparatorChar));
      }
      else // UNIX
      {
        System.Diagnostics.Debug.Assert(name.Length>=1);
      }
      System.Diagnostics.Debug.Assert(name[name.Length-1]==System.IO.Path.DirectorySeparatorChar);
    }

    public static void Assert_AbspathFilename(string name)
    {
      System.Diagnostics.Debug.Assert(name!=null);
      if('\\'==System.IO.Path.DirectorySeparatorChar) // WINDOWS
      {
        System.Diagnostics.Debug.Assert(name.Length>=4);
        System.Diagnostics.Debug.Assert((name[1]==':' && name[2]==System.IO.Path.DirectorySeparatorChar) || (name[0]==System.IO.Path.DirectorySeparatorChar && name[1]==System.IO.Path.DirectorySeparatorChar));
      }
      else // UNIX
      {
        System.Diagnostics.Debug.Assert(name.Length>=2);
      }
    }

    public static void SplitInto_Relpath_Filename(string relpathfilename, out string relpath, out string filename)
    {
      int pos = relpathfilename.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
      relpath = relpathfilename.Substring(0,pos+1);
      filename = relpathfilename.Substring(pos+1);

#if DEBUG
      Assert_Relpath(relpath);
      Assert_Filename(filename);
#endif
    }

    public static void SplitInto_Relpath_Dirname(string relpathfilename, out string relpath, out string dirname)
    {
      int pos = relpathfilename.LastIndexOf(System.IO.Path.DirectorySeparatorChar,relpathfilename.Length-2);
      relpath = relpathfilename.Substring(0,pos+1);
      dirname = relpathfilename.Substring(pos+1,relpathfilename.Length-pos-2);

#if DEBUG
      Assert_Relpath(relpath);
      Assert_Dirname(dirname);
#endif
    }
    

   
    /// <summary>
    /// Returns a path with is forced to end with a DirectorySeparatorChar
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string NormalizeAbspath(string path)
    {
      string result = path;
      if(path[path.Length-1]!=System.IO.Path.DirectorySeparatorChar)
        result += System.IO.Path.DirectorySeparatorChar;
#if DEBUG
      Assert_Abspath(result);
#endif
      return result;
    }

    /// <summary>
    /// Returns a path with is forced to start and end with a DirectorySeparatorChar
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string NormalizeRelpath(string path)
    {
      string result = path[0]==System.IO.Path.DirectorySeparatorChar ? path : System.IO.Path.DirectorySeparatorChar.ToString()+path;
      if(path[path.Length-1]!=System.IO.Path.DirectorySeparatorChar)
        result += System.IO.Path.DirectorySeparatorChar;
#if DEBUG
      Assert_Relpath(result);
#endif
      return result;
    }

    /// <summary>
    /// This normalizes the name so that it can be compared with other file names. Currently,
    /// the name is converted to lower letters to be case insensitive.
    /// </summary>
    /// <param name="name">Name to normalized.</param>
    /// <returns>Normalized path.</returns>
    public static string NormalizeForComparison(string name)
    {
      return name.ToLower();
    }

    /// <summary>
    /// Determines if two paths are equal. For Windows systems, two names are compared
    /// using case insensitive comparison.
    /// </summary>
    /// <param name="name1">First path.</param>
    /// <param name="name2">Second path.</param>
    /// <returns>True if the two paths are considered equal.</returns>
    public static bool ArePathsEqual(string name1, string name2)
    {
      return NormalizeForComparison(name1)==NormalizeForComparison(name2);
    }
  }
}
