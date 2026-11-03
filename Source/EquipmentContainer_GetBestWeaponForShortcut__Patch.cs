using HarmonyLib;
using System;

[HarmonyPatch(typeof(EquipmentContainer), "GetBestWeaponForShortcut")]
public class EquipmentContainer_GetBestWeaponForShortcut__Patch
{
    private static void Postfix(ref Equipment __result, EquipmentContainer __instance, Character character, Type type, int count)
    {
        if (type != typeof(MeleeWeapon))
        {
            return;
        }

        int num = 0;
        foreach (Equipment equipment in character.Inventory.Contents)
        {
            if (equipment is MeleeWeapon)
            {
                if (num == count)
                {
                    __result = equipment;
                    break;
                }
                num++;
            }
        }
    }
}
