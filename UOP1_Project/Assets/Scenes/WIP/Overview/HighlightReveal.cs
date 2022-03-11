using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightReveal : MonoBehaviour
{
	public List<GameObject> objectsToReveal = new List<GameObject>();
	public List<GameObject> objectsToHide = new List<GameObject>();
	//public SpriteState spriteState = new SpriteState();

	private CustomInputButton _customInputButton;

	// Start is called before the first frame update
	void Start()
    {
		_customInputButton = this.gameObject.GetComponent<CustomInputButton>();
	}

    // Update is called once per frame
    void Update()
    {
        if (_customInputButton.isHighlighted == true)
		{
			
			foreach (GameObject obj2 in objectsToHide)
			{
				obj2.SetActive(false);
			}
			foreach (GameObject obj in objectsToReveal)
			{
				obj.SetActive(true);
			}
		}
		else
		{
			foreach (GameObject obj in objectsToReveal)
			{
				obj.SetActive(false);
			}
			foreach (GameObject obj2 in objectsToHide)
			{
				obj2.SetActive(true);
			}
		}
    }
}
