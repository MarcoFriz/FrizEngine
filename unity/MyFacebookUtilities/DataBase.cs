using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if FacebookApi
using Facebook.Unity;
using Facebook.MiniJSON;

namespace FrizEngine.MyFacebookUtilities
{
	[System.Serializable]
	public class DataBase
	{
		public void Init (object data)
		{
			string json = Json.Serialize (data);
			JsonUtility.FromJsonOverwrite (json, this);
		}

		public void Init (string json)
		{
			JsonUtility.FromJsonOverwrite (json, this);
		}
	}

	[System.Serializable]
	public class User:DataBase
	{
		public string id = string.Empty;
		public string name = string.Empty;
		Texture2D image = null;

		public bool isLoadingPicture { get; private set; }

		event System.Action<User> onImageLoaded = delegate {};

		public Texture2D GetImage (System.Action<User> CallBackHandler)
		{
			if (CallBackHandler != null)
				onImageLoaded += CallBackHandler;
			if (image != null)
				return image;
			if (isLoadingPicture)
				return null;
			if (id == string.Empty || id == null)
				return null;
			if (name == string.Empty || name == null)
				return null;
			isLoadingPicture = true;
			Dictionary<string,string> data = new Dictionary<string, string> ();
			data.Add ("type", "large");
			FB.API ("/" + id + "/picture", HttpMethod.GET, ProfilePhotoCallback, data);
//			data.Add ("type", "small");
//			FB.API ("/me/picture", HttpMethod.GET, this.ProfilePhotoCallback);
			return null;
		}

		void ProfilePhotoCallback (IGraphResult result)
		{
			if (string.IsNullOrEmpty (result.Error) && result.Texture != null)
				image = result.Texture;
			if (!string.IsNullOrEmpty (result.Error))
				Debug.LogError (id + " " + name + ":\n" + result.Error);
			isLoadingPicture = false;
			if (onImageLoaded == null)
				return;
			onImageLoaded (this);
			//
			System.Delegate[] list = onImageLoaded.GetInvocationList ();
			foreach (System.Delegate item in list) {
				onImageLoaded -= (System.Action<User>)item;
			}
		}
	}

	[System.Serializable]
	public class UsersList:DataBase
	{
		public List<User> data;

		public bool IsLoadingPicture ()
		{
			bool loading = true;
			foreach (var user in data) {
				loading = loading && user.isLoadingPicture;
			}
			return loading;
		}

		public Texture2D[] GetImages ()
		{
			if (data == null || data.Count != 0)
				return null;
			Texture2D[] textures = new Texture2D[data.Count];
			for (int i = 0, dataCount = data.Count; i < dataCount; i++) {
				var user = data [i];
				textures [i] = user.GetImage (null);
			}
			return textures;
		}
	}

	[System.Serializable]
	public class Score:DataBase
	{
		public int score = 0;
		public User user = new User ();
	}

	[System.Serializable]
	public class ScoresList:DataBase
	{
		public List<Score> data;
	}

}
#endif