using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Interaction/ListScrollRect")]
public class ListScrollRect : MonoBehaviour
{
	private class ModelData
	{
		public int modelIndex;

		public RectTransform model;

		public int dataIndex;
	}

	public delegate void EventUpdateItem(int modelIndex, int dataIndex);

	public ScrollRect mScrollRect;

	public int columns = 1;

	public bool isAdaptive;

	public Vector2 CellSize = Vector2.zero;

	public Vector2 Spacing = Vector2.zero;

	public Vector2 GroupSize = Vector2.zero;

	public int mMaxNum;

	public bool isCenter;

	public float offsetMaxDown = 70f;

	private RectTransform mRectScrollRect;

	private RectTransform mRectSelf;

	private Vector3 UpdatePagePos;

	public int mGroupItemNum;

	private int mMaxGroupNum;

	private int mOneGroupMaxRow;

	private int mGroupMaxItemNum;

	private int mModelCount;

	private Func<int, int, Transform> mFun_InstantiateItem;

	private Func<int, Transform> mFun_InstantiateGroup;

	private List<ListScrollRect.ModelData> mlis_ModelData = new List<ListScrollRect.ModelData>();

	private Vector2[] conners = new Vector2[4];

	private Vector3 ScrollRectStartPos;

	public ED_Void ED_UpdatePage;

	public ListScrollRect.EventUpdateItem ED_UpdateItem;

	private void Awake()
	{
		this.CacheScrollView();
	}

	[ContextMenu("Sort Based on Scroll Movement")]
	public void SortBasedOnScrollMovement()
	{
		this.CacheScrollView();
		this.mMaxNum = 999;
		this.mlis_ModelData.Clear();
		for (int i = 0; i < this.mScrollRect.content.transform.childCount; i++)
		{
			ListScrollRect.ModelData modelData = new ListScrollRect.ModelData();
			modelData.model = (RectTransform)this.mScrollRect.content.transform.GetChild(i);
			this.mlis_ModelData.Add(modelData);
		}
		this.mModelCount = this.mlis_ModelData.Count;
		this.InitLayoutData();
		this.ResetChildPositions();
	}

