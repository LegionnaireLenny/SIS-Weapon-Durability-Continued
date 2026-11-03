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

    public static float ReduceDurability(Equipment equip, float amount = 1f)
    {
        float num;
        if (amount == 0f || equip == null)
        {
            num = -1f;
        }
        else
        {
            float num2 = Mathf.Max(equip.GetDampness() - amount, 0f);
            if (equip.IsAuthoritative())
            {
                equip.SetDampness(num2);
                SISWeaponDurability_Config.Log("Reduced durability for " + equip.GetPrototype().Name + ", dur = " + num2.ToString());
                if (num2 == 0f)
                {
                    SISWeaponDurability_Utils.BreakEquipment(equip);
                }
            }
            num = num2;
        }
        return num;
    }

    public static void PlaySound(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        SISWeaponDurability_Utils.PlaySound(clip, pos, volume, 1f, false, 1f, 3f);
    }

    public static void PlaySoundFromList(List<AudioClip> clips, Vector3 pos, float volume = 1f)
    {
        SISWeaponDurability_Utils.PlaySound(clips[MathUtil.NonDeterministicRand.Next(clips.Count)], pos, volume, 1f, false, 1f, 3f);
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
        global::UnityEngine.Object.Destroy(gameObject, clip.length);
    }

    public static void BreakEquipment(Equipment equip)
    {
        if (equip.GetAsCharacter() == null)
        {
            return;
        }

        bool ownerIsPlayerControlled = equip.GetAsCharacter() == Hud.Instance.LocalControlledCharacter;
        bool characterIsConscious = equip.GetAsCharacter().Alive && !equip.GetAsCharacter().IsSleepingOrUnconscious();
        if (characterIsConscious)
        {
            if (ownerIsPlayerControlled)
            {
                if (SISWeaponDurability_Config.ShowStatusMsgOnWeaponBroken)
                {
                    string text = GameImpl.Translate(SISWeaponDurability_Config.SpeechTranslateKey).Replace("%1", GameImpl.Translate(equip.GetPrototype().GetNameKey()));
                    SISWeaponDurability_Utils.SetStatusBarMsgWithTimeout(text, 2f);
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

        Character asCharacter = equip.GetAsCharacter();
        if (SISWeaponDurability_Config.PlaySoundOnWeaponBroken)
        {
            try
            {
                SISWeaponDurability_Utils.PlaySound(SISWeaponDurability_Config.WeaponBreakSound, asCharacter.Pos, 1f);
            }
            catch
            {
                SISWeaponDurability_Config.Log("ERROR playing sound for weapon broken.");
            }
        }

        if (SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && Session.Instance.GetLocalPlayerCharacter() == asCharacter)
        {
            NotificationManager.Instance.AddEquipmentNotification(equip.GetAsCharacter(), null, equip, 1);
        }

        EquipmentPrototype equipmentPrototype = SISWeaponDurability_Utils.JunkParts;
        if (equip is AmmoWeapon ammoWeapon)
        {
            ammoWeapon.Unload(asCharacter);
            if (equip.GetInjuryType() == 0)
            {
                equipmentPrototype = SISWeaponDurability_Utils.GunParts;

            }
        }

        asCharacter.Inventory.Remove(asCharacter, equip);
        if (equipmentPrototype != null)
        {
            int weaponMinPartsOnBroken = SISWeaponDurability_Config.GetWeaponMinPartsOnBroken(equip);
            int weaponMaxPartsOnBroken = SISWeaponDurability_Config.GetWeaponMaxPartsOnBroken(equip);
            int num = SISWeaponDurability_Utils.NextBetweenInts(asCharacter, weaponMinPartsOnBroken, weaponMaxPartsOnBroken, 1);
            if (num > 0 && asCharacter != null)
            {
                Equipment equipment = Equipment.Spawn(equipmentPrototype, num);
                asCharacter.Inventory.Add(asCharacter, equipment);
                if (SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && Session.Instance.GetLocalPlayerCharacter() == asCharacter)
                {
                    NotificationManager.Instance.AddEquipmentNotification(null, asCharacter, equipment, num);
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
