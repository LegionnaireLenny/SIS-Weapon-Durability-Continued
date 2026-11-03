using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "WantRareUpdate")]
public class Equipment_WantRareUpdate__Patch
{
    private static void Postfix(ref bool __result, Equipment __instance)
    {
        if (__result && (__instance is MeleeWeapon || __instance is AmmoWeapon))
        {
            __result = false;
        }
    }
}
