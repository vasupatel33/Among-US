using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
	[SerializeField]
	private List<Image> crewImgs;

	[SerializeField]
	private List<Button> imposterCountButtons;

	[SerializeField]
	private List<Button> maxPlayerCountButtons;

	private CreateGameRoomData roomData;

	private void Start()
	{
		for (int i = 0; i < crewImgs.Count; i++)
		{
			Material material = Object.Instantiate(crewImgs[i].material);
			crewImgs[i].material = material;
		}
		roomData = new CreateGameRoomData
		{
			imposterCount = 1,
			maxPlayerCount = 10
		};
		UpdateCrewImages();
	}

	public void UpdateImposterCount(int count)
	{
		roomData.imposterCount = count;
		for (int i = 0; i < imposterCountButtons.Count; i++)
		{
			if (i == count - 1)
			{
				imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
			}
		}
		int num = count switch
		{
			1 => 4, 
			2 => 7, 
			_ => 9, 
		};
		if (roomData.maxPlayerCount < num)
		{
			UpdateMaxPlayerCount(num);
		}
		else
		{
			UpdateMaxPlayerCount(roomData.maxPlayerCount);
		}
		for (int j = 0; j < maxPlayerCountButtons.Count; j++)
		{
			Text componentInChildren = maxPlayerCountButtons[j].GetComponentInChildren<Text>();
			if (j < num - 4)
			{
				maxPlayerCountButtons[j].interactable = false;
				componentInChildren.color = Color.gray;
			}
			else
			{
				maxPlayerCountButtons[j].interactable = true;
				componentInChildren.color = Color.white;
			}
		}
	}

	public void UpdateMaxPlayerCount(int count)
	{
		roomData.maxPlayerCount = count;
		for (int i = 0; i < maxPlayerCountButtons.Count; i++)
		{
			if (i == count - 4)
			{
				maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
			}
		}
		UpdateCrewImages();
	}

	private void UpdateCrewImages()
	{
		for (int i = 0; i < crewImgs.Count; i++)
		{
			crewImgs[i].material.SetColor("_PlayerColor", Color.white);
		}
		int num = roomData.imposterCount;
		int num2 = 0;
		while (num != 0)
		{
			if (num2 >= roomData.maxPlayerCount)
			{
				num2 = 0;
			}
			if (crewImgs[num2].material.GetColor("_PlayerColor") != Color.red && Random.Range(0, 5) == 0)
			{
				crewImgs[num2].material.SetColor("_PlayerColor", Color.red);
				num--;
			}
			num2++;
		}
		for (int j = 0; j < crewImgs.Count; j++)
		{
			if (j < roomData.maxPlayerCount)
			{
				crewImgs[j].gameObject.SetActive(value: true);
			}
			else
			{
				crewImgs[j].gameObject.SetActive(value: false);
			}
		}
	}
}
