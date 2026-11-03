using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(AmmoWeapon), "OnFired")]
public class AmmoWeapon_OnFired__Patch
{
	private static void Postfix(AmmoWeapon __instance, Character character, Target target, TargettableBodyLocation targetBodyLocation, Vector3 targetPos, float throwAngle, float throwSpeed, bool assassinate, SecrecyMode secret, bool fromAI)
	{
        SISWeaponDurability_Config.Log($"{character?.GetDisplayNameString()} shot at {target?.Object?.GetDisplayNameString()} with {__instance.GetDisplayNameString()}");
		if (__instance == null ||
			character == null ||
			!SISWeaponDurability_Config.EnabledForAmmoWeapons ||
			SISWeaponDurability_Config.ExcludedWeapons.IndexOf(__instance.GetPrototype().Name) != -1 ||
			!SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(character))
		{
			return;
		}

        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance);
		SISWeaponDurability_Utils.ReduceDurability(__instance, 1f * weaponModifiersForEquipment.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate);
	}
}
