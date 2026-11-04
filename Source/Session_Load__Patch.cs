using HarmonyLib;

[HarmonyPatch(typeof(Session), "Load")]
public class Session_Load__Patch
{
    private static bool IsFirstLoad = true;

    private static void Finalizer(Session __instance, bool newGame)
    {
        if (!IsFirstLoad)
        {
            SISWeaponDurability_Config.ReloadSettings();
            SISWeaponDurability_Config.Log("Settings reloaded!");
        }
        IsFirstLoad = false;
        SISWeaponDurability_Utils.LoadProtos();
    }
}
