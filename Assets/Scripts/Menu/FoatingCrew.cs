using UnityEngine;

public class FoatingCrew : MonoBehaviour
{
	public EPlayerColor playerColor;

	private SpriteRenderer spriteRenderer;

	private Vector3 direction;

	private float floatingSpeed;

	private float rotateSpeed;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetFloatingCrew(Sprite sprite, EPlayerColor playerColor, Vector3 direction, float floatingSpeed, float rotateSpeed, float size)
	{
		this.playerColor = playerColor;
		this.direction = direction;
		this.floatingSpeed = floatingSpeed;
		this.rotateSpeed = rotateSpeed;
		spriteRenderer.sprite = sprite;
		spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
		base.transform.localScale = new Vector3(size, size, size);
		spriteRenderer.sortingOrder = (int)Mathf.Lerp(1f, 32767f, size);
	}

	private void Update()
	{
		base.transform.position += direction * floatingSpeed * Time.deltaTime;
		base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
	}
}
