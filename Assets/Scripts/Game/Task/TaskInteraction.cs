using System.Collections;
using Photon.Pun;
using UnityEngine;

public class TaskInteraction : MonoBehaviourPun
{
	[SerializeField]
	private float _range = 1f;

	private LineRenderer _lineRenderer;

	private Interactble _target;

	private void Awake()
	{
		if (base.photonView.IsMine)
		{
			_lineRenderer = GetComponent<LineRenderer>();
			StartCoroutine(SearchForKillable());
		}
	}

	private void Update()
	{
		if (base.photonView.IsMine)
		{
			if (_target != null)
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
			Interactble newTarget = null;
			Interactble[] killlist = Object.FindObjectsOfType<Interactble>();
			GameManager.instance.hasInteractabe = false;
			Interactble[] array = killlist;
			foreach (Interactble kill in array)
			{
				if (!(kill == this))
				{
					float distance = Vector3.Distance(base.transform.position, kill.transform.position);
					if (!(distance > _range))
					{
						newTarget = kill;
						GameManager.instance.hasInteractabe = true;
						break;
					}
				}
			}
			if (GameManager.instance.currentInteractable != newTarget && GameManager.instance.currentInteractable != null)
			{
				GameManager.instance.currentInteractable.Use(isActive: false);
			}
			_target = newTarget;
			GameManager.instance.currentInteractable = _target;
			yield return new WaitForSeconds(0.25f);
		}
	}
}
