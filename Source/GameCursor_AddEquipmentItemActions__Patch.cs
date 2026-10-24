using HarmonyLib;

[HarmonyPatch(typeof(GameCursor), "AddEquipmentItemActions")]
public class GameCursor_AddEquipmentItemActions__Patch
{
	private static void RemoveRepairActions(GameCursor __instance)
	{
		for (int i = __instance.AvailableActions.Count - 1; i >= 0; i--)
		{
            if (__instance.AvailableActions[i].Recipe != null && __instance.AvailableActions[i].ActionType == CursorAction.Craft)
			{
				__instance.AvailableActions.RemoveAt(i);
			}
		}
	}

	private static void Postfix(GameCursor __instance, Equipment item, InfoPage page, TileObject carrier, Character controlledCharacter)
    {
        if (page is TradePage || item == null)
        {
            return;
        }

        CursorActionDisabledReason cursorActionDisabledReason = CursorActionDisabledReason.Enabled;
        if (controlledCharacter.Consciousness == Consciousness.Unconscious || controlledCharacter.Consciousness == Consciousness.Dead)
        {
            cursorActionDisabledReason = Main.CharacterIncapacitated;
        }
        else if (GameCursor.IsTargetCharacterInCombat(controlledCharacter) && controlledCharacter.IsOutdoors())
        {
            cursorActionDisabledReason = Main.CantRepairInCombat;
        }
        else if (item.GetDampness() >= SISWeaponDurability_Config.GetMaxDurabilityForEquipment(item))
        {
            cursorActionDisabledReason = Main.ItemNotDamaged;
        }

        if (item is MeleeWeapon || item is Bow)
        {
            if (controlledCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.DuctTape) < 1)
            {
                cursorActionDisabledReason = Main.NotEnoughDuctTape;
            }
            __instance.AvailableActions.Add(new AvailableAction(Main.RepairCursorActionID, controlledCharacter, item, cursorActionDisabledReason));
        }
        else if (item is Gun)
        {
            if (controlledCharacter.Inventory.CountItemsOfType(SISWeaponDurability_Utils.GunRepairKit) < 1)
            {
                cursorActionDisabledReason = Main.NotEnoughGunRepairKits;
            }
            __instance.AvailableActions.Add(new AvailableAction(Main.RepairCursorActionID, controlledCharacter, item, cursorActionDisabledReason));
        }
    }
}
