using System;
using HarmonyLib;

// Token: 0x02000013 RID: 19
public class Main
{
	// Token: 0x06000027 RID: 39 RVA: 0x00002ED1 File Offset: 0x000010D1
	public static void Load()
	{
		SISWeaponDurability_Config.ReloadSettings();
		Main.HarmonyInstance = new Harmony("SISWeaponDurability");
		Main.HarmonyInstance.PatchAll();
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002EF4 File Offset: 0x000010F4
	public static void Unload()
	{
		bool flag = Main.HarmonyInstance != null;
		if (flag)
		{
			Main.HarmonyInstance.UnpatchAll(null);
		}
	}

	// Token: 0x04000001 RID: 1
	public static Harmony HarmonyInstance;
}
