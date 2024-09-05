using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LIBII
{
	public class LiteSM
	{
		protected Dictionary<string, LiteState> mStates = new Dictionary<string, LiteState>();

		protected Dictionary<string, int> mIntParams = new Dictionary<string, int>();

		protected Dictionary<string, float> mFloatParams = new Dictionary<string, float>();

		protected Dictionary<string, long> mLongParams = new Dictionary<string, long>();

		protected Dictionary<string, bool> mBoolParams = new Dictionary<string, bool>();

		protected Dictionary<string, string> mStringParams = new Dictionary<string, string>();

		protected Dictionary<string, Vector3> mVec3Params = new Dictionary<string, Vector3>();

		protected Dictionary<string, GameObject> mGameObjectParams = new Dictionary<string, GameObject>();

		protected LiteState mCurState;

		protected string mNextState = "None";

		private bool _StateLocked_k__BackingField;

		public LiteState CurState
		{
			get
			{
				return this.mCurState;
			}
		}

		public string NextState
		{
			get
			{
				return this.mNextState;
			}
			set
			{
				if (this.StateLocked)
				{
					return;
				}
				if (this.CurState == null || this.CurState.Name != value)
				{
					this.mNextState = value;
					if (this.mNextState == "None")
					{
						if (this.CurState != null)
						{
							this.CurState.OnNotifyExit();
						}
						this.mCurState = null;
					}
				}
				else
				{
					this.mNextState = "None";
				}
			}
		}

		public bool StateLocked
		{
			get;
			set;
		}

		public LiteSM()
		{
			this.StateLocked = false;
		}

		public void Update(float deltaTime)
		{
			if (this.NextState != "None")
			{
				this.mStates[this.NextState].LastState = ((this.CurState != null) ? this.CurState.Name : "None");
				this.mStates[this.NextState].TickTime = 0f;
				string nextState = this.NextState;
				if (this.CurState != null)
				{
					this.CurState.OnNotifyExit();
				}
				this.mNextState = "None";
				this.mCurState = this.mStates[nextState];
				this.CurState.OnNotifyEnter();
			}
			else if (this.mCurState != null)
			{
				this.mCurState.TickTime += deltaTime;
				this.mCurState.OnNotifyUpdate();
			}
		}

		public void Clear()
		{
			this.mCurState = null;
			this.mStates.Clear();
		}

		public LiteState Add(string name, LiteState state)
		{
			state.Name = name;
			state.Performer = this;
			this.mStates.Add(name, state);
			return state;
		}

		public LiteState Add(string name, ED_1Param<LiteState> onEnter, ED_1Param<LiteState> onUpdate, ED_1Param<LiteState> onExit)
		{
			LiteState liteState = new LiteState();
			LiteState expr_07 = liteState;
			expr_07.OnUpdate = (ED_1Param<LiteState>)Delegate.Combine(expr_07.OnUpdate, onUpdate);
			LiteState expr_1E = liteState;
			expr_1E.OnEnter = (ED_1Param<LiteState>)Delegate.Combine(expr_1E.OnEnter, onEnter);
			LiteState expr_35 = liteState;
			expr_35.OnExit = (ED_1Param<LiteState>)Delegate.Combine(expr_35.OnExit, onExit);
			return this.Add(name, liteState);
		}

		public float SetFloatParam(string paramName, float v)
		{
			if (this.mFloatParams.ContainsKey(paramName))
			{
				this.mFloatParams[paramName] = v;
			}
			else
			{
				this.mFloatParams.Add(paramName, v);
			}
			return v;
		}

		public float GetFloatParam(string paramName, float defaultfloat = 0f)
		{
			return (!this.mFloatParams.ContainsKey(paramName)) ? defaultfloat : this.mFloatParams[paramName];
		}

		public long SetLongParam(string paramName, long v)
		{
			if (this.mLongParams.ContainsKey(paramName))
			{
				this.mLongParams[paramName] = v;
			}
			else
			{
				this.mLongParams.Add(paramName, v);
			}
			return v;
		}

		public long GetLongParam(string paramName, long defaultLong = 0L)
		{
			return (!this.mLongParams.ContainsKey(paramName)) ? defaultLong : this.mLongParams[paramName];
		}

		public int SetIntParam(string paramName, int v)
		{
			if (this.mIntParams.ContainsKey(paramName))
			{
				this.mIntParams[paramName] = v;
			}
			else
			{
				this.mIntParams.Add(paramName, v);
			}
			return v;
		}

		public int GetIntParam(string paramName, int defaultInt = 0)
		{
			return (!this.mIntParams.ContainsKey(paramName)) ? defaultInt : this.mIntParams[paramName];
		}

		public bool SetBoolParam(string paramName, bool v)
		{
			if (this.mBoolParams.ContainsKey(paramName))
			{
				this.mBoolParams[paramName] = v;
			}
			else
			{
				this.mBoolParams.Add(paramName, v);
			}
			return v;
		}

		public bool GetBoolParam(string paramName, bool defaultBool = false)
		{
			return (!this.mBoolParams.ContainsKey(paramName)) ? defaultBool : this.mBoolParams[paramName];
		}

		public string SetStringParam(string paramName, string v)
		{
			if (this.mStringParams.ContainsKey(paramName))
			{
				this.mStringParams[paramName] = v;
			}
			else
			{
				this.mStringParams.Add(paramName, v);
			}
			return v;
		}

		public string GetStringParam(string paramName, string defaultString = "")
		{
			return (!this.mStringParams.ContainsKey(paramName)) ? defaultString : this.mStringParams[paramName];
		}

		public Vector3 SetVec3Param(string paramName, Vector3 v)
		{
			if (this.mVec3Params.ContainsKey(paramName))
			{
				this.mVec3Params[paramName] = v;
			}
			else
			{
				this.mVec3Params.Add(paramName, v);
			}
			return v;
		}

		public Vector3 GetVec3Param(string paramName)
		{
			return this.GetVec3Param(paramName, Vector3.zero);
		}

		public Vector3 GetVec3Param(string paramName, Vector3 defaultVec3)
		{
			return (!this.mVec3Params.ContainsKey(paramName)) ? defaultVec3 : this.mVec3Params[paramName];
		}

		public GameObject SetGameObjParam(string paramName, GameObject go)
		{
			if (this.mGameObjectParams.ContainsKey(paramName))
			{
				this.mGameObjectParams[paramName] = go;
			}
			else
			{
				this.mGameObjectParams.Add(paramName, go);
			}
			return go;
		}

		public GameObject GetGameObjParam(string paramName)
		{
			return this.GetGameObjParam(paramName, null);
		}

		public GameObject GetGameObjParam(string paramName, GameObject defaultObj)
		{
			return (!this.mGameObjectParams.ContainsKey(paramName)) ? defaultObj : this.mGameObjectParams[paramName];
		}
	}
}
