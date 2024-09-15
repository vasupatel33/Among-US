using System.Collections;
using TMPro;
using UnityEngine;

public class TextDebugger : MonoBehaviour
{
	public TextMeshProUGUI debugLabel;

	public float activeTime;

	private Coroutine coroutine;

	public bool isActive;

	public void ShowDebugger(string text, Color color)
	{
		base.transform.GetChild(0).gameObject.SetActive(value: true);
		isActive = true;
		debugLabel.SetText(text);
		debugLabel.color = color;
		coroutine = StartCoroutine(CloseDebugger());
	}

	private IEnumerator CloseDebugger()
	{
		yield return new WaitForSeconds(activeTime);
		base.transform.GetChild(0).gameObject.SetActive(value: false);
		isActive = false;
	}

	public void DeactivateDebugger()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		base.transform.GetChild(0).gameObject.SetActive(value: false);
		isActive = false;
	}
}
