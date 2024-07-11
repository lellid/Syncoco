#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco: offline file synchronization
//    Copyright (C) 2004-2099 Dr. Dirk Lellinger
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

namespace Syncoco
{
  /// <summary>
  /// Summary description for IParentDirectory.
  /// </summary>
  public interface IParentDirectory
  {
    /// <summary>
    /// Returns true if this is a FileSystemRoot. 
    /// </summary>
    bool IsFileSystemRoot { get; }

    /// <summary>
    /// Returns the directory name. For a FileSystemRoot, this returns the complete path of the root.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the parent directory. For a FileSystemRoot, this returns null.
    /// </summary>
    IParentDirectory ParentDirectory { get; }

  }
}
