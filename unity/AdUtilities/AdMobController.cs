#if GoogleApi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

namespace FrizEngine.MyAdMobUtilities
{
	public class AdMobController : MonoBehaviour
	{
		#region Class members

		public static AdMobController instance = null;
		BannerView bannerView;
		RewardBasedVideoAd rewardBaseVideo;

		event System.Action onBanner_Leaving = delegate{};
		event System.Action onVideo_Failed   = delegate{};
		event System.Action onVideo_Loaded   = delegate{};
		event System.Action onVideo_Opening  = delegate{};
		event System.Action onVideo_Started  = delegate{};
		event System.Action onVideo_Closed   = delegate{};
		event System.Action onVideo_Leaving  = delegate{};
		event System.Action onVideo_Rewarded = delegate{};

		System.Action ClearDelegate (System.Action delegateHandler)
		{
			if (delegateHandler == null) {
				return delegate {
				};
			}
			System.Delegate[] list = delegateHandler.GetInvocationList ();
			foreach (System.Delegate del in list) {
				delegateHandler -= (System.Action)del;
			}
			return delegateHandler;
		}

		void OnBannerEvent (object sender, System.EventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log ("BennerEvent:" + e);
#endif
			bannerView.Show ();
		}

		void OnBannerFailed (object sender, AdFailedToLoadEventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log (e.Message);
#endif
		}

		void OnBannerLeavingApplication (object sender, System.EventArgs e)
		{
			onBanner_Leaving ();
		}

		void OnVideoFailed (object sender, AdFailedToLoadEventArgs e)
		{


#if !RELEASE_VERSION
			if (e != null)
				Debug.Log ("Video Failed: " + e.Message);
			else
				Debug.Log ("Video Failed: " + "MockUp");
#endif
			if (onVideo_Failed != null)
				onVideo_Failed ();
		}

		void OnVideoLoaded (object sender, System.EventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log ("Video Loaded:" + e);
#endif
			if (onVideo_Loaded != null)
				onVideo_Loaded ();
		}

		void OnVideoOpening (object sender, System.EventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log ("Video Opening:" + e);
#endif
			if (onVideo_Opening != null)
				onVideo_Opening ();
		}

		void OnVideoStarted (object sender, System.EventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log ("Video Started:" + e);
#endif
			if (onVideo_Started != null)
				onVideo_Started ();
		}

		void OnVideoClosed (object sender, System.EventArgs e)
		{


#if !RELEASE_VERSION
			Debug.Log ("Video Closed:" + e);
#endif
			if (onVideo_Closed != null)
				onVideo_Closed ();
			ReLoadVideo ();
		}

		void OnVideoLeaving (object sender, System.EventArgs e)
		{
#if !RELEASE_VERSION
			Debug.Log ("Video Leaving:" + e);
#endif
			if (onVideo_Leaving != null)
				onVideo_Leaving ();
		}

		void OnVideoRewarded (object sender, Reward e)
		{
#if !RELEASE_VERSION
			Debug.Log ("Video Reward: " + e);
#endif
			if (onVideo_Rewarded != null)
				onVideo_Rewarded ();
		}

		void ReLoadVideo ()
		{
			LoadVideo ();
		}

		#endregion

		#region Class Accesors


		public bool isVideoLoaded{ get { return rewardBaseVideo.IsLoaded (); } }

		public AdMobController  Init ()
		{
			MobileAds.Initialize (GetApiId ("App"));
			return this;
		}

		public void ShowVideo ()
		{
			if (rewardBaseVideo.IsLoaded ())
				rewardBaseVideo.Show ();
		}

		public void DestroyBanner ()
		{
			bannerView.Destroy ();
		}

		public void DestroyVideo ()
		{
			onVideo_Failed = ClearDelegate (onVideo_Failed);
			onVideo_Loaded = ClearDelegate (onVideo_Loaded);
			onVideo_Opening = ClearDelegate (onVideo_Opening);
			onVideo_Started = ClearDelegate (onVideo_Started);
			onVideo_Leaving = ClearDelegate (onVideo_Leaving);
			onVideo_Rewarded = ClearDelegate (onVideo_Rewarded);
			onVideo_Closed = ClearDelegate (onVideo_Closed);
		}

