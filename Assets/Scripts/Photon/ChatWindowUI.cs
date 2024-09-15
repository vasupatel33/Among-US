using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatWindowUI : MonoBehaviourPun
{
	[SerializeField]
	private ChatItemUI chatItemUI;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private TMP_InputField inputField;

	public PlayerMovement player;

	private void OnEnable()
	{
		inputField.text = string.Empty;
		inputField.ActivateInputField();
	}

	private void GenerateChat(string text, string name)
	{
		ChatItemUI chatItemUI = Object.Instantiate(this.chatItemUI);
		chatItemUI.transform.SetParent(content.transform);
		chatItemUI.transform.localPosition = Vector3.zero;
		chatItemUI.transform.localScale = Vector3.one;
		chatItemUI.Initialized(text, name);
	}

	public void SendMessage()
	{
		if (!string.IsNullOrEmpty(inputField.text) && !(player == null))
		{
			base.photonView.RPC("ReceiveMessageRPC", RpcTarget.All, inputField.text, PhotonNetwork.LocalPlayer.NickName);
			inputField.text = string.Empty;
			inputField.ActivateInputField();
		}
	}

	[PunRPC]
	public void ReceiveMessageRPC(string text, string playername)
	{
		GenerateChat(text, playername);
	}
}
