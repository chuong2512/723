using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorTransformBorder : BaseView
{
	private sealed class _Init_c__AnonStorey0
	{
		internal List<ObjTypeEditData> list;

		internal EditorTransformBorder _this;

		internal void __m__0(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this._this.target.OriginalRotation = new Vector3(0f, 0f, float.Parse(value));
				this._this.target.mTrans.rotation = Quaternion.Euler(new Vector3(0f, 0f, float.Parse(value)));
				this._this.moveTrans.rotation = this._this.target.mTrans.rotation;
			}
		}

		internal bool __m__1(ObjTypeEditData o)
		{
			return o.ObjType == this._this.target.prefabType;
		}

		internal void __m__2(int index)
		{
			string prefabType = (index != 0) ? this.list[index - 1].ObjType : string.Empty;
			this._this.target.prefabType = prefabType;
			this._this.CopyObj(false);
			this._this.OnDeleClick();
			UnityEngine.Debug.Log("换类型" + this._this.target.prefabType);
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey1
	{
		internal Serration serration;

		internal void __m__0(string val)
		{
			this.serration.width = int.Parse(val);
		}

		internal void __m__1(bool isOn)
		{
			this.serration.needBind = isOn;
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey2
	{
		internal Conveyor conveyor;

		internal void __m__0(bool isOn)
		{
			this.conveyor.IsRight = isOn;
		}

		internal void __m__1(string val)
		{
			this.conveyor.power = float.Parse(val);
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey3
	{
		internal PoleCaster poleCaster;

		internal void __m__0(string val)
		{
			this.poleCaster.force = float.Parse(val.Trim());
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey4
	{
		internal InteractObj interact;

		internal void __m__0(string val)
		{
			this.interact.mass = float.Parse(val.Trim());
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey5
	{
		internal Spring spring;

		internal void __m__0(bool isOn)
		{
			this.spring.isTrrige = isOn;
		}

		internal void __m__1(string val)
		{
			this.spring.Mass = float.Parse(val);
		}

		internal void __m__2(float val)
		{
			this.spring.CompressDis = val;
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey7
	{
		private sealed class _SetOtherProperty_c__AnonStorey8
		{
			internal Transform item;

			internal EditorTransformBorder._SetOtherProperty_c__AnonStorey7 __f__ref_7;

			internal void __m__0()
			{
				UnityEngine.Object.Destroy(this.item.gameObject);
				this.__f__ref_7.list.Remove(this.item);
			}
		}

		internal List<Transform> list;

		internal Transform content;

		internal MoveCube moveCube;

		internal InputField speedIn;

		internal EditorTransformBorder _this;

		internal void __m__0()
		{
			Transform item = UnityEngine.Object.Instantiate<Transform>(this._this.xyTrans, this.content);
			item.gameObject.SetActive(true);
			InputField component = item.Find("X/Value").GetComponent<InputField>();
			InputField component2 = item.Find("Y/Value").GetComponent<InputField>();
			component.text = this.moveCube.mTrans.localPosition.x.ToString();
			component2.text = this.moveCube.mTrans.localPosition.y.ToString();
			this.list.Add(item);
			Button component3 = item.Find("Button").GetComponent<Button>();
			component3.onClick.AddListener(delegate
			{
				UnityEngine.Object.Destroy(item.gameObject);
				this.list.Remove(item);
			});
		}

		internal void __m__1()
		{
			this.moveCube.movelist.Clear();
			for (int i = 0; i < this.list.Count; i++)
			{
				Transform transform = this.list[i];
				InputField component = transform.Find("X/Value").GetComponent<InputField>();
				InputField component2 = transform.Find("Y/Value").GetComponent<InputField>();
				float x = (!string.IsNullOrEmpty(component.text)) ? Convert.ToSingle(component.text) : 0f;
				float y = (!string.IsNullOrEmpty(component2.text)) ? Convert.ToSingle(component2.text) : 0f;
				this.moveCube.SetVec(new Vector2(x, y));
			}
			this.moveCube.Speed = Convert.ToSingle(this.speedIn.text);
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey6
	{
		internal Transform item;

		internal EditorTransformBorder._SetOtherProperty_c__AnonStorey7 __f__ref_7;

		internal void __m__0()
		{
			UnityEngine.Object.Destroy(this.item.gameObject);
			this.__f__ref_7.list.Remove(this.item);
		}
	}

	private sealed class _SetOtherProperty_c__AnonStorey9
	{
		internal InputField input1;

		internal InputField input2;

		internal InputField input3;

		internal InputField input4;

		internal Fan fan;

		internal void __m__0()
		{
			float height = Convert.ToSingle(this.input1.text);
			float width = Convert.ToSingle(this.input2.text);
			float maxForce = Convert.ToSingle(this.input3.text);
			float minForce = Convert.ToSingle(this.input4.text);
			this.fan.SetParameter(height, width, maxForce, minForce);
			this.fan.DrawRegion();
		}
	}

	private sealed class _SetHintObj_c__AnonStoreyA
	{
		internal HintObj hint;

		internal void __m__0(string val)
		{
			this.hint.hintStrKey = val;
			this.hint.ShowString();
		}

		internal void __m__1(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v);
			this.hint.textTrans.localPosition = Utils.ScreenToLocalPos(pData.position, this.hint.mTrans);
		}
	}

	private sealed class _SetTeeterboard_c__AnonStoreyB
	{
		internal Teeterboard te;

		internal void __m__0(bool isOn)
		{
			if (isOn)
			{
				this.te.achor = Teeterboard.Achor.Left;
			}
		}

		internal void __m__1(bool isOn)
		{
			if (isOn)
			{
				this.te.achor = Teeterboard.Achor.Mid;
			}
		}

		internal void __m__2(bool isOn)
		{
			if (isOn)
			{
				this.te.achor = Teeterboard.Achor.Right;
			}
		}
	}

	private sealed class _SetTeeterboard_c__AnonStoreyC
	{
		internal Block block;

		internal void __m__0(string val)
		{
			this.block.speed = float.Parse(val);
		}
	}

	private sealed class _RopeEdit_c__AnonStoreyD
	{
		internal Rope rope;

		internal void __m__0(bool isOn)
		{
			this.rope.isPole = isOn;
		}

		internal void __m__1(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v);
			this.rope.start.localPosition = Utils.ScreenToLocalPos(pData.position, this.rope.mTrans);
		}

		internal void __m__2(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v);
			this.rope.end.localPosition = Utils.ScreenToLocalPos(pData.position, this.rope.mTrans);
		}
	}

	public Transform parent;

	public Primitives target;

	public Action ResetCallBacl;

	private Image LeftDrag;

	private Transform LeftDragCollider;

	private Image RightDrag;

	private Transform RightDragCollider;

	private Image UpDrag;

	private Transform UpDragCollider;

	private Image DownDrag;

	private Transform DownDragCollider;

	private Image EndRotate;

	private Transform EndRotateCollider;

	private Transform moveTrans;

	private Transform rotateCollider;

	private Image moveBorderImg;

	private Vector3 addVec3 = new Vector3(100f, 100f, 0f);

	private bool isButtonchange;

	private Button deleteBtn;

	private Transform otherTrans;

	private Transform porpItemTrans;

	private Toggle propToggle;

	private Button sureBtn;

	private Transform cubeMoveTrans;

	private Transform xyTrans;

	private Button copyBtn;

	private InputField angle;

	private InputField changePrefabIn;

	private Dropdown dropdown;

	private Vector2 uiOffset = Vector2.zero;

	private Vector2 offset = Vector2.zero;

	private static Action<PointerEventData, UGUIEvent> __f__am_cache0;

	private void Awake()
	{
		this.rotateCollider = base.transform.Find("RotateCollider");
		this.moveTrans = base.transform.Find("MoveCollider");
		this.moveBorderImg = this.moveTrans.GetComponent<Image>();
		this.LeftDragCollider = this.moveTrans.Find("LeftDragcollider");
		this.RightDragCollider = this.moveTrans.Find("RightDragcollider");
		this.UpDragCollider = this.moveTrans.Find("UpDragcollider");
		this.DownDragCollider = this.moveTrans.Find("DownDragcollider");
		this.RightDragCollider = this.moveTrans.Find("RightDragcollider");
		this.deleteBtn = this.moveTrans.Find("Delete").GetComponent<Button>();
		this.otherTrans = base.transform.Find("Other");
		this.porpItemTrans = this.otherTrans.Find("item");
		this.propToggle = this.otherTrans.Find("Toggle").GetComponent<Toggle>();
		this.sureBtn = this.otherTrans.Find("Button").GetComponent<Button>();
		this.cubeMoveTrans = base.transform.Find("MoveCube");
		this.xyTrans = this.cubeMoveTrans.Find("ScrollView/Viewport/Content/itemXY");
		Transform transform = base.transform.Find("Common");
		this.copyBtn = transform.Find("CopyBtn").GetComponent<Button>();
		this.angle = base.Find<InputField>(transform, "Angle/InputField");
		this.changePrefabIn = base.Find<InputField>(transform, "ChangePrefab/InputField");
		this.dropdown = base.Find<Dropdown>(transform, "ChangePrefab/Dropdown");
		this.AttachEvent();
		UGUIEvent uGUIEvent = base.Find<UGUIEvent>(base.transform, "Common");
		uGUIEvent.onDrag += delegate(PointerEventData eventData, UGUIEvent sender)
		{
			Vector2 v = UIManager.GetInst(false).m_UICamera.ScreenToWorldPoint(eventData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v);
		};
	}

	public override void Init(params object[] args)
	{
		this.target = (args[0] as Primitives);
		this.InitBorder();
		EditObjData component = this.target.gameObject.GetComponent<EditObjData>();
		TransformConstraints type = (TransformConstraints)0;
		if (component != null)
		{
			type = component.transformConstraints;
		}
		this.InitTransformConstraintsBorder(type);
		this.SetOtherProperty(this.target);
		this.angle.text = this.target.mTrans.eulerAngles.z.ToString("f2");
		this.angle.onEndEdit.AddListener(delegate(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.target.OriginalRotation = new Vector3(0f, 0f, float.Parse(value));
				this.target.mTrans.rotation = Quaternion.Euler(new Vector3(0f, 0f, float.Parse(value)));
				this.moveTrans.rotation = this.target.mTrans.rotation;
			}
		});
		List<ObjTypeEditData> list = SceneSetting.inst.FindTypeDataListbyType(this.target.GetType().ToString());
		if (list.Count >= 1)
		{
			List<string> list2 = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(list[i].EditorShowName);
			}
			this.dropdown.AddOptions(list2);
			if (!this.target.prefabType.IsNullOrEntry())
			{
				this.dropdown.value = list.FindIndex((ObjTypeEditData o) => o.ObjType == this.target.prefabType) + 1;
			}
			this.dropdown.onValueChanged.AddListener(delegate(int index)
			{
				string prefabType = (index != 0) ? list[index - 1].ObjType : string.Empty;
				this.target.prefabType = prefabType;
				this.CopyObj(false);
				this.OnDeleClick();
				UnityEngine.Debug.Log("换类型" + this.target.prefabType);
			});
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.D))
		{
			this.CopyObj();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Delete))
		{
			this.OnDeleClick();
		}
		if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
		{
			this.target.SetOriginalPos(this.target.OriginalPos + new Vector3(-0.1f * Time.deltaTime, 0f, 0f));
			this.InitBorder();
		}
		if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
		{
			this.target.SetOriginalPos(this.target.OriginalPos + new Vector3(0.1f * Time.deltaTime, 0f, 0f));
			this.InitBorder();
		}
		if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
		{
			this.target.SetOriginalPos(this.target.OriginalPos + new Vector3(0f, 0.11f * Time.deltaTime, 0f));
			this.InitBorder();
		}
		if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
		{
			this.target.SetOriginalPos(this.target.OriginalPos + new Vector3(0f, -0.11f * Time.deltaTime, 0f));
			this.InitBorder();
		}
	}

	public void InitBorder()
	{
		Transform transform = this.target.transform;
		Vector3 position = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(transform.position));
		this.moveTrans.localPosition = base.transform.InverseTransformPoint(position);
		this.moveTrans.rotation = transform.rotation;
		float num = PBase.ScreenRightUp.x - PBase.ScreenLeftDown.x - 2f;
		num = UIManager.GetInst(false).MainCnvas.GetComponent<RectTransform>().sizeDelta.x / num;
		this.moveBorderImg.rectTransform.sizeDelta = this.target.OriginalScale * num + this.addVec3;
	}

	private void AttachEvent()
	{
		UGUIEvent uGUIEvent = this.moveTrans.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnMoveDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnTouchMove);
		uGUIEvent = this.LeftDragCollider.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnLeftDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnTouchStart);
		uGUIEvent = this.RightDragCollider.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnRightDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnTouchStart);
		uGUIEvent = this.UpDragCollider.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnUpDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnTouchStart);
		uGUIEvent = this.DownDragCollider.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnDownDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnTouchStart);
		uGUIEvent = this.rotateCollider.gameObject.AddComponent<UGUIEvent>();
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.StartRotate);
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnRotate);
		uGUIEvent.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.BackGroundClick);
		this.deleteBtn.gameObject.AddComponent<UGUIEvent>();
		this.deleteBtn.onClick.AddListener(new UnityAction(this.OnDeleClick));
		this.copyBtn.onClick.AddListener(new UnityAction(this.CopyObj));
	}

	private void ChangePrefabClick()
	{
		if (!string.IsNullOrEmpty(this.changePrefabIn.text))
		{
			this.target.prefabType = this.changePrefabIn.text;
			UnityEngine.Debug.Log(this.target.prefabType);
			this.CopyObj(false);
			this.OnDeleClick();
		}
	}

	private void CopyObj()
	{
		this.CopyObj(true);
	}

	private void CopyObj(bool isCopy)
	{
		Primitives primitives = EntitiesFactory.CopyObj(this.target, isCopy);
		LevelStage.CurEditStageInst.AddObjWithEditor(primitives);
		UGUIEvent uGUIEvent = primitives.gameObject.AddComponent<UGUIEvent>();
		EditorUIWindow openWindow = UIManager.GetInst(false).GetOpenWindow<EditorUIWindow>();
		uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(openWindow.OnPuzzleDrag);
		uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(openWindow.OnPointDown);
		uGUIEvent.onPointerClick += new Action<PointerEventData, UGUIEvent>(openWindow.OnPointerClick);
	}

	private void InitTransformConstraintsBorder(TransformConstraints type)
	{
		if (type == (TransformConstraints)0)
		{
			return;
		}
		Array values = Enum.GetValues(typeof(TransformConstraints));
		for (int i = 0; i < values.Length; i++)
		{
			TransformConstraints transformConstraints = (TransformConstraints)values.GetValue(i);
			uint num = (uint)(transformConstraints & type);
			bool flag = num == (uint)transformConstraints;
			if (flag)
			{
				if (transformConstraints != TransformConstraints.ScaleX)
				{
					if (transformConstraints != TransformConstraints.ScaleY)
					{
						if (transformConstraints == TransformConstraints.Rotate)
						{
							UGUIEvent component = this.rotateCollider.gameObject.GetComponent<UGUIEvent>();
							component.onPointerDown -= new Action<PointerEventData, UGUIEvent>(this.StartRotate);
							component.onDrag -= new Action<PointerEventData, UGUIEvent>(this.OnRotate);
							this.angle.transform.parent.gameObject.SetActive(false);
						}
					}
					else
					{
						this.DownDragCollider.gameObject.SetActive(false);
						this.UpDragCollider.gameObject.SetActive(false);
					}
				}
				else
				{
					this.LeftDragCollider.gameObject.SetActive(false);
					this.RightDragCollider.gameObject.SetActive(false);
				}
			}
		}
	}

	private void OnMoveDrag(PointerEventData eventData, UGUIEvent sender)
	{
		Vector2 a = Utils.ScreenToLocalPos(eventData.position, this.target.transform.parent);
		this.target.SetOriginalPos(a + this.offset);
		Vector2 a2 = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position));
		this.moveTrans.localPosition = a2 + this.uiOffset;
	}

	private void OnTouchMove(PointerEventData eventData, UGUIEvent sender)
	{
		this.uiOffset = sender.transform.localPosition - sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position));
		this.offset = this.target.transform.localPosition - Utils.ScreenToLocalPos(eventData.position, this.target.transform.parent);
	}

	private void OnLeftDrag(PointerEventData eventData, UGUIEvent sender)
	{
		Vector2 vector = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position));
		float num = sender.transform.localPosition.x - (vector.x + this.uiOffset.x);
		num *= Vector3.Distance(sender.transform.TransformPoint(new Vector3(1f, 0f, 0f)), sender.transform.TransformPoint(Vector3.zero));
		this.target.DragLeft(num);
		this.InitBorder();
	}

	private void OnRightDrag(PointerEventData eventData, UGUIEvent sender)
	{
		float num = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position)).x + this.uiOffset.x - sender.transform.localPosition.x;
		num *= Vector3.Distance(sender.transform.TransformPoint(new Vector3(1f, 0f, 0f)), sender.transform.TransformPoint(Vector3.zero));
		this.target.DragRight(num);
		this.InitBorder();
	}

	private void OnTouchStart(PointerEventData eventData, UGUIEvent sender)
	{
		this.uiOffset = sender.transform.localPosition - sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position));
	}

	private void OnUpDrag(PointerEventData eventData, UGUIEvent sender)
	{
		float num = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position)).y + this.uiOffset.y - sender.transform.localPosition.y;
		num *= Vector3.Distance(sender.transform.TransformPoint(new Vector3(0f, 1f, 0f)), sender.transform.TransformPoint(Vector3.zero));
		this.target.DragUp(num);
		this.InitBorder();
	}

	private void OnDownDrag(PointerEventData eventData, UGUIEvent sender)
	{
		Vector2 vector = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(eventData.pressEventCamera.ScreenToWorldPoint(eventData.position));
		float num = sender.transform.localPosition.y - (vector.y + this.uiOffset.y);
		num *= Vector3.Distance(sender.transform.TransformPoint(new Vector3(0f, 1f, 0f)), sender.transform.TransformPoint(Vector3.zero));
		this.target.DragDown(num);
		this.InitBorder();
	}

	private void OnDeleClick()
	{
		LevelStage.CurEditStageInst.RemoveObjWithEditor(this.target);
		UnityEngine.Object.Destroy(this.target.gameObject);
		this.Close();
		if (this.target is Portal)
		{
			((Portal)this.target).DeleOther();
		}
		else if (this.target is TriggerObj)
		{
			((TriggerObj)this.target).DeleOther();
		}
	}

	public void StartRotate(PointerEventData eventData, UGUIEvent sender)
	{
		this.target.StartRotate(eventData.position);
	}

	public void OnRotate(PointerEventData eventData, UGUIEvent sender)
	{
		this.target.OnRotate(eventData.position);
		this.moveTrans.rotation = this.target.transform.rotation;
		this.angle.text = this.target.mTrans.eulerAngles.z.ToString("f2");
	}

	private void BackGroundClick(PointerEventData eventData, UGUIEvent sender)
	{
		if (eventData.dragging)
		{
			return;
		}
		this.Close();
	}

	private void SetOtherProperty(Primitives obj)
	{
		if (obj is Rope)
		{
			this.RopeEdit(obj as Rope);
		}
		else if (obj is Serration)
		{
			this.otherTrans.gameObject.SetActive(true);
			Serration serration = obj as Serration;
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform.Find("Text").GetComponent<Text>().text = "宽度";
			transform.gameObject.SetActive(true);
			InputField component = transform.Find("InputField").GetComponent<InputField>();
			component.text = serration.width.ToString();
			component.contentType = InputField.ContentType.IntegerNumber;
			component.onValueChanged.AddListener(delegate(string val)
			{
				serration.width = int.Parse(val);
			});
			this.propToggle.gameObject.SetActive(true);
			this.propToggle.isOn = serration.needBind;
			Utils.Find<Text>(this.propToggle.transform, "Label").text = "是否需要绑定";
			this.propToggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				serration.needBind = isOn;
			});
		}
		else if (obj is Teeterboard)
		{
			this.SetTeeterboard(obj);
		}
		else if (obj is HintObj)
		{
			HintObj hintObj = obj as HintObj;
			this.SetHintObj(hintObj);
		}
		else if (obj is Conveyor)
		{
			Conveyor conveyor = obj as Conveyor;
			this.propToggle.gameObject.SetActive(true);
			this.propToggle.isOn = conveyor.IsRight;
			Utils.Find<Text>(this.propToggle.transform, "Label").text = "是否向右";
			this.propToggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				conveyor.IsRight = isOn;
			});
			Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform2.Find("Text").GetComponent<Text>().text = "速度";
			transform2.gameObject.SetActive(true);
			InputField component2 = transform2.Find("InputField").GetComponent<InputField>();
			component2.text = conveyor.power.ToString();
			component2.contentType = InputField.ContentType.DecimalNumber;
			component2.onValueChanged.AddListener(delegate(string val)
			{
				conveyor.power = float.Parse(val);
			});
		}
		else if (obj is PoleCaster)
		{
			PoleCaster poleCaster = obj as PoleCaster;
			Transform transform3 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform3.Find("Text").GetComponent<Text>().text = "力";
			transform3.gameObject.SetActive(true);
			InputField component3 = transform3.Find("InputField").GetComponent<InputField>();
			component3.text = poleCaster.force.ToString();
			component3.contentType = InputField.ContentType.DecimalNumber;
			component3.onValueChanged.AddListener(delegate(string val)
			{
				poleCaster.force = float.Parse(val.Trim());
			});
		}
		else if (obj is InteractObj)
		{
			InteractObj interact = obj as InteractObj;
			Transform transform4 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform4.Find("Text").GetComponent<Text>().text = "质量";
			transform4.gameObject.SetActive(true);
			InputField component4 = transform4.Find("InputField").GetComponent<InputField>();
			component4.text = interact.mass.ToString();
			component4.contentType = InputField.ContentType.DecimalNumber;
			component4.onValueChanged.AddListener(delegate(string val)
			{
				interact.mass = float.Parse(val.Trim());
			});
		}
		else if (obj is Spring)
		{
			Spring spring = obj as Spring;
			this.propToggle.gameObject.SetActive(true);
			this.propToggle.isOn = spring.isTrrige;
			Utils.Find<Text>(this.propToggle.transform, "Label").text = "是否碰撞才激活";
			this.propToggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				spring.isTrrige = isOn;
			});
			Transform transform5 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform5.Find("Text").GetComponent<Text>().text = "质量";
			transform5.gameObject.SetActive(true);
			InputField component5 = transform5.Find("InputField").GetComponent<InputField>();
			component5.text = spring.Mass.ToString();
			component5.contentType = InputField.ContentType.DecimalNumber;
			component5.onValueChanged.AddListener(delegate(string val)
			{
				spring.Mass = float.Parse(val);
			});
			Slider slider = Utils.Find<Slider>(this.otherTrans, "Slider");
			slider.gameObject.SetActive(true);
			slider.maxValue = 2f;
			slider.minValue = 0f;
			slider.value = spring.CompressDis;
			slider.onValueChanged.AddListener(delegate(float val)
			{
				spring.CompressDis = val;
			});
		}
		else if (obj is MoveCube)
		{
			this.cubeMoveTrans.gameObject.SetActive(true);
			MoveCube moveCube = obj as MoveCube;
			Transform content = this.cubeMoveTrans.Find("ScrollView/Viewport/Content");
			List<Transform> list = new List<Transform>();
			using (List<Vector3>.Enumerator enumerator = moveCube.movelist.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector2 vector = enumerator.Current;
					Transform item = UnityEngine.Object.Instantiate<Transform>(this.xyTrans, content);
					item.gameObject.SetActive(true);
					InputField component6 = item.Find("X/Value").GetComponent<InputField>();
					InputField component7 = item.Find("Y/Value").GetComponent<InputField>();
					component6.text = vector.x.ToString();
					component7.text = vector.y.ToString();
					list.Add(item);
					Button component8 = item.Find("Button").GetComponent<Button>();
					component8.onClick.AddListener(delegate
					{
						UnityEngine.Object.Destroy(item.gameObject);
						list.Remove(item);
					});
				}
			}
			Button component9 = this.cubeMoveTrans.Find("addBtn").GetComponent<Button>();
			component9.onClick.AddListener(delegate
			{
				Transform item = UnityEngine.Object.Instantiate<Transform>(this.xyTrans, content);
				item.gameObject.SetActive(true);
				InputField component11 = item.Find("X/Value").GetComponent<InputField>();
				InputField component12 = item.Find("Y/Value").GetComponent<InputField>();
				component11.text = moveCube.mTrans.localPosition.x.ToString();
				component12.text = moveCube.mTrans.localPosition.y.ToString();
				list.Add(item);
				Button component13 = item.Find("Button").GetComponent<Button>();
				component13.onClick.AddListener(delegate
				{
					UnityEngine.Object.Destroy(item.gameObject);
					list.Remove(item);
				});
			});
			Button component10 = this.cubeMoveTrans.Find("sureBtn").GetComponent<Button>();
			InputField speedIn = this.cubeMoveTrans.Find("speedInput").GetComponent<InputField>();
			speedIn.text = moveCube.Speed.ToString();
			component10.onClick.AddListener(delegate
			{
				moveCube.movelist.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					Transform transform7 = list[i];
					InputField component11 = transform7.Find("X/Value").GetComponent<InputField>();
					InputField component12 = transform7.Find("Y/Value").GetComponent<InputField>();
					float x = (!string.IsNullOrEmpty(component11.text)) ? Convert.ToSingle(component11.text) : 0f;
					float y = (!string.IsNullOrEmpty(component12.text)) ? Convert.ToSingle(component12.text) : 0f;
					moveCube.SetVec(new Vector2(x, y));
				}
				moveCube.Speed = Convert.ToSingle(speedIn.text);
			});
		}
		else if (obj is Fan && obj is Fan)
		{
			this.otherTrans.gameObject.SetActive(true);
			Fan fan = obj as Fan;
			fan.DrawRegion();
			Transform transform6 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform6.Find("Text").GetComponent<Text>().text = "风区高";
			transform6.gameObject.SetActive(true);
			InputField input1 = transform6.Find("InputField").GetComponent<InputField>();
			input1.text = fan.height.ToString();
			transform6 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform6.gameObject.SetActive(true);
			transform6.Find("Text").GetComponent<Text>().text = "风区宽";
			InputField input2 = transform6.Find("InputField").GetComponent<InputField>();
			input2.text = fan.width.ToString();
			transform6 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform6.gameObject.SetActive(true);
			transform6.Find("Text").GetComponent<Text>().text = "最大力";
			InputField input3 = transform6.Find("InputField").GetComponent<InputField>();
			input3.text = fan.MaxForce.ToString();
			transform6 = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform6.gameObject.SetActive(true);
			transform6.Find("Text").GetComponent<Text>().text = "最小力";
			InputField input4 = transform6.Find("InputField").GetComponent<InputField>();
			input4.text = fan.MinForce.ToString();
			this.sureBtn.gameObject.SetActive(true);
			this.sureBtn.transform.SetAsLastSibling();
			this.sureBtn.onClick.AddListener(delegate
			{
				float height = Convert.ToSingle(input1.text);
				float width = Convert.ToSingle(input2.text);
				float maxForce = Convert.ToSingle(input3.text);
				float minForce = Convert.ToSingle(input4.text);
				fan.SetParameter(height, width, maxForce, minForce);
				fan.DrawRegion();
			});
		}
	}

	private void SetHintObj(HintObj hint)
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
		transform.Find("Text").GetComponent<Text>().text = "文本key";
		transform.gameObject.SetActive(true);
		InputField component = transform.Find("InputField").GetComponent<InputField>();
		component.text = hint.hintStrKey;
		component.onValueChanged.AddListener(delegate(string val)
		{
			hint.hintStrKey = val;
			hint.ShowString();
		});
		Transform transform2 = this.moveTrans.Find("BezierLine");
		transform2.gameObject.SetActive(true);
		UGUIEvent uGUIEvent = Utils.Find<UGUIEvent>(transform2, "P0");
		transform2.Find("P1").gameObject.SetActive(false);
		Vector2 v = hint.textTrans.position;
		Vector2 v2 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(v));
		uGUIEvent.transform.localPosition = transform2.worldToLocalMatrix.MultiplyPoint(v2);
		uGUIEvent.onDrag += delegate(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v3 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v3);
			hint.textTrans.localPosition = Utils.ScreenToLocalPos(pData.position, hint.mTrans);
		};
	}

	private void SetTeeterboard(Primitives obj)
	{
		this.otherTrans.gameObject.SetActive(true);
		Teeterboard te = obj as Teeterboard;
		Toggle toggle = UnityEngine.Object.Instantiate<Toggle>(this.propToggle, this.otherTrans);
		toggle.gameObject.SetActive(true);
		Utils.Find<Text>(toggle.transform, "Label").text = "左端";
		ToggleGroup component = toggle.GetComponent<ToggleGroup>();
		toggle.group = component;
		Toggle toggle2 = UnityEngine.Object.Instantiate<Toggle>(this.propToggle, this.otherTrans);
		toggle2.gameObject.SetActive(true);
		Utils.Find<Text>(toggle2.transform, "Label").text = "中间";
		toggle2.group = component;
		Toggle toggle3 = UnityEngine.Object.Instantiate<Toggle>(this.propToggle, this.otherTrans);
		toggle3.gameObject.SetActive(true);
		Utils.Find<Text>(toggle3.transform, "Label").text = "右端";
		toggle3.group = component;
		Teeterboard.Achor achor = te.achor;
		if (achor != Teeterboard.Achor.Left)
		{
			if (achor != Teeterboard.Achor.Mid)
			{
				if (achor == Teeterboard.Achor.Right)
				{
					toggle3.isOn = true;
				}
			}
			else
			{
				toggle2.isOn = true;
			}
		}
		else
		{
			toggle.isOn = true;
		}
		toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			if (isOn)
			{
				te.achor = Teeterboard.Achor.Left;
			}
		});
		toggle2.onValueChanged.AddListener(delegate(bool isOn)
		{
			if (isOn)
			{
				te.achor = Teeterboard.Achor.Mid;
			}
		});
		toggle3.onValueChanged.AddListener(delegate(bool isOn)
		{
			if (isOn)
			{
				te.achor = Teeterboard.Achor.Right;
			}
		});
		if (obj is Block)
		{
			Block block = obj as Block;
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.porpItemTrans, this.otherTrans);
			transform.Find("Text").GetComponent<Text>().text = "速度";
			transform.gameObject.SetActive(true);
			InputField component2 = transform.Find("InputField").GetComponent<InputField>();
			component2.text = block.speed.ToString();
			component2.contentType = InputField.ContentType.DecimalNumber;
			component2.onValueChanged.AddListener(delegate(string val)
			{
				block.speed = float.Parse(val);
			});
		}
	}

	private void RopeEdit(Rope rope)
	{
		this.deleteBtn.gameObject.SetActive(false);
		this.propToggle.gameObject.SetActive(true);
		this.propToggle.isOn = rope.isPole;
		Utils.Find<Text>(this.propToggle.transform, "Label").text = "是否为杆";
		this.propToggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			rope.isPole = isOn;
		});
		Transform transform = this.moveTrans.Find("BezierLine");
		transform.gameObject.SetActive(true);
		UGUIEvent uGUIEvent = Utils.Find<UGUIEvent>(transform, "P0");
		UGUIEvent uGUIEvent2 = Utils.Find<UGUIEvent>(transform, "P1");
		Vector2 v = rope.start.position;
		Vector2 v2 = rope.end.position;
		Vector2 v3 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(v));
		uGUIEvent.transform.localPosition = transform.worldToLocalMatrix.MultiplyPoint(v3);
		Vector2 v4 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(v2));
		uGUIEvent2.transform.localPosition = transform.worldToLocalMatrix.MultiplyPoint(v4);
		uGUIEvent.onDrag += delegate(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v5 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v5);
			rope.start.localPosition = Utils.ScreenToLocalPos(pData.position, rope.mTrans);
		};
		uGUIEvent2.onDrag += delegate(PointerEventData pData, UGUIEvent sender)
		{
			Vector2 v5 = UIManager.GetInst(false).MainCnvas.worldCamera.ScreenToWorldPoint(pData.position);
			sender.transform.localPosition = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(v5);
			rope.end.localPosition = Utils.ScreenToLocalPos(pData.position, rope.mTrans);
		};
	}
}
