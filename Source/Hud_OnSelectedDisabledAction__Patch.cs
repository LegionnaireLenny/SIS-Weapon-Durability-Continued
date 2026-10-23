using System;
using HarmonyLib;

// Token: 0x02000006 RID: 6
[HarmonyPatch(typeof(Hud), "OnSelectedDisabledAction")]
public class Hud_OnSelectedDisabledAction__Patch
{
	// Token: 0x06000009 RID: 9 RVA: 0x000022A0 File Offset: 0x000004A0
	private static bool Prefix(Hud __instance, AvailableAction action)
	{
		Character actor = action.Actor;
		BaseObject target = action.Target;
		if (actor != null && target != null)
		{
			if (action.Enabled == (CursorActionDisabledReason)7005)
			{
				GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_NoDuctTape"));
				return false;
			}
			if (action.Enabled == (CursorActionDisabledReason)7004)
			{
				GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_NoGunRepairKit"));
				return false;
			}
			if (action.Enabled == (CursorActionDisabledReason)7006)
			{
				GameImpl.Instance.ShowMessageBox(GameImpl.Translate("SPEECH_HUD_CantRepairInCombat"));
				return false;
			}
		}
		return true;
	}
}
