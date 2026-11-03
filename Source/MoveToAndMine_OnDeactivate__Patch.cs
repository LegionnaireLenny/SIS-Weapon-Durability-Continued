using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndMine), "OnDeactivate")]
public class MoveToAndMine_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndMine __instance, Character character, Goal parent)
	{
        SISWeaponDurability_Config.Log($"{character.GetDisplayNameString()} finished mining");
        if (SISWeaponDurability_Config.MiningReducesDurability &&
            __instance.Success &&
            character.EquippedItem is Pickaxe &&
            SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(character))
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.MiningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
