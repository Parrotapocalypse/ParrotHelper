using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System.Reflection;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/StaminaBerry")]
	[RegisterStrawberry(false, true)]
	[TrackedAs(typeof(Strawberry))]
	class StaminaBerry : Strawberry, IStrawberry
	{
		private static bool flyAway = false;
		private DynData<Strawberry> dynData;
		private static MethodInfo strawberryFlyAway = typeof(Strawberry).GetMethod("FlyAwayRoutine", BindingFlags.Instance | BindingFlags.NonPublic);
		public StaminaBerry(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
		{
			dynData = new DynData<Strawberry>(this);
			dynData["Winged"] = data.Bool("winged", false);
			dynData["Golden"] = true;
		}
		public override void Awake(Scene scene)
		{
			base.Awake(scene);
			if (flyAway) RemoveSelf();
		}
		public override void Update()
		{
			base.Update();
			if (flyAway) Add(new Coroutine((System.Collections.IEnumerator)strawberryFlyAway.Invoke(this, new object[] {}), true));
		}
		public static void Load()
		{
			On.Celeste.Player.Update += Player_Update; 
		}

		private static void Player_Update(On.Celeste.Player.orig_Update orig, Player self)
		{
			orig(self);
			if (self.Stamina == 0) flyAway = true;
		}
		public static void Unload()
		{
			On.Celeste.Player.Update -= Player_Update;
		}
	}
}
