module ParrotHelperCustomFakeHeart

using ..Ahorn, Maple

@mapdef Entity "ParrotHelper/CustomFakeHeart" CustomFakeHeart(x::Integer, y::Integer, presets::String="Gray", color::String="", fakeHeartDialog::String="CH9_FAKE_HEART", keepGoingDialog::String="CH9_KEEP_GOING", flag::String="fake_heart", removeCameraTriggers::Bool=false)

const placements = Ahorn.PlacementDict(
   "Custom Fake Heart (ParrotHelper)" => Ahorn.EntityPlacement(
	  CustomFakeHeart
   )
)

const heartTypes = ["Blue", "Red", "Gold", "Gray", ""]

Ahorn.editingOptions(entity::CustomFakeHeart) = Dict{String, Any}(
  "presets" => heartTypes
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
	return Ahorn.getSpriteRectangle("collectables/heartGem/3/00", x, y)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::CustomFakeHeart, room::Maple.Room)
	if entity.presets == "Blue"
		Ahorn.drawSprite(ctx, "collectables/heartGem/0/00", 0, 0)
	elseif entity.presets == "Red"
		Ahorn.drawSprite(ctx, "collectables/heartGem/1/00", 0, 0)
	elseif entity.presets == "Gold"
		Ahorn.drawSprite(ctx, "collectables/heartGem/2/00", 0, 0)
	elseif entity.presets == "Gray"
		Ahorn.drawSprite(ctx, "collectables/heartGem/3/00", 0, 0)
	else
		inputcolor = entity.color
		color = getColor(inputcolor)
		Ahorn.drawSprite(ctx, "collectables/ParrotHelper/FakeHeart/00", 0, 0, tint=color)
	end
end

end