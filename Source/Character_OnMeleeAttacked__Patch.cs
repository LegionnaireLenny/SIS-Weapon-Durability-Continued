using System;
using HarmonyLib;
using UnityEngine;

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
	private static void Postfix(Character __instance, Character source, TileObject intendedTarget, InjuryType injuryType, InjuryLocation injuryLocation, InfectionType infectionType, Vector3 nonDeterministicAttackPos, Vector3 attackDir, Bone bone, Vector3 hitPosInBoneSpace, float damage)
    {
        if (source == null ||
            source.EquippedItem == null ||
            (!SISWeaponDurability_Config.EnabledForMeleeWeapons && source.EquippedItem is MeleeWeapon) ||
            (injuryType != InjuryType.BluntObject && injuryType != InjuryType.SharpObject) ||
            (SISWeaponDurability_Config.ExcludedWeapons.IndexOf(source.EquippedItem.GetPrototype().Name) != -1) ||
            (!SISWeaponDurability_Config.EnabledForPlayerCommunity && source.IsOnPlayersTeam() && source != Hud.Instance.LocalControlledCharacter) ||
            (!SISWeaponDurability_Config.EnabledForNPCs && !source.IsControllableByPlayer() && !source.IsOnPlayersTeam()))
        {
            return;
        }

        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(source.EquippedItem);
        float num = 1f * weaponModifiersForEquipment.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate;
        bool hitLocationAffectsDegradation = SISWeaponDurability_Config.HitLocationAffectsDegradation;
        if (hitLocationAffectsDegradation)
        {
            switch (injuryLocation)
            {
                case InjuryLocation.Torso:
                case InjuryLocation.LeftArm:
                case InjuryLocation.RightArm:
                    num = SISWeaponDurability_Config.HitLocationTopModifierPcnt * num;
                    break;
                case InjuryLocation.Head:
                    num = SISWeaponDurability_Config.HitLocationHeadModifierPcnt * num;
                    break;
                case InjuryLocation.LeftLeg:
                case InjuryLocation.RightLeg:
                    num = SISWeaponDurability_Config.HitLocationBottomModifierPcnt * num;
                    break;
            }
        }
        SISWeaponDurability_Utils.ReduceDurability(source.EquippedItem, num);
    }
}
