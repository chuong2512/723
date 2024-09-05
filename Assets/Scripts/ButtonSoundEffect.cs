using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour
{
	public enum ButtonType
	{
		Nomal,
		Use,
		Coins
	}

	public ButtonSoundEffect.ButtonType type;

	private void Start()
	{
		Button component = base.gameObject.GetComponent<Button>();
		if (component != null)
		{
			component.onClick.AddListener(delegate
			{
				if (!Voice.GetVoice())
				{
					return;
				}
				if (this.type == ButtonSoundEffect.ButtonType.Nomal)
				{
					Common.PlaySoundEffect("Common:button");
				}
				else if (this.type == ButtonSoundEffect.ButtonType.Use)
				{
					Common.PlaySoundEffect("Common:newbutton");
				}
				else if (this.type == ButtonSoundEffect.ButtonType.Coins)
				{
					Common.PlaySoundEffect("SFX:coins");
				}
			});
		}
		else if (base.gameObject.GetComponent<Toggle>() != null)
		{
			base.gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool v)
			{
				if (!Voice.GetVoice())
				{
					return;
				}
				if (this.type == ButtonSoundEffect.ButtonType.Nomal)
				{
					Common.PlaySoundEffect("Common:button");
				}
				else if (this.type == ButtonSoundEffect.ButtonType.Use)
				{
					Common.PlaySoundEffect("Common:newbutton");
				}
				else if (this.type == ButtonSoundEffect.ButtonType.Coins)
				{
					Common.PlaySoundEffect("SFX:coins");
				}
			});
		}
	}
}
