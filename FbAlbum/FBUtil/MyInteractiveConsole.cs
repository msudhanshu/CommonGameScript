using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class MyInteractiveConsole : InteractiveConsole {

	public static MyInteractiveConsole instance;
	private static List<object>                 friends         = null;
	private static Dictionary<string, string>   profile         = null;
	public string FbUserName;
	public delegate void FbLoggedIn();
	public static event FbLoggedIn OnFbLogin;
    
	void Start() {
		StartCoroutine(InitFacebook());
	}

	override protected void Awake() {
		base.Awake();
		instance = this;
    }


	/////////////////////////////////////// 
	public bool IsLoggedIn() {
		return FB.IsLoggedIn;
	}
	
	public void Login() {
		StartCoroutine(InitFacebook());
	}
	
	public void Logout() {
		if(IsLoggedIn())
            CallFBLogout();
    }

    public IEnumerator InitFacebook() {
		Debug.Log ("Facebook Init called");
		if(!IsLoggedIn()) {
			Debug.Log ("Facebook initilising");
			CallFBInit();
			status = "FB.Init() called with " + FB.AppId;
			while(!FB.IsInitialized) yield return 0;
			
			Debug.Log (status);
			CallFBLogin();
			status = "Login called";
			while(!FB.IsLoggedIn) yield return 0;
		}
		Debug.Log (status);
		GetFbProfileInfo ();

	//	while(!FB.IsLoggedIn) yield return 0;
	//	GetFbProfileInfo ();
	}

	//https://developers.facebook.com/tools/explorer?method=GET&path=me%3Ffields%3Did%2Cfriends%2Cname&version=v2.2&
	//me?fields=gender,name,hometown,education,id,interested_in,last_name,address,location,relationship_status,interests,posts,videos,friends{relationship_status,name,gender,address,education,location,hometown,birthday,interested_in}

	//me?fields=id,name,picture,photos.type(uploaded).limit(5){album,name,picture,images}

	//me?fields=id,name,picture,albums.limit(15){count,name,link,cover_photo,location,type}

	//me?fields=id,name,picture,albums.limit(15){count,name,link,cover_photo,location,type,likes,photos}

	//me?fields=id,name,friends.limit(10){photos.type(uploaded).limit(2){picture,name,album,images},name}

	public void GetFbProfileInfo()
	{
		FbDebug.Log("Logged in. ID: " + FB.UserId);
		Debug.Log("......../me?fields=id,name,friends.limit(10).fields(name,id)");
		// Reqest player info and profile picture //,picture.type(large) //TODO TEMP MANJEET
		FB.API("/me?fields=id,name,friends.limit(10).fields(name,id)", Facebook.HttpMethod.GET, APICallback);
		FB.API(FBUtil.GetPictureURL("me", 1024, 1024), Facebook.HttpMethod.GET, MyPictureCallback);
	}
	
	void APICallback(FBResult result)
	{
		FbDebug.Log("APICallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again/////picture
			FB.API("/me?fields=id,name,friends.limit(5).fields(name,id)", Facebook.HttpMethod.GET, APICallback);
			return;
		}
		
		profile = FBUtil.DeserializeJSONProfile(result.Text);
		GameStateManager.Username = profile["name"];
		friends = FBUtil.DeserializeJSONFriends(result.Text);
		Debug.Log("Name "+profile["name"]+",allfriend list="+result.Text);
		OnFbLogin();
	}
	
	void MyPictureCallback(FBResult result)
	{
		FbDebug.Log("MyPictureCallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API(FBUtil.GetPictureURL("me", 512, 512), Facebook.HttpMethod.GET, MyPictureCallback);
			return;
		}
		GameStateManager.UserTexture = result.Texture;
	}
	
	public void GetFbFriend() {
		if (friends != null && friends.Count > 0)
		{
			// Select a random friend and get their picture
			Dictionary<string, string> friend = FBUtil.RandomFriend(friends);
			GameStateManager.FriendName = friend["name"];
			GameStateManager.FriendID = friend["id"];
			FB.API(FBUtil.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, FBUtil.FriendPictureCallback);
		}
	}

	public string[] GetAllFbFriendsIds(){
		List<string> picurls = new List<string>();
		if (friends != null && friends.Count > 0)
		{
			foreach (object s in friends) {
				var friend = ((Dictionary<string, object>)(s));
				Debug.Log(friend.ToString());
				Debug.Log("frind id "+(string)friend["id"] );
				picurls.Add((string)friend["id"] );
				//picurls.Add( FBUtil.DeserializePictureURLObject(friend["picture"] ) );

				//picurls.Add(FBUtil.GetPictureURL((string)friend["id"], 512, 512));
			}
			//FB.API(FBUtil.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, FBUtil.FriendPictureCallback);
		}
		return picurls.ToArray ();
	}

	public ImageRequest[] GetAllFbFriends(){
		List<ImageRequest> fbfimagerequest = new List<ImageRequest>();
		if (friends != null && friends.Count > 0)
		{
			foreach (object s in friends) {
				var friend = ((Dictionary<string, object>)(s));
				Debug.Log(friend.ToString());
				Debug.Log("frind id "+(string)friend["id"] );
				fbfimagerequest.Add( new ImageRequest( (string)friend["name"]  ,(string)friend["id"],ImageDownloadType.FACEBOOKAPI) );
				//picurls.Add( FBUtil.DeserializePictureURLObject(friend["picture"] ) );
				
				//picurls.Add(FBUtil.GetPictureURL((string)friend["id"], 512, 512));
			}
			//FB.API(FBUtil.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, FBUtil.FriendPictureCallback);
		}
		return fbfimagerequest.ToArray ();
	}

	public string[] GetAllFbFriendsURL(){
		List<string> picurls = new List<string>();
		if (friends != null && friends.Count > 0)
		{
			foreach (object s in friends) {
				var friend = ((Dictionary<string, object>)(s));
				Debug.Log(friend.ToString());
				Debug.Log("frind id "+(string)friend["id"] );
				picurls.Add( FBUtil.DeserializePictureURLObject(friend["picture"] ) );
				//picurls.Add(FBUtil.GetPictureURL((string)friend["id"], 512, 512));
			}
			//FB.API(FBUtil.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, FBUtil.FriendPictureCallback);
		}
		return picurls.ToArray ();
	}

	void OnGUI()
	{
	}

	public void ShareOnFbFeed()
	{
		//	CallFBFeed();
		FB.Feed(
			toId: FeedToId,
			link: Config.PLAY_STORE_URL,
			linkName:  Config.FeedLinkName,
			linkCaption: Config.FeedLinkCaption,
			linkDescription:  Config.FeedLinkDescription,
			//picture: Config.FBSHARE_PLAY_STORE_URL,
            properties: FeedProperties,
            callback: Callback
            );
    }
}