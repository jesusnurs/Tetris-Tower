using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
	[SerializeField] string _androidAdUnitId = "Interstitial_Android";
	[SerializeField] string _iosAdUnitId = "Interstitial_iOS";
	string _adUnitId;

	public Action OnInterstitialAdShowed;

	void Awake()
	{
#if UNITY_IOS
            _adUnitId = _iosAdUnitId;
#elif UNITY_ANDROID
		_adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
            //_adUnitId = _androidGameId; //Only for testing the functionality in the Editor
#endif
	}

	public void LoadAd()
	{
		Debug.Log("Loading Ad: " + _adUnitId);
		Advertisement.Load(_adUnitId, this);
	}

	public void ShowInterstitialAd()
	{
		Debug.Log("Showing Ad: " + _adUnitId);
		Advertisement.Show(_adUnitId, this);
		LoadAd();
	}

	public void OnUnityAdsAdLoaded(string adUnitId)
	{
		Debug.Log("Interstitial Ads loaded");
	}

	public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
	{
		LoadAd();
		Debug.Log($"Error loading interstitial Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
	}

	public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
	{
		LoadAd();
		Debug.Log($"Error showing interstitial Ad Unit {_adUnitId}: {error.ToString()} - {message}");
	}

	public void OnUnityAdsShowStart(string placementId)
	{
		Debug.Log("Interstitial Ads start");

	}

	public void OnUnityAdsShowClick(string placementId)
	{
		Debug.Log("Interstitial Ads click");
	}

	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		Debug.Log("Interstitial Ads Show complete");
		OnInterstitialAdShowed.Invoke();
		LoadAd();
	}
}