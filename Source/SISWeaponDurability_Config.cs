using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SISWeaponDurability_Config
{
	public static string GetBrokenPartsPrototypeName(string name)
	{
		WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(name, false);
		return weaponModifiersForEquipment.PartsPrototypeName;
	}

	public SISWeaponDurability_Config()
	{
		SISWeaponDurability_Config.instance = this;
	}

	public static void ResetSettings()
	{
		SISWeaponDurability_Config.IsConfigLoaded = false;
		SISWeaponDurability_Config.FillSettings(SISWeaponDurability_Settings.GetDefault());
	}

	public static void ClearSettings()
	{
		SISWeaponDurability_Config.IsConfigLoaded = false;
		SISWeaponDurability_Config.FillSettings(SISWeaponDurability_Settings.GetDefault());
		SISWeaponDurability_Config.weaponTypes.Clear();
	}

	private static void GetAndAddSounds()
	{
	}

	public static void LoadSettings()
	{
		try
		{
			bool fileExists = false;
			foreach (Story story in GameImpl.Instance.CurrentStories)
			{
				string text = story.Path + SISWeaponDurability_Config.SettingsFilePath;
                if (File.Exists(text))
				{
					fileExists = true;
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
					FileStream fileStream = new FileStream(text, FileMode.Open);
					SISWeaponDurability_Settings sisweaponDurability_Settings = (SISWeaponDurability_Settings)xmlSerializer.Deserialize(fileStream);
					fileStream.Dispose();
					SISWeaponDurability_Config.FillSettings(sisweaponDurability_Settings);
					SISWeaponDurability_Config.Log("Loaded settings from path " + text);
				}
			}
            if (!fileExists)
			{
				SISWeaponDurability_Settings @default = SISWeaponDurability_Settings.GetDefault();
				SISWeaponDurability_Config.FillSettings(@default);
				SISWeaponDurability_Config.Log("Loaded default settings since no settings file was found");
				SISWeaponDurability_Config.Log(JsonConvert.SerializeObject(@default));
				foreach (Story story2 in GameImpl.Instance.CurrentStories)
				{
					string text2 = story2.Path + SISWeaponDurability_Config.ModIdentifier;
                    if (File.Exists(text2))
					{
						SISWeaponDurability_Config.SaveSettings(SISWeaponDurability_Settings.GetDefault(), story2.Path);
						SISWeaponDurability_Config.Log("Saved settings file at path " + story2.Path);
					}
				}
			}
		}
		catch (Exception ex)
		{
			SISWeaponDurability_Config.Log(ex.ToString());
			SISWeaponDurability_Config.Log("Error loading settings");
			SISWeaponDurability_Config.FillSettings(SISWeaponDurability_Settings.GetDefault());
			throw new Exception("Error loading SISWeaponDurability settings: " + ex.Message);
		}
		SISWeaponDurability_Config.IsConfigLoaded = true;
	}

	private static void SaveSettings(SISWeaponDurability_Settings settings, string folderPath)
	{
		string text = folderPath + SISWeaponDurability_Config.SettingsFilePath;
		SISWeaponDurability_Config.Log("Saving to " + text);
        if (File.Exists(text))
		{
			File.Delete(text);
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
		FileStream fileStream = File.Create(text);
		xmlSerializer.Serialize(fileStream, settings);
		fileStream.Dispose();
	}

	public static void LoadWeaponTypeSettingsFromOtherMods()
	{
		foreach (Story story in GameImpl.Instance.CurrentStories)
		{
			string text = story.Path + SISWeaponDurability_Config.WeaponTypeSettingsOtherMods;
            if (File.Exists(text))
			{
				SISWeaponDurability_Config.Log("Found Addon at " + text);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_SettingsAddon));
				FileStream fileStream = new FileStream(text, FileMode.Open);
				SISWeaponDurability_SettingsAddon sisweaponDurability_SettingsAddon = (SISWeaponDurability_SettingsAddon)xmlSerializer.Deserialize(fileStream);
				fileStream.Dispose();
				sisweaponDurability_SettingsAddon.WeaponTypes.ForEach(delegate(WeaponTypeSettings x)
				{
					try
					{
						SISWeaponDurability_Config.weaponTypes[x.WeaponType] = x;
						SISWeaponDurability_Config.Log("Added durability settings for weapon " + x.WeaponType);
					}
					catch (Exception ex)
					{
						SISWeaponDurability_Config.Log("Error adding " + x.WeaponType + ": " + ex.Message);
					}
				});
				SISWeaponDurability_Config.Log("Loaded WeaponTypeSettingsAddon from path " + text);
			}
		}
	}

	public static void Log(string message)
	{
		try
		{
			bool loggingActive = SISWeaponDurability_Config.LoggingActive;
			if (loggingActive)
			{
                if (SISWeaponDurability_Config.StoryPath == "")
				{
					SISWeaponDurability_Config.StoryPath = SISWeaponDurability_Config.GetStoryPath();
				}
				FileLog.logPath = SISWeaponDurability_Config.StoryPath + SISWeaponDurability_Config.LogFilePath;
				FileLog.Log("[SISWeaponDurability] " + message);
			}
		}
		catch
		{
		}
	}

	public static void ReloadSettings()
	{
		SISWeaponDurability_Config.ResetSettings();
		SISWeaponDurability_Config.LoadSettings();
		SISWeaponDurability_Config.GetAndAddSounds();
		try
		{
			SISWeaponDurability_Config.LoadWeaponTypeSettingsFromOtherMods();
		}
		catch (Exception ex)
		{
			SISWeaponDurability_Config.Log("Error loading Weapon Type Addons from other mods: " + ex.Message);
		}
		SISWeaponDurability_Config.Log("Loaded!");
	}

	private static void FillSettings(SISWeaponDurability_Settings obj)
	{
		FileLog.logPath = SISWeaponDurability_Config.GetStoryPath() + SISWeaponDurability_Config.LogFilePath;
		try
		{
            if (File.Exists(FileLog.logPath))
			{
				File.Delete(FileLog.logPath);
			}
		}
		catch
		{
		}
		SISWeaponDurability_Config.Log(JsonConvert.SerializeObject(obj));
		try
		{
			SISWeaponDurability_Config.GetAndAddSounds();
		}
		catch
		{
		}
		SISWeaponDurability_Config.WeaponDurabilityBarColor = obj.WeaponDurabilityBarColor;
		SISWeaponDurability_Config.DefaultMeleeWeaponDurability = obj.DefaultMeleeWeaponDurability;
		SISWeaponDurability_Config.DefaultAmmoWeaponDurability = obj.DefaultAmmoWeaponDurability;
		SISWeaponDurability_Config.EnabledForNPCs = obj.EnabledForNPCs;
		SISWeaponDurability_Config.EnabledForPlayerCommunity = obj.EnabledForPlayerCommunity;
		SISWeaponDurability_Config.EnabledForMeleeWeapons = obj.EnabledForMeleeWeapons;
		SISWeaponDurability_Config.EnabledForAmmoWeapons = obj.EnabledForAmmoWeapons;
		SISWeaponDurability_Config.GlobalDurabilityDegradeRate = obj.GlobalDurabilityDegradeRate;
		SISWeaponDurability_Config.PlaySoundOnWeaponBroken = obj.PlaySoundOnWeaponBroken;
		SISWeaponDurability_Config.ShowStatusMsgOnWeaponBroken = obj.ShowStatusMsgOnWeaponBroken;
		SISWeaponDurability_Config.CharactersSpeakOnWeaponBroken = obj.CharactersSpeakOnWeaponBroken;
		SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken = obj.ShowEquipmentNotificationOnWeaponBroken;
		SISWeaponDurability_Config.HitLocationAffectsDegradation = obj.HitLocationAffectsDegradation;
		SISWeaponDurability_Config.HitLocationTopModifierPcnt = obj.HitLocationTopModifierPcnt;
		SISWeaponDurability_Config.HitLocationBottomModifierPcnt = obj.HitLocationBottomModifierPcnt;
		SISWeaponDurability_Config.HitLocationHeadModifierPcnt = obj.HitLocationHeadModifierPcnt;
		SISWeaponDurability_Config.WeaponsSpawnWithLessDurability = obj.WeaponsSpawnWithLessDurability;
		SISWeaponDurability_Config.MinWeaponDurabilityOnSpawnPcnt = obj.MinWeaponDurabilityOnSpawnPcnt;
		SISWeaponDurability_Config.MaxWeaponDurabilityOnSpawnPcnt = obj.MaxWeaponDurabilityOnSpawnPcnt;
		SISWeaponDurability_Config.SkinningReducesDurability = obj.SkinningReducesDurability;
		SISWeaponDurability_Config.ChoppingReducesDurability = obj.ChoppingReducesDurability;
		SISWeaponDurability_Config.MiningReducesDurability = obj.MiningReducesDurability;
		SISWeaponDurability_Config.AssassinationReducesDurability = obj.AssassinationReducesDurability;
		SISWeaponDurability_Config.MiningReducesDurability = obj.MiningReducesDurability;
		SISWeaponDurability_Config.SkinningDurabilityDegradePercentage = obj.SkinningDurabilityDegradePercentage;
		SISWeaponDurability_Config.ChoppingDurabilityDegradePercentage = obj.ChoppingDurabilityDegradePercentage;
		SISWeaponDurability_Config.AssassinationDurabilityDegradePercentage = obj.AssassinationDurabilityDegradePercentage;
		SISWeaponDurability_Config.MiningDurabilityDegradePercentage = obj.MiningDurabilityDegradePercentage;
		SISWeaponDurability_Config.ChanceAnimalFatOnSkin = obj.ChanceAnimalFatOnSkin;
		foreach (WeaponTypeSettings weaponTypeSettings in obj.WeaponTypes)
		{
			try
			{
				SISWeaponDurability_Config.weaponTypes[weaponTypeSettings.WeaponType] = weaponTypeSettings;
			}
			catch (Exception ex)
			{
				SISWeaponDurability_Config.Log("Error adding " + weaponTypeSettings.WeaponType + ": " + ex.Message);
			}
		}
		foreach (string text in obj.ExcludedWeapons)
		{
			try
			{
				SISWeaponDurability_Config.ExcludedWeapons.Add(text);
			}
			catch (Exception ex2)
			{
				SISWeaponDurability_Config.Log("Error adding " + text + ": " + ex2.Message);
			}
		}
	}

	private static string GetStoryPath()
	{
		foreach (Story story in GameImpl.Instance.CurrentStories)
		{
			string text = story.Path + SISWeaponDurability_Config.ModIdentifier;
			string text2 = story.Path + SISWeaponDurability_Config.SettingsFilePath;
            if (File.Exists(text) || File.Exists(text2))
			{
				return story.Path;
			}
		}
		return ".";
	}

	public static float GetMaxDurabilityForEquipment(string name, bool isAmmoWeapon = false)
	{
        if (!string.IsNullOrEmpty(name))
		{
			WeaponTypeSettings weaponTypeSettings;
            if (SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings))
			{
				return weaponTypeSettings.MaxDurability;
			}
		}
		float num;
		if (isAmmoWeapon)
		{
			num = SISWeaponDurability_Config.DefaultAmmoWeaponDurability;
		}
		else
		{
			num = SISWeaponDurability_Config.DefaultMeleeWeaponDurability;
		}
		return num;
	}

	public static WeaponTypeSettings GetWeaponModifiersForEquipment(string name, bool isAmmoWeapon = false)
	{
        if (!string.IsNullOrEmpty(name))
		{
			WeaponTypeSettings weaponTypeSettings;
            if (SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings))
			{
				return weaponTypeSettings;
			}
		}
		WeaponTypeSettings weaponTypeSettings2;
		if (isAmmoWeapon)
		{
			weaponTypeSettings2 = WeaponTypeSettings.GetDefaultAmmoWeapon();
		}
		else
		{
			weaponTypeSettings2 = WeaponTypeSettings.GetDefault();
		}
		return weaponTypeSettings2;
	}

	public static int GetWeaponMaxPartsOnBroken(string name)
	{
        if (!string.IsNullOrEmpty(name))
		{
			WeaponTypeSettings weaponTypeSettings;
			bool flag2 = SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings);
			if (flag2)
			{
				return weaponTypeSettings.MaxPartsOnBroken;
			}
		}
		return WeaponTypeSettings.GetDefault().MaxPartsOnBroken;
	}

	public static int GetWeaponMinPartsOnBroken(string name)
	{
        if (!string.IsNullOrEmpty(name))
		{
			WeaponTypeSettings weaponTypeSettings;
			bool flag2 = SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings);
			if (flag2)
			{
				return weaponTypeSettings.MinPartsOnBroken;
			}
		}
		return WeaponTypeSettings.GetDefault().MinPartsOnBroken;
	}

	public static SISWeaponDurability_Config instance;

	private static bool LoggingActive = false;

	private static string SettingsFilePath = "/Settings_WeaponDurability.xml";

	private static string WeaponTypeSettingsOtherMods = "/Settings_WeaponDurabilityAddon.xml";

	public static string ModIdentifier = "/SISWEAPONDURABILITYREWORKED.txt";

	private static string LogFilePath = "/log.txt";

	public static string StoryPath = "";

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

	public static float DefaultMeleeWeaponDurability;

	public static float DefaultAmmoWeaponDurability;

	public static bool SaveInProgress = false;

	public static bool LoadInProgress = false;

	public static bool IsConfigLoaded = false;

	public static bool EnabledForPlayer;

	public static bool EnabledForPlayerCommunity;

	public static bool EnabledForNPCs;

	public static bool EnabledForMeleeWeapons;

	public static bool EnabledForAmmoWeapons;

	public static bool ShowEquipmentNotificationOnWeaponBroken;

	public static bool ShowStatusMsgOnWeaponBroken;

	public static bool CharactersSpeakOnWeaponBroken;

	public static bool PlaySoundOnWeaponBroken;

	public static bool HitLocationAffectsDegradation;

	public static float HitLocationTopModifierPcnt;

	public static float HitLocationBottomModifierPcnt;

	public static float HitLocationHeadModifierPcnt;

	public static bool WeaponsSpawnWithLessDurability;

	public static float MinWeaponDurabilityOnSpawnPcnt;

	public static float MaxWeaponDurabilityOnSpawnPcnt;

	public static bool SkinningReducesDurability;

	public static bool ChoppingReducesDurability;

	public static bool MiningReducesDurability;

	public static bool AssassinationReducesDurability;

	public static float GlobalDurabilityDegradeRate;

	public static float ChanceAnimalFatOnSkin;

	public static float SkinningDurabilityDegradePercentage;

	public static float ChoppingDurabilityDegradePercentage;

	public static float AssassinationDurabilityDegradePercentage;

	public static float MiningDurabilityDegradePercentage;

	public static Color32 WeaponDurabilityBarColor;

	public static Dictionary<string, WeaponTypeSettings> weaponTypes = new Dictionary<string, WeaponTypeSettings>();

	public static List<string> ExcludedWeapons = new List<string>();

	public static AudioClip WeaponRepairSound;

	public static AudioClip GunRepairSound;

	public static AudioClip WeaponBreakSound;
}
