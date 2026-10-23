using HarmonyLib;
using System.IO;
using UnityEngine;

[HarmonyPatch(typeof(Story), "OnLoadedAssetBundle")]
public class Story_OnLoadedAssetBundle__Patch
{
	private static void Postfix(Story __instance, AssetBundle assetBundle)
	{
		SISWeaponDurability_Config.Log("OnLoadedAssetBundle " + assetBundle.name);
		string text = __instance.Path + SISWeaponDurability_Config.ModIdentifier;
        if (File.Exists(text) || assetBundle.name == "sisweapondurability")
		{
			SISWeaponDurability_Config.WeaponRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repair_sound1.wav");
			SISWeaponDurability_Config.GunRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repairgun_sound2.wav");
			SISWeaponDurability_Config.WeaponBreakSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/weaponbreak_sound1.wav");
		}
	}
}
