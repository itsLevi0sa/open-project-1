using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteraction : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("highlight!");
	}
}
