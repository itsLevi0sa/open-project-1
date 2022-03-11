using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightChildren : MonoBehaviour
{
	public GameObject objectToEnable;
	public List<GameObject> clips;
	public GameObject objectToDisable;
	private bool hasRevealedObject = false;

	private void Start()
	{
		objectToEnable.SetActive(true);
		HideClip();
	}
	// Update is called once per frame
	void Update()
    {
		if (objectToDisable.gameObject.GetComponent<CustomInputButton>().isHighlighted == true)
		{
			objectToDisable.GetComponent<CustomInputButton>().enabled = false;
			RevealClip();
		}
		if (hasRevealedObject == true)
		{
			if (objectToEnable.gameObject.GetComponentInChildren<CustomInputButton>().isHighlighted == true)
			{
				//do nothing
			}
			if (objectToEnable.gameObject.GetComponentInChildren<CustomInputButton>().isHighlighted == false)
			{
				hasRevealedObject = false;
			}
		}else if (hasRevealedObject == false)
		{
			HideClip();
		}

	}

	private void RevealClip()
	{
		hasRevealedObject = true;
		objectToDisable.gameObject.GetComponent<CustomInputButton>().isHighlighted = false;
		foreach (GameObject clip in clips)
		{
			clip.SetActive(true);
		}
		objectToDisable.GetComponent<CustomInputButton>().enabled = false;
		objectToEnable.gameObject.GetComponentInChildren<CustomInputButton>().enabled = true;
		
		foreach (Transform child in objectToEnable.transform)
		{
			child.gameObject.GetComponent<Image>().enabled = true;
		}
	
	}

	private void HideClip()
	{
		foreach (Transform child in objectToEnable.transform)
		{
			child.gameObject.GetComponent<Image>().enabled = false;			
		}
		foreach (GameObject clip in clips)
		{
			clip.SetActive(false);
		}
		objectToEnable.gameObject.GetComponentInChildren<CustomInputButton>().enabled = false;
		objectToDisable.GetComponent<CustomInputButton>().enabled = true;
	}
}
