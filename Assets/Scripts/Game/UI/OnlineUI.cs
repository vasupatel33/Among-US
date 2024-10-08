using UnityEngine;
using UnityEngine.UI;

public class OnlineUI : MonoBehaviour
{
	[SerializeField]
	private InputField nicknameInputField;

	[SerializeField]
	private GameObject createRoomUI;

	public void OnClickCreateRoomButton()
	{
		if (nicknameInputField.text != "")
		{
			PlayerSettings.nickname = nicknameInputField.text;
			createRoomUI.SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}
		else
		{
			nicknameInputField.GetComponent<Animator>().SetTrigger("on");
		}
	}
}
