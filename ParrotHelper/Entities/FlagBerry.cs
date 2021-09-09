using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using System.Collections;
using System;
using System.Collections.Generic;
using MonoMod.Utils;
using System.Reflection;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/FlagBerry")]
	[TrackedAs(typeof(Strawberry))]
	class FlagBerry : Strawberry, IStrawberry
	{
		protected string flag;
		protected bool set;
		protected DynData<Strawberry> dynData;
		protected bool n = true;
		public FlagBerry(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
		{
			flag = data.Attr("flag", "flagberry");
			set = data.Bool("set", true);
			dynData = new DynData<Strawberry>(this);
			dynData["Winged"] = data.Bool("winged", false);
		}
		public override void Update()
		{
			base.Update();
			if (dynData.Get<bool>("collected") == true && n)
			{
				n = false;
				(Scene as Level).Session.SetFlag(flag, set);
			}
		}

	}
}