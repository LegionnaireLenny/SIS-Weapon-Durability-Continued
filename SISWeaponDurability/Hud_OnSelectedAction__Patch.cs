using System;
using HarmonyLib;

// Token: 0x0200000E RID: 14
[HarmonyPatch(typeof(Hud), "OnSelectedAction")]
public class Hud_OnSelectedAction__Patch
{
	// Token: 0x0600001C RID: 28 RVA: 0x000029C0 File Offset: 0x00000BC0
	private static void Postfix(Hud __instance, InputFrame inputFrame, AvailableAction action, bool isDoubleClick, ref bool captureAltAction)
	{
		bool flag = action.Target == null;
		if (!flag)
		{
			Equipment equipment = action.Target as Equipment;
			bool flag2 = equipment == null;
			if (!flag2)
			{
				bool flag3 = action.ActionType == 2032;
				if (flag3)
				{
					EquipmentPrototype equipmentPrototype = ((equipment is MeleeWeapon || equipment is Bow) ? SISWeaponDurability_Utils.DuctTape : SISWeaponDurability_Utils.GunRepairKit);
					int amountPartsToRepair = SISWeaponDurability_Utils.GetAmountPartsToRepair(equipment);
					GameImpl.Instance.ShowConfirmationBox(GameImpl.Translate("SPEECH_HUD_ConfirmRepairWeapon").Replace("%1", amountPartsToRepair.ToString()).Replace("%2", GameImpl.Translate(equipmentPrototype.NameHash))
						.Replace("%3", equipment.GetDisplayNameString()), delegate(InputFrame inputFrame2)
					{
						InputAction inputAction;
						inputAction..ctor(2032);
						inputAction.ObjectId = action.Target.Id;
						inputAction.SetObject(action.Target);
						inputFrame.AddAction(inputAction);
					}, false);
				}
			}
		}
	}
}
