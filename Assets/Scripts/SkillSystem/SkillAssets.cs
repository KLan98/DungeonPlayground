using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillAssets
{
    public static readonly Dictionary<SkillID, string> IconPaths = new()
    {
        { SkillID.BOMB,                  "Icons/Items.png"        },
        { SkillID.TELEPORTATION,    "Icons/aerotheurge_teleport-icon.png"    },
    };

    public static Sprite GetIcon(SkillID skillID)
    {
        if (IconPaths.TryGetValue(skillID, out string path))
            return Resources.Load<Sprite>(path); // loads from Resources folder

        return null;
    }
}