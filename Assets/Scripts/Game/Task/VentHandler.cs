using Photon.Pun;
using UnityEngine;

public class VentHandler : MonoBehaviourPun
{
	private bool isAbleToVent = false;

	private void Start()
	{
		GameManager.OnVentButtonClicked += OnVentButtonClicked;
	}

	private void OnDestroy()
	{
		GameManager.OnVentButtonClicked -= OnVentButtonClicked;
	}

	private void OnVentButtonClicked()
	{
		Debug.Log("Vent Button clickeddddd   " + base.photonView.name);
		if (base.photonView.IsMine)
		{
			base.photonView.RPC("ToggleVentState", RpcTarget.All, !isAbleToVent);
		}
	}

	[PunRPC]
	private void ToggleVentState(bool newState)
	{
		isAbleToVent = newState;
		if (isAbleToVent)
		{
			base.transform.GetComponent<SpriteRenderer>().enabled = false;
			base.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			base.transform.GetComponent<Killable>().enabled = false;
		}
		else
		{
			base.transform.GetComponent<SpriteRenderer>().enabled = true;
			base.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			base.transform.GetComponent<Killable>().enabled = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (base.photonView.IsMine && collision.gameObject.CompareTag("Vent"))
		{
			Debug.Log("Vent Triggered = " + collision.gameObject.name);
			GameManager.instance.CheckingVentBtn(isOn: true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (base.photonView.IsMine && collision.gameObject.CompareTag("Vent"))
		{
			Debug.Log("Vent Exitted = " + collision.gameObject.name);
			GameManager.instance.CheckingVentBtn(isOn: false);
		}
	}
}
