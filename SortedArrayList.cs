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

namespace Syncoco
{
  /// <summary>
  /// Summary description for SortedArrayList.
  /// </summary>
  [Serializable]
  public class SortedArrayList : IList
  {
    protected IComparable[] _items;
    protected int _size;

    /// <summary>
    /// Total number of state changes.
    /// </summary>
    [NonSerialized]
    protected int _version;

    protected virtual int DefaultInitialCapacity { get { return 16; }}

    public SortedArrayList()
    {
      _items=new IComparable[0];
      _size=0;
      _version=0;
    }

    #region IList Members

    public bool IsReadOnly
    {
      get
      {
        
        return false;
      }
    }

    /// <summary>
    /// Gets/Sets an element in the list by index.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The index is less than 0 or more then or equal to the list count.
    /// </exception>
    public virtual object this[int index] 
    {
      get 
      {
        if (index < 0 || index >= _size) 
        {
          throw new ArgumentOutOfRangeException("index", index,
            "Index is less than 0 or more than or equal to the list count.");
        }

        return _items[index];
      }

      set 
      {
        throw new NotImplementedException("This is a sorted array, you can not set values at arbitrary locations. Use Add(...) instead.");
      }
    }

   

    public virtual void Insert(int index, object value) 
    {
      throw new NotImplementedException("Sorry, this is a sorted array, elements can not be inserted at arbitrary values");
    }

    public virtual void Remove(object value) 
    {
      int x;

      x = IndexOf(value);

      if (x > -1) 
      {
        RemoveAt(x);
      }

      _version++;
    }

    public virtual void RemoveAt(int index) 
    {
      if (index < 0 || index >= _size) 
      {
        throw new ArgumentOutOfRangeException("index", index,
          "Less than 0 or more than list count.");
      }

      Shift(index, -1);
      _size--;
      _version++;
    }

    public bool Contains(object value)
    {
      int idx=Array.BinarySearch(_items,0,_size,value);
      return idx>=0;
    }

    public virtual void Clear() 
    {
      // Keep the array but null all members so they can be garbage collected.

      Array.Clear(_items, 0, _size);

      _size = 0;
      _version++;
    }

    public int IndexOf(object value)
    {
      return Array.BinarySearch(_items,0,_size,value);
    }

    /// <remarks>
    /// Ensures that the list has the capacity to contain the given <c>count</c> by
    /// automatically expanding the capacity when required.
    /// </remarks>
    private void EnsureCapacity(int count) 
    {
      if (count <= _items.Length) 
      {
        return;
      }

      int newLength;
      IComparable[] newData;

      newLength = _items.Length << 1;
      if (newLength == 0)
        newLength = DefaultInitialCapacity;

      while (newLength < count) 
      {
        newLength <<= 1;
      }

      newData = new IComparable[newLength];

      Array.Copy(_items, 0, newData, 0, _items.Length);

      _items = newData;
    }
    
    /// <summary>
    /// Shifts a section of the list.
    /// </summary>
    /// <param name="index">
    /// The start of the section to shift (the element at index is included in the shift).
    /// </param>
    /// <param name="count">
    /// The number of positions to shift by (can be negative).
    /// </param>
    private void Shift(int index, int count) 
    {
      if (count > 0) 
      {
        if (_size + count > _items.Length) 
        {
          int newLength;
          IComparable[] newData;
          
          newLength = (_items.Length > 0) ? _items.Length << 1 : 1;

          while (newLength < _size + count) 
          {
            newLength <<= 1;
          }
          
          newData = new IComparable[newLength];

          Array.Copy(_items, 0, newData, 0, index);
          Array.Copy(_items, index, newData, index + count, _size - index);

          _items = newData;
        }
        else 
        {
          Array.Copy(_items, index, _items, index + count, _size - index);
        }
      }
      else if (count < 0) 
      {
        // Remember count is negative so this is actually index + (-count)

        int x = index - count ;

        Array.Copy(_items, x, _items, index, _size - x);
      }
    }

    public virtual int Add(object value) 
    {

      if(!(value is IComparable))
        throw new ArgumentException("Argument does not implement the IComparable interface");

      int index = Array.BinarySearch(_items,0,_size, value);

      if(index>=0)
        throw new ArgumentException(string.Format("Argument <<{0}>> is already contained in the list at index {1}", value, index));

      index = ~index;

      // Do a check here in case EnsureCapacity isn't inlined.

      if (_items.Length <= _size /* same as _items.Length < _size + 1) */) 
      {
        EnsureCapacity(_size + 1);
      }

      if(index<_size)
        Shift(index,1);

      _items[index] = (IComparable)value;
      
      _version++;
      _size++;

      return index;
    }

    public bool IsFixedSize
    {
      get
      {
        
        return false;
      }
    }

    #endregion

    #region ICollection Members

    public bool IsSynchronized
    {
      get
      {
        
        return false;
      }
    }

    public int Count
    {
      get
      {
        
        return _size;
      }
    }

    public void CopyTo(Array array, int index)
    {
      Array.Copy(_items,0,array,index,_size);
    }

    public object SyncRoot
    {
      get
      {
        
        return this;
      }
    }

    #endregion

    #region IEnumerable Members

    class OwnEnumerator : IEnumerator
    {
      #region IEnumerator Members

      int _version;
      int _idx;

      SortedArrayList _list;

      public OwnEnumerator(SortedArrayList list)
      {
        _list    = list;
        _version = list._version;
        _idx=-1;
      }

      public void Reset()
      {
        _idx=-1;
      }

      public object Current
      {
        get
        {
          if(_version!=_list._version)
            throw new ArgumentException("List has changed");
          if(_idx<0 || _idx>=_list._size)
            throw new ArgumentOutOfRangeException("Current position is invalid");
          return _list._items[_idx];
        }
      }

      public bool MoveNext()
      {
        if(_version!=_list._version)
          throw new ArgumentException("List has changed");

        ++_idx;
        return _idx<_list._size;
      }

      #endregion

    }


    public IEnumerator GetEnumerator()
    {
      
      return new OwnEnumerator(this);
    }



    #endregion

    public virtual void TrimToSize() 
    {
      if (_items.Length > _size) 
      {
        IComparable[] newArray;

        newArray = new IComparable[_size];
                
        Array.Copy(_items, newArray, _size);

        _items = newArray;
      }
    }
  }

  [Serializable]
  public class FileNodeList : SortedArrayList
  {
    public override int Add(object value)
    {
      if(value is FileNode)
        return base.Add(value);
      else
        throw new ArgumentException("Item to add is not of expected type");
    }

    public new FileNode this[int i]
    {
      get { return (FileNode)base[i]; }
      set { base[i] = value; }
    }

    public FileNode this[string name]
    {
      get 
      {
        int idx = Array.BinarySearch(_items,0,_size,name); 
        if(idx>=0)
          return (FileNode)base[idx];
        else
          return null;
      }
    }
  }

  [Serializable]
  public class DirectoryNodeList : SortedArrayList
  {
    public override int Add(object value)
    {
      if(value is DirectoryNode)
        return base.Add(value);
      else
        throw new ArgumentException("Item to add is not of expected type");
    }

    public new DirectoryNode this[int i]
    {
      get { return (DirectoryNode)base[i]; }
      set { base[i] = value; }
    }

    public DirectoryNode this[string name]
    {
      get 
      {
        int idx = Array.BinarySearch(_items,0,_size,name); 
        if(idx>=0)
          return (DirectoryNode)base[idx];
        else
          return null;
      }
    }
  
  }
}
