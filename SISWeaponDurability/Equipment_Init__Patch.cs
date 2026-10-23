using System;
using HarmonyLib;

// Token: 0x02000005 RID: 5
[HarmonyPatch(typeof(Equipment), "Init")]
public class Equipment_Init__Patch
{
	// Token: 0x06000007 RID: 7 RVA: 0x00002238 File Offset: 0x00000438
	private static void Postfix(Equipment __instance)
	{
		bool flag = (__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() == 0f;
		if (flag)
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance.GetPrototype().Name, __instance is AmmoWeapon);
			__instance.SetDampness(weaponModifiersForEquipment.MaxDurability);
		}
	}
}
