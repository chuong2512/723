using System;

public class AwardData
{
	public enum AwardType
	{
		Gold = 1,
		Hint,
		Skin
	}

	public AwardData.AwardType awardType;

	public int count;

	public string skinId;

	public void AddToUser()
	{
		AwardData.AwardType awardType = this.awardType;
		if (awardType != AwardData.AwardType.Gold)
		{
			if (awardType != AwardData.AwardType.Hint)
			{
				if (awardType == AwardData.AwardType.Skin)
				{
					UserModel.BuyGoods(this.skinId, true);
				}
			}
			else
			{
				UserModel.Inst.HintCount += this.count;
			}
		}
		else
		{
			UserModel.Inst.Money += this.count;
		}
	}
}
