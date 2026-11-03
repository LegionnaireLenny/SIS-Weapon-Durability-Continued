using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "GetBasePrice")]
public class Equipment_GetBasePrice__Patch
{
    private static void Postfix(ref float __result, Equipment __instance)
    {
        float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance);
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() < maxDurabilityForEquipment)
        {
            float dampness = __instance.GetDampness();
            __result = __instance.GetPrototype().BasePrice * (dampness / maxDurabilityForEquipment);
        }
    }
}
