//
//  FbImageDownloader.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System;

public class UrlImageDownloader : IImageDownloader
{
	IImageDownloadListner listner;
	MonoBehaviour mObject;
	public UrlImageDownloader(MonoBehaviour mObject) {
		this.mObject = mObject;
	}

	public void StartDownload(ImageRequest imageRequest, IImageDownloadListner _listner) {
		listner = _listner;
		listner.SetLoading(true);
		if(mObject!=null)
			mObject.StartCoroutine(DownloadFromUrl(imageRequest));
	}
	
	//1,abstractoin at this level , for facebook dowloader api coroutine 
	//2. abstraction for downloading through , www, or resourceload, or facebookdownloadapi
	IEnumerator DownloadFromUrl(ImageRequest imageRequest) {
		Debug.Log("File being loaded: "+ imageRequest.ImageUrl);
		WWW www= new WWW (imageRequest.ImageUrl);
		//todo : from cache;;;;;
		while(!www.isDone) {
			yield return 0;
		}
		// Wait for download to complete
		//  yield www;
		Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT5, false); //LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5    
		if(texTmp!=null) {
			try {
				www.LoadImageIntoTexture(texTmp);
				listner.SetLoading(false);
				listner.OnDownload(www.texture);
			} catch (Exception e) {
				Debug.Log(e.ToString());
			}
		}
		yield break;
	}
}

