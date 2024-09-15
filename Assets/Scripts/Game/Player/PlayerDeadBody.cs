using UnityEngine;

public class PlayerDeadBody : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _bodyFill;

	public void SetColor(Color color)
	{
		_bodyFill.color = color;
	}
}
