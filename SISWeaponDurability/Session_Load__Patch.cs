using System;
using HarmonyLib;

// Token: 0x02000019 RID: 25
[HarmonyPatch(typeof(Session), "Load")]
public class Session_Load__Patch
{
	// Token: 0x06000034 RID: 52 RVA: 0x00003240 File Offset: 0x00001440
	private static void Finalizer(Session __instance, bool newGame)
	{
		SISWeaponDurability_Config.ReloadSettings();
		SISWeaponDurability_Utils.LoadProtos();
	}
}
