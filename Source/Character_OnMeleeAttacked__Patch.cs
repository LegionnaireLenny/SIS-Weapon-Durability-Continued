using HarmonyLib;
using System;
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
        SISWeaponDurability_Config.Log($"{source?.GetDisplayNameString()} attacked {__instance?.GetDisplayNameString()} with {source?.EquippedItem?.GetDisplayNameString() ?? "unarmed attack"}");
        if (source == null ||
            source.EquippedItem == null ||
            !SISWeaponDurability_Config.EnabledForMeleeWeapons ||
            (injuryType != InjuryType.BluntObject && injuryType != InjuryType.SharpObject) ||
            (SISWeaponDurability_Config.ExcludedWeapons.IndexOf(source.EquippedItem.GetPrototype().Name) != -1) ||
            !SISWeaponDurability_Utils.IsWeaponDegradationEnabledForCharacter(source))
        {
            return;
        }

        if (!(source.EquippedItem is MeleeWeapon))
        {
            SISWeaponDurability_Config.Log($"OnMeleeAttack triggered with non-melee weapon");
        }

        float num = 1f;
        if (SISWeaponDurability_Config.HitLocationAffectsDegradation)
        {
            switch (injuryLocation)
            {
                case InjuryLocation.Torso:
                case InjuryLocation.LeftArm:
                case InjuryLocation.RightArm:
                    num *= SISWeaponDurability_Config.HitLocationTopModifierPcnt;
                    break;
                case InjuryLocation.Head:
                    num *= SISWeaponDurability_Config.HitLocationHeadModifierPcnt;
                    break;
                case InjuryLocation.LeftLeg:
                case InjuryLocation.RightLeg:
                    num *= SISWeaponDurability_Config.HitLocationBottomModifierPcnt;
                    break;
            }
        }
        SISWeaponDurability_Utils.ReduceDurability(source.EquippedItem, num);
    }
}
