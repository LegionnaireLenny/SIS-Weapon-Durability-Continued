using System;
using HarmonyLib;

// Token: 0x02000017 RID: 23
[HarmonyPatch(typeof(MoveToAndMine), "OnDeactivate")]
public class MoveToAndMine_OnDeactivate__Patch
{
	// Token: 0x06000030 RID: 48 RVA: 0x00003150 File Offset: 0x00001350
	private static void Postfix(MoveToAndMine __instance, Character character, Goal parent)
	{
		bool success = __instance.Success;
		if (success)
		{
			bool miningReducesDurability = SISWeaponDurability_Config.MiningReducesDurability;
			if (miningReducesDurability)
			{
				WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
				bool flag = character.EquippedItem is Pickaxe;
				if (flag)
				{
					SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.MiningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
				}
			}
		}
	}
}
