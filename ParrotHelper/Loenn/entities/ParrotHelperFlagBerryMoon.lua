local flagBerryMoon = {}

flagBerryMoon.name = "ParrotHelper/FlagBerryMoon"
flagBerryMoon.depth = -100
flagBerryMoon.fieldInformation = {
    order = {
        fieldType = "integer",
    },
    checkpointID = {
        fieldType = "integer"
    }
}
flagBerryMoon.texture = "collectables/moonBerry/normal00"

flagBerryMoon.placements = {
    {
        name = "moon",
        data = {
            checkpointID = -1,
            order = -1,
            flag = "flag_berry_moon",
            set = true
        }
    }
}
return flagBerryMoon