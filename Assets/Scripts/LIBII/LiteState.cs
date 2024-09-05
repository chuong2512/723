using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LIBII
{
	public class LiteState
	{
		private string _Name_k__BackingField;

		private string _LastState_k__BackingField;

		private float _TickTime_k__BackingField;

		private int _LoopTimes_k__BackingField;

		private LiteSM _Performer_k__BackingField;

		public ED_1Param<LiteState> OnUpdate;

		public ED_1Param<LiteState> OnEnter;

		public ED_1Param<LiteState> OnExit;

		public string Name
		{
			get;
			set;
		}

		public string LastState
		{
			get;
			set;
		}

		public float TickTime
		{
			get;
			set;
		}

		public int LoopTimes
		{
			get;
			set;
		}

		public LiteSM Performer
		{
			get;
			set;
		}

		public LiteState()
		{
			this.TickTime = 0f;
			this.LastState = "None";
			this.LoopTimes = 0;
			this.Performer = null;
			this.Name = "None";
		}

		public virtual void OnNotifyUpdate()
		{
			this.LoopTimes++;
			if (this.OnUpdate != null)
			{
				this.OnUpdate(this);
			}
		}

		public virtual void OnNotifyEnter()
		{
			this.LoopTimes = 0;
			if (this.OnEnter != null)
			{
				this.OnEnter(this);
			}
		}

		public virtual void OnNotifyExit()
		{
			if (this.OnExit != null)
			{
				this.OnExit(this);
			}
		}
	}
}
