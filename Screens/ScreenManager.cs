using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace Album3D {

public class ScreenManager : Manager<ScreenManager> {

	public MyInteractiveConsole fbManager;
	public bool temploadflag = false;
	List<Screen> allScreens = new List<Screen> ();

		//CONSUMER
	SortedDictionary<int,Queue<Screen>> screenQueue = new SortedDictionary<int, Queue<Screen>> ();
		//producer
	Queue<ImageRequest> imageURLS = new Queue<ImageRequest>();

	/* Implement this function */
	override public void StartInit(){
			fbManager = GetComponent<MyInteractiveConsole> ();
			//AddImageURL (@"/Users/manjeet/Desktop/");
			if(fbManager==null)
				fbManager = MyInteractiveConsole.instance;
			MyInteractiveConsole.OnFbLogin+=AddFbFriendImageURL;
		}
	
	/* Implement this function */
	override public void PopulateDependencies(){}

	void LateUpdate() {
		//	if (Input.GetMouseButtonDown (0))
		//						temploadflag = true;

		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H))
			temploadflag = true;
        
        if (temploadflag) {

				imageURLS.Clear();
				if(imageURLS.Count <=0) {

					if (Input.GetKeyDown(KeyCode.F))
					AddFbFriendImageURL();
					if (Input.GetKeyDown(KeyCode.G))
					AddImageURL (@"/Users/manjeet/Desktop/image/");		
					if ( Input.GetKeyDown(KeyCode.H) )
					AddResourceImageURL(1);
				}


			    ResetAllScreen ();
			    temploadflag = false;
		}

			FillScreens ();
	}

	public void AddScreen(Screen screen, int priority = 3) {
		screen.SetPriority (priority);
		allScreens.Add (screen);
		AddInPriorityScreen (screen);
	}

	
	public void FillScreen(Screen screen, Texture texture) {
		screen.SetTexture (texture);
	}

	public void ResetAllScreen() {
			screenQueue.Clear ();
			foreach (Screen screen in allScreens) 
						AddInPriorityScreen (screen);
	}

	private void AddInPriorityScreen(Screen screen) {
			if (!screenQueue.ContainsKey (screen.GetPriority ())) {
				screenQueue.Add (screen.GetPriority (), new Queue<Screen> ());
			}
			screenQueue [screen.GetPriority ()].Enqueue (screen);
	}

	private Screen GetPriorityScreen ()
	{
			foreach (int priorityKey in screenQueue.Keys) {
				if (screenQueue [priorityKey].Count > 0) {
					return screenQueue [priorityKey].Dequeue ();
			}
		}
		return null;
	}
	
	public void AddImageURL(string dirPath) {
			string defaultUrl;		
			foreach (string filePath in Directory.GetFiles(dirPath)) {
				Debug.Log("file added in imagequeue file://"+filePath);
				imageURLS.Enqueue( new ImageRequest(@"file://"+filePath,ImageDownloadType.URL) );
			}
			defaultUrl = @"http://www.nilacharal.com/enter/celeb/images/Genelia.jpg";
			imageURLS.Enqueue(new ImageRequest(defaultUrl,ImageDownloadType.URL));
			defaultUrl = @"http://www.elisha-cuthbert.com/hairstyles/elisha-cuthbert-hairstyle-6.jpg";
			imageURLS.Enqueue(new ImageRequest(defaultUrl,ImageDownloadType.URL));
			defaultUrl =@"https://graph.facebook.com/"  + FB.UserId +  @"/picture?type=large";
			imageURLS.Enqueue(new ImageRequest(defaultUrl,ImageDownloadType.URL));
	}

	//TODO : in case of resources .... dowload should be diffrent , not like www but resource.load()
	public void AddResourceImageURL(int albumNo) {
		string filePath ="Assets/Resources/Album3D/"+albumNo+"/" ;
    	DirectoryInfo dir = new DirectoryInfo (filePath);
		FileInfo[] info = dir.GetFiles ("*g");
		Debug.Log (info.ToString ());
		info.Select (f => f.FullName).ToArray ();
		foreach (FileInfo f in info) { 
			Debug.Log(f.FullName);
			imageURLS.Enqueue( new ImageRequest(filePath + Path.GetFileName(f.FullName) ,ImageDownloadType.RESOURCE) );
		}
	}

	public void AddFbFriendImageURL() {
		Debug.Log ("manjeet : Getting fb freinds");
		/*Three Mehtod :
		1.fb api (picture url from friend id)
		2. https://graph.facebook.com/[friend NameId]/picture?type=large
		3.GetAllFbFriendsURL .. friend image url
		*/
		
		/*string[] friendUrls = fbManager.GetAllFbFriendsIds(); //GetAllFbFriendsURL();
		foreach (string url in friendUrls) {
			Debug.Log(url);
			imageURLS.Enqueue(new ImageRequest(url,ImageDownloadType.FACEBOOKAPI)  );
		}*/

			ImageRequest[] friendUrls = fbManager.GetAllFbFriends(); //GetAllFbFriendsURL();
			foreach (ImageRequest url in friendUrls) {
				Debug.Log(url);
				imageURLS.Enqueue(url);
			}
	}

		//toughness will be combination of screen,screeneffect,image,and gridblock.
		//point will be function of toughness
	public void FillScreens() {
		if(imageURLS.Count > 0 ) {
			Screen screen = GetPriorityScreen ();
			if (screen != null) {
				ImageRequest imageRequest = imageURLS.Dequeue ();
				IImageDownloader imageDownloader = GetImageDownloader(imageRequest);
				screen.ImageName = imageRequest.ImageName;
				imageDownloader.StartDownload(imageRequest,screen);
				//StartCoroutine( CoroutineLoadFromUrl(screen,defaultUrl, new ImageDownloadDelegate(ImageDownloadCallback) ) );
			}
		}
	}


	public IImageDownloader GetImageDownloader(ImageRequest imageRequest) {
		switch(imageRequest.imageType) {
			case ImageDownloadType.FACEBOOKAPI:
				return new FbImageDownloader();
			case ImageDownloadType.URL:
				return new UrlImageDownloader(this);
			case ImageDownloadType.RESOURCE:
				return new ResourceImageDownloader();
			default:
				return new UrlImageDownloader(this);
			}
	}
















	public delegate void ImageDownloadDelegate (Screen screen, Texture texture);
	/// /////////
	void LoadFbImageAPI (Screen screen, string url, ImageDownloadDelegate callback)
	{
		FB.API(url,Facebook.HttpMethod.GET,result =>
		       {
			if (result.Error != null)
			{
				FBUtil.LogError(result.Error);
				return;
			}
			
			var imageUrl = FBUtil.DeserializePictureURLString(result.Text);
			StartCoroutine(CoroutineLoadFromUrl(screen,imageUrl,callback));
		});
	}
	void LoadPictureURL (Screen screen, string url, ImageDownloadDelegate callback){
			StartCoroutine(CoroutineLoadFromUrl(screen, url,callback));
	}
		//
		//FB.API(FBUtil.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, FBUtil.FriendPictureCallback);
		/// ///////////////



	public void ImageDownloadCallback(Screen screen,Texture texture) {
			screen.SetTexture(texture);
	}
	

		//1,abstractoin at this level , for facebook dowloader api coroutine 
		//2. abstraction for downloading through , www, or resourceload, or facebookdownloadapi
	IEnumerator CoroutineLoadFromUrl(Screen screen, string url, ImageDownloadDelegate callback) {
		Debug.Log("File being loaded: "+url);
		screen.SetLoading();
		WWW www= new WWW (url);
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
				screen.SetLoading(false);
				callback(screen, www.texture);
			} catch (Exception e) {
				Debug.Log(e.ToString());
			}
		}
		yield break;
	}

}

}




/*
FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);
 
foreach (FileInfo file in fileInfo) {
    // file name check
    if (file.Name == "something") {
        ...
    }
    // file extension check
    if (file.Extension == ".ext") {
        ...
    }
    // etc.
}
if (File.Exists("/path/to/file.ext")) {}

 * */