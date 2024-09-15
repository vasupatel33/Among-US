using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MasterClient : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject impostarWindow;

	[SerializeField]
	private TextMeshProUGUI impostarText;

	public List<int> playerIndex = new List<int>();

	public void Initialize()
	{
		StartCoroutine(PickImpostar());
		Debug.Log("<color=blue>Impostar Intitlized</color>");
	}

	private IEnumerator PickImpostar()
	{
		int tries = 0;
		GameObject[] players;
		do
		{
			players = GameObject.FindGameObjectsWithTag("Player");
			tries++;
			yield return new WaitForSeconds(0.25f);
		}
		while (players.Length < PhotonNetwork.PlayerList.Length && tries < 5);
		Debug.Log("<color=yellow>Total Length = </color>" + players.Length);
		for (int i = 0; i < players.Length; i++)
		{
			playerIndex.Add(i);
		}
		int imposterNumber = 1;
		int imposterNumberFinal = imposterNumber;
		while (imposterNumber > 0)
		{
			int pickedImposterIndex = playerIndex[Random.Range(0, playerIndex.Count)];
			playerIndex.Remove(pickedImposterIndex);
			PhotonView pv = players[pickedImposterIndex].GetComponent<PhotonView>();
			pv.RPC("SetImpostar", RpcTarget.All);
			imposterNumber--;
		}
		base.photonView.RPC("ImpostarPicked", RpcTarget.All, imposterNumberFinal);
	}

	[PunRPC]
	public void ImpostarPicked(int number)
	{
		StartCoroutine(ImpostarAnimation(number));
	}

	private IEnumerator ImpostarAnimation(int impostarNumber)
	{
		impostarWindow.SetActive(value: true);
		impostarText.gameObject.SetActive(value: true);
		impostarText.text = "There is " + impostarNumber + " imposter Available Among us";
		yield return new WaitForSeconds(2f);
		impostarWindow.SetActive(value: false);
	}
}
