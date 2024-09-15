using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	public bool isLeftWire;

	private Color customColor;

	private Image _image;

	private LineRenderer lineRenderer;

	private Canvas canvas;

	private bool _isDragStarted = false;

	public bool _isSuccess = false;

	private WireTask wireTask;

	public void Initialize()
	{
		_image = GetComponent<Image>();
		lineRenderer = GetComponent<LineRenderer>();
		canvas = GetComponentInParent<Canvas>();
		wireTask = GetComponentInParent<WireTask>();
		_isSuccess = false;
		_isDragStarted = false;
	}

	private void Update()
	{
		if (_isDragStarted)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out var localPoint);
			lineRenderer.SetPosition(0, base.transform.position);
			lineRenderer.SetPosition(1, canvas.transform.TransformPoint(localPoint));
		}
		else if (!_isSuccess)
		{
			lineRenderer.SetPosition(0, Vector3.zero);
			lineRenderer.SetPosition(1, Vector3.zero);
		}
		if (RectTransformUtility.RectangleContainsScreenPoint(base.transform as RectTransform, Input.mousePosition, canvas.worldCamera))
		{
			wireTask.CurrentHoveredWire = this;
		}
	}

	public void SetColor(Color color)
	{
		_image.color = color;
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
		customColor = color;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (isLeftWire && !_isSuccess)
		{
			_isDragStarted = true;
			wireTask.CurrentSelectedWire = this;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (wireTask.CurrentHoveredWire != null && wireTask.CurrentHoveredWire.customColor == customColor && !wireTask.CurrentHoveredWire.isLeftWire)
		{
			_isSuccess = true;
			wireTask.CurrentHoveredWire._isSuccess = true;
		}
		_isDragStarted = false;
		wireTask.CurrentSelectedWire = null;
	}

	public void OnDrag(PointerEventData eventData)
	{
	}
}
