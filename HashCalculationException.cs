using System;

namespace Syncoco
{
	/// <summary>
	/// Summary description for HashCalculationException.
	/// </summary>
	public class HashCalculationException : System.ApplicationException
	{
    public HashCalculationException(string filename, string message)
      : base(string.Format("unable to calculate hash for file {0} : {1}"))
    {
    }

	
	}
}
