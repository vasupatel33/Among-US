using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private TMP_InputField createRoomInputField;

	[SerializeField]
	private TMP_InputField joinRoomInputField;

	[SerializeField]
	private TMP_InputField nameInputField;

	[SerializeField]
	private TextMeshProUGUI roomIdText;

	[SerializeField]
	private Transform roomUIParent;

	[SerializeField]
	private Transform playerUIParent;

	[SerializeField]
	private GameObject loadingPanel;

	[SerializeField]
	private GameObject playerJoinedPanel;

	[SerializeField]
	private GameObject firstPanel;

	[SerializeField]
	private GameObject roomPanel;

	[SerializeField]
	private GameObject roomPrefab;

	[SerializeField]
	private GameObject playerPref;

	[SerializeField]
	private GameObject playBtn;

	[SerializeField]
	private GameObject playerObject;

	[SerializeField]
	private TextDebugger textDebugger;

	private List<GameObject> AllRoomList = new List<GameObject>();

	private List<GameObject> AllPlayerList = new List<GameObject>();

	private List<RoomInfo> availableRoomList = new List<RoomInfo>();

	private void Awake()
	{
	}

	private void Start()
	{
		RefreshRoomList();
		if (!PhotonNetwork.IsConnected)
		{
			firstPanel.SetActive(value: true);
			return;
		}
		firstPanel.SetActive(value: false);
		roomPanel.SetActive(value: true);
	}

	public void JoinRoom()
	{
		if (joinRoomInputField.text.Length == 0)
		{
			ShowDebugger("Please Enter Room Name !", Color.red);
		}
		else
		{
			PhotonNetwork.JoinRoom(joinRoomInputField.text);
		}
	}

	public void CreateRoom()
	{
		if (createRoomInputField.text.Length == 0)
		{
			ShowDebugger("Room Name is Invalid !", Color.red);
			return;
		}
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		PhotonNetwork.CreateRoom(createRoomInputField.text, roomOptions);
		Debug.Log(createRoomInputField.text);
	}

	public void UpdateRoomList(List<RoomInfo> roomList)
	{
		for (int i = 0; i < roomUIParent.transform.childCount; i++)
		{
			Object.Destroy(roomUIParent.transform.GetChild(i).gameObject);
		}
		AllRoomList.Clear();
		Debug.Log("Update Room List " + roomList.Count);
		for (int j = 0; j < roomList.Count; j++)
		{
			if (roomList[j].PlayerCount != 0)
			{
				GameObject gameObject = Object.Instantiate(roomPrefab);
				gameObject.transform.SetParent(roomUIParent);
				gameObject.GetComponent<AvailableRoomUI>().SetUI(roomList[j].Name, roomList[j].PlayerCount, roomList[j].MaxPlayers);
				Debug.Log("Room generated");
				AllRoomList.Add(gameObject);
			}
		}
		availableRoomList.Clear();
		foreach (RoomInfo room in roomList)
		{
			availableRoomList.Add(room);
		}
	}

	public void UpdatePlayerList()
	{
		for (int i = 0; i < playerUIParent.childCount; i++)
		{
			Object.Destroy(playerUIParent.GetChild(i).gameObject);
		}
		AllPlayerList.Clear();
		Player[] playerList = PhotonNetwork.PlayerList;
		Player[] array = playerList;
		foreach (Player player in array)
		{
			GameObject gameObject = Object.Instantiate(playerPref, playerUIParent.transform);
			gameObject.GetComponent<TextMeshProUGUI>().text = player.NickName;
			AllPlayerList.Add(gameObject);
		}
		if (PhotonNetwork.InRoom)
		{
			roomIdText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
		}
		playBtn.SetActive(PhotonNetwork.IsMasterClient);
	}

	public void LeaveRoom()
	{
		if (PhotonNetwork.InRoom)
		{
			if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
			{
				MigrateMaster();
				return;
			}
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
		}
	}

	private void MigrateMaster()
	{
		Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
		if (PhotonNetwork.SetMasterClient(players[players.Count - 1]))
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public void RefreshRoom()
	{
		for (int i = 0; i < playerUIParent.childCount; i++)
		{
			Object.Destroy(playerUIParent.GetChild(i).gameObject);
		}
		AllPlayerList.Clear();
		for (int j = 0; j < PhotonNetwork.CurrentRoom.PlayerCount; j++)
		{
			Player player = PhotonNetwork.PlayerList[j];
			if (player != null)
			{
				GameObject gameObject = Object.Instantiate(playerPref);
				gameObject.transform.SetParent(playerUIParent);
				AllPlayerList.Add(gameObject);
				gameObject.GetComponent<TextMeshProUGUI>().text = player.NickName;
				roomIdText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
				Debug.Log("<color=red>Player List Updated = </color>" + PhotonNetwork.CurrentRoom.Name);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	public void RefreshRoomList()
	{
		for (int i = 0; i < roomUIParent.childCount; i++)
		{
			Object.Destroy(roomUIParent.GetChild(i).gameObject);
		}
		Debug.Log(availableRoomList.Count);
		foreach (RoomInfo availableRoom in availableRoomList)
		{
			if (availableRoom.IsOpen && availableRoom.IsVisible && availableRoom.PlayerCount >= 1)
			{
				GameObject gameObject = Object.Instantiate(roomPrefab);
				gameObject.transform.SetParent(roomUIParent);
				gameObject.GetComponent<AvailableRoomUI>().SetUI(availableRoom.Name, availableRoom.PlayerCount, availableRoom.MaxPlayers);
				Debug.Log("Room generated: " + availableRoom.Name);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				AllRoomList.Add(gameObject);
			}
		}
	}

	public void PlayGame()
	{
		base.photonView.RPC("RPCLoadScene", RpcTarget.All);
	}

	[PunRPC]
	public void RPCLoadScene()
	{
		PhotonNetwork.LoadLevel(1);
	}

	public void SubmitBtnFirstPanel()
	{
		if (nameInputField.text.Length == 0)
		{
			ShowDebugger("Invalid Name Input !", Color.red);
			return;
		}
		firstPanel.SetActive(value: false);
		if (!PhotonNetwork.IsConnected)
		{
			loadingPanel.SetActive(value: true);
		}
		PhotonNetwork.NickName = nameInputField.text;
		PhotonNetwork.LocalPlayer.NickName = nameInputField.text;
		PhotonNetwork.ConnectUsingSettings();
	}

	public void ShowDebugger(string text, Color color)
	{
		textDebugger.ShowDebugger(text, color);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
		ShowDebugger("Room Created Successfully", Color.green);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("Room Player Entered");
		UpdatePlayerList();
		ShowDebugger(newPlayer?.ToString() + " Entered in the Room", Color.green);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		ShowDebugger(otherPlayer?.ToString() + " Left the Game", Color.red);
		Debug.Log("<color=red>Player Lefted </color>" + otherPlayer.NickName);
		foreach (Transform item in playerUIParent.transform)
		{
			if (item.GetComponent<TextMeshProUGUI>().text == otherPlayer.NickName)
			{
				Object.Destroy(item.gameObject);
				break;
			}
		}
	}

	public override void OnLeftRoom()
	{
		PhotonNetwork.LoadLevel("Menu");
		Debug.Log("Left room");
		UpdatePlayerList();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("<b>Room Updated = </b>" + roomList.Count);
		MenuManager.instance.UpdateRoom(roomList);
		UpdateRoomList(roomList);
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected with Master");
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("lobby joined");
		loadingPanel.SetActive(value: false);
		roomPanel.SetActive(value: true);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Room joined =" + PhotonNetwork.CurrentRoom.Name);
		playerJoinedPanel.SetActive(value: true);
		roomPanel.SetActive(value: false);
		UpdatePlayerList();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		PhotonNetwork.SetMasterClient(newMasterClient);
	}
}
