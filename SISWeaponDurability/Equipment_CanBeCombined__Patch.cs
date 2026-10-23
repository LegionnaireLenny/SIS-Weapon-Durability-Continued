using System;
using HarmonyLib;

// Token: 0x02000011 RID: 17
[HarmonyPatch(typeof(Equipment), "CanBeCombined")]
public class Equipment_CanBeCombined__Patch
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002DF0 File Offset: 0x00000FF0
	private static void Postfix(ref bool __result, Equipment __instance)
	{
		bool flag = __instance is MeleeWeapon || __instance is AmmoWeapon;
		if (flag)
		{
			__result = false;
		}
	}
}
