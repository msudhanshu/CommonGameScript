//
//  IImageDownloader.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//

using System;

public interface IImageDownloader
{
	void StartDownload(ImageRequest imageRequest, IImageDownloadListner listner);

}


