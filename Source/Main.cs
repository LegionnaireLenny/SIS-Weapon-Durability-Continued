using System;
using HarmonyLib;

public class Main
{
	public static Harmony HarmonyInstance;
	public static readonly int RepairActionID = 2032;
	public static readonly InputActionType RepairInputActionID = (InputActionType)RepairActionID;
	public static readonly CursorAction RepairCursorActionID = (CursorAction)RepairActionID;
	public static readonly CursorActionDisabledReason CharacterIncapacitated = (CursorActionDisabledReason)7007;
	public static readonly CursorActionDisabledReason CantRepairInCombat = (CursorActionDisabledReason)7006;
	public static readonly CursorActionDisabledReason NotEnoughDuctTape = (CursorActionDisabledReason)7005;
	public static readonly CursorActionDisabledReason NotEnoughGunRepairKits = (CursorActionDisabledReason)7004;
	public static readonly CursorActionDisabledReason ItemNotDamaged = (CursorActionDisabledReason)7003;


    public static void Load()
	{
		SISWeaponDurability_Config.ReloadSettings();
		HarmonyInstance = new Harmony("SISWeaponDurability");
		HarmonyInstance.PatchAll();
	}

	public static void Unload()
	{
		HarmonyInstance?.UnpatchAll(null);
	}
}
