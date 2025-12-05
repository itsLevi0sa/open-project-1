using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
	private Renderer renderer;
	private Color matColor;

	// Start is called before the first frame update
	void Start()
	{
		renderer = GetComponent<Renderer>();
		matColor = renderer.material.color;
	}

	[ContextMenu("HighlightColor")]
	public void HighlightColor()
	{
		renderer.material.color = Color.red;
	}

	public void DefaultColor()
	{
		renderer.material.color = matColor;
	}
}
