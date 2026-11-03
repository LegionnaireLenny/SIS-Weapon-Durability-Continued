using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndSkin), "OnDeactivate")]
public class MoveToAndSkin_OnDeactivate__Patch
{
    private static void Postfix(MoveToAndSkin __instance, Character character, Goal parent)
    {
        SISWeaponDurability_Config.Log($"{character.GetDisplayNameString()} finished skinning");
        if (SISWeaponDurability_Config.SkinningReducesDurability &&
            __instance.Success &&
            character.EquippedItem is HuntingKnife &&
            SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(character))
        {
            SISWeaponDurability_Config.Log("Skinning successful");
            WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem);
            SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.SkinningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
        }
    }
}
