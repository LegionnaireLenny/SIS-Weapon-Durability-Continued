using System.Collections.Generic;
using UnityEngine;

public class SISWeaponDurability_Utils
{
    public static int GetAmountPartsToRepair(Equipment item)
    {
        return 1;
    }

    public static void LoadProtos()
    {
        SISWeaponDurability_Utils.DuctTape = GameImpl.Instance.FindEquipmentPrototypeByName("DuctTape");
        SISWeaponDurability_Utils.GunRepairKit = GameImpl.Instance.FindEquipmentPrototypeByName("GunRepairKit");
        SISWeaponDurability_Utils.Glue = GameImpl.Instance.FindEquipmentPrototypeByName("Glue");
        SISWeaponDurability_Utils.JunkParts = GameImpl.Instance.FindEquipmentPrototypeByName("JunkParts");
        SISWeaponDurability_Utils.GunParts = GameImpl.Instance.FindEquipmentPrototypeByName("GunParts");
    }

    public static string GetDisassemblyPrototypeName(EquipmentPrototype equipment)
    {
        if (IsPrototypeGun(equipment))
        {
            return SISWeaponDurability_Config.GunPartPrototypeName;
        }
        else
        {
            return SISWeaponDurability_Config.WeaponPartPrototypeName;
        }
    }

    public static bool IsPrototypeMeleeWeapon(EquipmentPrototype equip)
    {
        return equip.CanBeEquipped &&
            (equip.TypeName == BaseObjectType.MeleeWeapon ||
            equip.TypeName == BaseObjectType.HuntingKnife ||
            equip.TypeName == BaseObjectType.Axe ||
            equip.TypeName == BaseObjectType.Shovel ||
            equip.TypeName == BaseObjectType.Pickaxe ||
            equip.InjuryType == InjuryType.SharpObject ||
            equip.InjuryType == InjuryType.BluntObject);
    }

    public static bool IsPrototypeBow(EquipmentPrototype equip)
    {
        return equip.CanBeEquipped &&
            (equip.TypeName == BaseObjectType.Bow ||
            equip.InjuryType == InjuryType.Arrow);
    }

    public static bool IsPrototypeGun(EquipmentPrototype equip)
    {
        return equip.CanBeEquipped &&
            (equip.TypeName == BaseObjectType.Pistol ||
            equip.TypeName == BaseObjectType.Shotgun ||
            equip.TypeName == BaseObjectType.AssaultRifle ||
            equip.TypeName == BaseObjectType.SniperRifle ||
            equip.TypeName == BaseObjectType.Gun ||
            equip.InjuryType == InjuryType.Bullet);
    }

    public static bool IsPrototypeAmmoWeapon(EquipmentPrototype equip)
    {
        return IsPrototypeBow(equip) || IsPrototypeGun(equip);
    }

    public static bool IsWeaponDegradationEnabledForCharacter(Character character)
    {
        bool val = (SISWeaponDurability_Config.EnabledForNPCs && !character.IsOnPlayersTeam()) ||
            (SISWeaponDurability_Config.EnabledForPlayerCommunity && character.IsOnPlayersTeam() && character != Hud.Instance.LocalControlledCharacter) ||
            (SISWeaponDurability_Config.EnabledForPlayer && character.IsOnPlayersTeam() && character == Hud.Instance.LocalControlledCharacter);

        //     SISWeaponDurability_Config.Log($"{character?.GetDisplayNameString()};{SISWeaponDurability_Config.EnabledForNPCs} && {!character.IsOnPlayersTeam()} ||;" +
        //$"{SISWeaponDurability_Config.EnabledForPlayerCommunity} && {character.IsOnPlayersTeam()} && {character != Hud.Instance.LocalControlledCharacter} ||" +
        //$"{SISWeaponDurability_Config.EnabledForPlayer} && {character.IsOnPlayersTeam()} && {character == Hud.Instance.LocalControlledCharacter} == {val}");
        return val;
    }

    public static void SetStatusBarMsgWithTimeout(string msg, float timeout)
    {
        HudBehaviour.Instance.StatusBarMsg = msg;
        HudBehaviour.Instance.StatusBarMsgTimeout = timeout;
        HudBehaviour.Instance.StatusBarMsgShowForOneFrame = false;
        HudBehaviour.Instance.StatusBarMsgAction = default;
    }

    public static float ReduceDurability(Equipment equip, float amount = 1f, bool isPercentageReduction = false)
    {
        if (amount == 0f || equip == null)
        {
            return -1f;
        }

        WeaponTypeSettings weaponSettings = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip);
        float remainingDurability;
        if (isPercentageReduction)
        {
            remainingDurability = Mathf.Max(0f, equip.GetDampness() - (weaponSettings.MaxDurability * (amount * weaponSettings.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate)));
        }
        else
        {
            remainingDurability = Mathf.Max(0f, equip.GetDampness() - (amount * weaponSettings.DegradeRate * SISWeaponDurability_Config.GlobalDurabilityDegradeRate));
        }

