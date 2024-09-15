using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewFloater : MonoBehaviour
{
	[SerializeField]
	private GameObject prefabs;

	[SerializeField]
	private List<Sprite> sprites;

	private bool[] crewStates = new bool[12];

	private float timer = 0.5f;

	private float distance = 6f;

	private void Start()
	{
		for (int i = 0; i < 12; i++)
		{
			SpawnFloatingCrew((EPlayerColor)i, UnityEngine.Random.Range(0f, distance));
		}
	}

	private void Update()
	{
		timer -= Time.fixedDeltaTime;
		if (timer <= 0f)
		{
			int playerColor = UnityEngine.Random.Range(0, 12);
			SpawnFloatingCrew((EPlayerColor)playerColor, distance);
			timer = 0.5f;
		}
	}

	public void SpawnFloatingCrew(EPlayerColor playerColor, float dist)
	{
		if (!crewStates[(int)playerColor])
		{
			crewStates[(int)playerColor] = true;
			float f = UnityEngine.Random.Range(0f, 360f) * MathF.PI / 180f;
			Vector3 position = new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0f) * dist;
			Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f);
			float floatingSpeed = UnityEngine.Random.Range(1f, 4f);
			float rotateSpeed = UnityEngine.Random.Range(-1f, 1f);
			GameObject gameObject = UnityEngine.Object.Instantiate(prefabs, position, Quaternion.identity);
			gameObject.GetComponent<FoatingCrew>().SetFloatingCrew(sprites[UnityEngine.Random.Range(0, sprites.Count)], playerColor, direction, floatingSpeed, rotateSpeed, UnityEngine.Random.Range(0.5f, 1f));
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		FoatingCrew component = collision.GetComponent<FoatingCrew>();
		if (component != null)
		{
			crewStates[(int)component.playerColor] = false;
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}
}
