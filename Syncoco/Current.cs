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
  /// Summary description for Current.
  /// </summary>
  public class Current
  {
    public static MainDocument Document = new MainDocument();
    public static System.Windows.Forms.Form MainForm;
    public static string ComputerName = System.Environment.MachineName;
    public static string InitialFileName = null;

    public static IErrorReporter ErrorReporter
    {
      get { return ((Syncoco.GUI.MainForm)MainForm).ErrorReporter; }
    }


    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main(string[] argv)
    {
      for (int i = 0; i < argv.Length; i++)
      {
        if (argv[i].ToLower() == "/m" || argv[i].ToLower() == "-m")
        {
          if ((i + 1) < argv.Length)
          {
            Current.ComputerName = argv[i + 1];
            i++;
          }
        }

        else // if it is nothing else, then it is the filename
        {
          InitialFileName = argv[i];
        }
      }

      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
#if !NETFRAMEWORK
      ApplicationConfiguration.Initialize();
#endif

      MainForm = new Syncoco.GUI.MainForm();
      //MainForm.Icon = new System.Drawing.Icon(typeof(Current), "App.ico");
      System.Windows.Forms.Application.Run(MainForm);
    }
  }
}
