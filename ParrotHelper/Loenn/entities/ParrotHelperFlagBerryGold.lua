local flagBerryGold = {}

flagBerryGold.name = "ParrotHelper/FlagBerryGold"
flagBerryGold.depth = -100
flagBerryGold.nodeLineRenderType = "fan"
flagBerryGold.nodeLimits = {0, -1}
function flagBerryGold.texture(room, entity)
    local winged = entity.winged
    local hasNodes = entity.nodes and #entity.nodes > 0

    if winged then
        if hasNodes then
            return "collectables/ghostgoldberry/wings01"

        else
            return "collectables/goldberry/wings01"
        end

    else
        if hasNodes then
            return "collectables/ghostgoldberry/idle00"

        else
            return "collectables/goldberry/idle00"
        end
    end
end

function flagBerryGold.nodeTexture(room, entity)
    local hasNodes = entity.nodes and #entity.nodes > 0

    if hasNodes then
        return "collectables/goldberry/seed00"
    end
end

flagBerryGold.placements = {
    {
        name = "golden",
        data = {
            winged = false,
            flag = "flag_berry_gold",
            set = true
        },
    },
    {
        name = "golden_winged",
        data = {
            winged = true,
            flag = "flag_berry_gold",
            set = true
        }
    }
}

return flagBerryGold