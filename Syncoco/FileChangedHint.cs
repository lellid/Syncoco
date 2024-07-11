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

using System;

namespace Syncoco
{
  /// <summary>
  /// Summary description for FileChangedHint.
  /// </summary>
  [Serializable]
  public class FileChangedHint : IFileHint
  {
    private DateTime _lastWriteTimeUtc;
    private DateTime _creationTimeUtc;
    private long _fileLength;
    private FileHash _fileHash;

    public FileChangedHint(DateTime creationTimeUtc, DateTime lastWriteTimeUtc, long fileLength, FileHash fileHash)
    {
      _creationTimeUtc = creationTimeUtc;
      _lastWriteTimeUtc = lastWriteTimeUtc;
      _fileLength = fileLength;
      _fileHash = fileHash;
    }

    public FileChangedHint(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }


    public void Save(System.Xml.XmlTextWriter tw)
    {

      tw.WriteElementString("LE", System.Xml.XmlConvert.ToString(_fileLength));
#if READWRITEDATEASPLAINTEXT
      tw.WriteElementString("CT",System.Xml.XmlConvert.ToString(_creationTimeUtc));
      tw.WriteElementString("WT",System.Xml.XmlConvert.ToString(_lastWriteTimeUtc));
#else
      tw.WriteElementString("CT", System.Xml.XmlConvert.ToString(_creationTimeUtc.Ticks));
      tw.WriteElementString("WT", System.Xml.XmlConvert.ToString(_lastWriteTimeUtc.Ticks));
#endif
      tw.WriteElementString("FH", _fileHash.BinHexRepresentation);
    }


    public void Open(System.Xml.XmlTextReader tr)
    {

      _fileLength = System.Xml.XmlConvert.ToInt64(tr.ReadElementString("LE"));
#if READWRITEDATEASPLAINTEXT
      _creationTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("CT"));
      _lastWriteTimeUtc = System.Xml.XmlConvert.ToDateTime(tr.ReadElementString("WT"));
#else
      _creationTimeUtc = new DateTime(System.Xml.XmlConvert.ToInt64(tr.ReadElementString("CT")));
      _lastWriteTimeUtc = new DateTime(System.Xml.XmlConvert.ToInt64(tr.ReadElementString("WT")));
#endif
      _fileHash = FileHash.FromBinHexRepresentation(tr.ReadElementString("FH"));
    }

  }


}
