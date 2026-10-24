using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "Init")]
public class Equipment_Init__Patch
{
	private static void Postfix(Equipment __instance)
	{
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() == 0f)
		{
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance);
			__instance.SetDampness(weaponModifiersForEquipment.MaxDurability);
		}
	}
}
