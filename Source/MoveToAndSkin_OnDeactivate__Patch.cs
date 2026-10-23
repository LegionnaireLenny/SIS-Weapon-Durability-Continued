using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndSkin), "OnDeactivate")]
public class MoveToAndSkin_OnDeactivate__Patch
{
	private static void Postfix(MoveToAndSkin __instance, Character character, Goal parent)
	{
        if (__instance.Success && 
			SISWeaponDurability_Config.SkinningReducesDurability && 
			character.EquippedItem is HuntingKnife)
		{
			SISWeaponDurability_Config.Log("Skinning successful");
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
			SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.SkinningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
		}
	}
}
