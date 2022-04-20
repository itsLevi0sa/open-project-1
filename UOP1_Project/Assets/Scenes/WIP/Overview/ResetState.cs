using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : MonoBehaviour
{
	public List<GameObject> itemsToReset = new List<GameObject>();
	public GameObject dropdownMenu;
	bool isReset = true;
	bool isDropdownShown = false;
	bool hasSelectedFromDropdown = false;

	public void ResetButton()
	{
		foreach (GameObject item in itemsToReset)
		{
			item.SetActive(false);
		}
		dropdownMenu.SetActive(false);
		isReset = true;
		isDropdownShown = false;
		hasSelectedFromDropdown = false;
	}

	private void ShowDropdownMenu()
	{
		foreach (GameObject item in itemsToReset)
		{
			item.SetActive(false);
		}
		dropdownMenu.SetActive(true);
		isDropdownShown = true;
		isReset = false;
		hasSelectedFromDropdown = false;
	}

	public void HasSelectedOption()
	{
		hasSelectedFromDropdown = true;
	}

	public void ButtonSelected()
	{
		if (isReset == true || hasSelectedFromDropdown == true)
		{
			ShowDropdownMenu();
		}else if (isDropdownShown == true)
		{
			ResetButton();
		}
	}
}
