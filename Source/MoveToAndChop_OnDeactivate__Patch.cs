using System;
using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndChop), "OnDeactivate")]
public class MoveToAndChop_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndChop __instance, Character character, Goal parent)
	{
        if (__instance.Success && 
			SISWeaponDurability_Config.ChoppingReducesDurability && 
			character.EquippedItem is Axe)
		{
			SISWeaponDurability_Config.Log("Chopping successful");
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.ChoppingDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
