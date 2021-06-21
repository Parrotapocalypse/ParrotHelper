using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System.Collections.Generic;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/CustomFakeHeart")]
	[TrackedAs(typeof(HeartGem))]
	public class CustomFakeHeart : HeartGem
	{
		private Color color;
		private DynData<HeartGem> dynData;
		private bool includeWalls;
		public CustomFakeHeart(EntityData data, Vector2 offset) : base(data, offset)
		{
			color = Calc.HexToColor(data.Attr("color", "00ffff"));
			dynData = new DynData<HeartGem>(this);
			includeWalls = data.Bool("invisibleBarriers", true);
			IsFake = true;
		}
        /*public override void Awake(Scene scene)
        {
			IsFake = false;
            base.Awake(scene);
			IsFake = true;
        }*/
        public override void Awake(Scene scene)
        {
            base.Awake(scene);

			Sprite sprite = dynData.Get<Sprite>("sprite");
			Remove(sprite);
			if (color == Calc.HexToColor("00ffff")) 
			{
				sprite = GFX.SpriteBank.Create("heartgem0");
            }
			if (color == Calc.HexToColor("ff0000"))
			{
				sprite = GFX.SpriteBank.Create("heartgem1");
			}
			if (color == Calc.HexToColor("ffd700"))
			{
				sprite = GFX.SpriteBank.Create("heartgem2");
			}
			if (color == Calc.HexToColor("dad8cc"))
			{
				sprite = GFX.SpriteBank.Create("heartgem3");
			}
			else
            {
				sprite = ParrotHelperModule.SpriteBank.Create("parrothelperfakeheart");
				sprite.Color = color;
				dynData["shineParticle"] = new ParticleType(P_BlueShine) { Color = color };
			}
			dynData["value"] = color;
			dynData["sprite"] = sprite;
			Add(sprite);
			sprite.OnLoop = anim =>
			{
				if (Visible && anim == "spin" && dynData.Get<bool>("autoPulse"))
				{
					ScaleWiggler.Start();
					(Scene as Level).Displacement.AddBurst(Position, 0.35f, 8f, 48f, 0.25f);
				}
			};
			Remove(ScaleWiggler);
			ScaleWiggler = Wiggler.Create(0.5f, 4f, f => sprite.Scale = Vector2.One * (1f + f * 0.25f));
			Add(ScaleWiggler);
            if (!includeWalls)
            {
				foreach (InvisibleBarrier wall in dynData.Get<List<InvisibleBarrier>>("walls"))
				{
					wall.RemoveSelf();
				}
			}
        }

    }
}
