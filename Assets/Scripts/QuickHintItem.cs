using System;
using UnityEngine;
using UnityEngine.UI;

public class QuickHintItem : MonoBehaviour
{
	public GameObject go;

	private float height;

	public Image image;

	public Text text;

	private QuickHintItem last;

	private const float waitS = 1.5f;

	private float hasGoneS;

	private Vector3 riseSpeed = new Vector3(0f, 2f, 0f);

	public CanvasGroup canv;

	private const float perCutA = 0.05f;

	public bool IsDestroy;

	public static QuickHintItem Create(GameObject _go, Transform parent, QuickHintItem _last, string content)
	{
		if (_go == null || parent == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_go, parent, false);
		gameObject.SetActive(true);
		QuickHintItem quickHintItem = gameObject.AddComponent<QuickHintItem>();
		quickHintItem.go = gameObject;
		quickHintItem.image = gameObject.GetComponent<Image>();
		quickHintItem.text = gameObject.transform.Find("Lb_description").GetComponent<Text>();
		quickHintItem.text.text = content;
		quickHintItem.canv = gameObject.GetComponent<CanvasGroup>();
		quickHintItem.last = _last;
		quickHintItem.height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
		return quickHintItem;
	}

	private void Start()
	{
		this.CheakLast();
	}

	private void Update()
	{
		if (this.hasGoneS < 1.5f)
		{
			this.hasGoneS += Time.unscaledDeltaTime;
		}
		else
		{
			this.go.transform.localPosition += this.riseSpeed;
			this.canv.alpha -= 0.05f;
			if (this.canv.alpha <= 0f)
			{
				this.IsDestroy = true;
				UnityEngine.Object.Destroy(this.go);
			}
		}
	}

	public void Reset()
	{
		this.hasGoneS = 0f;
		this.go.transform.localPosition = Vector3.zero;
		this.canv.alpha = 1f;
	}

	public void CheakLast()
	{
		if (this.last == null || this.last.IsDestroy || this.last.canv.alpha <= 0f)
		{
			return;
		}
		float num = Mathf.Abs(this.go.transform.localPosition.y - this.last.go.transform.localPosition.y);
		if (num < this.height)
		{
			this.last.go.transform.localPosition += new Vector3(this.last.go.transform.localPosition.x, this.height - num, this.last.go.transform.localPosition.z);
			this.last.CheakLast();
		}
	}

	private void OnDestroy()
	{
		this.IsDestroy = true;
		this.go = null;
		this.last = null;
	}
}
