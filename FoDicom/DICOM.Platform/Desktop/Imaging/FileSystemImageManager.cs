using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dicom.Imaging
{
	public sealed class FileSystemImageManager : FileImageManager
	{

		public static readonly FileImageManager Instance;
		
		static FileSystemImageManager()
        {
			Instance = new FileSystemImageManager();
        }

		public override bool IsDefault
		{
			get { return true; }
		}

		protected override IFileImage CreateImageImpl(int width,int height,string file)
		{
			return new FileSystemImage(width,height,file);
		}

	}
}
