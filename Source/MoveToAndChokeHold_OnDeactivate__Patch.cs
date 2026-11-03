using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndChokeHold), "OnDeactivate")]
public class MoveToAndChokeHold_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndChokeHold __instance, Character character, Goal parent)
	{
        SISWeaponDurability_Config.Log($"{character.GetDisplayNameString()} finished choking");
        if (SISWeaponDurability_Config.AssassinationReducesDurability &&
            __instance.Success && 
			__instance.Assassinate &&
            character.EquippedItem is HuntingKnife &&
            SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(character))
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.AssassinationDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
