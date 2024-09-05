using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EditLevelItemView : MonoBehaviour
{
	private sealed class _Init_c__AnonStorey0
	{
		internal Action<string> callBack;

		internal EditLevelItemView _this;

		internal void __m__0()
		{
			this.callBack(this._this.levelNum.text);
			UIManager.GetInst(false).GetOpenWindow<EditorListWindow>().Close();
		}
	}

	private RawImage rawImage;

	private LevelStage _levelStage;

	private Transform renderTrans;

	private Transform hereTrans;

	private Text levelNum;

	private Image perfactImage;

	private Button btn;

	private Image backImage;

	public LevelStage LevelStage
	{
		get
		{
			return this._levelStage;
		}
	}

	private void Awake()
	{
		this.rawImage = base.GetComponent<RawImage>();
		this.levelNum = Utils.Find<Text>(base.transform, "Text");
		this.hereTrans = base.transform.Find("Here");
		this.btn = this.rawImage.GetComponent<Button>();
	}

	public void Init(int id, Action<string> callBack)
	{
		this.btn.onClick.AddListener(delegate
		{
			callBack(this.levelNum.text);
			UIManager.GetInst(false).GetOpenWindow<EditorListWindow>().Close();
		});
	}

	public void UpdateItem(int id)
	{
		this.rawImage.texture = this.LoadTex(id.ToString());
		this.levelNum.text = id.ToString();
	}

	public Texture2D LoadTex(string id)
	{
		string path = EditorData.GetSaveTexturePath() + "/" + id + ".png";
		Texture2D texture2D = new Texture2D(215, 215);
		byte[] data;
		try
		{
			data = File.ReadAllBytes(path);
		}
		catch (Exception var_3_32)
		{
			return null;
		}
		texture2D.LoadImage(data);
		return texture2D;
	}
}
