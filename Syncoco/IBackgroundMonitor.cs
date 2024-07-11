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
    protected bool _shouldReport;
    private string _reportText;
    private bool _cancelledByUser;

    #region IBackgroundMonitor Members

    public virtual bool ShouldReport
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
      _shouldReport = false;
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

  public class ExternalDrivenTimeReportMonitor : ExternalDrivenBackgroundMonitor
  {
    private DateTime _timeBegin = DateTime.Now;

    public override bool ShouldReport
    {
      get
      {
        return _shouldReport;
      }
      set
      {
        _shouldReport |= value;

        if (_shouldReport)
          Report("Busy ... " + (DateTime.Now - _timeBegin).ToString());
      }
    }
  }

  public class TimedBackgroundMonitor : IBackgroundMonitor
  {
    private System.Timers.Timer _timer = new System.Timers.Timer(200);
    private bool _shouldReport;
    private bool _cancelledByUser;
    private string _reportText;

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
        return _shouldReport;
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
      if (this.Elapsed != null)
        this.Elapsed(sender, e);
    }
  }

}
