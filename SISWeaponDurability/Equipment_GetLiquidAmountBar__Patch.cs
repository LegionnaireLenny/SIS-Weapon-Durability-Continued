using System;
using HarmonyLib;
using UnityEngine;

// Token: 0x02000012 RID: 18
[HarmonyPatch(typeof(Equipment), "GetLiquidAmountBar")]
public class Equipment_GetLiquidAmountBar__Patch
{
	// Token: 0x06000025 RID: 37 RVA: 0x00002E24 File Offset: 0x00001024
	private static bool Prefix(ref bool __result, Equipment __instance, ref float amount, ref Color col)
	{
		bool flag = __instance is MeleeWeapon || __instance is AmmoWeapon;
		if (flag)
		{
			bool flag2 = __instance.InfectedWith == 0;
			if (flag2)
			{
				amount = __instance.GetDampness() / SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance.GetPrototype().Name, __instance is AmmoWeapon);
				bool flag3 = amount == 0f;
				if (flag3)
				{
					return true;
				}
				col = Color.Lerp(SISWeaponDurability_Config.WeaponDurabilityBarColor, Color.red, 1f - amount);
				__result = true;
				return false;
			}
			else
			{
				SISWeaponDurability_Config.Log("Showing infect bar");
			}
		}
		return true;
	}
}
