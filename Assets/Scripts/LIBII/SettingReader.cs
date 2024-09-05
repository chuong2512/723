using System;

namespace LIBII
{
	public class SettingReader : IScriptableObjectReader<SettingReader, SettingAsset>
	{
		protected override string ScriptableObjectAssetNameInResources()
		{
			return "SettingAsset";
		}
	}
}
