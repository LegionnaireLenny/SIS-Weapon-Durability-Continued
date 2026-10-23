using System;
using HarmonyLib;

// Token: 0x0200000A RID: 10
[HarmonyPatch(typeof(AvailableAction), "GetCaption")]
public class AvailableAction_GetCaption__Patch
{
	// Token: 0x06000011 RID: 17 RVA: 0x00002560 File Offset: 0x00000760
	private static bool Prefix(ref string __result, AvailableAction __instance)
	{
		bool flag = __instance.ActionType == 2032;
		bool flag2;
		if (flag)
		{
			__result = GameImpl.Translate("SPEECH_HUD_RepairWeapon");
			flag2 = false;
		}
		else
		{
			flag2 = true;
		}
		return flag2;
	}
}
