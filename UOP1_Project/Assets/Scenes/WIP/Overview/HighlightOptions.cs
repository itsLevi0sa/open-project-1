using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightOptions : MonoBehaviour
{
	public List<GameObject> dots = new List<GameObject>();
	//public SpriteState spriteState = new SpriteState();

	private MultiInputButton _multiInputButton;

	// Start is called before the first frame update
	void Start()
    {
		_multiInputButton = this.gameObject.GetComponent<MultiInputButton>();

	}

    // Update is called once per frame
    void Update()
    {
        if (_multiInputButton.isHighlighted == true)
		{
			foreach (GameObject obj in dots)
			{
				Image imgToChange = obj.gameObject.transform.GetChild(0).GetComponent<Image>();
				SpriteState spriteState = obj.GetComponent<Button>().spriteState;
				Sprite highlightedButtonState = obj.GetComponent<Button>().spriteState.highlightedSprite;
				spriteState.highlightedSprite = highlightedButtonState;
				imgToChange.sprite = highlightedButtonState;
			}
		}
		else
		{
			foreach (GameObject obj in dots)
			{
				Image imgToChange = obj.gameObject.transform.GetChild(0).GetComponent<Image>();
				SpriteState spriteState = obj.GetComponent<Button>().spriteState;
				Sprite disabledButtonState = obj.GetComponent<Button>().spriteState.disabledSprite;
				spriteState.disabledSprite = disabledButtonState;
				imgToChange.sprite = disabledButtonState;
			}
		}
    }
}
