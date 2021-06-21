module ParrotHelperCustomFakeHeart

using ..Ahorn, Maple

@mapdef Entity "ParrotHelper/CustomFakeHeart" CustomFakeHeart(x::Integer, y::Integer, color::String="dad8cc", fakeHeartDialog::String="CH9_FAKE_HEART", keepGoingDialog::String="CH9_KEEP_GOING", removeCameraTriggers::Bool=false, invisibleBarriers::Bool=true)

const placements = Ahorn.PlacementDict(
   "Custom Fake Heart (ParrotHelper)" => Ahorn.EntityPlacement(
	  CustomFakeHeart
   )
)

function getColor(color)
	if haskey(Ahorn.XNAColors.colors, color)
		return Ahorn.XNAColors.colors[color]

	else
		try
			return ((Ahorn.argb32ToRGBATuple(parse(Int, replace(color, "#" => ""), base=16))[1:3] ./ 255)..., 1.0)

		catch

		end
	end

	return (1.0, 1.0, 1.0, 1.0)
end

function Ahorn.selection(entity::CustomFakeHeart)
	x, y = Ahorn.position(entity)
	return Ahorn.getSpriteRectangle("collectables/heartGem/3/00.png", x, y)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::CustomFakeHeart, room::Maple.Room)
	if entity.color == "00ffff"
		Ahorn.drawSprite(ctx, "collectables/heartGem/0/00.png", 0, 0)
	elseif entity.color == "ff0000"
		Ahorn.drawSprite(ctx, "collectables/heartGem/1/00.png", 0, 0)
	else
		inputcolor = entity.color
		color = getColor(inputcolor)
		Ahorn.drawSprite(ctx, "collectables/ParrotHelper/FakeHeart/00.png", 0, 0, tint=color)
	end
end

end