		public void LoadBanner (
			AdSize size = null,
			System.Action loadedHandler = null)
		{
			size = (size) ?? AdSize.SmartBanner;
			string bannerId;
			bannerId = GetApiId ("Banner");
			bannerView = new BannerView (bannerId, size, AdPosition.Bottom);
			bannerView.OnAdLoaded += OnBannerEvent;
			bannerView.OnAdClosed += OnBannerEvent;
			bannerView.OnAdFailedToLoad += OnBannerFailed;
			bannerView.OnAdLeavingApplication += OnBannerLeavingApplication;
			//
			if (loadedHandler != null)
				onBanner_Leaving += loadedHandler;
			//


#if !RELEASE_VERSION
			AdRequest request = new AdRequest.Builder ().AddTestDevice ("5203d639fe5b9423").Build ();
#else
			AdRequest request = new AdRequest.Builder ().Build ();
#endif
			bannerView.LoadAd (request);
		}

		public void LoadVideo (
			System.Action videoFailed = null,
			System.Action videoLoaded = null,
			System.Action videoOpening = null,
			System.Action videoStarted = null,
			System.Action videoClosed = null,
			System.Action videoLeaving = null,
			System.Action videoRewarded = null)
		{
			string videoId;
			videoId = GetApiId ("Video");
			if (rewardBaseVideo == null) {
				rewardBaseVideo = RewardBasedVideoAd.Instance;
				rewardBaseVideo.OnAdFailedToLoad += OnVideoFailed;
				rewardBaseVideo.OnAdLoaded += OnVideoLoaded;
				rewardBaseVideo.OnAdOpening += OnVideoOpening;
				rewardBaseVideo.OnAdStarted += OnVideoStarted;
				rewardBaseVideo.OnAdClosed += OnVideoClosed;
				rewardBaseVideo.OnAdLeavingApplication += OnVideoLeaving;
				rewardBaseVideo.OnAdRewarded += OnVideoRewarded;
			}
			//Callbacks
			onVideo_Failed += videoFailed;
			onVideo_Loaded += videoLoaded;
			onVideo_Opening += videoOpening;
			onVideo_Started += videoStarted;
			onVideo_Closed += videoClosed;
			onVideo_Leaving += videoLeaving;
			onVideo_Rewarded += videoRewarded;
			//


#if !RELEASE_VERSION
			AdRequest request = new AdRequest.Builder ()
				.AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice ("5203d639fe5b9423").Build ();
#else
			AdRequest request = new AdRequest.Builder ().Build ();
#endif
			rewardBaseVideo.LoadAd (request, videoId);
		}

		public string GetApiId (string type)
		{
			switch (type) {


#if RELEASE_VERSION && UNITY_ANDROID
			case "App":
				return "ca-app-pub-2510245933564404~9143186373";
			case "Intersticial":
				return "ca-app-pub-2510245933564404/1372507569";
			case "Banner":
				return "ca-app-pub-2510245933564404/5147622115";
			case "Video":
				return "ca-app-pub-2510245933564404/2829486436";
			case "Video-Coins":
				return "ca-app-pub-2510245933564404/2386222553";
//				return "ca-app-pub-2510245933564404/1530567460";
			case "facebook_id":
				return "184161429032352";


#elif !RELEASE_VERSION && UNITY_ANDROID
			case "App":
				return "ca-app-pub-2510245933564404~9143186373";
			case "Intersticial":
				return "ca-app-pub-3940256099942544/1033173712";
			case "Banner":
				return "ca-app-pub-3940256099942544/6300978111";
			case "Video":
				return "ca-app-pub-3940256099942544/5224354917";
			case "facebook_id":
				return "184161429032352";


#elif !RELEASE_VERSION && UNITY_IPHONE
		case "App":
		return "ca-app-pub-2510245933564404~9143186373";
		case "Intersticial":
		return "ca-app-pub-3940256099942544/4411468910";
		case "Banner":
		return "ca-app-pub-3940256099942544/2934735716";
		case "Video":
		return "ca-app-pub-3940256099942544/1712485313";
		case "facebook_id":
		return "184161429032352";
#endif
			default:
				Debug.LogWarning ("No se reconoce el tipo:" + type);
				return "unexpected_platform";
			}
		}

		#endregion

		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		void Awake ()
		{
			if (instance == null)
				instance = this;
			else {
				Destroy (this);
			}
		}
		//void OnEnable(){}
		//void Start () {}

		///Physics
		//void FixedUpdate() {}
		//void OnTriggerEnter(Collider other){}
		//void OnCollisionEnter(Collision collision){}
	
		///Game Logic
		//void Update(){}
		//void LateUpdate(){}
	
		///Decommissioning
		//void OnApplicationQuit(){}
		//void OnDisable(){}
		//void OnDestroy(){}



		#endregion



		#region Super class overrides



		#endregion



		#region Class implementation



		#endregion



		#region Interface implementation



		#endregion
	
	}
}
#endif