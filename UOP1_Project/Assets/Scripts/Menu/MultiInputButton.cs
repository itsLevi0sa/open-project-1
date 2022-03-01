using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An extension of Unity's base Button class, to support input from both mouse and keyboard/joypad
/// </summary>
[AddComponentMenu("UOP1/UI/MultiInputButton")]
public class MultiInputButton : Button
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

	public override void OnSelect(BaseEventData eventData)
	{
		IsSelected = true;
		_menuSelectionHandler.UpdateSelection(gameObject);
		base.OnSelect(eventData);
	}

	public void UpdateSelected()
	{
		if (_menuSelectionHandler == null)
			_menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();

		_menuSelectionHandler.UpdateSelection(gameObject);
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		if (_menuSelectionHandler.AllowsSubmit())
			base.OnSubmit(eventData);
	}


}