	protected void CacheScrollView()
	{
		this.mRectScrollRect = (RectTransform)this.mScrollRect.transform;
		this.mRectSelf = (RectTransform)base.transform;
		this.mScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.WrapContent));
	}

	public void SetInitContentPos(Vector2 ve)
	{
		this.mRectSelf.anchoredPosition = ve;
		this.ResetChildPositions();
	}

	public void InitViewTran(Func<int, int, Transform> fun)
	{
		this.mFun_InstantiateItem = fun;
		this.mMaxNum = 0;
		this.InitLayoutData();
		this.InstantiateItem();
	}

	public void ListViewTranCount(int count, Transform tran = null)
	{
		this.mMaxNum = count;
		this.InitLayoutData();
		this.ResetChildPositions();
	}

	public void ListViewTranCount(int count, Func<int, int, Transform> fun, int groupitemnum = 0, Func<int, Transform> groupfun = null)
	{
		if (this.mFun_InstantiateItem == null)
		{
			this.CacheScrollView();
		}
		this.mFun_InstantiateItem = fun;
		this.mMaxNum = count;
		this.mFun_InstantiateGroup = groupfun;
		this.mGroupItemNum = groupitemnum;
		this.InitLayoutData();
		this.InstantiateItem();
		this.InstantiateGroup();
		this.ResetChildPositions();
	}

	public void UpdateShow()
	{
		this.ResetChildPositions();
	}

	public Vector2 GetAnchoredPositionByDataIndex(int dataIndex)
	{
		Vector2 result = default(Vector2);
		if (this.mGroupItemNum > 0)
		{
			int num = dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + dataIndex % this.mGroupItemNum;
			result = ((!this.mScrollRect.vertical) ? new Vector2((float)(num / this.columns) * this.CellSize.x + (float)(num / this.mGroupMaxItemNum) * this.GroupSize.x, (float)(-(float)(num % this.columns)) * this.CellSize.y + this.CellSize.y) : new Vector2((float)(num % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(num / this.columns)) * this.CellSize.y - (float)(num / this.mGroupMaxItemNum) * this.GroupSize.y));
		}
		else
		{
			result = ((!this.mScrollRect.vertical) ? new Vector2((float)(dataIndex / this.columns) * this.CellSize.x, (float)(dataIndex % this.columns) * this.CellSize.y) : new Vector2((float)(dataIndex % this.columns) * this.CellSize.x, (float)(dataIndex / this.columns) * this.CellSize.y));
		}
		return result;
	}

	private void InstantiateItem()
	{
		if (this.mFun_InstantiateItem == null)
		{
			return;
		}
		for (int i = 0; i < this.mlis_ModelData.Count; i++)
		{
			UnityEngine.Object.Destroy(this.mlis_ModelData[i].model);
		}
		this.mlis_ModelData.Clear();
		for (int j = 0; j < this.mModelCount; j++)
		{
			if (j >= this.mMaxNum)
			{
				break;
			}
			Transform transform = this.mFun_InstantiateItem(j, j);
			transform.name = j + string.Empty;
			transform.SetParent(base.transform);
			transform.localScale = Vector3.one;
			transform.gameObject.SetActive(true);
			transform.localPosition = new Vector3(-this.mRectSelf.rect.width * 0.5f - 9999f, -this.mRectSelf.rect.height * 0.5f - 9999f);
			ListScrollRect.ModelData modelData = new ListScrollRect.ModelData();
			modelData.dataIndex = j;
			modelData.modelIndex = j;
			modelData.model = (RectTransform)transform;
			this.mlis_ModelData.Add(modelData);
		}
	}

	private void InstantiateGroup()
	{
		if (this.mFun_InstantiateGroup == null)
		{
			return;
		}
		for (int i = 0; i < this.mMaxGroupNum; i++)
		{
			Transform transform = this.mFun_InstantiateGroup(i);
			transform.name = "Group_" + i;
			transform.parent = base.transform;
			transform.localScale = Vector3.one;
			transform.gameObject.SetActive(true);
			transform.localPosition = ((!this.mScrollRect.vertical) ? new Vector2((float)(i * this.mOneGroupMaxRow) * this.CellSize.x + (float)i * this.GroupSize.x + this.GroupSize.x * 0.5f, -this.mRectSelf.sizeDelta.y * 0.5f) : new Vector2(this.mRectSelf.sizeDelta.x * 0.5f, (float)(-(float)i * this.mOneGroupMaxRow) * this.CellSize.y - (float)i * this.GroupSize.y - this.GroupSize.y * 0.5f));
		}
	}

	private void InitLayoutData()
	{
		if (this.isAdaptive)
		{
			if (this.mScrollRect.vertical)
			{
				float width = this.mRectScrollRect.rect.width;
				this.columns = Mathf.FloorToInt(width / (this.CellSize.x + this.Spacing.x));
			}
			else
			{
				float height = this.mRectScrollRect.rect.height;
				this.columns = Mathf.FloorToInt(height / (this.CellSize.y + this.Spacing.y));
			}
		}
		if (this.mScrollRect.vertical)
		{
			this.mRectSelf.sizeDelta = new Vector2((this.CellSize.x + this.Spacing.x) * (float)this.columns - this.Spacing.x, (float)Mathf.CeilToInt((float)this.mMaxNum / (float)this.columns) * (this.CellSize.y + this.Spacing.y) - this.Spacing.y + this.offsetMaxDown);
			this.mModelCount = Mathf.FloorToInt(this.mRectScrollRect.rect.height / this.CellSize.y) * this.columns + 2 * this.columns;
			if (this.mGroupItemNum > 0)
			{
				this.mOneGroupMaxRow = this.mGroupItemNum / this.columns + ((this.mGroupItemNum % this.columns <= 0) ? 0 : 1);
				this.mGroupMaxItemNum = this.mOneGroupMaxRow * this.columns;
				this.mMaxGroupNum = this.mMaxNum / this.mGroupItemNum + ((this.mMaxNum % this.mGroupItemNum <= 0) ? 0 : 1);
				this.mRectSelf.sizeDelta = new Vector2((this.CellSize.x + this.Spacing.x) * (float)this.columns - this.Spacing.x, (this.GroupSize.y + this.Spacing.y) * (float)this.mMaxGroupNum + (float)(this.mMaxGroupNum * this.mOneGroupMaxRow) * (this.CellSize.y + this.Spacing.y) - this.Spacing.y + this.offsetMaxDown);
			}
			if (this.isCenter)
			{
				this.mRectSelf.anchoredPosition = new Vector2((this.mRectScrollRect.rect.width - this.mRectSelf.sizeDelta.x) * 0.5f, this.mRectSelf.anchoredPosition.y);
			}
		}
		else
		{
			this.mRectSelf.sizeDelta = new Vector2((float)Mathf.CeilToInt((float)this.mMaxNum / (float)this.columns) * (this.CellSize.x + this.Spacing.x) - this.Spacing.x, (this.CellSize.y + this.Spacing.y) * (float)this.columns - this.Spacing.y);
			this.mModelCount = Mathf.FloorToInt(this.mRectScrollRect.rect.width / this.CellSize.x) * this.columns + 2 * this.columns;
			if (this.mGroupItemNum > 0)
			{
				this.mOneGroupMaxRow = this.mGroupItemNum / this.columns + ((this.mGroupItemNum % this.columns <= 0) ? 0 : 1);
				this.mGroupMaxItemNum = this.mOneGroupMaxRow * this.columns;
				this.mMaxGroupNum = this.mMaxNum / this.mGroupItemNum + ((this.mMaxNum % this.mGroupItemNum <= 0) ? 0 : 1);
				this.mRectSelf.sizeDelta = new Vector2((this.GroupSize.x + this.Spacing.x) * (float)this.mMaxGroupNum + (float)(this.mMaxGroupNum * this.mOneGroupMaxRow) * (this.CellSize.x + this.Spacing.x) - this.Spacing.x, (this.CellSize.y + this.Spacing.y) * (float)this.columns - this.Spacing.y);
			}
			if (this.isCenter)
			{
				this.mRectSelf.anchoredPosition = new Vector2(0f, -(this.mRectScrollRect.rect.height - this.mRectSelf.sizeDelta.y) * 0.5f);
			}
		}
		Vector3 vector = this.mRectScrollRect.rect.size;
		this.ScrollRectStartPos = this.mRectScrollRect.transform.position;
		this.conners[0] = new Vector3(-vector.x / 2f, vector.y / 2f, 0f);
		this.conners[1] = new Vector3(vector.x / 2f, vector.y / 2f, 0f);
		this.conners[2] = new Vector3(-vector.x / 2f, -vector.y / 2f, 0f);
		this.conners[3] = new Vector3(vector.x / 2f, -vector.y / 2f, 0f);
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector2 = this.conners[i];
			vector2 = this.mRectScrollRect.TransformPoint(vector2);
			this.conners[i] = vector2;
		}
		if (this.mScrollRect.vertical)
		{
			this.UpdatePagePos = Vector3.Lerp(this.conners[2], this.conners[3], 0.5f);
		}
		else
		{
			this.UpdatePagePos = Vector3.Lerp(this.conners[0], this.conners[3], 0.5f);
		}
	}

	private void ResetChildPositions()
	{
		for (int i = 0; i < this.mlis_ModelData.Count; i++)
		{
			this.mlis_ModelData[i].dataIndex = i;
			this.CallEventUpdateItem(true, this.mlis_ModelData[i].modelIndex, this.mlis_ModelData[i].dataIndex);
			if (this.mGroupItemNum > 0)
			{
				int num = this.mlis_ModelData[i].dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + this.mlis_ModelData[i].dataIndex % this.mGroupItemNum;
				this.mlis_ModelData[i].model.localPosition = ((!this.mScrollRect.vertical) ? new Vector2((float)(num / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f + (float)(num / this.mGroupMaxItemNum + 1) * this.GroupSize.x, (float)(-(float)(num % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f) : new Vector2((float)(num % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(num / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f - (float)(num / this.mGroupMaxItemNum + 1) * this.GroupSize.y));
			}
			else
			{
				this.mlis_ModelData[i].model.localPosition = ((!this.mScrollRect.vertical) ? new Vector2((float)(i / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(i % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f) : new Vector2((float)(i % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(i / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f));
			}
		}
		this.WrapContent(true);
	}

	private void Update()
	{
		if (this.ScrollRectStartPos != this.mRectScrollRect.transform.position)
		{
			Vector3 vector = this.mRectScrollRect.rect.size;
			this.conners[0] = new Vector3(-vector.x / 2f, vector.y / 2f, 0f);
			this.conners[1] = new Vector3(vector.x / 2f, vector.y / 2f, 0f);
			this.conners[2] = new Vector3(-vector.x / 2f, -vector.y / 2f, 0f);
			this.conners[3] = new Vector3(vector.x / 2f, -vector.y / 2f, 0f);
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector2 = this.conners[i];
				vector2 = this.mRectScrollRect.TransformPoint(vector2);
				this.conners[i] = vector2;
			}
		}
	}

	private void WrapContent(Vector2 arg0)
	{
		this.WrapContent(false);
	}

	public void WrapContent(bool isUpdate = false)
	{
		Vector3[] array = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = this.mRectSelf.InverseTransformPoint(this.conners[i]);
		}
		Vector3 vector = Vector3.Lerp(array[0], array[3], 0.5f);
		if (!this.mScrollRect.vertical)
		{
			float num = this.CellSize.x * (float)this.mlis_ModelData.Count / (float)this.columns * 0.5f;
			float num2 = array[0].x - this.CellSize.x * 2f;
			float num3 = array[3].x + this.CellSize.x * 2f;
			for (int j = 0; j < this.mlis_ModelData.Count; j++)
			{
				Transform model = this.mlis_ModelData[j].model;
				float num4 = model.localPosition.x - vector.x;
				while (num4 < -num)
				{
					if (this.mlis_ModelData[j].dataIndex + this.mModelCount >= this.mMaxNum)
					{
						break;
					}
					this.mlis_ModelData[j].dataIndex = this.mlis_ModelData[j].dataIndex + this.mModelCount;
					if (this.mGroupItemNum > 0)
					{
						int num5 = this.mlis_ModelData[j].dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + this.mlis_ModelData[j].dataIndex % this.mGroupItemNum;
						model.localPosition = new Vector3((float)(num5 / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f + (float)(num5 / this.mGroupMaxItemNum + 1) * this.GroupSize.x, (float)(-(float)(num5 % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f);
					}
					else
					{
						model.localPosition = new Vector3((float)(this.mlis_ModelData[j].dataIndex / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(this.mlis_ModelData[j].dataIndex % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f, 0f);
					}
					num4 = model.localPosition.x - vector.x;
					this.CallEventUpdateItem(true, this.mlis_ModelData[j].modelIndex, this.mlis_ModelData[j].dataIndex);
				}
				if (num4 > num)
				{
					if (this.mlis_ModelData[j].dataIndex - this.mModelCount >= 0)
					{
						this.mlis_ModelData[j].dataIndex = this.mlis_ModelData[j].dataIndex - this.mModelCount;
						if (this.mGroupItemNum > 0)
						{
							int num6 = this.mlis_ModelData[j].dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + this.mlis_ModelData[j].dataIndex % this.mGroupItemNum;
							model.localPosition = new Vector3((float)(num6 / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f + (float)(num6 / this.mGroupMaxItemNum + 1) * this.GroupSize.x, (float)(-(float)(num6 % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f);
						}
						else
						{
							model.localPosition = new Vector3((float)(this.mlis_ModelData[j].dataIndex / this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(this.mlis_ModelData[j].dataIndex % this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f, 0f);
						}
						num4 = model.localPosition.x - vector.x;
						this.CallEventUpdateItem(true, this.mlis_ModelData[j].modelIndex, this.mlis_ModelData[j].dataIndex);
					}
				}
				else if (num4 >= -num && num4 <= num)
				{
					this.CallEventUpdateItem(isUpdate, this.mlis_ModelData[j].modelIndex, this.mlis_ModelData[j].dataIndex);
				}
				if (this.mlis_ModelData[j].dataIndex >= this.mMaxNum)
				{
					model.gameObject.SetActive(false);
				}
				else
				{
					float x = model.transform.localPosition.x;
				}
			}
			if (this.ED_UpdatePage != null)
			{
				Vector3 vector2 = Vector3.Lerp(array[2], array[3], 0.5f);
				if (this.UpdatePagePos.y - vector2.y > 50f)
				{
					this.ED_UpdatePage();
					this.ED_UpdatePage = null;
				}
			}
		}
		else
		{
			float num7 = this.CellSize.y * (float)this.mlis_ModelData.Count / (float)this.columns * 0.5f;
			float num8 = array[0].y + this.CellSize.y * 2f;
			float num9 = array[2].y - this.CellSize.y * 2f;
			for (int k = 0; k < this.mlis_ModelData.Count; k++)
			{
				Transform model2 = this.mlis_ModelData[k].model;
				float num10 = model2.localPosition.y - vector.y;
				while (num10 < -num7)
				{
					if (this.mlis_ModelData[k].dataIndex - this.mModelCount < 0)
					{
						break;
					}
					this.mlis_ModelData[k].dataIndex = this.mlis_ModelData[k].dataIndex - this.mModelCount;
					if (this.mGroupItemNum > 0)
					{
						int num11 = this.mlis_ModelData[k].dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + this.mlis_ModelData[k].dataIndex % this.mGroupItemNum;
						model2.localPosition = new Vector3((float)(num11 % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(num11 / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f - (float)(num11 / this.mGroupMaxItemNum + 1) * this.GroupSize.y);
					}
					else
					{
						model2.localPosition = new Vector3((float)(this.mlis_ModelData[k].dataIndex % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(this.mlis_ModelData[k].dataIndex / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f);
					}
					num10 = model2.localPosition.y - vector.y;
					this.CallEventUpdateItem(true, this.mlis_ModelData[k].modelIndex, this.mlis_ModelData[k].dataIndex);
				}
				while (num10 > num7)
				{
					if (this.mlis_ModelData[k].dataIndex + this.mModelCount >= this.mMaxNum)
					{
						break;
					}
					this.mlis_ModelData[k].dataIndex = this.mlis_ModelData[k].dataIndex + this.mModelCount;
					if (this.mGroupItemNum > 0)
					{
						int num12 = this.mlis_ModelData[k].dataIndex / this.mGroupItemNum * this.mGroupMaxItemNum + this.mlis_ModelData[k].dataIndex % this.mGroupItemNum;
						model2.localPosition = new Vector3((float)(num12 % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(num12 / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f - (float)(num12 / this.mGroupMaxItemNum + 1) * this.GroupSize.y);
					}
					else
					{
						model2.localPosition = new Vector3((float)(this.mlis_ModelData[k].dataIndex % this.columns) * this.CellSize.x + this.CellSize.x * 0.5f, (float)(-(float)(this.mlis_ModelData[k].dataIndex / this.columns)) * this.CellSize.y - this.CellSize.y * 0.5f);
					}
					num10 = model2.localPosition.y - vector.y;
					this.CallEventUpdateItem(true, this.mlis_ModelData[k].modelIndex, this.mlis_ModelData[k].dataIndex);
				}
				if (num10 >= -num7 && num10 <= num7 && this.mlis_ModelData[k].dataIndex < this.mMaxNum)
				{
					this.CallEventUpdateItem(isUpdate, this.mlis_ModelData[k].modelIndex, this.mlis_ModelData[k].dataIndex);
				}
				if (this.mlis_ModelData[k].dataIndex >= this.mMaxNum)
				{
					model2.gameObject.SetActive(false);
				}
			}
			if (this.ED_UpdatePage != null)
			{
				Vector3 vector3 = Vector3.Lerp(array[0], array[3], 0.5f);
				if (this.UpdatePagePos.y - vector3.y > 50f)
				{
					this.ED_UpdatePage();
					this.ED_UpdatePage = null;
				}
			}
		}
	}

	private void CallEventUpdateItem(bool isUpdate, int modelIndex, int dataIndex)
	{
		if (isUpdate && this.ED_UpdateItem != null)
		{
			this.ED_UpdateItem(modelIndex, dataIndex);
		}
	}
}
