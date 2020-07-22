﻿using com.adjust.sdk;
using MiddleGround;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class AdjustEventLogger : MonoBehaviour
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string Getidfa();
#endif
#if UNITY_IOS
    public const string APP_TOKEN = "mga1qo7h0ge8";
    public const string TOKEN_open = "9rpph2";
    public const string TOKEN_ad = "qq0bje";
    public const string TOKEN_noads = "i2981u";
    public const string TOKEN_stage_end = "hoi51j";
    public const string TOKEN_wheel="77ya28";
    public const string TOKEN_slots="3jwo2v";
    public const string TOKEN_ggl = "li80rh";
    public const string TOKEN_deeplink = "hhrdvl";
    public const string TOKEN_packb = "acha2z";
#elif UNITY_ANDROID
    public const string APP_TOKEN = "jd7c6pgj74lc";
    public const string TOKEN_open = "uhq3e9";
    public const string TOKEN_ad = "dq77bh";
    public const string TOKEN_noads = "kzt12m";
    /// <summary>
    /// dice
    /// </summary>
    public const string TOKEN_stage_end = "f8d9fb";
    public const string TOKEN_wheel = "iscrs0";
    public const string TOKEN_slots = "cmcx0h";
    /// <summary>
    /// scratch
    /// </summary>
    public const string TOKEN_ggl = "8cn8pq";
    public const string TOKEN_deeplink = "6sm9ed";
    public const string TOKEN_packb = "nij82z";
#endif
    public static AdjustEventLogger Instance;
    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        AdjustConfig adjustConfig = new AdjustConfig(APP_TOKEN, AdjustEnvironment.Sandbox);
#else
        AdjustConfig adjustConfig = new AdjustConfig(APP_TOKEN, AdjustEnvironment.Production);
#endif
        adjustConfig.sendInBackground = true;
        adjustConfig.launchDeferredDeeplink = true;
        adjustConfig.eventBufferingEnabled = true;
        adjustConfig.logLevel = AdjustLogLevel.Info;
        adjustConfig.setAttributionChangedDelegate(OnAttributionChangedCallback);
        Adjust.start(adjustConfig);
    }
    private void Start()
    {
        GetAdID();
        StartCoroutine(CheckAttributeTo());
        MG_Manager.Instance.SendAdjustGameStartEvent();
    }
    public void AdjustEventNoParam(string token)
    {
        AdjustEvent adjustEvent = new AdjustEvent(token);
        Adjust.trackEvent(adjustEvent);
    }
    public void AdjustEvent(string token, params (string key, string value)[] list)
    {
        AdjustEvent adjustEvent = new AdjustEvent(token);
        foreach (var (key, value) in list)
        {
            adjustEvent.addCallbackParameter(key, value);
        }
        Adjust.trackEvent(adjustEvent);
    }
    void OnAttributionChangedCallback(AdjustAttribution attribution)
    {
        if (attribution.network.Equals("Organic"))
        {
        }
        else
        {
            if (!MG_Manager.Instance.loadEnd)
                MG_Manager.Instance.Set_Save_isPackB();
        }
    }
    private string AppName = Ads.AppName;
    private string ifa;
    private IEnumerator CheckAttributeTo()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(ifa));
        string url = string.Format("http://a.mobile-mafia.com:3838/adjust/is_unity_clicked?ifa={0}&app_name={1}", ifa, AppName);

        var web = new UnityWebRequest(url);
        web.downloadHandler = new DownloadHandlerBuffer();
        yield return web.SendWebRequest();
        if (web.responseCode == 200)
        {
            if (web.downloadHandler.text.Equals("1"))
            {
                if (!MG_Manager.Instance.loadEnd)
                    MG_Manager.Instance.Set_Save_isPackB();
            }
        }
    }
    private void GetAdID()
    {
#if UNITY_ANDROID
        Application.RequestAdvertisingIdentifierAsync(
           (string advertisingId, bool trackingEnabled, string error) =>
           {
               ifa = advertisingId;
           }
       );
#elif UNITY_IOS && !UNITY_EDITOR
        ifa = Getidfa();
#endif
    }
}

