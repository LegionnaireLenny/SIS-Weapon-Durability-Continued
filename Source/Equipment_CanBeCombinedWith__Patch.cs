using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "CanBeCombinedWith")]
public class Equipment_CanBeCombinedWith__Patch
{
    private static void Postfix(ref bool __result, Equipment __instance, Equipment other)
    {
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && (other is MeleeWeapon || other is AmmoWeapon))
        {
            __result = false;
        }
    }
}
