using Monocle;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System.Reflection;
using MonoMod.Cil;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/FlagBerry")]
	[TrackedAs(typeof(Strawberry))]
	[RegisterStrawberry(true, false)]
	public class FlagBerry : Strawberry, IStrawberry
	{
		protected string flag;
		protected bool set;
		protected DynData<Strawberry> dynData;
		protected bool n = true;
		protected Sprite sprite;
		public FlagBerry(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
		{
			flag = data.Attr("flag", "flag_berry");
			set = data.Bool("set", true);
			dynData = new DynData<Strawberry>(this);
			dynData["Winged"] = data.Bool("winged");
		}
		public override void Update()
		{
			base.Update();
			if (n && dynData.Get<bool>("collected") == true)
			{
				n = false;
				(Scene as Level).Session.SetFlag(flag, set);
			}
		}
		public void Load()
		{
            IL.Celeste.Strawberry.Added += Strawberry_Added;
		}
		public void Unload()
		{
			IL.Celeste.Strawberry.Added -= Strawberry_Added;
		}
        private void Strawberry_Added(ILContext il)
        {
            ILCursor cursor = new ILCursor(il);
			if (cursor.TryGotoNext(MoveType.AfterLabel, instr => instr.MatchCallvirt<SpriteBank>("Create")))
			{

			}
        }
    }
}