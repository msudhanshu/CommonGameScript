//
//  IImageDownloader.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//

using System;
using UnityEngine;

public interface IImageDownloadListner
{
	void SetLoading(bool on);
	void OnDownload(Texture texture);
}


