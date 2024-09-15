using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Killable : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private float _range = 3f;

	private LineRenderer _lineRenderer;

	private Killable _target;

	public bool isImpostar = false;

	private void Awake()
	{
		if (base.photonView.IsMine)
		{
			_lineRenderer = GetComponent<LineRenderer>();
			StartCoroutine(SearchForKillable());
		}
	}

	private void Start()
	{
		if (base.photonView.IsMine)
		{
			GameManager.instance.currentPlayerKillable = this;
		}
	}

	private void Update()
	{
		if (base.photonView.IsMine)
		{
			if (_target != null && isImpostar)
			{
				_lineRenderer.SetPosition(0, base.transform.position);
				_lineRenderer.SetPosition(1, _target.transform.position);
			}
			else
			{
				_lineRenderer.SetPosition(0, Vector3.zero);
				_lineRenderer.SetPosition(1, Vector3.zero);
			}
		}
	}

	private IEnumerator SearchForKillable()
	{
		while (true)
		{
			Killable newTarget = null;
			Killable[] killlist = Object.FindObjectsOfType<Killable>();
			GameManager.instance.hasTarget = false;
			Killable[] array = killlist;
			foreach (Killable kill in array)
			{
				if (!(kill == this))
				{
					float distance = Vector3.Distance(base.transform.position, kill.transform.position);
					if (!(distance > _range))
					{
						Debug.Log("Target = " + GameManager.instance.hasTarget);
						newTarget = kill;
						GameManager.instance.hasTarget = true;
						break;
					}
				}
			}
			_target = newTarget;
			yield return new WaitForSeconds(0.25f);
		}
	}

	public void Kill()
	{
		if (!(_target == null))
		{
			PhotonView component = _target.GetComponent<PhotonView>();
			component.RPC("KillRPC", RpcTarget.All);
		}
	}

	[PunRPC]
	public void KillRPC()
	{
		if (base.photonView.IsMine)
		{
			Debug.Log("PLayer KILLED");
			PlayerDeadBody component = PhotonNetwork.Instantiate("PlayerDeadBody", base.transform.position, Quaternion.identity, 0).GetComponent<PlayerDeadBody>();
			PlayerMovement component2 = base.transform.GetComponent<PlayerMovement>();
			base.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0f);
			PhotonNetwork.Destroy(base.photonView);
			GameManager.instance.OnThisPlayerKilled();
		}
	}

	[PunRPC]
	public void SetImpostar()
	{
		isImpostar = true;
		Debug.Log("<color=red>Imposter Setted</color>" + PhotonNetwork.LocalPlayer.NickName);
	}

	[PunRPC]
	public void SetRendererProperties(bool isOpen)
	{
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		component.flipX = isOpen;
	}
}
