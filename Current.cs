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

    public static IErrorReporter ErrorReporter
    {
      get { return ((Syncoco)MainForm).ErrorReporter; }
    }


    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] argv) 
    {
      for(int i=0;i<argv.Length;i++)
      {
        if(argv[i].ToLower()=="/n" || argv[i].ToLower()=="-n")
        {
          if((i+1)<argv.Length)
          {
            Current.ComputerName = argv[i+1];
            i++;
          }
        }
      }

      MainForm = new Syncoco();
      MainForm.Icon = new System.Drawing.Icon(typeof(Current),"App.ico");
      System.Windows.Forms.Application.Run(MainForm);
    }
  }
}
