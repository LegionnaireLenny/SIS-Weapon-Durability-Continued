using HarmonyLib;

[HarmonyPatch(typeof(AvailableAction), "GetCaption")]
public class AvailableAction_GetCaption__Patch
{
    private static bool Prefix(ref string __result, AvailableAction __instance)
    {
        if (__instance.ActionType == Main.RepairCursorActionID)
        {
            __result = GameImpl.Translate("SPEECH_HUD_RepairWeapon");
            return false;
        }
        return true;
    }
}
