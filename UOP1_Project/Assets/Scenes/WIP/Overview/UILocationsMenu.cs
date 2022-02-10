using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILocationsMenu : MonoBehaviour
{
	[SerializeField] private Button _BeachButton = default;
	[SerializeField] private Button _HillsButton = default;
	[SerializeField] private Button _FarmsButton = default;
	[SerializeField] private Button _TownButton = default;
	[SerializeField] private Button _ForestButton = default;
	[SerializeField] private Button _MountainButton = default;

	public UnityAction BeachButtonAction;
	public UnityAction HillsButtonAction;
	public UnityAction FarmsButtonAction;
	public UnityAction TownButtonAction;
	public UnityAction ForestButtonAction;
	public UnityAction MountainButtonAction;

	public void BeachButton()
	{
		BeachButtonAction.Invoke();
	}

	public void HillsButton()
	{
		HillsButtonAction.Invoke();
	}

	public void FarmsButton()
	{
		FarmsButtonAction.Invoke();
	}

	public void TownButton()
	{
		TownButtonAction.Invoke();
	}

	public void ForestButton()
	{
		ForestButtonAction.Invoke();
	}
	public void MountainButton()

	{
		MountainButtonAction.Invoke();
	}
}
