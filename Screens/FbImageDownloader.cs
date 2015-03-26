//
//  FbImageDownloader.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public class FbImageDownloader : IImageDownloader
{
	IImageDownloadListner listner;

	public void StartDownload(ImageRequest imageRequest, IImageDownloadListner _listner) {
		this.listner = _listner;
		listner.SetLoading(true);
		if(FB.IsLoggedIn) {
			Debug.Log("Downloading fb image : " + GetPictureURL(imageRequest));
			FB.API(GetPictureURL(imageRequest), Facebook.HttpMethod.GET, FbPictureCallback);
		}
	}

	private string GetPictureURL(ImageRequest imageRequest) {
		return GetPictureURL(imageRequest.ImageUrl,imageRequest.width,imageRequest.height);
	}

	private string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
	{
		string url = string.Format("/{0}/picture", facebookID);
		string query = width != null ? "&width=" + width.ToString() : "";
		query += height != null ? "&height=" + height.ToString() : "";
		query += type != null ? "&type=" + type : "";
		if (query != "") url += ("?g" + query);
		return url;
	}


	public void FbPictureCallback(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
		listner.SetLoading(false);
		listner.OnDownload(result.Texture);
	}

}

