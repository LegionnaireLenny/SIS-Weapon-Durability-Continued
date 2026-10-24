using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Equipment), "GetLiquidAmountBar")]
public class Equipment_GetLiquidAmountBar__Patch
{
	private static bool Prefix(ref bool __result, Equipment __instance, ref float amount, ref Color col)
    {
        if (!(__instance is MeleeWeapon || __instance is AmmoWeapon))
        {
            return true;
        }

        if (__instance.InfectedWith != 0)
        {
            SISWeaponDurability_Config.Log("Showing infect bar");
            return true;
        }

        amount = __instance.GetDampness() / SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance);
        if (amount == 0f)
        {
            return true;
        }
        col = Color.Lerp(SISWeaponDurability_Config.WeaponDurabilityBarColor, Color.red, 1f - amount);
        __result = true;
        return false;
    }
}
