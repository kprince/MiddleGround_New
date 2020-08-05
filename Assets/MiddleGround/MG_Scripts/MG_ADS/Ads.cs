using MiddleGround;
using MiddleGround.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
#if UNITY_ANDROID
	private const string APP_KEY = "";
#elif UNITY_IOS
	private const string APP_KEY = "";
#endif
	public static Ads _instance;
	public string adDes = string.Empty;
	public const string AppName = "";
	private void Awake()
	{
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		//Dynamic config example
		IronSourceConfig.Instance.setClientSideCallbacks(true);

		string id = IronSource.Agent.getAdvertiserId();

		//IronSource.Agent.validateIntegration();

		// SDK init
		IronSource.Agent.init(APP_KEY);
		IronSource.Agent.loadInterstitial();

	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="rewardedCallback">when load ad successfully , it will be invoked.</param>
	/// <param name="failCallback">when clicktime more than 2 times and load ad fail, it will be invoked.</param>
	/// <param name="clickAdTime">the same button click time</param>
	/// <param name="des">the description of ad's origin.</param>
	public void ShowRewardVideo(Action rewardedCallback,Action failCallback, int clickAdTime,string des)
	{
		adDes = des;
		rewardCallback = rewardedCallback;
		rewardFailCallback = failCallback;
#if UNITY_EDITOR
        rewardedCallback();
        Debug.Log("Show RV : 【" + des + "】");
        return;
#endif
#if UNITY_IOS
		if (!MG_Manager.Instance.Get_Save_PackB())
		{
			rewardCallback();
			return true;
		}
#endif
        if (IronSource.Agent.isRewardedVideoAvailable())
		{
			IronSource.Agent.showRewardedVideo();
			return;
		}
		else
		{
			StartCoroutine(WaitLoadAD(true,clickAdTime));
			return;
		}
	}
	float interstialLasttime = 0;
	public void ShowInterstialAd(Action callback, string des)
	{
		popCallback = callback;
#if UNITY_EDITOR
		callback?.Invoke();
		Debug.Log("Show IV : 【" + des + "】");
		return;
#endif
		adDes = des;
#if UNITY_IOS
		if (!MG_Manager.Instance.Get_Save_PackB()) 
		{
			callback?.Invoke();
			return;
		}
#endif
		if (Time.realtimeSinceStartup - interstialLasttime < 30)
		{
			callback?.Invoke();
			return;
        }
		if (IronSource.Agent.isInterstitialReady())
		{
			interstialLasttime = Time.realtimeSinceStartup;
			IronSource.Agent.showInterstitial();
		}
		else
		{
			callback?.Invoke();
			MG_Manager.Instance.SendAdjustPlayAdEvent(false, false, adDes);
		}
	}
	void OnApplicationPause(bool isPaused)
	{
		IronSource.Agent.onApplicationPause(isPaused);
	}
	public GameObject notice;
	const string text = "No Video is ready , please try again later.";
	public void ShowNoAdNotice()
	{
		MG_Manager.Instance.Show_PopTipsPanel(text);
	}
	IEnumerator WaitLoadAD(bool isRewardedAd,int clickAdTime)
	{
		notice.SetActive(true);
		StringBuilder content = new StringBuilder("Loading.");
		Text noticeText = notice.GetComponentInChildren<Text>();
		noticeText.text = content.ToString();
		int timeOut = 6;
		while (timeOut > 0)
		{
			yield return new WaitForSeconds(Time.timeScale);
			timeOut--;
			content.Append('.');
			noticeText.text = content.ToString();
			if (isRewardedAd && IronSource.Agent.isRewardedVideoAvailable())
			{
				IronSource.Agent.showRewardedVideo();
				notice.SetActive(false);
				yield break;
			}
		}
		MG_Manager.Instance.SendAdjustPlayAdEvent(false, true, adDes);
		if (clickAdTime >= 2)
		{
			if (rewardFailCallback is object)
				rewardFailCallback.Invoke();
			//MG_UIManager.Instance.CloseTopPopPanelAsync();
			MG_Manager.Instance.Show_PopTipsPanel(text);
		}
		notice.SetActive(false);
	}
	Action rewardCallback;
	Action rewardFailCallback;
	private bool canGetReward = false;
	public void GetReward()
	{
		canGetReward = true;
	}
	public void InvokeGetRewardMethod()
	{
		if (canGetReward)
		{
			rewardCallback();
			canGetReward = false;
		}
	}
	Action popCallback;
	public void InvokePopAd()
    {
		popCallback?.Invoke();
    }
}
//FB A:774613690010415
//FB IOS:308946513807140
