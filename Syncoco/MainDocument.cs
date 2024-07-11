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

#endregion Copyright

using System;
using System.Collections;

namespace Syncoco
{
  using System.Windows.Forms;

  /// <summary>
  /// Summary description for MainDocument.
  /// </summary>
  [Serializable]
  public class MainDocument
  {
    [NonSerialized]
    private string _FileName;

    [NonSerialized]
    private bool _IsDirty;

    [NonSerialized]
    private MD5SumFileNodesHashTable _allFilesHere;

    public event EventHandler DirtyChanged;

    public event EventHandler FileNameChanged;

    public static string DefaultXmlExtension = ".syncox";
    public static string DefaultBinaryExtension = ".syncob";
    public static string DefaultFilterExtension = ".syncof";

    //ArrayList _root1, _root2;
    private string _root1ComputerName, _root2ComputerName;

    private ArrayList _rootPairs;

    [NonSerialized]
    private bool _ExchangeRoots;

    public MainDocument()
    {
      //_root1 = new ArrayList();
      //_root2 = new ArrayList();
      _rootPairs = new ArrayList();
      _root1ComputerName = Current.ComputerName;
    }

    public void RestoreParentOfChildObjects()
    {
      for (int i = 0; i < _rootPairs.Count; i++)
      {
        this.RootPair(i).RestoreParentOfChildObjects(this);
      }
    }

    public void Save(string filename)
    {
      if (!System.IO.Path.HasExtension(filename))
      {
        filename += DefaultXmlExtension;
      }

      SaveXML(filename);
    }

