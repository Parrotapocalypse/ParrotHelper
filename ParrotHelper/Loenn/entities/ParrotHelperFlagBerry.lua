-- more or less all from Lönn
local flagBerry = {}

flagBerry.name = "ParrotHelper/FlagBerry"
flagBerry.depth = -100
flagBerry.nodeLineRenderType = "fan"
flagBerry.nodeLimits = {0, -1}
flagBerry.fieldInformation = {
    order = {
        fieldType = "integer",
    },
    checkpointID = {
        fieldType = "integer"
    }
}
function flagBerry.texture(room, entity)
    local winged = entity.winged
    local hasNodes = entity.nodes and #entity.nodes > 0

    if winged then
        if hasNodes then
            return "collectables/ghostberry/wings01"

        else
            return "collectables/strawberry/wings01"
        end

    else
        if hasNodes then
            return "collectables/ghostberry/idle00"

        else
            return "collectables/strawberry/normal00"
        end
    end
end

function flagBerry.nodeTexture(room, entity)
    local hasNodes = entity.nodes and #entity.nodes > 0

    if hasNodes then
        return "collectables/strawberry/seed00"
    end
end

flagBerry.placements = {
    {
        name = "normal",
        data = {
            winged = false,
            checkpointID = -1,
            order = -1,
            flag = "flag_berry",
            set = true
        },
    },
    {
        name = "normal_winged",
        data = {
            winged = true,
            checkpointID = -1,
            order = -1,
            flag = "flag_berry",
            set = true
        },
    }
}
return flagBerry