using System;

namespace Syncoco.Online
{
  [Serializable]
  public class SimpleDirectoryNodeList : SortedArrayList
  {
    public override int Add(object value)
    {
      if(value is SimpleDirectoryNode)
        return base.Add(value);
      else
        throw new ArgumentException("Item to add is not of expected type");
    }

    public new SimpleDirectoryNode this[int i]
    {
      get { return (SimpleDirectoryNode)base[i]; }
      set { base[i] = value; }
    }

    public SimpleDirectoryNode this[string name]
    {
      get 
      {
        int idx = Array.BinarySearch(_items,0,_size,name); 
        if(idx>=0)
          return (SimpleDirectoryNode)base[idx];
        else
          return null;
      }
    }
  
  }
}
