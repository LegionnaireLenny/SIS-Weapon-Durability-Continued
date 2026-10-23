using System;
using HarmonyLib;

// Token: 0x02000003 RID: 3
[HarmonyPatch(typeof(Equipment), "GetBaseDamage")]
public class Equipment_GetBaseDamage__Patch
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002124 File Offset: 0x00000324
	private static void Postfix(ref float __result, Equipment __instance)
	{
		float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance.GetPrototype().Name, __instance is AmmoWeapon);
		bool flag = (__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() < maxDurabilityForEquipment;
		if (flag)
		{
			float dampness = __instance.GetDampness();
			__result = __instance.GetPrototype().Damage * (dampness / maxDurabilityForEquipment) * 0.5f;
		}
	}
}
