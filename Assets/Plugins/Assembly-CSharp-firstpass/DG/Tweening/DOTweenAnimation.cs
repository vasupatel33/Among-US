using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		public enum AnimationType
		{
			None = 0,
			Move = 1,
			LocalMove = 2,
			Rotate = 3,
			LocalRotate = 4,
			Scale = 5,
			Color = 6,
			Fade = 7,
			Text = 8,
			PunchPosition = 9,
			PunchRotation = 10,
			PunchScale = 11,
			ShakePosition = 12,
			ShakeRotation = 13,
			ShakeScale = 14,
			CameraAspect = 15,
			CameraBackgroundColor = 16,
			CameraFieldOfView = 17,
			CameraOrthoSize = 18,
			CameraPixelRect = 19,
			CameraRect = 20,
			UIWidthHeight = 21
		}

		public enum TargetType
		{
			Unset = 0,
			Camera = 1,
			CanvasGroup = 2,
			Image = 3,
			Light = 4,
			RectTransform = 5,
			Renderer = 6,
			SpriteRenderer = 7,
			Rigidbody = 8,
			Rigidbody2D = 9,
			Text = 10,
			Transform = 11,
			tk2dBaseSprite = 12,
			tk2dTextMesh = 13,
			TextMeshPro = 14,
			TextMeshProUGUI = 15
		}

		public bool targetIsSelf = true;

		public GameObject targetGO = null;

		public bool tweenTargetIsTargetGO = true;

		public float delay;

		public float duration = 1f;

		public Ease easeType = Ease.OutQuad;

		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public LoopType loopType = LoopType.Restart;

		public int loops = 1;

		public string id = "";

		public bool isRelative;

		public bool isFrom;

		public bool isIndependentUpdate = false;

		public bool autoKill = true;

		public bool isActive = true;

		public bool isValid;

		public Component target;

		public AnimationType animationType;

		public TargetType targetType;

		public TargetType forcedTargetType;

		public bool autoPlay = true;

		public bool useTargetAsV3;

		public float endValueFloat;

		public Vector3 endValueV3;

		public Vector2 endValueV2;

		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		public string endValueString = "";

		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		public Transform endValueTransform;

		public bool optionalBool0;

		public float optionalFloat0;

		public int optionalInt0;

		public RotateMode optionalRotationMode = RotateMode.Fast;

		public ScrambleMode optionalScrambleMode = ScrambleMode.None;

		public string optionalString;

		private bool _tweenCreated;

		private int _playCount = -1;

		public static event Action<DOTweenAnimation> OnReset;

		private static void Dispatch_OnReset(DOTweenAnimation anim)
		{
			if (DOTweenAnimation.OnReset != null)
			{
				DOTweenAnimation.OnReset(anim);
			}
		}

		private void Awake()
		{
			if (isActive && isValid && (animationType != AnimationType.Move || !useTargetAsV3))
			{
				CreateTween();
				_tweenCreated = true;
			}
		}

		private void Start()
		{
			if (!_tweenCreated && isActive && isValid)
			{
				CreateTween();
				_tweenCreated = true;
			}
		}

		private void Reset()
		{
			Dispatch_OnReset(this);
		}

		private void OnDestroy()
		{
			if (tween != null && tween.IsActive())
			{
				tween.Kill();
			}
			tween = null;
		}

		public void CreateTween()
		{
			GameObject tweenGO = GetTweenGO();
			if (target == null || tweenGO == null)
			{
				if (targetIsSelf && target == null)
				{
					Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject);
				}
				else
				{
					Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject);
				}
				return;
			}
			if (forcedTargetType != 0)
			{
				this.targetType = forcedTargetType;
			}
			if (this.targetType == TargetType.Unset)
			{
				this.targetType = TypeToDOTargetType(target.GetType());
			}
			switch (animationType)
			{
			case AnimationType.Move:
				if (useTargetAsV3)
				{
					isRelative = false;
					if (endValueTransform == null)
					{
						Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject);
						endValueV3 = Vector3.zero;
					}
					else if (this.targetType == TargetType.RectTransform)
					{
						RectTransform rectTransform = endValueTransform as RectTransform;
						if (rectTransform == null)
						{
							Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject);
							endValueV3 = Vector3.zero;
						}
						else
						{
							RectTransform rectTransform2 = target as RectTransform;
							if (rectTransform2 == null)
							{
								Debug.LogWarning($"{base.gameObject.name} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject);
							}
							else
							{
								endValueV3 = DOTweenModuleUI.Utils.SwitchToRectTransform(rectTransform, rectTransform2);
							}
						}
					}
					else
					{
						endValueV3 = endValueTransform.position;
					}
				}
				switch (this.targetType)
				{
				case TargetType.Transform:
					tween = ((Transform)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOAnchorPos3D(endValueV3, duration, optionalBool0);
					break;
				case TargetType.Rigidbody:
					tween = ((Rigidbody)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				case TargetType.Rigidbody2D:
					tween = ((Rigidbody2D)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				}
				break;
			case AnimationType.LocalMove:
				tween = tweenGO.transform.DOLocalMove(endValueV3, duration, optionalBool0);
				break;
			case AnimationType.Rotate:
				switch (this.targetType)
				{
				case TargetType.Transform:
					tween = ((Transform)target).DORotate(endValueV3, duration, optionalRotationMode);
					break;
				case TargetType.Rigidbody:
					tween = ((Rigidbody)target).DORotate(endValueV3, duration, optionalRotationMode);
					break;
				case TargetType.Rigidbody2D:
					tween = ((Rigidbody2D)target).DORotate(endValueFloat, duration);
					break;
				}
				break;
			case AnimationType.LocalRotate:
				tween = tweenGO.transform.DOLocalRotate(endValueV3, duration, optionalRotationMode);
				break;
			case AnimationType.Scale:
			{
				TargetType targetType5 = this.targetType;
				TargetType targetType6 = targetType5;
				if (targetType6 != TargetType.tk2dBaseSprite && targetType6 != TargetType.tk2dTextMesh)
				{
					tween = tweenGO.transform.DOScale(optionalBool0 ? new Vector3(endValueFloat, endValueFloat, endValueFloat) : endValueV3, duration);
				}
				break;
			}
			case AnimationType.UIWidthHeight:
				tween = ((RectTransform)target).DOSizeDelta(optionalBool0 ? new Vector2(endValueFloat, endValueFloat) : endValueV2, duration);
				break;
			case AnimationType.Color:
				isRelative = false;
				switch (this.targetType)
				{
				case TargetType.Renderer:
					tween = ((Renderer)target).material.DOColor(endValueColor, duration);
					break;
				case TargetType.Light:
					tween = ((Light)target).DOColor(endValueColor, duration);
					break;
				case TargetType.SpriteRenderer:
					tween = ((SpriteRenderer)target).DOColor(endValueColor, duration);
					break;
				case TargetType.Image:
					tween = ((Graphic)target).DOColor(endValueColor, duration);
					break;
				case TargetType.Text:
					tween = ((Text)target).DOColor(endValueColor, duration);
					break;
				case TargetType.TextMeshProUGUI:
					tween = ((TextMeshProUGUI)target).DOColor(endValueColor, duration);
					break;
				case TargetType.TextMeshPro:
					tween = ((TextMeshPro)target).DOColor(endValueColor, duration);
					break;
				}
				break;
			case AnimationType.Fade:
				isRelative = false;
				switch (this.targetType)
				{
				case TargetType.Renderer:
					tween = ((Renderer)target).material.DOFade(endValueFloat, duration);
					break;
				case TargetType.Light:
					tween = ((Light)target).DOIntensity(endValueFloat, duration);
					break;
				case TargetType.SpriteRenderer:
					tween = ((SpriteRenderer)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.Image:
					tween = ((Graphic)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.Text:
					tween = ((Text)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.CanvasGroup:
					tween = ((CanvasGroup)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.TextMeshProUGUI:
					tween = ((TextMeshProUGUI)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.TextMeshPro:
					tween = ((TextMeshPro)target).DOFade(endValueFloat, duration);
					break;
				}
				break;
			case AnimationType.Text:
			{
				TargetType targetType = this.targetType;
				TargetType targetType2 = targetType;
				if (targetType2 == TargetType.Text)
				{
					tween = ((Text)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				}
				TargetType targetType3 = this.targetType;
				TargetType targetType4 = targetType3;
				if (targetType4 != TargetType.tk2dTextMesh)
				{
				}
				switch (this.targetType)
				{
				case TargetType.TextMeshProUGUI:
					tween = ((TextMeshProUGUI)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
					break;
				case TargetType.TextMeshPro:
					tween = ((TextMeshPro)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
					break;
				}
				break;
			}
			case AnimationType.PunchPosition:
				switch (this.targetType)
				{
				case TargetType.Transform:
					tween = ((Transform)target).DOPunchPosition(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
					break;
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOPunchAnchorPos(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
					break;
				}
				break;
			case AnimationType.PunchScale:
				tween = tweenGO.transform.DOPunchScale(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case AnimationType.PunchRotation:
				tween = tweenGO.transform.DOPunchRotation(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case AnimationType.ShakePosition:
				switch (this.targetType)
				{
				case TargetType.Transform:
					tween = ((Transform)target).DOShakePosition(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
					break;
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOShakeAnchorPos(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
					break;
				}
				break;
			case AnimationType.ShakeScale:
				tween = tweenGO.transform.DOShakeScale(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case AnimationType.ShakeRotation:
				tween = tweenGO.transform.DOShakeRotation(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case AnimationType.CameraAspect:
				tween = ((Camera)target).DOAspect(endValueFloat, duration);
				break;
			case AnimationType.CameraBackgroundColor:
				tween = ((Camera)target).DOColor(endValueColor, duration);
				break;
			case AnimationType.CameraFieldOfView:
				tween = ((Camera)target).DOFieldOfView(endValueFloat, duration);
				break;
			case AnimationType.CameraOrthoSize:
				tween = ((Camera)target).DOOrthoSize(endValueFloat, duration);
				break;
			case AnimationType.CameraPixelRect:
				tween = ((Camera)target).DOPixelRect(endValueRect, duration);
				break;
			case AnimationType.CameraRect:
				tween = ((Camera)target).DORect(endValueRect, duration);
				break;
			}
			if (tween == null)
			{
				return;
			}
			if (isFrom)
			{
				((Tweener)tween).From(isRelative);
			}
			else
			{
				tween.SetRelative(isRelative);
			}
			GameObject gameObject = ((targetIsSelf || !tweenTargetIsTargetGO) ? base.gameObject : targetGO);
			tween.SetTarget(gameObject).SetDelay(delay).SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.OnKill(delegate
				{
					tween = null;
				});
			if (isSpeedBased)
			{
				tween.SetSpeedBased();
			}
			if (easeType == Ease.INTERNAL_Custom)
			{
				tween.SetEase(easeCurve);
			}
			else
			{
				tween.SetEase(easeType);
			}
			if (!string.IsNullOrEmpty(id))
			{
				tween.SetId(id);
			}
			tween.SetUpdate(isIndependentUpdate);
			if (hasOnStart)
			{
				if (onStart != null)
				{
					tween.OnStart(onStart.Invoke);
				}
			}
			else
			{
				onStart = null;
			}
			if (hasOnPlay)
			{
				if (onPlay != null)
				{
					tween.OnPlay(onPlay.Invoke);
				}
			}
			else
			{
				onPlay = null;
			}
			if (hasOnUpdate)
			{
				if (onUpdate != null)
				{
					tween.OnUpdate(onUpdate.Invoke);
				}
			}
			else
			{
				onUpdate = null;
			}
			if (hasOnStepComplete)
			{
				if (onStepComplete != null)
				{
					tween.OnStepComplete(onStepComplete.Invoke);
				}
			}
			else
			{
				onStepComplete = null;
			}
			if (hasOnComplete)
			{
				if (onComplete != null)
				{
					tween.OnComplete(onComplete.Invoke);
				}
			}
			else
			{
				onComplete = null;
			}
			if (hasOnRewind)
			{
				if (onRewind != null)
				{
					tween.OnRewind(onRewind.Invoke);
				}
			}
			else
			{
				onRewind = null;
			}
			if (autoPlay)
			{
				tween.Play();
			}
			else
			{
				tween.Pause();
			}
			if (hasOnTweenCreated && onTweenCreated != null)
			{
				onTweenCreated.Invoke();
			}
		}

		public override void DOPlay()
		{
			DOTween.Play(base.gameObject);
		}

		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(base.gameObject);
		}

		public override void DOPlayForward()
		{
			DOTween.PlayForward(base.gameObject);
		}

		public override void DOPause()
		{
			DOTween.Pause(base.gameObject);
		}

		public override void DOTogglePause()
		{
			DOTween.TogglePause(base.gameObject);
		}

		public override void DORewind()
		{
			_playCount = -1;
			DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
			for (int num = components.Length - 1; num > -1; num--)
			{
				Tween tween = components[num].tween;
				if (tween != null && tween.IsInitialized())
				{
					components[num].tween.Rewind();
				}
			}
		}

		public override void DORestart()
		{
			DORestart(fromHere: false);
		}

		public override void DORestart(bool fromHere)
		{
			_playCount = -1;
			if (tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(tween);
				}
				return;
			}
			if (fromHere && isRelative)
			{
				ReEvaluateRelativeTween();
			}
			DOTween.Restart(base.gameObject);
		}

		public override void DOComplete()
		{
			DOTween.Complete(base.gameObject);
		}

		public override void DOKill()
		{
			DOTween.Kill(base.gameObject);
			tween = null;
		}

		public void DOPlayById(string id)
		{
			DOTween.Play(base.gameObject, id);
		}

		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		public void DOPauseAllById(string id)
		{
			DOTween.Pause(id);
		}

		public void DOPlayBackwardsById(string id)
		{
			DOTween.PlayBackwards(base.gameObject, id);
		}

		public void DOPlayBackwardsAllById(string id)
		{
			DOTween.PlayBackwards(id);
		}

		public void DOPlayForwardById(string id)
		{
			DOTween.PlayForward(base.gameObject, id);
		}

		public void DOPlayForwardAllById(string id)
		{
			DOTween.PlayForward(id);
		}

		public void DOPlayNext()
		{
			DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
			while (_playCount < components.Length - 1)
			{
				_playCount++;
				DOTweenAnimation dOTweenAnimation = components[_playCount];
				if (dOTweenAnimation != null && dOTweenAnimation.tween != null && !dOTweenAnimation.tween.IsPlaying() && !dOTweenAnimation.tween.IsComplete())
				{
					dOTweenAnimation.tween.Play();
					break;
				}
			}
		}

		public void DORewindAndPlayNext()
		{
			_playCount = -1;
			DOTween.Rewind(base.gameObject);
			DOPlayNext();
		}

		public void DORewindAllById(string id)
		{
			_playCount = -1;
			DOTween.Rewind(id);
		}

		public void DORestartById(string id)
		{
			_playCount = -1;
			DOTween.Restart(base.gameObject, id);
		}

		public void DORestartAllById(string id)
		{
			_playCount = -1;
			DOTween.Restart(id);
		}

		public List<Tween> GetTweens()
		{
			List<Tween> list = new List<Tween>();
			DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
			DOTweenAnimation[] array = components;
			foreach (DOTweenAnimation dOTweenAnimation in array)
			{
				list.Add(dOTweenAnimation.tween);
			}
			return list;
		}

		public static TargetType TypeToDOTargetType(Type t)
		{
			string text = t.ToString();
			int num = text.LastIndexOf(".");
			if (num != -1)
			{
				text = text.Substring(num + 1);
			}
			if (text.IndexOf("Renderer") != -1 && text != "SpriteRenderer")
			{
				text = "Renderer";
			}
			if (text == "RawImage" || text == "Graphic")
			{
				text = "Image";
			}
			return (TargetType)Enum.Parse(typeof(TargetType), text);
		}

		public Tween CreateEditorPreview()
		{
			if (Application.isPlaying)
			{
				return null;
			}
			CreateTween();
			return tween;
		}

		private GameObject GetTweenGO()
		{
			return targetIsSelf ? base.gameObject : targetGO;
		}

		private void ReEvaluateRelativeTween()
		{
			GameObject tweenGO = GetTweenGO();
			if (tweenGO == null)
			{
				Debug.LogWarning($"{base.gameObject.name} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject);
			}
			else if (animationType == AnimationType.Move)
			{
				((Tweener)tween).ChangeEndValue(tweenGO.transform.position + endValueV3, snapStartValue: true);
			}
			else if (animationType == AnimationType.LocalMove)
			{
				((Tweener)tween).ChangeEndValue(tweenGO.transform.localPosition + endValueV3, snapStartValue: true);
			}
		}
	}
}
