using UnityEngine;
using System.Runtime.InteropServices;

namespace IOSAD{
	public class ChartboostSDK{
		#if UNITY_IPHONE && !UNITY_EDITOR_OSX
		[DllImport("__Internal")]
		extern static public void Chartboost_init (string appId, string channelId);
		#endif
		public static void init (string appId, string channelId) {
			#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			Chartboost_init (appId, channelId);
			#endif
		}
	};
	public class Adwords {
		#if UNITY_IPHONE && !UNITY_EDITOR_OSX
		[DllImport("__Internal")]
		extern static public void Adwords_init ();
		#endif
		public static void init () {
			#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			Adwords_init ();
			#endif
		}
	};
	public class TalkingDataWapper {
		private const string TALKING_DATA_JAVA_CLASS = "com.tendcloud.appcpa.TalkingDataAppCpa";

#if UNITY_IPHONE && !UNITY_EDITOR_OSX

		[DllImport("__Internal")]
		extern static public void TalkingData_init (string appId, string channelId);

		[DllImport("__Internal")]
		extern static public void TalkingData_onPay (string account, string orderId, int amount, string currencyName, string payType);

		[DllImport("__Internal")]
		extern static public void TalkingData_onRegister (string account);

		[DllImport("__Internal")]
		extern static public void TalkingData_onLogin (string account);

#endif
		
		public static void init () {
#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			TalkingData_init ("9071a987fafe401d9615d1937e95841c", "_default_");
#elif UNITY_ANDROID
			using (AndroidJavaClass cls = new AndroidJavaClass(TALKING_DATA_JAVA_CLASS)) {
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
				cls.CallStatic("init", jo, "1735CA5F3C3547E398D70D7B464B970C", "_default_");
			}
#endif
		}

		public static void onPay(string account, string orderId, int amount, string currencyName, string payType) {
#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			TalkingData_onPay (account, orderId, amount, currencyName, payType);
#elif UNITY_ANDROID
			using (AndroidJavaClass cls = new AndroidJavaClass(TALKING_DATA_JAVA_CLASS)) {           
				cls.CallStatic("onPay", account, orderId, amount, currencyName, payType);
			}
#endif
		}

		public static void onRegister(string account) {
#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			TalkingData_onRegister (account);
#elif UNITY_ANDROID
			using (AndroidJavaClass cls = new AndroidJavaClass(TALKING_DATA_JAVA_CLASS)) {           
				cls.CallStatic("onRegister", account);
			}
#endif
		}

		public static void onLogin(string account) {
#if UNITY_IPHONE && !UNITY_EDITOR_OSX
			TalkingData_onLogin (account);
#elif UNITY_ANDROID
			using (AndroidJavaClass cls = new AndroidJavaClass(TALKING_DATA_JAVA_CLASS)) {           
				cls.CallStatic("onLogin", account);
			}
#endif
		}
	}
}