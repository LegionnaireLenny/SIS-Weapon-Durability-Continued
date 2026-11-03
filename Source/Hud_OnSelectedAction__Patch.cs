using HarmonyLib;

[HarmonyPatch(typeof(Hud), "OnSelectedAction")]
public class Hud_OnSelectedAction__Patch
{
    private static void Postfix(Hud __instance, InputFrame inputFrame, AvailableAction action, bool isDoubleClick, ref bool captureAltAction)
    {
        if (action.Target == null)
        {
            return;
        }

        Equipment equipment = action.Target as Equipment;
        if (equipment == null || action.ActionType != Main.RepairCursorActionID)
        {
            return;
        }

        EquipmentPrototype equipmentPrototype;
        if (equipment is MeleeWeapon || equipment is Bow)
        {
            equipmentPrototype = SISWeaponDurability_Utils.DuctTape;
        }
        else if (equipment is Gun)
        {
            equipmentPrototype = SISWeaponDurability_Utils.GunRepairKit;
        }
        else
        {
            return;
        }

        GameImpl.Instance.ShowConfirmationBox(
            GameImpl.Translate("SPEECH_HUD_ConfirmRepairWeapon")
                .Replace("%1", "1")
                .Replace("%2", GameImpl.Translate(equipmentPrototype.NameHash))
                .Replace("%3", equipment.GetDisplayNameString()
                ),
            delegate (InputFrame inputFrame2)
            {
                InputAction inputAction = new InputAction((InputActionType)2032);
                inputAction.ObjectId = action.Target.Id;
                inputAction.SetObject(action.Target);
                inputFrame.AddAction(inputAction);
            },
            false);
    }
}
