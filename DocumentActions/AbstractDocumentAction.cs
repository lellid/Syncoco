using System;
using SyncTwoCo.Filter;
using SyncTwoCo.Traversing;
namespace SyncTwoCo.DocumentActions
{
	/// <summary>
	/// Summary description for AbstractAction.
	/// </summary>
	public abstract class AbstractDocumentAction
	{
    protected IBackgroundMonitor _monitor=new DummyBackgroundMonitor();
    protected MainDocument _doc;

    public AbstractDocumentAction(MainDocument doc)
      : this(doc,null)
    {
    }


    public AbstractDocumentAction(MainDocument doc, IBackgroundMonitor monitor)
    {
      _doc = doc;
      _monitor = monitor!=null ? monitor : new DummyBackgroundMonitor();
    }


    public virtual void BackgroundExecute()
    {
      System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.DirectExecute));
      thread.Name = this.GetType().Name;
      thread.IsBackground = true;
      thread.Start();
     
      _monitor = new ExternalDrivenBackgroundMonitor();
      GUI.BackgroundCancelDialog dlg = new GUI.BackgroundCancelDialog(thread,(ExternalDrivenBackgroundMonitor)_monitor);
      dlg.ShowDialog(Current.MainForm);
    }

   
    public abstract void DirectExecute();

	}
}
