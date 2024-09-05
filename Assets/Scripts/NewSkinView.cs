using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewSkinView : BaseView
{
	private Button useBtn;

	private Button noUseBtn;

	private SkinTemplate skin;

	private Action<bool> UseAction;

	private MeshRenderer skinRenderer;

	private void Awake()
	{
		this.useBtn = base.Find<Button>(base.transform, "Btn/Use");
		this.noUseBtn = base.Find<Button>(base.transform, "Btn/NoUse");
		this.useBtn.onClick.AddListener(new UnityAction(this.UseBtnClick));
		this.noUseBtn.onClick.AddListener(new UnityAction(this.NoUseBtnClick));
		this.skinRenderer = base.Find<MeshRenderer>(base.transform, "mc");
	}

	public override void Init(params object[] args)
	{
		if (args.Length >= 1)
		{
			this.skin = (args[0] as SkinTemplate);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetTexture("_MainTex", Resources.Load<Texture>(this.skin.aniIcon + "/mc"));
			this.skinRenderer.SetPropertyBlock(materialPropertyBlock);
		}
		if (args.Length >= 2)
		{
			this.UseAction = (args[1] as Action<bool>);
		}
		Common.PlaySoundEffect("SFX:award");
	}

	private void UseBtnClick()
	{
		UserModel.UsedSkinKey = this.skin.key;
		if (this.UseAction != null)
		{
			this.UseAction(true);
		}
		this.Close();
	}

	private void NoUseBtnClick()
	{
		if (this.UseAction != null)
		{
			this.UseAction(false);
		}
		this.Close();
	}
}
