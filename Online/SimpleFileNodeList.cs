using System;

namespace Syncoco.Online
{
  [Serializable]
  public class SimpleFileNodeList : SortedArrayList
  {
    public override int Add(object value)
    {
      if(value is SimpleFileNode)
        return base.Add(value);
      else
        throw new ArgumentException("Item to add is not of expected type");
    }

    public new SimpleFileNode this[int i]
    {
      get { return (SimpleFileNode)base[i]; }
      set { base[i] = value; }
    }

    public SimpleFileNode this[string name]
    {
      get 
      {
        int idx = Array.BinarySearch(_items,0,_size,name); 
        if(idx>=0)
          return (SimpleFileNode)base[idx];
        else
          return null;
      }
    }
  }

  
}
