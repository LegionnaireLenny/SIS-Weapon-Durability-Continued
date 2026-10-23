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
		bool flag = action.Type == 2032;
		if (flag)
		{
			Equipment equipment = action.GetObject(__instance) as Equipment;
			bool flag2 = equipment != null;
			if (flag2)
			{
				EquipmentPrototype equipmentPrototype = ((equipment is MeleeWeapon || equipment is Bow) ? SISWeaponDurability_Utils.DuctTape : SISWeaponDurability_Utils.GunRepairKit);
				int amountPartsToRepair = SISWeaponDurability_Utils.GetAmountPartsToRepair(equipment);
				bool flag3 = playerRecord.PlayerCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.DuctTape) >= amountPartsToRepair;
				if (flag3)
				{
					InfectionType infectionType;
					int num = playerRecord.PlayerCharacter.Inventory.UseItemOfType(playerRecord.PlayerCharacter, playerRecord.PlayerCharacter, equipmentPrototype, amountPartsToRepair, false, ref infectionType, 0, false);
					bool flag4 = num > 0;
					if (flag4)
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
