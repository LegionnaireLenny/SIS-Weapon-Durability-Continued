using System;
using HarmonyLib;
using UnityEngine;

// Token: 0x02000002 RID: 2
[HarmonyPatch(typeof(AmmoWeapon), "OnFired")]
public class AmmoWeapon_OnFired__Patch
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private static void Postfix(AmmoWeapon __instance, Character character, Target target, TargettableBodyLocation targetBodyLocation, Vector3 targetPos, float throwAngle, float throwSpeed, bool assassinate, SecrecyMode secret, bool fromAI)
	{
		bool flag = character == null;
		if (!flag)
		{
			bool flag2 = !SISWeaponDurability_Config.EnabledForAmmoWeapons && __instance != null;
			if (!flag2)
			{
				bool flag3 = SISWeaponDurability_Config.ExcludedWeapons.IndexOf(__instance.GetPrototype().Name) != -1;
				if (!flag3)
				{
					bool flag4 = !SISWeaponDurability_Config.EnabledForPlayerCommunity && character.IsOnPlayersTeam() && character != Hud.Instance.LocalControlledCharacter;
					if (!flag4)
					{
						bool flag5 = !SISWeaponDurability_Config.EnabledForNPCs && !character.IsControllableByPlayer() && !character.IsOnPlayersTeam();
						if (!flag5)
						{
							WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__instance.GetPrototype().Name, false);
							SISWeaponDurability_Utils.ReduceDurability(__instance, 1f * weaponModifiersForEquipment.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate);
						}
					}
				}
			}
		}
	}
}
