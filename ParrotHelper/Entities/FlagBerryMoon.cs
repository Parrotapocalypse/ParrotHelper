using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using System;
using MonoMod.Utils;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/FlagBerryMoon")]
	[TrackedAs(typeof(Strawberry))]
	[RegisterStrawberry(false, false)]
	public class FlagBerryMoon : FlagBerry
	{
		public FlagBerryMoon(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
		{
			flag = data.Attr("flag", "flagberrymoon");
			set = data.Bool("set", true);
			dynData["Moon"] = true;
		}
	}
}