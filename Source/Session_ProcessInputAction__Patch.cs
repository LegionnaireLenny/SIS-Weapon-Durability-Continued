using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Session), "ProcessInputAction")]
public class Session_ProcessInputAction__Patch
{
	private static void Postfix(Session __instance, PlayerRecord playerRecord, InputAction action)
    {
        if (action.Type != Main.RepairInputActionID)
        {
            return;
        }

        Equipment equipment = action.GetObject(__instance) as Equipment;
        if (equipment == null)
        {
            return;
        }

        EquipmentPrototype equipmentPrototype;
        AudioClip repairSound;
        if (equipment is MeleeWeapon || equipment is Bow)
        {
            equipmentPrototype = SISWeaponDurability_Utils.DuctTape;
            repairSound = SISWeaponDurability_Config.WeaponRepairSound;
        }
        else if (equipment is Gun)
        {
            equipmentPrototype = SISWeaponDurability_Utils.GunRepairKit;
            repairSound = SISWeaponDurability_Config.GunRepairSound;
        }
        else
        {
            return;
        }

        if (playerRecord.PlayerCharacter.Inventory.CountItemsOfType(equipmentPrototype) < 1)
        {
            return;
        }

        int num = playerRecord.PlayerCharacter.Inventory.UseItemOfType(playerRecord.PlayerCharacter, playerRecord.PlayerCharacter, equipmentPrototype, 1, false, out InfectionType infectionType, 0, false);
        if (num > 0)
        {
            SISWeaponDurability_Config.Log("Repairing " + equipment.GetDisplayNameString());
            float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(equipment.GetPrototype().Name, equipment is AmmoWeapon);
            equipment.SetDampness(Mathf.Min(maxDurabilityForEquipment, equipment.GetDampness() + 0.4f * maxDurabilityForEquipment));
            SoundManager.PlaySound3D(repairSound, playerRecord.PlayerCharacter.Pos, 1f);
        }
    }
}
