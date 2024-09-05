using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class MultMoneyView : BaseView
{
	public override void Init(params object[] args)
	{
		if (args.Length >= 1)
		{
			Common.PlaySoundEffect("SFX:award");
			AwardData awardData = args[0] as AwardData;
			AwardData.AwardType awardType = awardData.awardType;
			if (awardType != AwardData.AwardType.Gold)
			{
				if (awardType == AwardData.AwardType.Hint)
				{
					base.transform.Find("Hint").gameObject.SetActive(true);
				}
			}
			else
			{
				base.transform.Find("Money").gameObject.SetActive(true);
			}
			base.Find<Text>(base.transform, "Text").text = awardData.count.ToString();
		}
		base.Find<Button>(base.transform, "Close").onClick.AddListener(new UnityAction(this.Close));
	}
}
