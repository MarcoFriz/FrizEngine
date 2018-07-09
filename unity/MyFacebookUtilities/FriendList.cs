using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System;

#if FacebookApi
using Facebook.Unity;
using Facebook.MiniJSON;

namespace FrizEngine.MyFacebookUtilities
{
	public class FriendList:MonoBehaviour
	{

		#if !RELEASE_VERSION
		[SerializeField] bool showGUI = false;
		[SerializeField] bool verbose = false;

		void OnGUI ()
		{
			if (!showGUI)
				return;
			GUILayout.Label (GetType ().ToString ());
			if (!FB.IsLoggedIn && GUILayout.Button ("Login")) {
				Login (null);
			}
			if (FB.IsLoggedIn && GUILayout.Button ("Logout")) {
				Logout ();
			}
			if (FB.IsLoggedIn) {
				if (GUILayout.Button ("Get ID")) {
					Debug.Log (GetId ());
				}
				if (GUILayout.Button ("Get Friends")) {
					RequestFriends (null);
				}
				if (GUILayout.Button ("Get Scores")) {
					RequestScores (null);
				}
				if (GUILayout.Button ("Set Score")) {
					RequestSetScore (10, -1, -2, -1, -1, null);
				}
			}
		}
		#endif


		#region Class members

		public static FriendList instance = null;
		[SerializeField] string serverURL = "snhgames.com/server.php";
		string user_id = null;
		string user_name = null;
		UsersList friends = null;
		ScoresList scores = null;

		event System.Action OnLoginHandler = delegate {};
		event System.Action OnGetNameHandler = delegate {};
		event System.Action OnGetFriendsHandler = delegate {};
		event System.Action OnGetScoresHandler = delegate {};
		event System.Action OnSetScoreHandler = delegate {};

		void GetFriendsListForScores ()
		{
			WWWForm data = new WWWForm ();
			string friends_str = string.Empty;
			foreach (var friend in friends.data) {
				friends_str += friend.id + ",";
			}
			if (friends_str.Length != 0) {
				friends_str = friends_str.Substring (0, friends_str.Length - 1);
			}
			data.AddField ("action", "get_scores");
			data.AddField ("fbid", user_id);
			data.AddField ("game", Application.identifier);
			data.AddField ("friends", friends_str);
			UnityWebRequest www = UnityWebRequest.Post (serverURL, data);
			www.chunkedTransfer = false;
			StartCoroutine (GetListOfScores (friends, www));
		}

		IEnumerator GetListOfScores (UsersList friendList, UnityWebRequest www)
		{
			yield return www.SendWebRequest ();
			if (www.isNetworkError || www.isHttpError) {	
				Debug.Log (www.error);
				yield break;
			}

#if !RELEASE_VERSION
			if (verbose)
				Debug.LogFormat ("<color=blue>{0}</color>",
					www.downloadHandler.text);
#endif
			WWWForm form = new WWWForm ();
			Dictionary<string, object> result = 
				(Dictionary<string,object>)MiniJSON.Json.Deserialize (www.downloadHandler.text);
			if (result.ContainsKey ("error")) {
				Debug.Log (result ["error"]);
				string error = (string)result ["error"];
				switch (error) {
				case "USER_NO_EXIST":
					form.AddField ("action", "register");
					form.AddField ("fbid", user_id);
					form.AddField ("game", Application.identifier);
					form.AddField ("username", user_name);
					www = UnityWebRequest.Post (serverURL, form);
					www.chunkedTransfer = false;
					yield return new WaitForSeconds (1f);
					yield return GetListOfScores (friendList, www);
					yield break;
				default:
					yield break;
				}
			}
			//registramos el usuario correctamente
			if (result.ContainsKey ("success")) {
				bool success = (bool)result ["success"];
				if (success) {
					GetFriendsListForScores ();
				}
				yield break;
			}
			//Mostramos los datos
			ScoresList scoreList = new ScoresList ();
			scoreList.Init (www.downloadHandler.text);
			//
			if (OnGetScoresHandler == null)
				yield break;
			this.scores = scoreList;
			OnGetScoresHandler ();
			ClearDelegate (OnGetScoresHandler);
		}

		IEnumerator RequestSetScoreHandler (UnityWebRequest www)
		{
			yield return www.SendWebRequest ();
			if (www.isNetworkError || www.isHttpError) {	
				Debug.Log (www.error);
				yield break;
			}
			Debug.Log (www.downloadHandler.text);
			if (OnSetScoreHandler == null)
				yield break;
			OnSetScoreHandler ();
			ClearDelegate (OnSetScoreHandler);
		}

		void ClearDelegate (System.Action delegateHandler)
		{
			if (delegateHandler == null)
				return;
			System.Delegate[] list = delegateHandler.GetInvocationList ();
			foreach (System.Delegate del in list) {
				delegateHandler -= (System.Action)del;
			}
		}

		#endregion


		#region Class acsesor

		public void Logout ()
		{
			FB.LogOut ();
		}

		public void Login (System.Action CallBackHandler)
		{
			if (CallBackHandler != null)
				OnLoginHandler += CallBackHandler;
			//
			FB.LogInWithReadPermissions (new List<string> () 
				{ "public_profile", "email", "user_friends" }, 
//			FB.LogInWithPublishPermissions (new List<string> () 
				//				{ "public_profile", "email", "user_friends", "publish_actions" }, 
				(ILoginResult result) => {

#if !RELEASE_VERSION
					if (verbose)
						Debug.Log (result.RawResult);
#endif
					if (result.ResultDictionary.ContainsKey ("user_id"))
						user_id = (string)result.ResultDictionary ["user_id"];
					if (result != null)
						OnLoginHandler ();
					//clear
					System.Delegate[] list = OnLoginHandler.GetInvocationList ();
					foreach (System.Delegate del in list) {
						OnLoginHandler -= (System.Action)del;
					}
				}
			);
		}

