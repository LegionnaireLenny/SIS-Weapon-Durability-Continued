using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(AmmoWeapon), "OnFired")]
public class AmmoWeapon_OnFired__Patch
{
	private static void Postfix(AmmoWeapon __instance, Character character, Target target, TargettableBodyLocation targetBodyLocation, Vector3 targetPos, float throwAngle, float throwSpeed, bool assassinate, SecrecyMode secret, bool fromAI)
	{
		if (__instance == null || 
			character == null || 
			!SISWeaponDurability_Config.EnabledForAmmoWeapons ||
            SISWeaponDurability_Config.ExcludedWeapons.IndexOf(__instance.GetPrototype().Name) != -1 ||
			(!SISWeaponDurability_Config.EnabledForPlayerCommunity && character.IsOnPlayersTeam() && character != Hud.Instance.LocalControlledCharacter) ||
			(!SISWeaponDurability_Config.EnabledForNPCs && !character.IsControllableByPlayer() && !character.IsOnPlayersTeam()))
		{
			return;
		}

		WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance.GetPrototype().Name, false);
		SISWeaponDurability_Utils.ReduceDurability(__instance, 1f * weaponModifiersForEquipment.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate);
	}
}
