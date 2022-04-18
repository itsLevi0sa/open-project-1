using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionButton : MonoBehaviour
{
	public Transform repositionTransform;
	Transform currentTransform;

	public void Reposition()
	{
		this.transform.position = repositionTransform.position;
	}
}
