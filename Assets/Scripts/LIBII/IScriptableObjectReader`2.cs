using System;
using UnityEngine;

namespace LIBII
{
	public abstract class IScriptableObjectReader<READER_T, ASSET_T> where READER_T : IScriptableObjectReader<READER_T, ASSET_T>, new() where ASSET_T : ScriptableObject
	{
		private static READER_T s_reader = Activator.CreateInstance<READER_T>();

		private static ASSET_T s_asset = (ASSET_T)((object)null);

		public static ASSET_T ScriptableObject
		{
			get
			{
				if (IScriptableObjectReader<READER_T, ASSET_T>.s_asset == null)
				{
					string text = IScriptableObjectReader<READER_T, ASSET_T>.s_reader.ScriptableObjectAssetNameInResources();
					IScriptableObjectReader<READER_T, ASSET_T>.s_asset = Resources.Load<ASSET_T>(text);
					if (IScriptableObjectReader<READER_T, ASSET_T>.s_asset == null)
					{
						UnityEngine.Debug.LogError("Resources 资源目录中无法找到 配置文件 -> " + text);
					}
				}
				return IScriptableObjectReader<READER_T, ASSET_T>.s_asset;
			}
		}

		protected abstract string ScriptableObjectAssetNameInResources();
	}
}
