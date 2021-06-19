module FriendlyHelperFlagBerry

using ..Ahorn, Maple

@mapdef Entity "FriendlyHelper/FlagBerry" FlagBerry(x::Integer, y::Integer, Flag::String="berry_flag", Set::Bool=false)

const placements = Ahorn.PlacementDict(
    "FlagBerry (FriendlyHelper)" => Ahorn.EntityPlacement(
        FlagBerry,
        "point"
    )
)

sprite = "collectables/strawberry/normal00"

function Ahorn.selection(entity::FlagBerry)
    x, y = Ahorn.position(entity)

    return Ahorn.getSpriteRectangle(sprite, x, y)
end

Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::FlagBerry, room::Maple.Room) = Ahorn.drawSprite(ctx, sprite, 0,0)

end