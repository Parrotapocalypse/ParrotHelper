module ParrotHelperFlagBerryMoon

using ..Ahorn, Maple

@mapdef Entity "ParrotHelper/FlagBerryMoon" FlagBerryMoon(x::Integer, y::Integer, flag::String="", set::Bool=true, nodes::Array{Tuple{Integer, Integer}, 1}=Tuple{Integer, Integer}[])

const placements = Ahorn.PlacementDict(
    "FlagBerry (Moon, ParrotHelper)" => Ahorn.EntityPlacement(
        FlagBerryMoon
    )
)

Ahorn.nodeLimits(entity::FlagBerryMoon) = 0, -1

sprite = "collectables/strawberry/normal00"

function Ahorn.selection(entity::FlagBerryMoon)
    x, y = Ahorn.position(entity)

    nodes = get(entity.data, "nodes", ())
    winged = get(entity.data, "winged", false)
    if winged
        sprite = "collectables/strawberry/wings01"
    else
        sprite = "collectables/moonBerry/normal00"
    end
    hasPips = length(nodes) > 0
    res = Ahorn.Rectangle[Ahorn.getSpriteRectangle(sprite, x, y)]
    
    if hasPips
        for node in nodes
            nx, ny = node

            push!(res, Ahorn.getSpriteRectangle("collectables/strawberry/seed00", nx, ny))
        end
    end

    return res
end

function Ahorn.renderSelectedAbs(ctx::Ahorn.Cairo.CairoContext, entity::FlagBerryMoon)
    x, y = Ahorn.position(entity)

    for node in get(entity.data, "nodes", ())
        nx, ny = node

        Ahorn.drawLines(ctx, Tuple{Number, Number}[(x, y), (nx, ny)], Ahorn.colors.selection_selected_fc)
    end
end

function Ahorn.renderAbs(ctx::Ahorn.Cairo.CairoContext, entity::FlagBerryMoon, room::Maple.Room)
    x, y = Ahorn.position(entity)

    nodes = get(entity.data, "nodes", ())
    winged = get(entity.data, "winged", false)
    hasPips = length(nodes) > 0

    if winged
        sprite = "collectables/strawberry/wings01"
    else
        sprite = "collectables/moonBerry/normal00"
    end

    for node in nodes
        nx, ny = node

        Ahorn.drawSprite(ctx, "collectables/strawberry/seed00", nx, ny)
    end

    Ahorn.drawSprite(ctx, sprite, x, y)
end

function Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::FlagBerryMoon, room::Maple.Room)
    x, y = Ahorn.position(entity)
    nodes = get(entity.data, "nodes", ())
    winged = get(entity.data, "winged", false)
    hasPips = length(nodes) > 0

    if winged
        sprite = "collectables/strawberry/wings01"
    else
        sprite = "collectables/moonBerry/normal00"
    end
    if hasPips
        for node in nodes
            nx, ny = node
            Ahorn.drawSprite(ctx, "collectables/strawberry/seed00", nx, ny)
        end
    end

    Ahorn.drawSprite(ctx, sprite, x, y)
end

end