    protected void SaveXML(string filename)
    {
      // first create a directory
      string pathname = System.IO.Path.GetDirectoryName(filename);
      string dirname = System.IO.Path.Combine(pathname, System.IO.Path.GetFileNameWithoutExtension(filename));
      System.IO.Directory.CreateDirectory(dirname);

      string tempfilename = null;
      Exception exception = null;
      System.IO.FileStream stream = null;
      try
      {
        if (System.IO.File.Exists(filename))
        {
          tempfilename = System.IO.Path.GetTempFileName();
        }

        stream = new System.IO.FileStream(null != tempfilename ? tempfilename : filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
        System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
        tw.WriteStartDocument();
        tw.WriteStartElement("TwoPointSynchronizationByDataMedium");

#if READWRITEDATEASPLAINTEXT
        bool writeDateAsTicks = false;
#else
        bool writeDateAsTicks = true;
#endif
        tw.WriteAttributeString("FormatDateAsTicks", System.Xml.XmlConvert.ToString(writeDateAsTicks));
        tw.WriteAttributeString("DirectorySeparatorChar", System.IO.Path.DirectorySeparatorChar.ToString());

        Save(tw);
        tw.WriteEndElement();
        tw.WriteEndDocument();

        tw.Flush();
        tw.Close();

        if (null != tempfilename)
        {
          System.IO.File.Copy(tempfilename, filename, true);
        }

        SetFileSavedFlag(filename);
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      finally
      {
        if (null != stream)
        {
          stream.Close();
        }

        if (null != tempfilename)
        {
          System.IO.File.Delete(tempfilename);
        }
      }

      if (null != exception)
      {
        throw exception;
      }
    }

    public void SaveFilterOnly(string filename)
    {
      if (!System.IO.Path.HasExtension(filename))
      {
        filename += DefaultFilterExtension;
      }

      string tempfilename = null;
      Exception exception = null;
      try
      {
        if (System.IO.File.Exists(filename))
        {
          tempfilename = System.IO.Path.GetTempFileName();
        }

        System.IO.FileStream stream = new System.IO.FileStream(null != tempfilename ? tempfilename : filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
        System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
        tw.WriteStartDocument();
        tw.WriteStartElement("TwoPointSynchronizationByDataMedium");
        SaveFilterOnly(tw);
        tw.WriteEndElement();
        tw.WriteEndDocument();
        tw.Flush();
        tw.Close();

        if (null != tempfilename)
        {
          System.IO.File.Copy(tempfilename, filename, true);
        }
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      finally
      {
        if (null != tempfilename)
        {
          System.IO.File.Delete(tempfilename);
        }
      }

      if (null != exception)
      {
        throw exception;
      }
    }

    public void Save(System.Xml.XmlTextWriter tw, bool saveFilterOnly)
    {
      tw.WriteElementString("ComputerName1", _root1ComputerName);
      tw.WriteElementString("ComputerName2", _root2ComputerName);
      tw.WriteStartElement("RootPairs");

      for (int i = 0; i < _rootPairs.Count; i++)
      {
        tw.WriteStartElement("RootPair");
        RootPair(i).Save(tw, saveFilterOnly);
        tw.WriteEndElement(); // RootPair
      }

      tw.WriteEndElement(); // RootPairs
    }

    public void Save(System.Xml.XmlTextWriter tw)
    {
      Save(tw, false);
    }

    public void SaveFilterOnly(System.Xml.XmlTextWriter tw)
    {
      Save(tw, true);
    }

    public void Save()
    {
      if (!HasFileName)
      {
        throw new ApplicationException("No file name for saving document (programming error)");
      }

      Save(_FileName);
    }

    public void Read(System.Xml.XmlTextReader tr)
    {
      string compu1 = tr.ReadElementString("ComputerName1");
      string compu2 = tr.ReadElementString("ComputerName2");

    TestComputer:
      if (Current.ComputerName != compu1 && Current.ComputerName != compu2 && compu2 != string.Empty)
      {
        if (DialogResult.Yes == MessageBox.Show(
            $"The file loaded is intended for synchronization between {compu1} and {compu2}, and not for your computer {Current.ComputerName} !" +
            "\r\n\r\nDo you want to change the one of the computer to this one?"
            ,
            "Error!", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
        {

          var result = MessageBox.Show(
              $"Do you want to replace {compu1} with {Current.ComputerName} ?" +
              $"\r\n\r\nPress 'Yes' to replace {compu1} with {Current.ComputerName}" +
              $"\r\n\r\nPress 'No' to replace {compu2} with {Current.ComputerName}" +
              "\r\n\r\nPress 'Cancel' to cancel the replacement.",
              "Specify replacement.",
              MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

          if (result == DialogResult.Yes)
          {
            compu1 = Current.ComputerName;
          }
          else if (result == DialogResult.No)
          {
            compu2 = Current.ComputerName;
          }
          goto TestComputer;
        }
        else
        {
          throw new DocumentNotForThisComputerException(string.Format("The file loaded is intended for synchronization between {0} and {1}, and not for your computer {2} !", compu1, compu2, Current.ComputerName));
        }
      }

      _root1ComputerName = compu1;
      _root2ComputerName = compu2;

      tr.ReadStartElement("RootPairs");
      while (tr.LocalName == "RootPair")
      {
        tr.ReadStartElement("RootPair");
        _rootPairs.Add(new RootPair(this, tr));
        tr.ReadEndElement(); // RootPair
      }
      tr.ReadEndElement(); // RootPairs
    }

    public void OpenAsXML(string filename)
    {
      System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
      System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(stream);
      tr.WhitespaceHandling = System.Xml.WhitespaceHandling.None;

      tr.MoveToContent();

      tr.ReadStartElement("TwoPointSynchronizationByDataMedium");
      Read(tr);
      tr.ReadEndElement();

      tr.Close();
      stream.Close();

      SetFileSavedFlag(filename);
    }

    protected void OnFileNameChanged()
    {
      if (FileNameChanged != null)
      {
        FileNameChanged(this, EventArgs.Empty);
      }
    }

    protected void OnDirtyChanged()
    {
      if (DirtyChanged != null)
      {
        DirtyChanged(this, EventArgs.Empty);
      }
    }

    public void SetFileSavedFlag(string filename)
    {
      bool oldDirty = _IsDirty;
      string oldFileName = _FileName;

      _FileName = filename;
      _IsDirty = false;

      if (_FileName != oldFileName)
      {
        OnFileNameChanged();
      }

      if (oldDirty != _IsDirty)
      {
        OnDirtyChanged();
      }
    }

    public void SetDirty()
    {
      bool oldDirty = _IsDirty;
      _IsDirty = true;

      if (oldDirty != _IsDirty)
      {
        OnDirtyChanged();
      }
    }

    public bool IsDirty
    {
      get { return _IsDirty; }
    }

    public bool HasFileName
    {
      get
      {
        return _FileName != null && _FileName != string.Empty;
      }
    }

    public string FileName
    {
      get { return _FileName; }
    }

    protected void ExchangeRoots()
    {
      _ExchangeRoots = !_ExchangeRoots;
    }

    /// <summary>
    /// Change the roots so that the actual computer gets the first root
    /// </summary>
    public void Align()
    {
      if ((_root1ComputerName == null || _root1ComputerName == string.Empty) && _root2ComputerName != Current.ComputerName)
      {
        _root1ComputerName = Current.ComputerName;
      }
      else if ((_root2ComputerName == null || _root2ComputerName == string.Empty) && _root1ComputerName != Current.ComputerName)
      {
        _root2ComputerName = Current.ComputerName;
      }

      if (_root2ComputerName == Current.ComputerName)
      {
        _ExchangeRoots = true;
      }
    }

    public void ExchangeRootPairPositions(int index1, int index2)
    {
      if (index1 < 0 || index2 < 0)
      {
        throw new ArgumentOutOfRangeException("Indizes have to be >=0");
      }

      if (index1 >= _rootPairs.Count || index2 >= _rootPairs.Count)
      {
        throw new ArgumentOutOfRangeException("Indizes have to be <Count");
      }

      object o = _rootPairs[index1];
      _rootPairs[index1] = _rootPairs[index2];
      _rootPairs[index2] = o;
    }

    public int RootCount
    {
      get { return _rootPairs.Count; }
    }

    public FileSystemRoot MyRoot(int i)
    {
      return ((RootPair)_rootPairs[i]).MyRoot;
    }

    public FileSystemRoot ForeignRoot(int i)
    {
      return ((RootPair)_rootPairs[i]).ForeignRoot;
    }

    public RootPair RootPair(int i)
    {
      return (RootPair)_rootPairs[i];
    }

    public string MyRootComputerName
    {
      get
      {
        return _ExchangeRoots ? _root2ComputerName : _root1ComputerName;
      }
    }

    public string ForeignRootComputerName
    {
      get
      {
        return _ExchangeRoots ? _root1ComputerName : _root2ComputerName;
      }
    }

    public bool RootsExchanged
    {
      get { return _ExchangeRoots; }
    }

    public static string MediumDirectoryNameFromFileName(string fileName)
    {
      string result = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName), System.IO.Path.GetFileNameWithoutExtension(fileName));
      result = PathUtil.NormalizeAbspath(result);
#if DEBUG
      PathUtil.Assert_Abspath(result);
#endif

      return result;
    }

    public string MediumDirectoryName
    {
      get
      {
        string result = MediumDirectoryNameFromFileName(this._FileName);
#if DEBUG
        PathUtil.Assert_Abspath(result);
#endif
        return result;
      }
    }

    public void EnsureAlignment()
    {
      if (Current.ComputerName == null || Current.ComputerName == string.Empty)
      {
        throw new ApplicationException("The current computer name is null or empty");
      }

      Align();

      if (this.MyRootComputerName != Current.ComputerName)
      {
        throw new ApplicationException("The current computer name is non of the computer names in this document");
      }
    }

    public int Count
    {
      get
      {
        return _rootPairs.Count;
      }
    }

    public void ClearCachedMyFiles()
    {
      this._allFilesHere = null;
    }

    public MD5SumFileNodesHashTable CachedAllMyFiles
    {
      get { return _allFilesHere; }
      set { _allFilesHere = value; }
    }

    public void AddRoot(string basename)
    {
      PathUtil.Assert_Abspath(basename);

      SetDirty();

      ClearCachedMyFiles();

      EnsureAlignment();

      RootPair added = new RootPair(this);
      _rootPairs.Add(added);
      added.MyRoot.SetFilePath(basename);
    }

    public void DeleteRoot(int index)
    {
      SetDirty();

      ClearCachedMyFiles();

      EnsureAlignment();

      _rootPairs.RemoveAt(index);
    }

    public void SetBasePath(int item, string path)
    {
      SetDirty();

      PathUtil.Assert_Abspath(path);

      EnsureAlignment();
      ((RootPair)_rootPairs[item]).MyRoot.SetFilePath(path);
    }
  }
}