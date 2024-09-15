using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	public void OnClickOnlineButton()
	{
		Debug.Log("Click Online");
	}

	public void OnClickQuitButton()
	{
		Application.Quit();
	}
}
