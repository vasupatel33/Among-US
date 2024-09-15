using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
	public delegate void VentButtonClicked();

	[SerializeField]
	private GameObject playerPrefab;

	[SerializeField]
	private GameObject KeyPadTaskPanel;

	[SerializeField]
	private GameObject WireTaskPanel;

	[SerializeField]
	private GameObject chatPanel;

	[SerializeField]
	private GameObject YouHaveBeenKilledPanel;

	public FixedJoystick joystick;

	private GameObject currentPlayer;

	public static GameManager instance;

	public Button killBtn;

	public Button useBtn;

	public Button ventBtn;

	public bool hasTarget;

	public Killable currentPlayerKillable;

	public MasterClient masterClient;

	private static int nameCount;

	public bool hasInteractabe;

	public Interactble currentInteractable;

	public ChatWindowUI chatWindowUI;

	public Vent vent;

	public List<Sprite> AllPlayerSprite;

	private int localColorIndex;

	public Action action_OnPlayerVent;

	public CamerFollow camerFollow;

	private HashSet<int> usedIndices = new HashSet<int>();

	private bool isAbleToVent = false;

	public static event VentButtonClicked OnVentButtonClicked;

	private void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
		nameCount = 0;
		InstantiatePlayer();
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log("<color=red>Master CLient Called Methiod</color>");
			masterClient.Initialize();
		}
	}

	private void OnDisable()
	{
		DestroyPlayer();
	}

	private void FixedUpdate()
	{
		if (currentPlayerKillable != null)
		{
			killBtn.transform.gameObject.SetActive(currentPlayerKillable.isImpostar);
		}
		killBtn.interactable = hasTarget;
		useBtn.interactable = hasInteractabe;
	}

	public void OnKillBtnPressed()
	{
		if (!(currentPlayerKillable == null))
		{
			currentPlayerKillable.Kill();
		}
	}

	public void OnUseBtnPressed()
	{
		if (!(currentInteractable == null))
		{
			currentInteractable.Use(isActive: true);
		}
	}

	private void InstantiatePlayer()
	{
		DestroyPlayer();
		currentPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
		currentPlayer.name = "Player" + nameCount;
		nameCount++;
		PlayerMovement component = currentPlayer.GetComponent<PlayerMovement>();
		component.colorIndex = RandomSpriteSelector();
		chatWindowUI.player = component;
		camerFollow.player = currentPlayer.transform;
	}

	public int RandomSpriteSelector()
	{
		if (usedIndices.Count == AllPlayerSprite.Count)
		{
			Debug.LogWarning("All sprites have been used. Resetting...");
			usedIndices.Clear();
		}
		int num;
		do
		{
			num = UnityEngine.Random.Range(0, AllPlayerSprite.Count);
		}
		while (usedIndices.Contains(num));
		usedIndices.Add(num);
		Debug.Log("<color=yellow>Color Index = </color>" + num);
		return num;
	}

	private void DestroyPlayer()
	{
		if (currentPlayer != null)
		{
			PhotonNetwork.Destroy(currentPlayer);
			currentPlayer = null;
		}
	}

	public void HomeBtnClicked()
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}

	public void KeyPadTaskPanelOpen()
	{
		KeyPadTaskPanel.SetActive(value: true);
		PlayerMovement.isAbleToMove = false;
	}

	public void KeyPadTaskPanelClose()
	{
		KeyPadTaskPanel.SetActive(value: false);
		PlayerMovement.isAbleToMove = true;
	}

	public void WireTaskPanelOpen()
	{
		WireTaskPanel.SetActive(value: true);
		PlayerMovement.isAbleToMove = false;
	}

	public void WireTaskPanelClose()
	{
		WireTaskPanel.SetActive(value: false);
		PlayerMovement.isAbleToMove = true;
	}

	public void ChatPanelOpen()
	{
		chatPanel.SetActive(value: true);
	}

	public void ChatPanelClose()
	{
		chatPanel.SetActive(value: false);
	}

	public void OnThisPlayerKilled()
	{
		Debug.Log("Playe Killed");
		YouHaveBeenKilledPanel.SetActive(value: true);
	}

	public void OnHomeBtnClicked()
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
	}

	public void CheckingVentBtn(bool isOn)
	{
		ventBtn.interactable = isOn;
	}

	public void OnVentBtnClicked()
	{
		GameManager.OnVentButtonClicked?.Invoke();
	}
}
