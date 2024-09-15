using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItemUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI chatText;

	[SerializeField]
	private TextMeshProUGUI playerName;

	[SerializeField]
	private Image image;

	public void Initialized(string text, string name)
	{
		chatText.text = text;
		playerName.text = name;
	}
}
