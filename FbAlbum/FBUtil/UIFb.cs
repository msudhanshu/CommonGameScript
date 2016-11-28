using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class UIFb : MonoBehaviour {

	public Button fbloginb;
	public Button fblogoutb;
	public Text FbUsertext;

	// Use this for initialization
	void Awake () {
		MyInteractiveConsole.OnFbLogin+=UpdateFbUI;
		UpdateFbUI();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateFbUI() {
//		if(MyInteractiveConsole.instance.IsLoggedIn()) 
		if(FB.IsLoggedIn)
		{
			fbloginb.gameObject.SetActive(false);
			fblogoutb.gameObject.SetActive(true);
			FbUsertext.gameObject.SetActive(true);
			FbUsertext.text = MyInteractiveConsole.instance.FbUserName;
		} else {
			fbloginb.gameObject.SetActive(true);
			fblogoutb.gameObject.SetActive(false);
			FbUsertext.gameObject.SetActive(false);
		}

	}

	public void FbLogin() {
		MyInteractiveConsole.instance.Login();
		UpdateFbUI();
	}

	public void FbLogout() {
		MyInteractiveConsole.instance.Logout();
		UpdateFbUI();
	}
}
