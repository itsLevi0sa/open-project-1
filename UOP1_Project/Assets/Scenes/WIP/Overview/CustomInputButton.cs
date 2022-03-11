using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CustomInputButton : Button
{
	[ReadOnly] public bool IsSelected;
	public bool isHighlighted = false;

	private MenuSelectionHandler _menuSelectionHandler;

	private new void Awake()
	{
		_menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseEnter(gameObject);
		isHighlighted = true;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseExit(gameObject);
		isHighlighted = false;
	}


}
