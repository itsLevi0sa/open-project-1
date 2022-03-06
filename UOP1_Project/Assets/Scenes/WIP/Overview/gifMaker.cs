using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gifMaker : MonoBehaviour
{
	public Sprite[] frames;
	public Image explosion;
	public float frameRate = 0.1f;

	private int currentImage;
	// Use this for initialization
	void OnEnable()
	{
		currentImage = 0;
		InvokeRepeating("ChangeImage", 0.1f, frameRate);
	}
	private void OnDisable()
	{
		CancelInvoke("ChangeImage");
	}
	private void ChangeImage()
	{
		if (currentImage == frames.Length - 1)
		{
			currentImage = 0;
		}
		currentImage += 1;
		explosion.sprite = frames[currentImage];
	}
}
