using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndMine), "OnDeactivate")]
public class MoveToAndMine_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndMine __instance, Character character, Goal parent)
	{
        if (__instance.Success &&
            SISWeaponDurability_Config.MiningReducesDurability &&
            character.EquippedItem is Pickaxe)
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.MiningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
