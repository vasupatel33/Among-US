using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;

	public List<RoomInfo> AllRooms = new List<RoomInfo>();

	private void Awake()
	{
		instance = this;
	}

	public void UpdateRoom(List<RoomInfo> room)
	{
		AllRooms.Clear();
		AllRooms.AddRange(room);
	}
}
