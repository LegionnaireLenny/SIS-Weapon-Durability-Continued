using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "CanBeCombined")]
public class Equipment_CanBeCombined__Patch
{
	private static void Postfix(ref bool __result, Equipment __instance)
	{
        if (__instance is MeleeWeapon || __instance is AmmoWeapon)
		{
			__result = false;
		}
	}
}
