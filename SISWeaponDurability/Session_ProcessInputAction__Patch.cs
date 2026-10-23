using System;
using HarmonyLib;
using UnityEngine;

// Token: 0x0200000F RID: 15
[HarmonyPatch(typeof(Session), "ProcessInputAction")]
public class Session_ProcessInputAction__Patch
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002ABC File Offset: 0x00000CBC
	private static void Postfix(Session __instance, PlayerRecord playerRecord, InputAction action)
	{
		if (action.Type == (InputActionType)2032)
		{
			Equipment equipment = action.GetObject(__instance) as Equipment;
			bool flag2 = equipment != null;
			if (flag2)
			{
				EquipmentPrototype equipmentPrototype = ((equipment is MeleeWeapon || equipment is Bow) ? SISWeaponDurability_Utils.DuctTape : SISWeaponDurability_Utils.GunRepairKit);
				int amountPartsToRepair = SISWeaponDurability_Utils.GetAmountPartsToRepair(equipment);
                if (playerRecord.PlayerCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.DuctTape) >= amountPartsToRepair)
				{
					InfectionType infectionType;
					int num = playerRecord.PlayerCharacter.Inventory.UseItemOfType(playerRecord.PlayerCharacter, playerRecord.PlayerCharacter, equipmentPrototype, amountPartsToRepair, false, out infectionType, 0, false);
                    if (num > 0)
					{
						SISWeaponDurability_Config.Log("Repairing " + equipment.GetDisplayNameString());
						float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(equipment.GetPrototype().Name, equipment is AmmoWeapon);
						equipment.SetDampness(Mathf.Min(maxDurabilityForEquipment, equipment.GetDampness() + 0.4f * maxDurabilityForEquipment));
						SoundManager.PlaySound3D((equipment is MeleeWeapon || equipment is Bow) ? SISWeaponDurability_Config.WeaponRepairSound : SISWeaponDurability_Config.GunRepairSound, playerRecord.PlayerCharacter.Pos, 1f);
					}
				}
			}
		}
	}
}
