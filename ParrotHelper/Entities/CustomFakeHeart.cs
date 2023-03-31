using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/CustomFakeHeart")]
	[TrackedAs(typeof(HeartGem))]
	public class CustomFakeHeart : Entity
	{
		private string FAKE_HEART_FLAG;

		public static ParticleType P_BlueShine = HeartGem.P_BlueShine;

		public static ParticleType P_RedShine = HeartGem.P_RedShine;

		public static ParticleType P_GoldShine = HeartGem.P_GoldShine;

		public static ParticleType P_FakeShine = HeartGem.P_FakeShine;

		public bool IsGhost;

		public const float GhostAlpha = 0.8f;

		public bool IsFake = true;

		private Sprite sprite;

		private Sprite white;

		private ParticleType shineParticle;

		public Wiggler ScaleWiggler;

		private Wiggler moveWiggler;

		private Vector2 moveWiggleDir;

		private BloomPoint bloom;

		private VertexLight light;

		private Poem poem;

		private BirdNPC bird;

		private float timer;

		private bool collected;

		private bool autoPulse;

		private float bounceSfxDelay;

		private bool removeCameraTriggers;

		private SoundEmitter sfx;

		private List<InvisibleBarrier> walls;

		private HoldableCollider holdableCollider;

		private InvisibleBarrier fakeRightWall;

		private string fakeHeartDialog;

		private string keepGoingDialog;
		private Color color;

		private enum Preset
		{
			Blank,
			Blue,
			Red,
			Gold,
			Gray
		}
		private Preset presets;

		public CustomFakeHeart(Vector2 position) : base(position)
		{
			autoPulse = true;
			walls = new List<InvisibleBarrier>();
			Add(holdableCollider = new HoldableCollider(OnHoldable));
			Add(new MirrorReflection());
		}

		public CustomFakeHeart(EntityData data, Vector2 offset) : this(data.Position + offset)
		{
			fakeHeartDialog = data.Attr("fakeHeartDialog", "CH9_FAKE_HEART");
			keepGoingDialog = data.Attr("keepGoingDialog", "CH9_KEEP_GOING");
			color = Calc.HexToColor(data.Attr("color", "dad8cc"));
			presets = data.Enum("presets", Preset.Blank);
			switch (data.Attr("color"))
			{
				case "00ffff":
					presets = Preset.Blue;
					break;
				case "ff0000":
					presets = Preset.Red;
					break;
				case "ffd700":
					presets = Preset.Gold;
					break;
				case "dad8cc":
					presets = Preset.Gray;
					break;
				default:
					break;
					
			}
			removeCameraTriggers = data.Bool("removeCameraTriggers", false);
			FAKE_HEART_FLAG = data.Attr("flag", "fake_heart");
		}

		public override void Awake(Scene scene)
		{
			base.Awake(scene);
			AreaKey area = (Scene as Level).Session.Area;
			IsGhost = false;
			sprite = SetHeart(false);
			Add(sprite);
			sprite.Play("spin");
			sprite.OnLoop = delegate (string anim)
			{
				if (Visible && anim == "spin" && autoPulse)
				{
					if (IsFake)
					{
						Audio.Play("event:/new_content/game/10_farewell/fakeheart_pulse", Position);
					}
					ScaleWiggler.Start();
					(Scene as Level).Displacement.AddBurst(Position, 0.35f, 8f, 48f, 0.25f);
				}
			};
			if (IsGhost)
			{
				sprite.Color = Color.White * 0.8f;
			}
			Collider = new Hitbox(16f, 16f, -8f, -8f);
			Add(new PlayerCollider(OnPlayer));
			Add(ScaleWiggler = Wiggler.Create(0.5f, 4f, delegate (float f)
			{
				sprite.Scale = Vector2.One * (1f + f * 0.25f);
			}));
			Add(bloom = new BloomPoint(0.75f, 16f));
			Color value;
			if (IsFake)
			{
				value = Calc.HexToColor("dad8cc");
				shineParticle = P_FakeShine;
			}
			else if (area.Mode == AreaMode.Normal)
			{
				value = Color.Aqua;
				shineParticle = P_BlueShine;
			}
			else if (area.Mode == AreaMode.BSide)
			{
				value = Color.Red;
				shineParticle = P_RedShine;
			}
			else
			{
				value = Color.Gold;
				shineParticle = P_GoldShine;
			}
			value = Color.Lerp(value, Color.White, 0.5f);
			Add(light = new VertexLight(value, 1f, 32, 64));
			if (IsFake)
			{
				bloom.Alpha = 0f;
				light.Alpha = 0f;
			}
			moveWiggler = Wiggler.Create(0.8f, 2f);
			moveWiggler.StartZero = true;
			Add(moveWiggler);
			if (!IsFake)
			{
				return;
			}
			Player entity = Scene.Tracker.GetEntity<Player>();
			if ((entity != null && entity.X > X) || (scene as Level).Session.GetFlag(FAKE_HEART_FLAG))
			{
				Visible = false;
				Alarm.Set(this, 0.0001f, delegate
				{
					FakeRemoveCameraTrigger();
					RemoveSelf();
				});
			}
			else
			{
				scene.Add(fakeRightWall = new InvisibleBarrier(new Vector2(X + 160f, Y - 200f), 8f, 400f));
			}
		}

		public override void Update()
		{
			bounceSfxDelay -= Engine.DeltaTime;
			timer += Engine.DeltaTime;
			sprite.Position = Vector2.UnitY * (float)Math.Sin(timer * 2f) * 2f + moveWiggleDir * moveWiggler.Value * -8f;
			if (white != null)
			{
				white.Position = sprite.Position;
				white.Scale = sprite.Scale;
				if (white.CurrentAnimationID != sprite.CurrentAnimationID)
				{
					white.Play(sprite.CurrentAnimationID);
				}
				white.SetAnimationFrame(sprite.CurrentAnimationFrame);
			}
			if (collected)
			{
				Player entity = Scene.Tracker.GetEntity<Player>();
				if (entity == null || entity.Dead)
				{
					EndCutscene();
				}
			}
			base.Update();
			if (!collected && Scene.OnInterval(0.1f))
			{
				SceneAs<Level>().Particles.Emit(P_BlueShine, 1, Center, Vector2.One * 8f);
			}
		}

		public void OnHoldable(Holdable h)
		{
			Player entity = Scene.Tracker.GetEntity<Player>();
			if (!collected && entity != null && h.Dangerous(holdableCollider))
			{
				Collect(entity);
			}
		}

		public void OnPlayer(Player player)
		{
			if (collected || (Scene as Level).Frozen)
			{
				return;
			}
			if (player.DashAttacking)
			{
				Collect(player);
				return;
			}
			if (bounceSfxDelay <= 0f)
			{
				if (IsFake)
				{
					Audio.Play("event:/new_content/game/10_farewell/fakeheart_bounce", Position);
				}
				else
				{
					Audio.Play("event:/game/general/crystalheart_bounce", Position);
				}
				bounceSfxDelay = 0.1f;
			}
			player.PointBounce(Center);
			moveWiggler.Start();
			ScaleWiggler.Start();
			moveWiggleDir = (Center - player.Center).SafeNormalize(Vector2.UnitY);
			Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
		}

		private void Collect(Player player)
		{
			Scene.Tracker.GetEntity<AngryOshiro>()?.StopControllingTime();
			Coroutine coroutine = new Coroutine(CollectRoutine(player))
			{
				UseRawDeltaTime = true
			};
			Add(coroutine);
			collected = true;
			if (!removeCameraTriggers)
			{
				return;
			}
			foreach (CameraOffsetTrigger item in Scene.Entities.FindAll<CameraOffsetTrigger>())
			{
				item.RemoveSelf();
			}
		}

		private IEnumerator DoFakeRoutineWithBird(Player player)
		{
			Level level = Scene as Level;
			int panAmount = 64;
			Vector2 panFrom = level.Camera.Position;
			Vector2 panTo = level.Camera.Position + new Vector2(-panAmount, 0f);
			Vector2 birdFrom = new Vector2(panTo.X - 16f, player.Y - 20f);
			Vector2 birdTo = new Vector2(panFrom.X + 320f + 16f, player.Y - 20f);
			yield return 2f;
			Glitch.Value = 0.75f;
			while (Glitch.Value > 0f)
			{
				Glitch.Value = Calc.Approach(Glitch.Value, 0f, Engine.RawDeltaTime * 4f);
				level.Shake();
				yield return null;
			}
			yield return 1.1f;
			Glitch.Value = 0.75f;
			while (Glitch.Value > 0f)
			{
				Glitch.Value = Calc.Approach(Glitch.Value, 0f, Engine.RawDeltaTime * 4f);
				level.Shake();
				yield return null;
			}
			yield return 0.4f;
			for (float p3 = 0f; p3 < 1f; p3 += Engine.RawDeltaTime / 2f)
			{
				level.Camera.Position = panFrom + (panTo - panFrom) * Ease.CubeInOut(p3);
				poem.Offset = new Vector2(panAmount * 8, 0f) * Ease.CubeInOut(p3);
				yield return null;
			}
			bird = new BirdNPC(birdFrom, BirdNPC.Modes.None);
			bird.Sprite.Play("fly");
			bird.Sprite.UseRawDeltaTime = true;
			bird.Facing = Facings.Right;
			bird.Depth = -2000100;
			bird.Tag = Tags.FrozenUpdate;
			bird.Add(new VertexLight(Color.White, 0.5f, 8, 32));
			bird.Add(new BloomPoint(0.5f, 12f));
			level.Add(bird);
			for (float p3 = 0f; p3 < 1f; p3 += Engine.RawDeltaTime / 2.6f)
			{
				level.Camera.Position = panTo + (panFrom - panTo) * Ease.CubeInOut(p3);
				poem.Offset = new Vector2(panAmount * 8, 0f) * Ease.CubeInOut(1f - p3);
				float num = 0.1f;
				float num2 = 0.9f;
				if (p3 > num && p3 <= num2)
				{
					float num3 = (p3 - num) / (num2 - num);
					bird.Position = birdFrom + (birdTo - birdFrom) * num3 + Vector2.UnitY * (float)Math.Sin(num3 * 8f) * 8f;
				}
				if (level.OnRawInterval(0.2f))
				{
					TrailManager.Add(bird, Calc.HexToColor("639bff"), 1f, frozenUpdate: true, useRawDeltaTime: true);
				}
				yield return null;
			}
			bird.RemoveSelf();
			bird = null;
			Engine.TimeRate = 0f;
			level.Frozen = false;
			player.Active = false;
			player.StateMachine.State = 11;
			while (Engine.TimeRate != 1f)
			{
				Engine.TimeRate = Calc.Approach(Engine.TimeRate, 1f, 0.5f * Engine.RawDeltaTime);
				yield return null;
			}
			Engine.TimeRate = 1f;
			yield return Textbox.Say(fakeHeartDialog);
			sfx.Source.Param("end", 1f);
			yield return 0.283f;
			level.FormationBackdrop.Display = false;
			for (float p3 = 0f; p3 < 1f; p3 += Engine.RawDeltaTime / 0.2f)
			{
				poem.TextAlpha = Ease.CubeIn(1f - p3);
				poem.ParticleSpeed = poem.TextAlpha;
				yield return null;
			}
			poem.Heart.Play("break");
			while (poem.Heart.Animating)
			{
				poem.Shake += Engine.DeltaTime;
				yield return null;
			}
			poem.RemoveSelf();
			poem = null;
			for (int i = 0; i < 10; i++)
			{
				Vector2 position = level.Camera.Position + new Vector2(320f, 180f) * 0.5f;
				Vector2 value = level.Camera.Position + new Vector2(160f, -64f);
				Scene.Add(new AbsorbOrb(position, null, value));
			}
			level.Shake();
			Glitch.Value = 0.8f;
			while (Glitch.Value > 0f)
			{
				Glitch.Value -= Engine.DeltaTime * 4f;
				yield return null;
			}
			yield return 0.25f;
			level.Session.Audio.Music.Event = "event:/new_content/music/lvl10/intermission_heartgroove";
			level.Session.Audio.Apply(forceSixteenthNoteHack: false);
			player.Active = true;
			player.Depth = 0;
			player.StateMachine.State = 11;
			while (!player.OnGround() && player.Bottom < level.Bounds.Bottom)
			{
				yield return null;
			}
			player.Facing = Facings.Right;
			yield return 0.5f;
			yield return Textbox.Say(keepGoingDialog, PlayerStepForward);
			SkipFakeHeartCutscene(level);
			level.EndCutscene();
		}

		private IEnumerator PlayerStepForward()
		{
			yield return 0.1f;
			Player entity = Scene.Tracker.GetEntity<Player>();
			if (entity != null && entity.CollideCheck<Solid>(entity.Position + new Vector2(12f, 1f)))
			{
				yield return entity.DummyWalkToExact((int)entity.X + 10);
			}
			yield return 0.2f;
		}

		private void SkipFakeHeartCutscene(Level level)
		{
			Engine.TimeRate = 1f;
			Glitch.Value = 0f;
			sfx?.Source.Stop();
			level.Session.SetFlag(FAKE_HEART_FLAG);
			level.Frozen = false;
			level.FormationBackdrop.Display = false;
			level.Session.Audio.Music.Event = "event:/new_content/music/lvl10/intermission_heartgroove";
			level.Session.Audio.Apply(forceSixteenthNoteHack: false);
			Player entity = Scene.Tracker.GetEntity<Player>();
			if (entity != null)
			{
				entity.Sprite.Play("idle");
				entity.Active = true;
				entity.StateMachine.State = 0;
				entity.Dashes = 1;
				entity.Speed = Vector2.Zero;
				entity.MoveV(200f);
				entity.Depth = 0;
				for (int i = 0; i < 10; i++)
				{
					entity.UpdateHair(applyGravity: true);
				}
			}
			foreach (AbsorbOrb item in Scene.Entities.FindAll<AbsorbOrb>())
			{
				item.RemoveSelf();
			}
			poem?.RemoveSelf();
			bird?.RemoveSelf();
			fakeRightWall?.RemoveSelf();
			FakeRemoveCameraTrigger();
			foreach (InvisibleBarrier wall in walls)
			{
				wall.RemoveSelf();
			}
			RemoveSelf();
		}

		private void FakeRemoveCameraTrigger()
		{
			CameraTargetTrigger cameraTargetTrigger = CollideFirst<CameraTargetTrigger>();
			if (cameraTargetTrigger != null)
			{
				cameraTargetTrigger.LerpStrength = 0f;
			}
		}

		private IEnumerator orig_CollectRoutine(Player player)
		{
			Level level = Scene as Level;
			AreaKey area = level.Session.Area;
			string poemID = AreaData.Get(level).Mode[(int)area.Mode].PoemID;
			if (IsFake)
			{
				level.StartCutscene(SkipFakeHeartCutscene);
			}
			else
			{
				level.CanRetry = false;
			}
			if (IsFake)
			{
				Audio.SetMusic(null);
				Audio.SetAmbience(null);
			}
			string text = "event:/game/general/crystalheart_blue_get";
			if (IsFake)
			{
				text = "event:/new_content/game/10_farewell/fakeheart_get";
			}
			else if (area.Mode == AreaMode.BSide)
			{
				text = "event:/game/general/crystalheart_red_get";
			}
			else if (area.Mode == AreaMode.CSide)
			{
				text = "event:/game/general/crystalheart_gold_get";
			}
			sfx = SoundEmitter.Play(text, this);
			Add(new LevelEndingHook(delegate
			{
				sfx.Source.Stop();
			}));
			walls.Add(new InvisibleBarrier(new Vector2(level.Bounds.Right, level.Bounds.Top), 8f, level.Bounds.Height));
			walls.Add(new InvisibleBarrier(new Vector2(level.Bounds.Left - 8, level.Bounds.Top), 8f, level.Bounds.Height));
			walls.Add(new InvisibleBarrier(new Vector2(level.Bounds.Left, level.Bounds.Top - 8), level.Bounds.Width, 8f));
			foreach (InvisibleBarrier wall in walls)
			{
				Scene.Add(wall);
			}
			Add(white = GFX.SpriteBank.Create("heartGemWhite"));
			Depth = -2000000;
			yield return null;
            Celeste.Freeze(0.2f);
			yield return null;
			Engine.TimeRate = 0.5f;
			player.Depth = -2000000;
			//for (int i = 0; i < 10; i++)
			//{
			//	Scene.Add(new AbsorbOrb(Position));
			//}
			level.Shake();
			Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
			level.Flash(Color.White);
			level.FormationBackdrop.Display = true;
			level.FormationBackdrop.Alpha = 1f;
			light.Alpha = (bloom.Alpha = 0f);
			Visible = false;
			for (float t3 = 0f; t3 < 2f; t3 += Engine.RawDeltaTime)
			{
				Engine.TimeRate = Calc.Approach(Engine.TimeRate, 0f, Engine.RawDeltaTime * 0.25f);
				yield return null;
			}
			yield return null;
			if (player.Dead)
			{
				yield return 100f;
			}
			Engine.TimeRate = 1f;
			Tag = Tags.FrozenUpdate;
			level.Frozen = true;
			string text2 = null;
			if (!string.IsNullOrEmpty(poemID))
			{
				text2 = Dialog.Clean("poem_" + poemID);
			}
			poem = new Poem(text2, 0, 1f)
			{
				Alpha = 0f
			};
			if (presets == Preset.Blank)
			{
				poem.Heart = ParrotHelperModule.GuiSpriteBank.Create("parrothelperrecolorheart");
				poem.Heart.Color = GetColor();
			}
			else
			{
				poem.Heart = GFX.GuiSpriteBank.Create("heartgem" + ((int)presets-1));
			}
			Scene.Add(poem);
			poem.Heart.Play("spin");
			for (float t3 = 0f; t3 < 1f; t3 += Engine.RawDeltaTime)
			{
				poem.Alpha = Ease.CubeOut(t3);
				yield return null;
			}
			if (IsFake)
			{
				yield return DoFakeRoutineWithBird(player);
				yield break;
			}
			while (!Input.MenuConfirm.Pressed && !Input.MenuCancel.Pressed)
			{
				yield return null;
			}
			sfx.Source.Param("end", 1f);
			if (IsFake)
			{
				level.FormationBackdrop.Display = false;
				for (float t3 = 0f; t3 < 1f; t3 += Engine.RawDeltaTime * 2f)
				{
					poem.Alpha = Ease.CubeIn(1f - t3);
					yield return null;
				}
				player.Depth = 0;
				EndCutscene();
			}
			else
			{
				FadeWipe fadeWipe = new FadeWipe(level, wipeIn: false)
				{
					Duration = 3.25f
				};
				yield return fadeWipe.Duration;
				level.CompleteArea(spotlightWipe: false, skipScreenWipe: true, skipCompleteScreen: false);
			}
		}

		private IEnumerator CollectRoutine(Player player)
		{
			return orig_CollectRoutine(player);
		}
		private void EndCutscene()
		{
			Level obj = Scene as Level;
			obj.Frozen = false;
			obj.CanRetry = true;
			obj.FormationBackdrop.Display = false;
			Engine.TimeRate = 1f;
			poem?.RemoveSelf();
			foreach (InvisibleBarrier wall in walls)
			{
				wall.RemoveSelf();
			}
			RemoveSelf();
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
						break;
					case Preset.Red:
						heart = GFX.SpriteBank.Create("heartgem1");
						break;
					case Preset.Gold:
						heart = GFX.SpriteBank.Create("heartgem2");
						break;
					case Preset.Gray:
						heart = GFX.SpriteBank.Create("heartgem3");
						break;
					default:
						heart = ParrotHelperModule.SpriteBank.Create("parrothelperfakeheart");
						heart.Color = color;
						shineParticle = new ParticleType(P_BlueShine) { Color = color };
						break;
				}
			}
			return heart;
		}
		private Color GetColor()
		{
			Color color2;
			switch (presets)
			{
				case Preset.Blue:
					color2 = Color.Aqua;
					break;
				case Preset.Red:
					color2 = Color.Red;
					break;
				case Preset.Gold:
					color2 = Color.Gold;
					break;
				case Preset.Gray:
					color2 = Calc.HexToColor("dad8cc");
					break;
				default:
					color2 = color;
					break;
			}
			return color2;
		}
	}
}