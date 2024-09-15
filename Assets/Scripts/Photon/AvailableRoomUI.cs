using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailableRoomUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI roomName;

	[SerializeField]
	private TextMeshProUGUI playerCount;

	[SerializeField]
	private Button joinBtn;

	public void SetUI(string roomNameString, int playerCountInt, int maxPlayerCount)
	{
		roomName.text = roomNameString;
		playerCount.text = playerCountInt + "/" + maxPlayerCount;
	}

	public void JoinCreatedRoom()
	{
		foreach (RoomInfo allRoom in MenuManager.instance.AllRooms)
		{
			Debug.Log(allRoom.Name + "--------------" + roomName.text);
			if (roomName.text == allRoom.Name)
			{
				Debug.Log(allRoom.PlayerCount + "--------------" + allRoom.MaxPlayers);
				if (allRoom.PlayerCount < allRoom.MaxPlayers)
				{
					PhotonNetwork.JoinRoom(roomName.text);
				}
				else
				{
					PhotonNetwork.CreateRoom("test " + Random.Range(0, 100));
				}
			}
		}
	}
}
