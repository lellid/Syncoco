using System;

namespace SyncTwoCo
{
	/// <summary>
	/// Summary description for IBackgroundMonitor.
	/// </summary>
	public interface IBackgroundMonitor
	{
    /// <summary>
    /// True when we should send a string what we do currently
    /// </summary>
    bool ShouldReport { get; }

    /// <summary>
    /// Sets a text that the user can read.
    /// </summary>
    /// <param name="text"></param>
    void Report(string text);

    /// <summary>
    /// Returns true if the activity was cancelled by the user
    /// </summary>
    bool CancelledByUser { get; }
		
	}


  public class DummyBackgroundMonitor : IBackgroundMonitor
  {
    #region IBackgroundMonitor Members

    public bool ShouldReport
    {
      get
      {
       return false;
      }
    }

    public void Report(string text)
    {
      
    }

    public bool CancelledByUser
    {
      get
      {
        
        return false;
      }
    }

    #endregion

  }

  public class ExternalDrivenBackgroundMonitor : IBackgroundMonitor
  {
    bool _shouldReport;
    string _reportText;
    bool _cancelledByUser;

    #region IBackgroundMonitor Members

    public bool ShouldReport
    {
      get
      {
        return _shouldReport;
      }
      set
      {
        _shouldReport |= value;
      }
    }

    public void Report(string text)
    {
      _reportText = text;
    }

    public string ReportText
    {
      get { return _reportText; }
    }

    public bool CancelledByUser
    {
      get
      {
        return _cancelledByUser;
      }
      set 
      {
        _cancelledByUser |= value;
      }
    }

    #endregion

  }

  public class TimedBackgroundMonitor : IBackgroundMonitor
  {
    System.Timers.Timer _timer = new System.Timers.Timer(200);
    bool _shouldReport;
    bool _cancelledByUser;
    string _reportText;

    public event System.Timers.ElapsedEventHandler Elapsed;

    public TimedBackgroundMonitor()
    {
      _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
    }

    public void Start()
    {
      _timer.Start();
    }

    public void Stop()
    {
    _timer.Stop();
    }

    public System.ComponentModel.ISynchronizeInvoke SynchronizingObject
    {
      get { return _timer.SynchronizingObject; }
        set { _timer.SynchronizingObject = value; }
    }

    #region IBackgroundMonitor Members

    public bool ShouldReport
    {
      get
      {
        return false;
      }
    }

    public void Report(string text)
    {
      _shouldReport = false;
      _reportText = text;
      
    }

    public string ReportText
    {
      set { _reportText = value; }
    }

    public bool CancelledByUser
    {
      get
      {
        return _cancelledByUser;
      }
      set
      {
        _cancelledByUser = value;
      }
    }

    #endregion

    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      _shouldReport = true;
      if(this.Elapsed!=null)
        this.Elapsed(sender,e);
    }
  }

}
