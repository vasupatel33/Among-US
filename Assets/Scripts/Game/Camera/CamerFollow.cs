using UnityEngine;

public class CamerFollow : MonoBehaviour
{
	public Transform player;

	[SerializeField]
	private float smoothTime = 0.3f;

	private Vector3 velocity = Vector3.zero;

	private void Update()
	{
		if (!(player == null))
		{
			Vector3 target = player.TransformPoint(0f, 0f, -15f);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref velocity, smoothTime);
		}
	}
}
