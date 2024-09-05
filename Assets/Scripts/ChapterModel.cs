using System;
using System.Collections.Generic;
using System.Linq;

public class ChapterModel
{
	private static ChapterModel inst;

	public static ChapterModel Inst
	{
		get
		{
			if (ChapterModel.inst == null)
			{
				ChapterModel.inst = new ChapterModel();
			}
			return ChapterModel.inst;
		}
	}

	private ChapterModel()
	{
	}

	public static List<LevelData> GetChapterAllLevel(ChapterTemplate chapter)
	{
		List<LevelData> list = new List<LevelData>();
		for (int i = 0; i < chapter.num; i++)
		{
			LevelData levelData = UserModel.Inst.GetLevelData((int.Parse(chapter.startLevel) + i).ToString());
			if (levelData != null)
			{
				list.Add(levelData);
			}
		}
		return list;
	}

	public static int AllLevelStarNum(string chapterId)
	{
		int num = 0;
		ChapterTemplate chapterTemplate = ChapterTemplate.Tem(new object[]
		{
			chapterId
		});
		if (chapterTemplate == null)
		{
			return 0;
		}
		for (int i = 0; i < chapterTemplate.num; i++)
		{
			LevelData levelData = UserModel.Inst.GetLevelData((int.Parse(chapterTemplate.startLevel) + i).ToString());
			if (levelData != null && levelData.passGrade > 0)
			{
				num += levelData.passGrade;
			}
		}
		return num;
	}

	public static int ChapterIsOpen(string curChapterId)
	{
		if (ChapterTemplate.Dic().Keys.First<string>() == curChapterId)
		{
			return -1;
		}
		ChapterTemplate chapterTemplate = ChapterTemplate.Tem(new object[]
		{
			curChapterId
		});
		return chapterTemplate.lockStarNum - UserModel.Inst.GetStarCount();
	}
}
