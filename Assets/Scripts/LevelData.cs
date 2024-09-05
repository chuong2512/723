using System;

public class LevelData
{
	public string key;

	public int jsonIndex;

	public int starNum;

	public const int MaxStarNum = 3;

	private int _passGrade;

	public float gameTime;

	public LevelTemplate baseData;

	public string LevelJson;

	private float[] starTime;

	public int passGrade
	{
		get
		{
			return this._passGrade;
		}
		set
		{
			if (value > this._passGrade)
			{
				this._passGrade = value;
			}
		}
	}

	public float[] StarWaterCount
	{
		get
		{
			if (this.starTime == null)
			{
				string[] array = this.baseData.starWaterCount.Split(new char[]
				{
					'+'
				});
				this.starTime = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.starTime[i] = float.Parse(array[i]);
				}
			}
			return this.starTime;
		}
	}

	public LevelData(string key, int index)
	{
		this.key = key;
		this.jsonIndex = index;
		this.baseData = LevelTemplate.Tem(new object[]
		{
			key
		});
	}

	public void Reset()
	{
		this.starNum = 0;
	}

	public void BallDeathReset()
	{
		this.starNum = 0;
	}

	public bool IsLock()
	{
		if (this.key == SceneSetting.inst.FristLevelId || ChapterTemplate.Tem(new object[]
		{
			this.baseData.chapterId
		}).startLevel == this.key)
		{
			return false;
		}
		LevelData levelData = UserModel.Inst.GetLevelData(this.jsonIndex - 1);
		return levelData.passGrade <= 0;
	}
}
