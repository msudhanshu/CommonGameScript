using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;

//fql

public class FBUtil : ScriptableObject
{
    public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
    {
        string url = string.Format("/{0}/picture", facebookID);
        string query = width != null ? "&width=" + width.ToString() : "";
        query += height != null ? "&height=" + height.ToString() : "";
        query += type != null ? "&type=" + type : "";
        if (query != "") url += ("?g" + query);
        return url;
    }


    //FBTODO
//    public static void FriendPictureCallback(FBResult result)
//    {
//        if (result.Error != null)
//        {
//            Debug.LogError(result.Error);
//            return;
//        }
//
//		//TODO : MANJEET
//       // GameStateManager.FriendTexture = result.Texture;
//    }

    public static Dictionary<string, string> RandomFriend(List<object> friends)
    {
        var fd = ((Dictionary<string, object>)(friends[Random.Range(0, friends.Count - 1)]));
        var friend = new Dictionary<string, string>();
        friend["id"] = (string)fd["id"];
        friend["name"] = (string)fd["name"];
        return friend;
    }

    public static Dictionary<string, string> DeserializeJSONProfile(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object nameH;
        var profile = new Dictionary<string, string>();
        if (responseObject.TryGetValue("name", out nameH))
        {
            profile["name"] = (string)nameH;
        }
        return profile;
    }
    
    public static List<object> DeserializeScores(string response) 
    {

        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object scoresh;
        var scores = new List<object>();
        if (responseObject.TryGetValue ("data", out scoresh)) 
        {
            scores = (List<object>) scoresh;
        }

        return scores;
    }

    public static List<object> DeserializeJSONFriends(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object friendsH;
        var friends = new List<object>();
        if (responseObject.TryGetValue("friends", out friendsH))
        {
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
        }
        return friends;
    }
    
	public static string DeserializePictureURLString(string response)
	{
		return DeserializePictureURLObject(Json.Deserialize(response));
	}
	
	public static string DeserializePictureURLObject(object pictureObj)
	{
		var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
		object urlH = null;
		if (picture.TryGetValue("url", out urlH))
		{
			return (string)urlH;
		}
		
		return null;
	}

    
    public static void DrawActualSizeTexture (Vector2 pos, Texture texture, float scale = 1.0f)
    {
        Rect rect = new Rect (pos.x, pos.y, texture.width * scale , texture.height * scale);
        GUI.DrawTexture(rect, texture);
    }
    public static void DrawSimpleText (Vector2 pos, GUIStyle style, string text)
    {
        Rect rect = new Rect (pos.x, pos.y, Screen.width, Screen.height);
        GUI.Label (rect, text, style);
    }

	private static void JavascriptLog(string msg)
	{
		Application.ExternalCall("console.log", msg);
	}
	public static void Log (string message)
	{
		Debug.Log(message);
		if (Application.isWebPlayer)
			JavascriptLog(message);
	}
	public static void LogError (string message)
	{
		Debug.LogError(message);
		if (Application.isWebPlayer)
			JavascriptLog(message);
	}


}



/*
    $statuses = $facebook->api('/me/statuses');

    foreach($statuses['data'] as $status){
    // processing likes array for calculating fanbase. 

            foreach($status['likes']['data'] as $likesData){
                $frid = $likesData['id']; 
                $frname = $likesData['name']; 
                $friendArray[$frid] = $frname;
            }

         foreach($status['comments']['data'] as $comArray){
         // processing comments array for calculating fanbase
                    $frid = $comArray['from']['id'];
                    $frname = $comArray['from']['name'];
    }
}
*/
