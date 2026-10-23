using System;
using HarmonyLib;

// Token: 0x02000007 RID: 7
[HarmonyPatch(typeof(Equipment), "CanBeCombinedWith")]
public class Equipment_CanBeCombinedWith__Patch
{
	// Token: 0x0600000B RID: 11 RVA: 0x00002368 File Offset: 0x00000568
	private static void Postfix(ref bool __result, Equipment __instance, Equipment other)
	{
		bool flag = (__instance is MeleeWeapon || __instance is AmmoWeapon) && (other is MeleeWeapon || other is AmmoWeapon);
		if (flag)
		{
			__result = false;
		}
	}
}