		public string GetId ()
		{
			return user_id;
		}

		public string GetName ()
		{
			return user_name;
		}

		public UsersList GetFriends ()
		{
			return friends;
		}

		public ScoresList GetScores ()
		{
			return scores;
		}

		public void RequestName (System.Action CallbackHandler)
		{
			if (CallbackHandler != null)
				OnGetNameHandler += CallbackHandler;
			string query = "/me";
			Dictionary<string,string> data = new Dictionary<string,string> ();
			data.Add ("fields", "id,name");
			FB.API (query, HttpMethod.POST, 
				(IGraphResult result) => {

#if !RELEASE_VERSION
					if (verbose)
						Debug.Log (result.RawResult);
#endif
					if (result.ResultDictionary.ContainsKey ("id"))
						user_id = (string)result.ResultDictionary ["id"];
					if (result.ResultDictionary.ContainsKey ("name"))
						user_name = (string)result.ResultDictionary ["name"];
					if (OnGetNameHandler == null)
						return;
					OnGetNameHandler ();
					//limpiamos el delegate
					System.Delegate[] list = OnGetNameHandler.GetInvocationList ();
					foreach (System.Delegate del in list) {
						OnGetNameHandler -= (System.Action)del;
					}
				}
				, data);
		}

		public void RequestFriends (System.Action CallbackHandler)
		{
			if (CallbackHandler != null)
				OnGetFriendsHandler += CallbackHandler;
			string query = "/me/friends";
			FB.API (query, HttpMethod.GET, 
				(IGraphResult result) => {

#if !RELEASE_VERSION
					if (verbose)
						Debug.Log (result.RawResult);
#endif
					string json = result.RawResult;
					UsersList users = new UsersList ();
					users.Init (json);
					//TODO verificar en el pagin si es el listado completo
					//si no lo es, no enviar evento hasta completar el listado
					if (OnGetFriendsHandler == null)
						return;
					friends = users; 
					OnGetFriendsHandler ();
					//limpiamos el delegate
					System.Delegate[] list = OnGetFriendsHandler.GetInvocationList ();
					foreach (System.Delegate del in list) {
						OnGetFriendsHandler -= (System.Action)del;
					}
				}
			);
		}

		public void RequestScores (System.Action CallbackHandler)
		{
			if (CallbackHandler != null)
				OnGetScoresHandler += CallbackHandler;
			//Obtener listado de Amigos
			if (friends == null) {
				RequestFriends (GetFriendsListForScores);
				return;
			}
			GetFriendsListForScores ();
			//esto ya no funciona en facebook
//
//			string query = string.Format ("/{0}/scores", FB.AppId);
//			FB.API (query, HttpMethod.GET,
//				(IGraphResult result) => {
//					#if !RELEASE_VERSION
//					if (verbose)
//						Debug.Log (result.RawResult);
//					#endif
//					string json = result.RawResult;
//					ScoresList scores = new ScoresList ();
//					scores.Init (json);
//					//
//					if (OnGetScoresHandler == null)
//						return;
//					OnGetScoresHandler (scores);
//					//limpiamos el delegate
//					System.Delegate[] list = OnGetScoresHandler.GetInvocationList ();
//					foreach (System.Delegate del in list) {
//						OnGetScoresHandler -= (System.Action<ScoresList>)del;
//					}
//				}
//			);
		}

		public void RequestSetScore (
			int score,
			int level, 
			int laps, 
			int currentSocket,
			int sockets, 
			System.Action CallbackHandler)
		{
			if (CallbackHandler != null)
				OnSetScoreHandler += CallbackHandler;
			WWWForm data = new WWWForm ();
			//
			data.AddField ("action", "set_score");
			data.AddField ("fbid", user_id);
			data.AddField ("name", user_name);
			data.AddField ("game", Application.identifier);
			//
			data.AddField ("score", score);
			data.AddField ("level", level);
			data.AddField ("laps", laps);
			data.AddField ("currentSocket", currentSocket);
			data.AddField ("sockets", sockets);
			//
			UnityWebRequest www = UnityWebRequest.Post (serverURL, data);
			www.chunkedTransfer = false;
			StartCoroutine (RequestSetScoreHandler (www));

			// Este metodo ya no funciona
//			string query = string.Format ("/{0}/scores", "me");
//			Dictionary<string, string> data = new Dictionary<string,string> ();
//			data.Add ("score", score.ToString ());
//			OnSetScoreHandler += callback;
//			FB.API (query, HttpMethod.POST, 
//				(IGraphResult result) => {
//					#if !RELEASE_VERSION
//					if (verbose)
//						Debug.Log (result.RawResult);
//					#endif
//					if (OnSetScoreHandler == null)
//						return;
//					OnSetScoreHandler ();
//					//clear
//					System.Delegate[] list = OnSetScoreHandler.GetInvocationList ();
//					foreach (System.Delegate del in list) {
//						OnSetScoreHandler -= (System.Action)del;
//					}	
//				}
//				, data);
			//
		}


		#endregion


		#region MonoBehaviour overrides

		///Initialization
		//void Reset(){}
		void Awake ()
		{
			if (!FB.IsInitialized)
				FB.Init ();
			if (instance == null)
				instance = this;
		}

		//void OnEnable(){}
		//void Start (){}

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