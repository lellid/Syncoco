using System;

namespace Syncoco.Online
{
  /// <summary>
  /// Summary description for SimpleFileNode.
  /// </summary>
  public class SimpleFileNode : FileNodeBase
  {
    [NonSerialized]
    bool _IsUpdated;

    public bool IsUpdated
    {
      get { return _IsUpdated; }
      set { _IsUpdated = value; }
    }


    /// <summary>
    /// Only for deserialization purposes.
    /// </summary>
    protected SimpleFileNode()
    {
    }

    public SimpleFileNode(FileNodeBase from)
      : base(from)
    {
    }
  
    /// <summary>
    /// Constructor. Creates a SimpleFileNode out of a file info for that file.
    /// </summary>
    /// <param name="info"></param>
    public SimpleFileNode(System.IO.FileInfo info)
      : base(info)
    {
    }



  }
}
