using System;
using UnityEngine;

public class EditorScene : PBase
{
	public static EditorScene Inst;

	[HideInInspector]
	public EditorUIWindow editorUi;

	public string DeafultLevelPath = "D:\\NewUnityProject";

	public string levelFilePath
	{
		get
		{
			return PlayerPrefs.GetString("levelFilePath", this.DeafultLevelPath);
		}
		set
		{
			PlayerPrefs.SetString("levelFilePath", value);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		EditorScene.Inst = this;
		LevelStage.LoadAllObjToEditing(string.Empty);
		this.editorUi = UIManager.OpenWindow<EditorUIWindow>(new object[0]);
	}

	private void Update()
	{
		base.MainCameraAdjust();
	}
}
