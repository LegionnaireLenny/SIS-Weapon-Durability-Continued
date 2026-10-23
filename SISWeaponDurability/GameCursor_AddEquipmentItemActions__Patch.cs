using System;
using HarmonyLib;

// Token: 0x02000010 RID: 16
[HarmonyPatch(typeof(GameCursor), "AddEquipmentItemActions")]
public class GameCursor_AddEquipmentItemActions__Patch
{
	// Token: 0x06000020 RID: 32 RVA: 0x00002C08 File Offset: 0x00000E08
	private static void RemoveRepairActions(GameCursor __instance)
	{
		for (int i = __instance.AvailableActions.Count - 1; i >= 0; i--)
		{
			bool flag = __instance.AvailableActions[i].ActionType == 246 && __instance.AvailableActions[i].Recipe != null;
			if (flag)
			{
				__instance.AvailableActions.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002C7C File Offset: 0x00000E7C
	private static void Postfix(GameCursor __instance, Equipment item, InfoPage page, TileObject carrier, Character controlledCharacter)
	{
		bool flag = page is TradePage;
		if (!flag)
		{
			bool flag2 = item != null;
			if (flag2)
			{
				CursorActionDisabledReason cursorActionDisabledReason = ((controlledCharacter.Consciousness == 2 || controlledCharacter.Consciousness == 3) ? 1 : 0);
				CursorActionDisabledReason cursorActionDisabledReason2 = ((GameCursor.IsTargetCharacterInCombat(controlledCharacter) && controlledCharacter.IsOutdoors()) ? 7006 : cursorActionDisabledReason);
				float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(item.GetPrototype().Name, item is AmmoWeapon);
				bool flag3 = item is MeleeWeapon || item is Bow;
				if (flag3)
				{
					bool flag4 = item.GetDampness() == maxDurabilityForEquipment;
					if (!flag4)
					{
						int amountPartsToRepair = SISWeaponDurability_Utils.GetAmountPartsToRepair(item);
						CursorActionDisabledReason cursorActionDisabledReason3 = ((controlledCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.DuctTape) >= amountPartsToRepair) ? cursorActionDisabledReason2 : 7005);
						__instance.AvailableActions.Add(new AvailableAction(2032, controlledCharacter, item, (item.GetDampness() < maxDurabilityForEquipment) ? cursorActionDisabledReason3 : 1));
					}
				}
				else
				{
					bool flag5 = item is Gun;
					if (flag5)
					{
						bool flag6 = item.GetDampness() == maxDurabilityForEquipment;
						if (!flag6)
						{
							CursorActionDisabledReason cursorActionDisabledReason4 = ((controlledCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.GunRepairKit) >= 1) ? cursorActionDisabledReason2 : 7004);
							__instance.AvailableActions.Add(new AvailableAction(2032, controlledCharacter, item, (item.GetDampness() < maxDurabilityForEquipment) ? cursorActionDisabledReason4 : 1));
						}
					}
				}
			}
		}
	}
}
