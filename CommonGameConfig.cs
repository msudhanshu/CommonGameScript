using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CommonGameConfig {
	public static  readonly string SCORE_PREF = "SCORE_PREF";
	public static readonly string SCOREPREFIX = "Score : ";
	public static readonly string MAXSCOREPREFIX = "MaxScore : ";
	public static readonly string PLAY_STORE_URL = "https://play.google.com/store/apps/details?id=com.terabytesolution.Bubbleup"; //"http://unity3d.com/";
	public static readonly string RATE_APP_PREF = "RATE_APP_PREF";
	public static readonly string NEVER_SHOW_RATEAPP_PREFKEY = "NeverShowRateApp";
	public static readonly string LASTSHOWN_RATEAPP_PREFKEY = "LastShownRateApp";
	public static readonly string ALLTIMESHOWN_RATEAPP_PREFKEY = "AllTimeShownRateApp";

	public static readonly int[] LIMIT_COUNT_RATESHOW = {3,5,10};
	
	public static readonly string SOUND_ON_PREFKEY = "ShoundSettingOn";


	public static readonly string TapToStart_GameInfo = "Touch \n the 	SMALLEST bubble \n on the Screen!";
	public static readonly string TapToEnd_Score = "Total Score: {0} !";

	public static readonly float scorePointAnimDuration = 0.7f;
	public static readonly float scorePointAnimSpeed = 0.5f;

	public static readonly string GA_GAMEPLAY = "Gameplay";

	public static bool DEBUGING = true;
	public static bool DEBUG_EVENTLOG = false;

	public static string FeedLinkName {
		get {
			return "My best scored "+ 0 +" !";
		}
	}
	public static string FeedLinkCaption {
		get {
			return "BUBBLE BURSTER";
		}
	}
	public static string FeedLinkDescription {
		get {
			return "Come on beat me..";
		}
	}
}
