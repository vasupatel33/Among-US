using System;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
	public FixedJoystick joystick;

	public Rigidbody2D rb;

	public Animator animator;

	private float speed = 10f;

	private Vector2 velocity;

	public static bool isAbleToMove;

	public int colorIndex;

	public CamerFollow camerFxollow;

	private void OnEnable()
	{
		if (base.photonView.IsMine)
		{
			StaticAction.action_KeyPadOpen = (Action)Delegate.Combine(StaticAction.action_KeyPadOpen, new Action(SetVelocityDefault));
		}
	}

	private void OnDisable()
	{
		if (base.photonView.IsMine)
		{
			StaticAction.action_KeyPadOpen = (Action)Delegate.Remove(StaticAction.action_KeyPadOpen, new Action(SetVelocityDefault));
		}
	}

	private void Start()
	{
		isAbleToMove = true;
		if (base.photonView.IsMine)
		{
			Debug.Log(PhotonNetwork.LocalPlayer.NickName + "Current Player = " + base.photonView.name);
			joystick = GameManager.instance.joystick;
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			velocity = Vector2.zero;
		}
	}

	private void FixedUpdate()
	{
		if (base.photonView.IsMine)
		{
			velocity.x = joystick.Horizontal;
			velocity.y = joystick.Vertical;
			rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
			SpriteRenderer component = rb.transform.GetComponent<SpriteRenderer>();
			animator.SetBool("isRun", velocity.x != 0f || velocity.y != 0f);
			component.flipX = velocity.x < 0f;
			component.sprite = GameManager.instance.AllPlayerSprite[colorIndex];
			base.photonView.RPC("SetRendererProperties", RpcTarget.All, velocity.x < 0f);
		}
	}

	private void SetVelocityDefault()
	{
		if (base.photonView.IsMine)
		{
			Debug.Log("Velocity zerpo");
			rb.velocity = Vector3.zero;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(colorIndex);
			return;
		}
		int index = (int)stream.ReceiveNext();
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		component.sprite = GameManager.instance.AllPlayerSprite[index];
	}
}
