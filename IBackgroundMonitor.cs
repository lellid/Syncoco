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

}
