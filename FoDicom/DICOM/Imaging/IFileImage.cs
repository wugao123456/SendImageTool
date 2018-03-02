using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dicom.Imaging
{
	public interface IFileImage : IDisposable
	{
		Stream Pixels { get; }

		void Flush();

	}
}
