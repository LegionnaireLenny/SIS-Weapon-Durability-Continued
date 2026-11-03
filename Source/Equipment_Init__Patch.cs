using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "Init")]
public class Equipment_Init__Patch
{
    private static void Postfix(Equipment __instance)
    {
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() == 0f)
        {
            __instance.SetDampness(SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance).MaxDurability);
        }
    }
}
