using System;
using Syncoco.Filter;
using Syncoco.Traversing;
namespace Syncoco.DocumentActions
{
  /// <summary>
  /// Summary description for AbstractAction.
  /// </summary>
  public abstract class AbstractDocumentAction
  {
    protected IBackgroundMonitor _monitor=new DummyBackgroundMonitor();
    protected MainDocument _doc;
    protected System.Text.StringBuilder _errors = new System.Text.StringBuilder();

    public AbstractDocumentAction(MainDocument doc)
      : this(doc,null)
    {
    }


    public AbstractDocumentAction(MainDocument doc, IBackgroundMonitor monitor)
    {
      _doc = doc;
      _monitor = monitor!=null ? monitor : new DummyBackgroundMonitor();
    }


    public string Errors
    {
      get
      {
        return _errors.Length==0 ? null : _errors.ToString();
      }
    }

    public virtual void BackgroundExecute()
    {
      System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.DirectExecute));
      thread.Name = this.GetType().Name;
      thread.IsBackground = true;
      thread.Start();
     
      if(!(_monitor is ExternalDrivenBackgroundMonitor))
        _monitor = new ExternalDrivenBackgroundMonitor();

      GUI.BackgroundCancelDialog dlg = new GUI.BackgroundCancelDialog(thread,(ExternalDrivenBackgroundMonitor)_monitor);
      dlg.ShowDialog(Current.MainForm);
    }

   
    public abstract void DirectExecute();

  }
}
