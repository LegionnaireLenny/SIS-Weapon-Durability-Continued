using System;
using HarmonyLib;

// Token: 0x02000008 RID: 8
[HarmonyPatch(typeof(Equipment), "WantRareUpdate")]
public class Equipment_WantRareUpdate__Patch
{
	// Token: 0x0600000D RID: 13 RVA: 0x000023B0 File Offset: 0x000005B0
	private static void Postfix(ref bool __result, Equipment __instance)
	{
		bool flag = __result;
		if (flag)
		{
			bool flag2 = __instance is MeleeWeapon || __instance is AmmoWeapon;
			if (flag2)
			{
				__result = false;
			}
		}
	}
}
