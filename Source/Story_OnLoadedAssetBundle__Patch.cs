using HarmonyLib;
using System.IO;
using UnityEngine;

[HarmonyPatch(typeof(Story), "OnLoadedAssetBundle")]
public class Story_OnLoadedAssetBundle__Patch
{
	private static void Postfix(Story __instance, AssetBundle assetBundle)
	{
        if (assetBundle.name == "sisweapondurability")
		{
			SISWeaponDurability_Config.Log("OnLoadedAssetBundle " + assetBundle.name);
			SISWeaponDurability_Config.WeaponRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repair_sound1.wav");
			SISWeaponDurability_Config.GunRepairSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/repairgun_sound2.wav");
			SISWeaponDurability_Config.WeaponBreakSound = assetBundle.LoadAsset<AudioClip>("assets/sweetcaptn/sisweapondurability/sounds/weaponbreak_sound1.wav");
		}
	}
}
