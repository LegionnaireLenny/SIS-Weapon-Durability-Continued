using System;
using HarmonyLib;

// Token: 0x02000014 RID: 20
[HarmonyPatch(typeof(MoveToAndChokeHold), "OnDeactivate")]
public class MoveToAndChokeHold_OnDeactivate__Patch
{
	// Token: 0x0600002A RID: 42 RVA: 0x00002F28 File Offset: 0x00001128
	private static void Postfix(MoveToAndChokeHold __instance, Character character, Goal parent)
	{
		bool flag = __instance.Success && __instance.Assassinate;
		if (flag)
		{
			bool assassinationReducesDurability = SISWeaponDurability_Config.AssassinationReducesDurability;
			if (assassinationReducesDurability)
			{
				WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
				bool flag2 = character.EquippedItem is HuntingKnife;
				if (flag2)
				{
					SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.AssassinationDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
				}
			}
		}
	}
}
