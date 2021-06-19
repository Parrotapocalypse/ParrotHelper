using Monocle;
using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Celeste.Mod.FriendlyHelper.Entities
{
	[CustomEntity("FriendlyHelper/FlagBerry")]
	[RegisterStrawberry(true, false)]
	[Tracked]
	public class FlagBerry : Strawberry, IStrawberry
	{
		public static string flag;
		private static bool set;
		public FlagBerry(EntityData data, Vector2 offset) : base(data, offset, new EntityID(data.Level.Name, data.ID))
		{
			flag = data.Attr("Flag", "berry_flag");
			set = data.Bool("Set", true);
		}
		public override void Added(Scene scene)
		{
			//On.Celeste.Strawberry.CollectRoutine += onStrawberryCollect;
			base.Added(scene);
			Session session = SceneAs<Level>().Session;
			if (!SaveData.Instance.CheckStrawberry(this.ID) || Position == null)
			{
				if (set)
				{
					session.SetFlag(flag);
				}
				else
				{
					session.SetFlag(flag, false);
				}
			}
		}
		private IEnumerator onStrawberryCollect(On.Celeste.Strawberry.orig_CollectRoutine orig, Strawberry self, int collectIndex)
		{
			IEnumerator output = orig(self, collectIndex);
			while (output.MoveNext())
			{
				yield return null;
			}
			if (ReferenceEquals(self, this))
			{
				Session session = self.SceneAs<Level>().Session;
				if (set)
				{
					session.SetFlag(flag);
				}
				else
				{
					session.SetFlag(flag, false);
				}
				RemoveSelf();
			}
			yield break;
		}
	}
}
