using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
	public bool isVisible;

    public void ToggleObjectVisibility()
	{
		if (isVisible == true)
		{
			this.gameObject.SetActive(true);
		}
		else
		{
			this.gameObject.SetActive(false);
		}
		isVisible =! isVisible;
	}
}
