using Celeste;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

namespace Celeste.Mod.ParrotHelper.Entities
{
	[CustomEntity("ParrotHelper/CustomFakeHeart")]
	[TrackedAs(typeof(HeartGem))]
	public class CustomFakeHeart : HeartGem
	{
		private Color color;
		public CustomFakeHeart(EntityData data, Vector2 offset) : base(data, offset)
		{
			color = Calc.HexToColor(data.Attr("color", "dad8cc"));
			IsFake = true;
		}
	}
}
