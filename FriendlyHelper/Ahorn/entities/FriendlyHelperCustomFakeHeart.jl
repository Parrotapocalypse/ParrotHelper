module FriendlyHelperCustomFakeHeart

using ..Ahorn, Maple

@mapdef Entity "FriendlyHelper/CustomFakeHeart" CustomFakeHeart(x::Integer, y::Integer, color::String="dad8cc", flag::String="fake_heart", removeCameraTriggers::Bool=false)

const placements = Ahorn.PlacementDict(
   "Custom Fake Heart (Friendly Helper)" => Ahorn.EntityPlacement(
      CustomFakeHeart
   )
)

function Ahorn.selection(entity::CustomFakeHeart)
    x, y = Ahorn.position(entity)
    return Ahorn.getSpriteRectangle("collectables/heartGem/3/00.png", x, y)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::CustomFakeHeart, room::Maple.Room)
	recolor = Ahorn.argb32ToRGBATuple(parse(Int, get(entity.data, "color", "dad8cc"), base=16))
    Ahorn.drawSprite(ctx, "collectables/heartGem/3/00.png", 0, 0)
end

end