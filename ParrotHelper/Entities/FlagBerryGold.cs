using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Celeste;
using System;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/FlagBerryGold")]
	[TrackedAs(typeof(Strawberry))]
	[RegisterStrawberry(false, true)]
	class FlagBerryGold : FlagBerry
	{
		public FlagBerryGold(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
		{
			flag = data.Attr("flag", "flagberrygold");
			set = data.Bool("set", true);
			dynData["Golden"] = true;
		}
		public override void Awake(Scene scene)
		{
			Level level = scene as Level;
			base.Awake(scene);
			{
				bool cheatMode = SaveData.Instance.CheatMode;
				bool flag6 = level.Session.FurthestSeenLevel == level.Session.Level || level.Session.Deaths == 0;
				bool flag7 = SaveData.Instance.UnlockedModes >= 3 || SaveData.Instance.DebugMode;
				bool completed = SaveData.Instance.Areas_Safe[level.Session.Area.ID].Modes[(int)level.Session.Area.Mode].Completed;
				if (!Winged)
				{
					if (!((cheatMode || (flag7 && completed)) && flag6))
					{
						RemoveSelf();
					}
				}
				else if (Winged)
				{
					if (!(level.Session.Dashes == 0 && level.Session.StartedFromBeginning))
					{
						RemoveSelf();
					}
				}
			}
		}
	}
}