using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuitView : BaseView
{
	private static UnityAction __f__mg_cache0;

	private void Awake()
	{
		base.Find<Button>(base.transform, "Btn_Cancel").onClick.AddListener(new UnityAction(this.Close));
		UnityEvent arg_5B_0 = base.Find<Button>(base.transform, "Btn_ok").onClick;
		if (QuitView.__f__mg_cache0 == null)
		{
			QuitView.__f__mg_cache0 = new UnityAction(Application.Quit);
		}
		arg_5B_0.AddListener(QuitView.__f__mg_cache0);
	}
}
