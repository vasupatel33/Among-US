using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTask : MonoBehaviour
{
	public List<Color> _wireColors = new List<Color>();

	public List<Wire> _leftWire = new List<Wire>();

	public List<Wire> _rightWire = new List<Wire>();

	public List<Color> _availableColors;

	public List<int> _availableLeftWireIndex;

	public List<int> _availableRightWireIndex;

	public Wire CurrentSelectedWire;

	public Wire CurrentHoveredWire;

	private bool isTaskCompleted;

	private void OnEnable()
	{
		_availableColors = new List<Color>(_wireColors);
		_availableLeftWireIndex = new List<int>();
		_availableRightWireIndex = new List<int>();
		for (int i = 0; i < _leftWire.Count; i++)
		{
			_availableLeftWireIndex.Add(i);
			_leftWire[i].Initialize();
		}
		for (int j = 0; j < _rightWire.Count; j++)
		{
			_availableRightWireIndex.Add(j);
			_rightWire[j].Initialize();
		}
		while (_availableColors.Count > 0 && _availableLeftWireIndex.Count > 0 && _availableRightWireIndex.Count > 0)
		{
			Color color = _availableColors[Random.Range(0, _availableColors.Count)];
			int index = Random.Range(0, _availableLeftWireIndex.Count);
			int index2 = Random.Range(0, _availableRightWireIndex.Count);
			_leftWire[_availableLeftWireIndex[index]].SetColor(color);
			_rightWire[_availableRightWireIndex[index2]].SetColor(color);
			_availableColors.Remove(color);
			_availableLeftWireIndex.RemoveAt(index);
			_availableRightWireIndex.RemoveAt(index2);
		}
		StartCoroutine(CheckWire());
	}

	private IEnumerator CheckWire()
	{
		while (!isTaskCompleted)
		{
			int successfullWires = 0;
			for (int i = 0; i < _rightWire.Count; i++)
			{
				if (_rightWire[i]._isSuccess)
				{
					successfullWires++;
				}
			}
			if (successfullWires >= _rightWire.Count)
			{
				Debug.Log("TASK COMPLETED = " + successfullWires);
				ResetLinePosition();
			}
			else
			{
				Debug.Log("TASK INCOMPLETE = " + successfullWires);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	public void ResetLinePosition()
	{
		for (int i = 0; i < _leftWire.Count; i++)
		{
			_leftWire[i].transform.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
			_leftWire[i].transform.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
		}
		for (int j = 0; j < _rightWire.Count; j++)
		{
			_rightWire[j].transform.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
			_rightWire[j].transform.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
		}
		for (int k = 0; k < _rightWire.Count; k++)
		{
			_rightWire[k]._isSuccess = false;
		}
		for (int l = 0; l < _leftWire.Count; l++)
		{
			_leftWire[l]._isSuccess = false;
		}
	}
}
