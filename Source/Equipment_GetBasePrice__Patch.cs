using System;
using HarmonyLib;

// Token: 0x0200000C RID: 12
[HarmonyPatch(typeof(Equipment), "GetBasePrice")]
public class Equipment_GetBasePrice__Patch
{
	// Token: 0x06000018 RID: 24 RVA: 0x000028C4 File Offset: 0x00000AC4
	private static void Postfix(ref float __result, Equipment __instance)
	{
		float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance.GetPrototype().Name, __instance is AmmoWeapon);
		bool flag = (__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() < maxDurabilityForEquipment;
		if (flag)
		{
			float dampness = __instance.GetDampness();
			__result = __instance.GetPrototype().BasePrice * (dampness / maxDurabilityForEquipment);
		}
	}
}
