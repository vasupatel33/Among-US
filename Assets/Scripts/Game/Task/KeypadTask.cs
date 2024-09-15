using System.Collections;
using TMPro;
using UnityEngine;

public class KeypadTask : MonoBehaviour
{
	public TextMeshProUGUI _cardCode;

	public TMP_InputField _inputCode;

	public int _cardLength = 5;

	public float _codeResetTimeInSeconds = 0.5f;

	private bool _isResetting = false;

	private void OnEnable()
	{
		GenerateRandomCode();
	}

	public void GenerateRandomCode()
	{
		string text = string.Empty;
		for (int i = 0; i < _cardLength; i++)
		{
			text += Random.Range(1, 10);
		}
		_cardCode.text = text;
		_inputCode.text = string.Empty;
	}

	public void ButtonClick(int number)
	{
		if (!_isResetting)
		{
			_inputCode.text += number;
			if (_inputCode.text == _cardCode.text)
			{
				_inputCode.text = "Correct";
				StartCoroutine(ResetCode());
				GenerateRandomCode();
			}
			else if (_inputCode.text.Length > _cardLength)
			{
				_inputCode.text = "failed";
				StartCoroutine(ResetCode());
			}
		}
	}

	private IEnumerator ResetCode()
	{
		_isResetting = true;
		yield return new WaitForSeconds(_codeResetTimeInSeconds);
		_inputCode.text = string.Empty;
		_isResetting = false;
	}
}
