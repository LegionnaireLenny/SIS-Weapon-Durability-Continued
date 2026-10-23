using System;
using HarmonyLib;

// Token: 0x02000016 RID: 22
[HarmonyPatch(typeof(MoveToAndChop), "OnDeactivate")]
public class MoveToAndChop_OnDeactivate__Patch
{
	// Token: 0x0600002E RID: 46 RVA: 0x000030D4 File Offset: 0x000012D4
	private static void Postfix(MoveToAndChop __instance, Character character, Goal parent)
	{
		bool success = __instance.Success;
		if (success)
		{
			SISWeaponDurability_Config.Log("Chopping successful");
			bool choppingReducesDurability = SISWeaponDurability_Config.ChoppingReducesDurability;
			if (choppingReducesDurability)
			{
				WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
				bool flag = character.EquippedItem is HuntingKnife;
				if (flag)
				{
					SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.ChoppingDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
				}
			}
		}
	}
}
