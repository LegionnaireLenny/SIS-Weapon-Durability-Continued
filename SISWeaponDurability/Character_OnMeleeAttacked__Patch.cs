using System;
using HarmonyLib;
using UnityEngine;

// Token: 0x02000009 RID: 9
[HarmonyPatch(typeof(Character), "OnMeleeAttacked", new Type[]
{
	typeof(Character),
	typeof(TileObject),
	typeof(InjuryType),
	typeof(InjuryLocation),
	typeof(InfectionType),
	typeof(Vector3),
	typeof(Vector3),
	typeof(Bone),
	typeof(Vector3),
	typeof(float),
	typeof(bool),
	typeof(bool),
	typeof(bool),
	typeof(bool),
	typeof(TargettableBodyLocation),
	typeof(bool)
}, new ArgumentType[]
{
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Out,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal,
	ArgumentType.Normal
})]
public class Character_OnMeleeAttacked__Patch
{
	// Token: 0x0600000F RID: 15 RVA: 0x000023EC File Offset: 0x000005EC
	private static void Postfix(Character __instance, Character source, TileObject intendedTarget, InjuryType injuryType, InjuryLocation injuryLocation, InfectionType infectionType, Vector3 nonDeterministicAttackPos, Vector3 attackDir, Bone bone, Vector3 hitPosInBoneSpace, float damage)
	{
		bool flag = source == null || source.EquippedItem == null;
		if (!flag)
		{
			bool flag2 = !SISWeaponDurability_Config.EnabledForMeleeWeapons && source.EquippedItem is MeleeWeapon;
			if (!flag2)
			{
				bool flag3 = injuryType != 6 && injuryType != 5;
				if (!flag3)
				{
					bool flag4 = SISWeaponDurability_Config.ExcludedWeapons.IndexOf(source.EquippedItem.GetPrototype().Name) != -1;
					if (!flag4)
					{
						bool flag5 = !SISWeaponDurability_Config.EnabledForPlayerCommunity && source.IsOnPlayersTeam() && source != Hud.Instance.LocalControlledCharacter;
						if (!flag5)
						{
							bool flag6 = !SISWeaponDurability_Config.EnabledForNPCs && !source.IsControllableByPlayer() && !source.IsOnPlayersTeam();
							if (!flag6)
							{
								WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(source.EquippedItem.GetPrototype().Name, false);
								float num = 1f * weaponModifiersForEquipment.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate;
								bool hitLocationAffectsDegradation = SISWeaponDurability_Config.HitLocationAffectsDegradation;
								if (hitLocationAffectsDegradation)
								{
									switch (injuryLocation)
									{
									case 0:
									case 2:
									case 3:
										num = SISWeaponDurability_Config.HitLocationTopModifierPcnt * num;
										break;
									case 1:
										num = SISWeaponDurability_Config.HitLocationHeadModifierPcnt * num;
										break;
									case 4:
									case 5:
										num = SISWeaponDurability_Config.HitLocationBottomModifierPcnt * num;
										break;
									}
								}
								SISWeaponDurability_Utils.ReduceDurability(source.EquippedItem, num);
							}
						}
					}
				}
			}
		}
	}
}
