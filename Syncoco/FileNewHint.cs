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
  /// Summary description for FileNewHint.
  /// </summary>
  [Serializable]
  public class FileNewHint : IFileHint
  {
    public FileNewHint()
    {
    }

    public FileNewHint(System.Xml.XmlTextReader tr)
    {
      Open(tr);
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {

    }

    public void Open(System.Xml.XmlTextReader tr)
    {

    }
  }
}
