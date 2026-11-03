using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SISWeaponDurability_Config
{
    public static string GetBrokenPartsPrototypeName(Equipment equipment)
    {
        return GetWeaponModifiersForEquipment(equipment).PartsPrototypeName;
    }

    public SISWeaponDurability_Config()
    {
        instance = this;
    }

    public static void ClearSettings()
    {
        IsConfigLoaded = false;
        FillSettings(SISWeaponDurability_Settings.GetDefault());
        weaponTypes.Clear();
    }

    public static void Initialize()
    {
        StoryPath = GetStoryPath();
        LogPath = StoryPath + LogFilename;
        SettingsPath = StoryPath + SettingsFilename;
        WeaponTypeSettingsOtherModsPath = StoryPath + WeaponTypeSettingsOtherModsFilename;
        ModIdentifierPath = StoryPath + ModIdentifierFilename;

        if (File.Exists(LogPath))
        {
            string previous = StoryPath + LogFilename.Insert(LogFilename.IndexOf("."), ".previous");
            if (File.Exists(previous))
            {
                File.Delete(previous);
            }
            File.Move(LogPath, previous);
        }
    }

    public static void LoadSettings()
    {
        if (IsConfigLoaded)
        {
            IsConfigLoaded = false;
            return;
        }

        try
        {
            if (File.Exists(SettingsPath))
            {
                using (FileStream fileStream = new FileStream(SettingsPath, FileMode.Open))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
                    FillSettings(xmlSerializer.Deserialize(fileStream) as SISWeaponDurability_Settings);
                }
                Log($"Loaded settings from path {SettingsPath}");
            }
            else
            {
                SISWeaponDurability_Settings @default = SISWeaponDurability_Settings.GetDefault();
                FillSettings(@default);
                using (FileStream fileStream = File.Create(SettingsPath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
                    xmlSerializer.Serialize(fileStream, @default);
                }
                Log("Loaded default settings since no settings file was found");
                Log($"Saved settings file at path {StoryPath}");
            }
        }
        catch (Exception ex)
        {
            Log($"Error loading settings: {ex}");
            FillSettings(SISWeaponDurability_Settings.GetDefault());
            throw new Exception($"Error loading SISWeaponDurability settings: {ex.Message}");
        }
        LoadWeaponTypeSettingsFromOtherMods();
        IsConfigLoaded = true;
    }

    public static void LoadWeaponTypeSettingsFromOtherMods()
    {
        if (File.Exists(WeaponTypeSettingsOtherModsPath))
        {
            Log($"Found Addon at {WeaponTypeSettingsOtherModsPath}");
            using (FileStream fileStream = new FileStream(WeaponTypeSettingsOtherModsPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_SettingsAddon));
                (xmlSerializer.Deserialize(fileStream) as SISWeaponDurability_SettingsAddon).WeaponTypes.ForEach(delegate (WeaponTypeSettings x)
                {
                    try
                    {
                        weaponTypes[x.WeaponType] = x;
                        Log($"Added durability settings for weapon {x.WeaponType}");
                    }
                    catch (Exception ex)
                    {
                        Log($"Error adding {x.WeaponType}: {ex.Message}");
                    }
                });
            }
            Log($"Loaded WeaponTypeSettingsAddon from path {WeaponTypeSettingsOtherModsPath}");
        }
    }

    public static void Log(string message)
    {
        try
        {
            if (LoggingEnabled)
            {
                using (StreamWriter streamWriter = File.AppendText(LogPath))
                {
                    streamWriter.WriteLine($"{new string(FileLog.indentChar, FileLog.indentLevel)}[SISWeaponDurability] {message}");
                }
            }
        }
        catch { }
    }

    private static void FillSettings(SISWeaponDurability_Settings obj)
    {
        if (!IsConfigLoaded)
        {
            Log(JsonConvert.SerializeObject(obj));
        }

        LoggingEnabled = obj.LoggingEnabled;
        EnabledForPlayer = obj.EnabledForPlayer;
        EnabledForPlayerCommunity = obj.EnabledForPlayerCommunity;
        EnabledForNPCs = obj.EnabledForNPCs;
        EnabledForMeleeWeapons = obj.EnabledForMeleeWeapons;
        EnabledForAmmoWeapons = obj.EnabledForAmmoWeapons;

        SkinningReducesDurability = obj.SkinningReducesDurability;
        SkinningDurabilityDegradePercentage = obj.SkinningDurabilityDegradePercentage;

        ChoppingReducesDurability = obj.ChoppingReducesDurability;
        ChoppingDurabilityDegradePercentage = obj.ChoppingDurabilityDegradePercentage;

        MiningReducesDurability = obj.MiningReducesDurability;
        MiningDurabilityDegradePercentage = obj.MiningDurabilityDegradePercentage;

        AssassinationReducesDurability = obj.AssassinationReducesDurability;
        AssassinationDurabilityDegradePercentage = obj.AssassinationDurabilityDegradePercentage;

        WeaponsSpawnWithLessDurability = obj.WeaponsSpawnWithLessDurability;
        MinWeaponDurabilityOnSpawnPcnt = obj.MinWeaponDurabilityOnSpawnPcnt;
        MaxWeaponDurabilityOnSpawnPcnt = obj.MaxWeaponDurabilityOnSpawnPcnt;

        HitLocationAffectsDegradation = obj.HitLocationAffectsDegradation;
        HitLocationTopModifierPcnt = obj.HitLocationTopModifierPcnt;
        HitLocationBottomModifierPcnt = obj.HitLocationBottomModifierPcnt;
        HitLocationHeadModifierPcnt = obj.HitLocationHeadModifierPcnt;

        PlaySoundOnWeaponBroken = obj.PlaySoundOnWeaponBroken;
        ShowStatusMsgOnWeaponBroken = obj.ShowStatusMsgOnWeaponBroken;
        CharactersSpeakOnWeaponBroken = obj.CharactersSpeakOnWeaponBroken;
        ShowEquipmentNotificationOnWeaponBroken = obj.ShowEquipmentNotificationOnWeaponBroken;

        GlobalWeaponBaseDamageMultiplier = obj.GlobalWeaponBaseDamageMultiplier;
        DeterioratedWeaponMinimumDamagePercent = obj.DeterioratedWeaponMinimumDamagePercent;
        GlobalDurabilityDegradeRate = obj.GlobalDurabilityDegradeRate;
        DefaultMeleeWeaponDurability = obj.DefaultMeleeWeaponDurability;
        DefaultBowDurability = obj.DefaultGunDurability;
        DefaultGunDurability = obj.DefaultGunDurability;

        ChanceAnimalFatOnSkinAnimal = obj.ChanceAnimalFatOnSkinAnimal;
        ChanceAnimalFatOnSkinHuman = obj.ChanceAnimalFatOnSkinHuman;
        ChanceAnimalFatOnSkinZombie = obj.ChanceAnimalFatOnSkinZombie;

        DisassemblyTime = obj.DisassemblyTime;

        WeaponDurabilityBarColor = obj.WeaponDurabilityBarColor;

        foreach (WeaponTypeSettings weaponTypeSettings in obj.WeaponTypes)
        {
            try
            {
                weaponTypes[weaponTypeSettings.WeaponType] = weaponTypeSettings;
            }
            catch (Exception ex)
            {
                Log($"Error adding {weaponTypeSettings.WeaponType}: {ex.Message}");
            }
        }

        foreach (string text in obj.ExcludedWeapons)
        {
            try
            {
                ExcludedWeapons.Add(text);
            }
            catch (Exception ex2)
            {
                Log($"Error adding {text}: {ex2.Message}");
            }
        }
    }

    private static string GetStoryPath()
    {
        foreach (Story story in GameImpl.Instance.CurrentStories)
        {
            if (File.Exists(story.Path + ModIdentifierFilename) ||
                File.Exists(story.Path + SettingsFilename))
            {
                return story.Path;
            }
        }
        return ".";
    }

    public static float GetMaxDurabilityForEquipment(Equipment equipment)
    {
        string name = equipment.GetPrototype().Name;
        if (!string.IsNullOrEmpty(name))
        {
            if (weaponTypes.TryGetValue(name, out WeaponTypeSettings weaponTypeSettings))
            {
                return weaponTypeSettings.MaxDurability;
            }
        }

        float num;
        if (equipment is Gun)
        {
            num = DefaultGunDurability;
        }
        else if (equipment is Bow)
        {
            num = DefaultBowDurability;
        }
        else
        {
            num = DefaultMeleeWeaponDurability;
        }
        return num;
    }

    public static WeaponTypeSettings GetWeaponModifiersForEquipment(Equipment equipment)
    {
        string name = equipment.GetPrototype().Name;
        if (!string.IsNullOrEmpty(name))
        {
            if (weaponTypes.TryGetValue(name, out WeaponTypeSettings weaponTypeSettings))
            {
                return weaponTypeSettings;
            }
        }

        return WeaponTypeSettings.GetDefaultDurability(equipment);
    }

    public static WeaponTypeSettings GetWeaponModifiersForEquipment(EquipmentPrototype equipment)
    {
        string name = equipment.Name;
        if (!string.IsNullOrEmpty(name))
        {
            if (weaponTypes.TryGetValue(name, out WeaponTypeSettings weaponTypeSettings))
            {
                return weaponTypeSettings;
            }
        }

        return WeaponTypeSettings.GetDefaultDurability(equipment);
    }

    public static int GetWeaponMaxPartsOnBroken(Equipment equipment)
    {
        string name = equipment.GetPrototype().Name;
        if (!string.IsNullOrEmpty(name))
        {
            if (weaponTypes.TryGetValue(name, out WeaponTypeSettings weaponTypeSettings))
            {
                return weaponTypeSettings.MaxPartsOnBroken;
            }
        }
        return WeaponTypeSettings.GetDefaultDurability(equipment).MaxPartsOnBroken;
    }

    public static int GetWeaponMinPartsOnBroken(Equipment equipment)
    {
        string name = equipment.GetPrototype().Name;
        if (!string.IsNullOrEmpty(name))
        {
            if (weaponTypes.TryGetValue(name, out WeaponTypeSettings weaponTypeSettings))
            {
                return weaponTypeSettings.MinPartsOnBroken;
            }
        }
        return WeaponTypeSettings.GetDefaultDurability(equipment).MinPartsOnBroken;
    }

    public static SISWeaponDurability_Config instance;

    public static bool LoggingEnabled = false;
    public static string StoryPath = "";
    public static string LogPath = "";
    public static string SettingsPath = "";
    public static string ModIdentifierPath = "";
    public static string WeaponTypeSettingsOtherModsPath = "";
    private static string LogFilename = "/Log_SISWeaponDurability.txt";
    private static string SettingsFilename = "/Settings_WeaponDurability.xml";
    private static string WeaponTypeSettingsOtherModsFilename = "/Settings_WeaponDurabilityAddon.xml";
    private static string ModIdentifierFilename = "/SISWEAPONDURABILITYREWORKED.txt";

    public static string SpeechWeaponBrokenUniqueId = "NPCWeaponBroken";
    public static string DisassembleMeleeWeaponWorkbenchRecipeUniqueId = "SISWeaponDurability_DisassembleMeleeWeaponWorkbench";
    public static string DisassembleGunWorkbenchRecipeUniqueId = "SISWeaponDurability_DisassembleGunWorkbench";

    public static string RepairWeaponRecipeNameKey = "SPEECH_HUD_RepairWeapon";
    public static string DisassembleWeaponRecipeNameKey = "SPEECH_HUD_DisassembleWeapon";
    public static string SpeechTranslateKey = "SPEECH_HINT_YourWeaponBroke";

    public static string DuctTapePrototypeName = "DuctTape";
    public static string GunRepairKitPrototypeName = "GunRepairKit";
    public static string GunPartPrototypeName = "GunParts";
    public static string WeaponPartPrototypeName = "JunkParts";
    public static string GluePrototypeName = "Glue";
    public static string HomemadeGluePrototypeName = "HomemadeGlue";
    public static string AnimalFatPrototypeName = "AnimalFat";

    public static bool SaveInProgress = false;
    public static bool LoadInProgress = false;
    public static bool IsConfigLoaded = false;

    public static bool EnabledForPlayer;
    public static bool EnabledForPlayerCommunity;
    public static bool EnabledForNPCs;
    public static bool EnabledForMeleeWeapons;
    public static bool EnabledForAmmoWeapons;

    public static bool SkinningReducesDurability;
    public static float SkinningDurabilityDegradePercentage;

    public static bool ChoppingReducesDurability;
    public static float ChoppingDurabilityDegradePercentage;

    public static bool MiningReducesDurability;
    public static float MiningDurabilityDegradePercentage;

    public static bool AssassinationReducesDurability;
    public static float AssassinationDurabilityDegradePercentage;

    public static bool WeaponsSpawnWithLessDurability;
    public static float MinWeaponDurabilityOnSpawnPcnt;
    public static float MaxWeaponDurabilityOnSpawnPcnt;

    public static bool HitLocationAffectsDegradation;
    public static float HitLocationHeadModifierPcnt;
    public static float HitLocationTopModifierPcnt;
    public static float HitLocationBottomModifierPcnt;

    public static bool ShowEquipmentNotificationOnWeaponBroken;
    public static bool ShowStatusMsgOnWeaponBroken;
    public static bool CharactersSpeakOnWeaponBroken;
    public static bool PlaySoundOnWeaponBroken;

    public static float GlobalWeaponBaseDamageMultiplier;
    public static float DeterioratedWeaponMinimumDamagePercent;
    public static float GlobalDurabilityDegradeRate;
    public static float DefaultMeleeWeaponDurability;
    public static float DefaultBowDurability;
    public static float DefaultGunDurability;

    public static float ChanceAnimalFatOnSkinAnimal;
    public static float ChanceAnimalFatOnSkinHuman;
    public static float ChanceAnimalFatOnSkinZombie;

    public static float DisassemblyTime;

    public static Color32 WeaponDurabilityBarColor;

    public static Dictionary<string, WeaponTypeSettings> weaponTypes = new Dictionary<string, WeaponTypeSettings>();
    public static List<string> ExcludedWeapons = new List<string>();

    public static AudioClip WeaponRepairSound;
    public static AudioClip GunRepairSound;
    public static AudioClip WeaponBreakSound;
}
