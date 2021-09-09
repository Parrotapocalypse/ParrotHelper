using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Celeste;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/FlagBerryGold")]
	[TrackedAs(typeof(Strawberry))]
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
			Session session = SceneAs<Level>().Session;
			base.Awake(scene);
			if (Winged && !(session.Dashes == 0 && session.StartedFromBeginning))
			{
				RemoveSelf();
			}
		}
	}
}