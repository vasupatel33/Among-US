using UnityEngine;

public class Interactble : MonoBehaviour
{
	[SerializeField]
	private GameObject _taskWindow;

	public void Use(bool isActive)
	{
		if (base.transform.gameObject.name != "FirstVent")
		{
			Debug.Log("Interactable object = " + base.transform.gameObject.name);
			_taskWindow.SetActive(isActive);
		}
		else
		{
			Debug.Log("Vent interactable = " + base.transform.gameObject.name);
			GameManager.instance.OnVentBtnClicked();
		}
	}
}
