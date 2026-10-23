using HarmonyLib;

[HarmonyPatch(typeof(Hud), "OnSelectedDisabledAction")]
public class Hud_OnSelectedDisabledAction__Patch
{
	private static bool Prefix(Hud __instance, AvailableAction action)
    {
        if (action.Actor == null || action.Target == null)
        {
            return true;
        }

        if (action.Enabled == Main.NotEnoughDuctTape)
        {
            GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_NoDuctTape"));
            return false;
        }

        if (action.Enabled == Main.NotEnoughGunRepairKits)
        {
            GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_NoGunRepairKit"));
            return false;
        }

        if (action.Enabled == Main.CantRepairInCombat)
        {
            GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_CantRepairInCombat"));
            return false;
        }

        return true;
    }
}
