using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class SISWeaponDurability_Config
{
	// Token: 0x0600003F RID: 63 RVA: 0x00003994 File Offset: 0x00001B94
	public static string GetBrokenPartsPrototypeName(string name)
	{
		WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(name, false);
		return weaponModifiersForEquipment.PartsPrototypeName;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000039B4 File Offset: 0x00001BB4
	public SISWeaponDurability_Config()
	{
		SISWeaponDurability_Config.instance = this;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000039C4 File Offset: 0x00001BC4
	public static void ResetSettings()
	{
		SISWeaponDurability_Config.IsConfigLoaded = false;
		SISWeaponDurability_Config.FillSettings(SISWeaponDurability_Settings.GetDefault());
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000039D8 File Offset: 0x00001BD8
	public static void ClearSettings()
	{
		SISWeaponDurability_Config.IsConfigLoaded = false;
		SISWeaponDurability_Config.FillSettings(SISWeaponDurability_Settings.GetDefault());
		SISWeaponDurability_Config.weaponTypes.Clear();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000039F7 File Offset: 0x00001BF7
	private static void GetAndAddSounds()
	{
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000039FC File Offset: 0x00001BFC
	public static void LoadSettings()
	{
		try
		{
			bool flag = false;
			foreach (Story story in GameImpl.Instance.CurrentStories)
			{
				string text = story.Path + SISWeaponDurability_Config.SettingsFilePath;
				bool flag2 = File.Exists(text);
				if (flag2)
				{
					flag = true;
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
					FileStream fileStream = new FileStream(text, FileMode.Open);
					SISWeaponDurability_Settings sisweaponDurability_Settings = (SISWeaponDurability_Settings)xmlSerializer.Deserialize(fileStream);
					fileStream.Dispose();
					SISWeaponDurability_Config.FillSettings(sisweaponDurability_Settings);
					SISWeaponDurability_Config.Log("Loaded settings from path " + text);
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				SISWeaponDurability_Settings @default = SISWeaponDurability_Settings.GetDefault();
				SISWeaponDurability_Config.FillSettings(@default);
				SISWeaponDurability_Config.Log("Loaded default settings since no settings file was found");
				SISWeaponDurability_Config.Log(JsonConvert.SerializeObject(@default));
				foreach (Story story2 in GameImpl.Instance.CurrentStories)
				{
					string text2 = story2.Path + SISWeaponDurability_Config.ModIdentifier;
					bool flag4 = File.Exists(text2);
					if (flag4)
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

	// Token: 0x06000045 RID: 69 RVA: 0x00003BFC File Offset: 0x00001DFC
	private static void SaveSettings(SISWeaponDurability_Settings settings, string folderPath)
	{
		string text = folderPath + SISWeaponDurability_Config.SettingsFilePath;
		SISWeaponDurability_Config.Log("Saving to " + text);
		bool flag = File.Exists(text);
		if (flag)
		{
			File.Delete(text);
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(SISWeaponDurability_Settings));
		FileStream fileStream = File.Create(text);
		xmlSerializer.Serialize(fileStream, settings);
		fileStream.Dispose();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003C64 File Offset: 0x00001E64
	public static void LoadWeaponTypeSettingsFromOtherMods()
	{
		foreach (Story story in GameImpl.Instance.CurrentStories)
		{
			string text = story.Path + SISWeaponDurability_Config.WeaponTypeSettingsOtherMods;
			bool flag = File.Exists(text);
			if (flag)
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

	// Token: 0x06000047 RID: 71 RVA: 0x00003D60 File Offset: 0x00001F60
	public static void Log(string message)
	{
		try
		{
			bool loggingActive = SISWeaponDurability_Config.LoggingActive;
			if (loggingActive)
			{
				bool flag = SISWeaponDurability_Config.StoryPath == "";
				if (flag)
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

	// Token: 0x06000048 RID: 72 RVA: 0x00003DD4 File Offset: 0x00001FD4
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

	// Token: 0x06000049 RID: 73 RVA: 0x00003E34 File Offset: 0x00002034
	private static void FillSettings(SISWeaponDurability_Settings obj)
	{
		FileLog.logPath = SISWeaponDurability_Config.GetStoryPath() + SISWeaponDurability_Config.LogFilePath;
		try
		{
			bool flag = File.Exists(FileLog.logPath);
			if (flag)
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

	// Token: 0x0600004A RID: 74 RVA: 0x000040FC File Offset: 0x000022FC
	private static string GetStoryPath()
	{
		foreach (Story story in GameImpl.Instance.CurrentStories)
		{
			string text = story.Path + SISWeaponDurability_Config.ModIdentifier;
			string text2 = story.Path + SISWeaponDurability_Config.SettingsFilePath;
			bool flag = File.Exists(text) || File.Exists(text2);
			if (flag)
			{
				return story.Path;
			}
		}
		return ".";
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000041A0 File Offset: 0x000023A0
	public static float GetMaxDurabilityForEquipment(string name, bool isAmmoWeapon = false)
	{
		bool flag = !string.IsNullOrEmpty(name);
		if (flag)
		{
			WeaponTypeSettings weaponTypeSettings;
			bool flag2 = SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings);
			if (flag2)
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

	// Token: 0x0600004C RID: 76 RVA: 0x000041F0 File Offset: 0x000023F0
	public static WeaponTypeSettings GetWeaponModifiersForEquipment(string name, bool isAmmoWeapon = false)
	{
		bool flag = !string.IsNullOrEmpty(name);
		if (flag)
		{
			WeaponTypeSettings weaponTypeSettings;
			bool flag2 = SISWeaponDurability_Config.weaponTypes.TryGetValue(name, out weaponTypeSettings);
			if (flag2)
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

	// Token: 0x0600004D RID: 77 RVA: 0x0000423C File Offset: 0x0000243C
	public static int GetWeaponMaxPartsOnBroken(string name)
	{
		bool flag = !string.IsNullOrEmpty(name);
		if (flag)
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

	// Token: 0x0600004E RID: 78 RVA: 0x00004284 File Offset: 0x00002484
	public static int GetWeaponMinPartsOnBroken(string name)
	{
		bool flag = !string.IsNullOrEmpty(name);
		if (flag)
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

	// Token: 0x04000031 RID: 49
	public static SISWeaponDurability_Config instance;

	// Token: 0x04000032 RID: 50
	private static bool LoggingActive = false;

	// Token: 0x04000033 RID: 51
	private static string SettingsFilePath = "/Settings_WeaponDurability.xml";

	// Token: 0x04000034 RID: 52
	private static string WeaponTypeSettingsOtherMods = "/Settings_WeaponDurabilityAddon.xml";

	// Token: 0x04000035 RID: 53
	public static string ModIdentifier = "/SISWEAPONDURABILITYREWORKED.txt";

	// Token: 0x04000036 RID: 54
	private static string LogFilePath = "/log.txt";

	// Token: 0x04000037 RID: 55
	public static string StoryPath = "";

	// Token: 0x04000038 RID: 56
	public static string SpeechWeaponBrokenUniqueId = "NPCWeaponBroken";

	// Token: 0x04000039 RID: 57
	public static string DisassembleMeleeWeaponWorkbenchRecipeUniqueId = "SISWeaponDurability_DisassembleMeleeWeaponWorkbench";

	// Token: 0x0400003A RID: 58
	public static string DisassembleGunWorkbenchRecipeUniqueId = "SISWeaponDurability_DisassembleGunWorkbench";

	// Token: 0x0400003B RID: 59
	public static string RepairWeaponRecipeNameKey = "SPEECH_HUD_RepairWeapon";

	// Token: 0x0400003C RID: 60
	public static string DisassembleWeaponRecipeNameKey = "SPEECH_HUD_DisassembleWeapon";

	// Token: 0x0400003D RID: 61
	public static string SpeechTranslateKey = "SPEECH_HINT_YourWeaponBroke";

	// Token: 0x0400003E RID: 62
	public static string DuctTapePrototypeName = "DuctTape";

	// Token: 0x0400003F RID: 63
	public static string GunRepairKitPrototypeName = "GunRepairKit";

	// Token: 0x04000040 RID: 64
	public static string GunPartPrototypeName = "GunParts";

	// Token: 0x04000041 RID: 65
	public static string WeaponPartPrototypeName = "JunkParts";

	// Token: 0x04000042 RID: 66
	public static string GluePrototypeName = "Glue";

	// Token: 0x04000043 RID: 67
	public static string HomemadeGluePrototypeName = "HomemadeGlue";

	// Token: 0x04000044 RID: 68
	public static string AnimalFatPrototypeName = "AnimalFat";

	// Token: 0x04000045 RID: 69
	public static float DefaultMeleeWeaponDurability;

	// Token: 0x04000046 RID: 70
	public static float DefaultAmmoWeaponDurability;

	// Token: 0x04000047 RID: 71
	public static bool SaveInProgress = false;

	// Token: 0x04000048 RID: 72
	public static bool LoadInProgress = false;

	// Token: 0x04000049 RID: 73
	public static bool IsConfigLoaded = false;

	// Token: 0x0400004A RID: 74
	public static bool EnabledForPlayer;

	// Token: 0x0400004B RID: 75
	public static bool EnabledForPlayerCommunity;

	// Token: 0x0400004C RID: 76
	public static bool EnabledForNPCs;

	// Token: 0x0400004D RID: 77
	public static bool EnabledForMeleeWeapons;

	// Token: 0x0400004E RID: 78
	public static bool EnabledForAmmoWeapons;

	// Token: 0x0400004F RID: 79
	public static bool ShowEquipmentNotificationOnWeaponBroken;

	// Token: 0x04000050 RID: 80
	public static bool ShowStatusMsgOnWeaponBroken;

	// Token: 0x04000051 RID: 81
	public static bool CharactersSpeakOnWeaponBroken;

	// Token: 0x04000052 RID: 82
	public static bool PlaySoundOnWeaponBroken;

	// Token: 0x04000053 RID: 83
	public static bool HitLocationAffectsDegradation;

	// Token: 0x04000054 RID: 84
	public static float HitLocationTopModifierPcnt;

	// Token: 0x04000055 RID: 85
	public static float HitLocationBottomModifierPcnt;

	// Token: 0x04000056 RID: 86
	public static float HitLocationHeadModifierPcnt;

	// Token: 0x04000057 RID: 87
	public static bool WeaponsSpawnWithLessDurability;

	// Token: 0x04000058 RID: 88
	public static float MinWeaponDurabilityOnSpawnPcnt;

	// Token: 0x04000059 RID: 89
	public static float MaxWeaponDurabilityOnSpawnPcnt;

	// Token: 0x0400005A RID: 90
	public static bool SkinningReducesDurability;

	// Token: 0x0400005B RID: 91
	public static bool ChoppingReducesDurability;

	// Token: 0x0400005C RID: 92
	public static bool MiningReducesDurability;

	// Token: 0x0400005D RID: 93
	public static bool AssassinationReducesDurability;

	// Token: 0x0400005E RID: 94
	public static float GlobalDurabilityDegradeRate;

	// Token: 0x0400005F RID: 95
	public static float ChanceAnimalFatOnSkin;

	// Token: 0x04000060 RID: 96
	public static float SkinningDurabilityDegradePercentage;

	// Token: 0x04000061 RID: 97
	public static float ChoppingDurabilityDegradePercentage;

	// Token: 0x04000062 RID: 98
	public static float AssassinationDurabilityDegradePercentage;

	// Token: 0x04000063 RID: 99
	public static float MiningDurabilityDegradePercentage;

	// Token: 0x04000064 RID: 100
	public static Color32 WeaponDurabilityBarColor;

	// Token: 0x04000065 RID: 101
	public static Dictionary<string, WeaponTypeSettings> weaponTypes = new Dictionary<string, WeaponTypeSettings>();

	// Token: 0x04000066 RID: 102
	public static List<string> ExcludedWeapons = new List<string>();

	// Token: 0x04000067 RID: 103
	public static AudioClip WeaponRepairSound;

	// Token: 0x04000068 RID: 104
	public static AudioClip GunRepairSound;

	// Token: 0x04000069 RID: 105
	public static AudioClip WeaponBreakSound;
}
