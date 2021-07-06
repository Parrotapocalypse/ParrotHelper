using Celeste.Mod.Entities;
using Celeste.Mod.ParrotHelper;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/CustomFakeHeart")]
	[TrackedAs(typeof(HeartGem))]
	public class CustomFakeHeart : HeartGem
	{
		private Color color;

		private enum Preset
		{
			Blue,
			Red,
			Gold,
			Gray,
			Blank
		}
		private Preset presets;

		private DynData<HeartGem> dynData;

		public CustomFakeHeart(EntityData data, Vector2 offset) : base(data, offset)
		{
			dynData = new DynData<HeartGem>(this);
			color = Calc.HexToColor(data.Attr("color", "dad8cc"));
			
			presets = data.Enum<Preset>("presets", Preset.Blank);
			switch (data.Attr("color"))
			{
				case "00ffff":
					presets = Preset.Blue;
					Logger.Log(LogLevel.Debug, "ParrotHelper", "Blue!");
					break;
				case "ff0000":
					presets = Preset.Red;
					Logger.Log(LogLevel.Debug, "ParrotHelper", "Red!");
					break;
				case "ffd700":
					presets = Preset.Gold;
					Logger.Log(LogLevel.Debug, "ParrotHelper", "Gold!");
					break;
				case "dad8cc":
					presets = Preset.Gray;
					Logger.Log(LogLevel.Debug, "ParrotHelper", "Gray!");
					break;
				default:
					Logger.Log(LogLevel.Debug, "ParrotHelper", "Default!");
					break;
					
			}
			
			IsFake = true;
			//Sprite poem = dynData.Get<Sprite>("poem.Heart");
			//Remove(poem);
			//poem = SetHeart(true);
			//poem = new Poem(data.Attr("poem"), (int)data.Enum<Preset>("presets", Preset.Gray), (float)1.0);

		}

		public override void Awake(Scene scene)
		{
			base.Awake(scene);

			Sprite sprite = dynData.Get<Sprite>("sprite");
			Remove(sprite);
			sprite = SetHeart(false);
			Add(sprite);
			sprite.Play("spin");

			sprite.OnLoop = anim =>
			{
				if (Visible && anim == "spin" && dynData.Get<bool>("autoPulse"))
				{
					Audio.Play("event:/new_content/game/10_farewell/fakeheart_pulse", Position);
					ScaleWiggler.Start();
					(Scene as Level).Displacement.AddBurst(Position, 0.35f, 8f, 48f, 0.25f);
				}
			};

			Remove(ScaleWiggler);
			ScaleWiggler = Wiggler.Create(0.5f, 4f, f => sprite.Scale = Vector2.One * (1f + f * 0.25f));
			Add(ScaleWiggler);

			dynData.Get<VertexLight>("light").Color = Color.Lerp(color, Color.White, 0.5f);

		}

		private Sprite SetHeart(bool Gui)
		{
			Sprite heart;
			if (Gui)
			{
				switch (presets)
				{
					case Preset.Blue:
						heart = GFX.GuiSpriteBank.Create("heartgem0");
						break;
					case Preset.Red:
						heart = GFX.GuiSpriteBank.Create("heartgem1");
						break;
					case Preset.Gold:
						heart = GFX.GuiSpriteBank.Create("heartgem2");
						break;
					case Preset.Gray:
						heart = GFX.GuiSpriteBank.Create("heartgem3");
						break;
					default:
						heart = ParrotHelperModule.GuiSpriteBank.Create("parrothelperrecolorheart");
						heart.Color = color;
						break;
				}
			}
			else
			{
				switch (presets)
				{
					case Preset.Blue:
						heart = GFX.SpriteBank.Create("heartgem0");
						dynData["shineParticle"] = new ParticleType(P_BlueShine);
						break;
					case Preset.Red:
						heart = GFX.SpriteBank.Create("heartgem1");
						dynData["shineParticle"] = new ParticleType(P_RedShine);
						break;
					case Preset.Gold:
						heart = GFX.SpriteBank.Create("heartgem2");
						dynData["shineParticle"] = new ParticleType(P_GoldShine);
						break;
					case Preset.Gray:
						heart = GFX.SpriteBank.Create("heartgem3");
						dynData["shineParticle"] = new ParticleType(P_FakeShine);
						break;
					default:
						heart = ParrotHelperModule.SpriteBank.Create("parrothelperfakeheart");
						heart.Color = color;
						dynData["shineParticle"] = new ParticleType(P_BlueShine) { Color = color };
						break;
				}
			}
			return heart;
		}
		


	}
}