local fakeHeart = {}
local texture =
{
    [""] = "collectables/ParrotHelper/FakeHeart/00",
    ["Blue"] = "collectables/heartGem/0/00",
    ["Red"] = "collectables/heartGem/1/00",
    ["Gold"] = "collectables/heartGem/2/00",
    ["Gray"] = "collectables/heartGem/3/00"
}
fakeHeart.name = "ParrotHelper/CustomFakeHeart"
fakeHeart.depth = -2000000
fakeHeart.placements = {
    {
        name = "fakeheart",
        data = {
            presets = "None",
            color = "ffffff",
            fakeHeartDialog = "CH9_FAKE_HEART",
            keepGoingDialog = "CH9_KEEP_GOING",
            flag = "fake_heart",
            removeCameraTriggers = false
        }
    }
}
fakeHeart.fieldInformation = {
    color = {fieldType = "color"},
    presets = {
        options = {
            {"None", ""},
            {"Blue", "Blue"},
            {"Red", "Red"},
            {"Gold", "Gold"},
            {"Gray", "Gray"}
        }
    }
}
function fakeHeart.texture(room, entity)
    return texture[entity.presets]
end
return fakeHeart