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
  [Serializable]
  public struct FileHash : IComparable
  {
    public ulong Lo;
    public ulong Hi;

    public FileHash(byte[] hash)
      : this(hash,hash.Length)
    {
    }
    public FileHash(byte[] hash, int len)
    {
      if(hash==null)
      {
        Lo=0;
        Hi=0;
      }
      else if(len==16)
      {
        Lo = System.BitConverter.ToUInt64(hash,0);
        Hi = System.BitConverter.ToUInt64(hash,8);
      }
      else
      {
        throw new ArgumentException("Unexpected hash length of " + hash.Length);
      }
      
    }


    public static FileHash FromBinHexRepresentation(string binhex)
    {
      if(binhex.Length!=32)
        throw new ArgumentException("BinHexRepresentation must have a length of 32");

      FileHash hash;
      hash.Hi = ulong.Parse(binhex.Substring( 0,16),System.Globalization.NumberStyles.AllowHexSpecifier);
      hash.Lo = ulong.Parse(binhex.Substring(16,16),System.Globalization.NumberStyles.AllowHexSpecifier);
      return hash;
    }


    public bool Valid 
    {
      get 
      { 
        return Lo!=0 || Hi!=0;
      }
    }

    public override bool Equals(object obj)
    {
      return (obj is FileHash) && (this==(FileHash)obj);
    }

    public override int GetHashCode()
    {
      return Lo.GetHashCode() + Hi.GetHashCode();
    }


    public string BinHexRepresentation
    {
      get
      {
        return Hi.ToString("X16")+Lo.ToString("X16");
      }
    }

    public static bool operator ==(FileHash a, FileHash b)
    {
      return  a.Hi==b.Hi && a.Lo==b.Lo ;
    }

    public static bool operator !=(FileHash a, FileHash b)
    {
      return a.Hi!=b.Hi || a.Lo!=b.Lo;
    }

    public static bool operator <(FileHash a, FileHash b)
    {
      return a.Hi<b.Hi || a.Lo<b.Lo;
    }

    public static bool operator >(FileHash a, FileHash b)
    {
      return a.Hi>b.Hi || a.Lo>b.Lo;
    }
    #region IComparable Members

    public int CompareTo(object obj)
    {
      if(obj is FileHash)
      {
        return this==(FileHash)obj ? 0 : (this>(FileHash)obj ? 1 : -1);
      }
      else if(obj==null)
      {
        return 0;
      }
      else
        throw new ArgumentException("Argument is not of expected type, but of type " + obj.GetType().ToString());
    }

    #endregion
  }
}
