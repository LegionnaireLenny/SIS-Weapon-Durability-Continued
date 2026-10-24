using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndChokeHold), "OnDeactivate")]
public class MoveToAndChokeHold_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndChokeHold __instance, Character character, Goal parent)
	{
        if (__instance.Success && 
			__instance.Assassinate &&
            SISWeaponDurability_Config.AssassinationReducesDurability &&
            character.EquippedItem is HuntingKnife)
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.AssassinationDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
