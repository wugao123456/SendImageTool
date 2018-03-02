using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dicom.Imaging
{
	/// <summary>
	/// FileImageManager 类 平行于ImageManager类实现基于文件系统的图像
	/// </summary>
	public abstract class FileImageManager : IClassifiedManager 
	{
		private static FileImageManager implementation;


		static FileImageManager()
        {
			SetImplementation(Setup.GetDefaultPlatformInstance<FileImageManager>());
        }
		public abstract bool IsDefault { get; }

		public static void SetImplementation(FileImageManager impl)
		{
			implementation = impl;
		}

		public static IFileImage CreateImage(int width,int height,string file)
		{
			return implementation.CreateImageImpl(width,height,file);
		}
		protected abstract IFileImage CreateImageImpl(int width,int height,string file);
	}
}
