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

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] argv) 
    {
      for(int i=0;i<argv.Length;i++)
      {
        if(argv[i].ToLower()=="/m" || argv[i].ToLower()=="-m")
        {
          if((i+1)<argv.Length)
          {
            Current.ComputerName = argv[i+1];
            i++;
          }
        }
      }

      MainForm = new Syncoco();
      System.Windows.Forms.Application.Run(MainForm);
    }
  }
}
