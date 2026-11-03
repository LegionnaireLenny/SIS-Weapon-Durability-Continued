using HarmonyLib;

[HarmonyPatch(typeof(Session), "Load")]
public class Session_Load__Patch
{
    private static void Finalizer(Session __instance, bool newGame)
    {
        SISWeaponDurability_Config.LoadSettings();
        SISWeaponDurability_Utils.LoadProtos();
    }
}
