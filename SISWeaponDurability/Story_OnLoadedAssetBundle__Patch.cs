using System;
using System.IO;
using HarmonyLib;
using UnityEngine;

// Token: 0x0200000D RID: 13
[HarmonyPatch(typeof(Story), "OnLoadedAssetBundle")]
public class Story_OnLoadedAssetBundle__Patch
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002930 File Offset: 0x00000B30
	private static void Postfix(Story __instance, AssetBundle assetBundle)
	{
		SISWeaponDurability_Config.Log("OnLoadedAssetBundle " + assetBundle.name);
		string text = __instance.Path + SISWeaponDurability_Config.ModIdentifier;
		bool flag = File.Exists(text) || assetBundle.name == "sisweapondurability";
		if (flag)
		{
			SISWeaponDurability_Config.WeaponRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repair_sound1.wav");
			SISWeaponDurability_Config.GunRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repairgun_sound2.wav");
			SISWeaponDurability_Config.WeaponBreakSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/weaponbreak_sound1.wav");
		}
	}
}
