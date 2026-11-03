using HarmonyLib;

[HarmonyPatch(typeof(MoveToAndChop), "OnDeactivate")]
public class MoveToAndChop_OnDeactivate__Patch
{
    private static void Postfix(MoveToAndChop __instance, Character character, Goal parent)
    {
        SISWeaponDurability_Config.Log($"{character.GetDisplayNameString()} finished chopping");
        if (SISWeaponDurability_Config.ChoppingReducesDurability &&
            __instance.Success &&
            character.EquippedItem is Axe &&
            SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(character))
        {
            SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.ChoppingDurabilityDegradePercentage, true);
        }
    }
}
