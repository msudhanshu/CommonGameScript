//
//  FbImageDownloader.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class ResourceImageDownloader : IImageDownloader
{
	public void StartDownload(ImageRequest imageRequest, IImageDownloadListner listner) {
		listner.SetLoading(true);
		Texture2D texture ;
		texture = Resources.LoadAssetAtPath<Texture2D>(imageRequest.ImageUrl);
		Debug.Log("Loading image from resource "+imageRequest.ImageUrl);


		//IN CASE OF LOAD() YOU HAVE TO PASS PATH UNDER RESOURCES FOLDER ONLY . WITH FILE NAME EXTENSION TOO
		if(texture==null) {
			Debug.Log("loading failed and try again");
			texture = Resources.Load<Texture2D>(Path.GetFileName(imageRequest.ImageUrl));
		}

		if(texture==null) {
			Debug.Log("loading failed and try again");
			texture = Resources.Load<Texture2D>(Path.GetFileNameWithoutExtension (imageRequest.ImageUrl));
		}
			//Resources.Load<Texture2D>(imageRequest.ImageUrl);// Resources.Load(url, typeof(Texture2D));
		// Resources.Load<Texture2D>(url);
		//string texture = "Assets/Resources/Textures/Turner.png";
		//Texture2D inputTexture = (Texture2D)Resources.LoadAssetAtPath(texture, typeof(Texture2D));
		listner.SetLoading(false);
		listner.OnDownload(texture);
	}
}


/*

			if(Input.GetKeyDown(KeyCode.I)) {
			string filename = 	"Assets/Resources/Album3D/1/screen_0x0_2014-08-06_16-45-51.png";
				Texture2D texture ;
				texture = Resources.LoadAssetAtPath<Texture2D>(filename);
				Debug.Log("Loading image from resource "+filename);
				if(texture==null) Debug.Log("loading failed");


				texture = Resources.Load<Texture2D>(Path.GetFileName(filename));
				Debug.Log("Loading image from resource "+Path.GetFileName(filename));
				if(texture==null) Debug.Log("loading failed");
				

				texture = Resources.Load<Texture2D>(Path.GetFileNameWithoutExtension (filename));
				Debug.Log("Loading image from resource "+Path.GetFileNameWithoutExtension(filename));
				if(texture==null) Debug.Log("loading failed");

			}
			*/