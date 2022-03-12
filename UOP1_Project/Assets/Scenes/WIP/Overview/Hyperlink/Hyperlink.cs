using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlink : MonoBehaviour
{
	//public string linkURL;

    public void OpenLink(string url)
	{
		Application.OpenURL(url);
	}

	public void OpenSwimmingAnimationAssetStoreLink()
	{
		Application.OpenURL("https://assetstore.unity.com/packages/3d/animations/realistic-swimming-animations-119979");
	}
}