        if (equip.IsAuthoritative())
        {
            SISWeaponDurability_Config.Log($"Reducing durability for {equip.GetPrototype().Name}; {equip.GetDampness()} -> {remainingDurability}");
            equip.SetDampness(remainingDurability);
            if (remainingDurability == 0f)
            {
                BreakEquipment(equip);
            }
        }
        return remainingDurability;
    }

    public static void PlaySound(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        PlaySound(clip, pos, volume, 1f, false, 1f, 3f);
    }

    public static void PlaySoundFromList(List<AudioClip> clips, Vector3 pos, float volume = 1f)
    {
        PlaySound(clips[MathUtil.NonDeterministicRand.Next(clips.Count)], pos, volume, 1f, false, 1f, 3f);
    }

    public static void PlaySound(AudioClip clip, Vector3 pos, float volume, float spatialBlend, bool realisticRolloff, float minRolloffDist, float maxRolloffDist)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = pos;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = volume;
        audioSource.minDistance = minRolloffDist;
        audioSource.maxDistance = maxRolloffDist;
        if (realisticRolloff)
        {
            AudioSourceExtensions.RealisticRolloff(audioSource);
        }
        else
        {
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }
        audioSource.Play();
        Object.Destroy(gameObject, clip.length);
    }

    public static void BreakEquipment(Equipment equip)
    {
        Character weaponOwner = equip.GetAsCharacter();
        if (weaponOwner == null)
        {
            return;
        }

        bool ownerIsPlayerControlled = weaponOwner == Hud.Instance.LocalControlledCharacter;
        bool characterIsConscious = weaponOwner.Alive && !weaponOwner.IsSleepingOrUnconscious();
        if (characterIsConscious)
        {
            if (ownerIsPlayerControlled)
            {
                if (SISWeaponDurability_Config.ShowStatusMsgOnWeaponBroken)
                {
                    string text = GameImpl.Translate(SISWeaponDurability_Config.SpeechTranslateKey).Replace("%1", GameImpl.Translate(equip.GetPrototype().GetNameKey()));
                    SetStatusBarMsgWithTimeout(text, 2f);
                }
            }
            else
            {
                if (SISWeaponDurability_Config.CharactersSpeakOnWeaponBroken)
                {
                    Speech speech = GameImpl.Instance.FindSpeechByUniqueID(SISWeaponDurability_Config.SpeechWeaponBrokenUniqueId);
                    equip.GetAsCharacter().Speak(speech);
                }
            }
        }

        if (SISWeaponDurability_Config.PlaySoundOnWeaponBroken)
        {
            try
            {
                PlaySound(SISWeaponDurability_Config.WeaponBreakSound, weaponOwner.Pos, 1f);
            }
            catch
            {
                SISWeaponDurability_Config.Log("ERROR playing sound for weapon broken.");
            }
        }

        if (SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && weaponOwner == Session.Instance.GetLocalPlayerCharacter())
        {
            NotificationManager.Instance.AddEquipmentNotification(equip.GetAsCharacter(), null, equip, 1);
        }

        EquipmentPrototype scrapParts = JunkParts;
        if (equip is AmmoWeapon ammoWeapon)
        {
            ammoWeapon.Unload(weaponOwner);
            if (equip.GetInjuryType() == InjuryType.Bullet)
            {
                scrapParts = GunParts;

            }
        }

        weaponOwner.Inventory.Remove(weaponOwner, equip);
        if (scrapParts != null)
        {
            int weaponMinPartsOnBroken = SISWeaponDurability_Config.GetWeaponMinPartsOnBroken(equip);
            int weaponMaxPartsOnBroken = SISWeaponDurability_Config.GetWeaponMaxPartsOnBroken(equip);
            int num = NextBetweenInts(weaponOwner, weaponMinPartsOnBroken, weaponMaxPartsOnBroken, 1);
            if (num > 0)
            {
                Equipment equipment = Equipment.Spawn(scrapParts, num);
                weaponOwner.Inventory.Add(weaponOwner, equipment);
                if (SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && Session.Instance.GetLocalPlayerCharacter() == weaponOwner)
                {
                    NotificationManager.Instance.AddEquipmentNotification(null, weaponOwner, equipment, num);
                }
            }
        }
    }

    public static float NextFloat(Character character)
    {
        int num = (int)PredictedObjectManager.Instance.GetCurrentTime(character.IsPredicted()).Ticks + character.Id * 9982;
        return MathUtil.RandomFloat((float)num);
    }

    public static int NextBetweenInts(Character character, int min, int max, int seedMult = 1)
    {
        int num = (int)PredictedObjectManager.Instance.GetCurrentTime(character.IsPredicted()).Ticks + character.Id * 9982 * seedMult;
        return MathUtil.RandomInt(num, min, max + 1);
    }

    public static int NextBetweenInts(bool isPredicted, int min, int max, int seedMult = 1)
    {
        int num = (int)PredictedObjectManager.Instance.GetCurrentTime(isPredicted).Ticks * 3854 * seedMult;
        return MathUtil.RandomInt(num, min, max + 1);
    }

    public static EquipmentPrototype DuctTape;
    public static EquipmentPrototype GunRepairKit;
    public static EquipmentPrototype Glue;
    public static EquipmentPrototype JunkParts;
    public static EquipmentPrototype GunParts;

    public static Color32 Pristine = new Color32(114, 181, 13, byte.MaxValue);
    public static Color32 Used = new Color32(224, 213, 0, byte.MaxValue);
    public static Color32 Damaged = new Color32(224, 142, 0, byte.MaxValue);
    public static Color32 VeryDamaged = new Color32(224, 41, 0, byte.MaxValue);
}
