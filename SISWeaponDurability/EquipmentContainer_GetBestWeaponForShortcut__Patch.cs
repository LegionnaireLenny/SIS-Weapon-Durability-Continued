using System;
using HarmonyLib;

// Token: 0x02000004 RID: 4
[HarmonyPatch(typeof(EquipmentContainer), "GetBestWeaponForShortcut")]
public class EquipmentContainer_GetBestWeaponForShortcut__Patch
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002198 File Offset: 0x00000398
	private static void Postfix(ref Equipment __result, EquipmentContainer __instance, Character character, Type type, int count)
	{
		bool flag = type == typeof(MeleeWeapon);
		if (flag)
		{
			int num = 0;
			foreach (Equipment equipment in character.Inventory.Contents)
			{
				bool flag2 = equipment is MeleeWeapon;
				if (flag2)
				{
					bool flag3 = num == count;
					if (flag3)
					{
						__result = equipment;
						break;
					}
					num++;
				}
			}
		}
	}
}
