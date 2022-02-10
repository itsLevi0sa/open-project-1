using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocationsManager : MonoBehaviour
{
	/*
	[SerializeField] private UIPopup _popupPanel = default;
	[SerializeField] private UILocationsMenu _locationsMenuPanel = default;

	[SerializeField] private InputReader _inputReader = default;


	[Header("Broadcasting on")]
	[SerializeField] private VoidEventChannelSO _loadBeachLocation = default;
	[SerializeField] private VoidEventChannelSO _loadHillsLocation = default;
	[SerializeField] private VoidEventChannelSO _loadFarmsLocation = default;
	[SerializeField] private VoidEventChannelSO _loadTownLocation = default;
	[SerializeField] private VoidEventChannelSO _loadForestLocation = default;
	[SerializeField] private VoidEventChannelSO _loadMountainLocation = default;

	private IEnumerator Start()
	{
		_inputReader.EnableMenuInput();
		yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded 
		SetMenuScreen();
	}
	void SetMenuScreen()
	{
		_locationsMenuPanel.BeachButtonAction += ShowBeachLocationLoadConfirmationPopup;
		_locationsMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
		_locationsMenuPanel.SettingsButtonAction += OpenSettingsScreen;
		_locationsMenuPanel.CreditsButtonAction += OpenCreditsScreen;
		_locationsMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;

	}

	void ButtonBeachClicked()
	{
		ShowBeachLocationLoadConfirmationPopup();
	}

	void ConfirmBeachLocationLoading()
	{
		_loadBeachLocation.RaiseEvent();
	}

	void ShowBeachLocationLoadConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += LoadBeachLocationPopupResponse;
		_popupPanel.ClosePopupAction += HidePopup;

		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.LocationLoad);

	}

	void LoadBeachLocationPopupResponse(bool loadBeachLocationConfirmed)
	{

		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		_popupPanel.ClosePopupAction -= HidePopup;

		_popupPanel.gameObject.SetActive(false);

		if (startNewGameConfirmed)
		{
			ConfirmStartNewGame();
		}
		else
		{
			_continueGameEvent.RaiseEvent();
		}

		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}

	void HidePopup()
	{
		_popupPanel.ClosePopupAction -= HidePopup;
		_popupPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}


	public void ShowExitConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.Quit);



	}
	void HideExitConfirmationPopup(bool quitConfirmed)
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(false);
		if (quitConfirmed)
		{
			Application.Quit();
		}
		_mainMenuPanel.SetMenuScreen(_hasSaveData);


	}
	private void OnDestroy()
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;

	}
	*/

}
