using System;

namespace SyncTwoCo
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
