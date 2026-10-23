using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
[Serializable]
public class SISWeaponDurability_Settings
{
	public static SISWeaponDurability_Settings GetDefault()
	{
		return new SISWeaponDurability_Settings
		{
			WeaponTypes = 
			{
				new WeaponTypeSettings("Axe", 1f, 65f, 2, 4),
				new WeaponTypeSettings("HuntingKnife", 1f, 40f, 0, 2),
				new WeaponTypeSettings("Machete", 1f, 60f, 2, 3),
				new WeaponTypeSettings("Shovel", 1f, 80f, 2, 3),
				new WeaponTypeSettings("BaseballBat", 1f, 60f, 2, 3),
				new WeaponTypeSettings("Toolbox", 1f, 60f, 1, 2),
				new WeaponTypeSettings("Baton", 1f, 60f, 1, 2),
				new WeaponTypeSettings("Cleaver", 1f, 30f, 1, 2),
				new WeaponTypeSettings("Crowbar", 1f, 60f, 2, 3),
				new WeaponTypeSettings("Pipe", 1f, 40f, 0, 2),
				new WeaponTypeSettings("GolfClub", 1f, 40f, 2, 4),
				new WeaponTypeSettings("FryingPan", 1f, 20f, 0, 2),
				new WeaponTypeSettings("TireIron", 1f, 40f, 0, 2),
				new WeaponTypeSettings("SpikedPlank", 2f, 35f, 0, 2),
				new WeaponTypeSettings("SpikedBaseballBat", 1f, 50f, 1, 3),
				new WeaponTypeSettings("Wrench", 1f, 65f, 3, 5),
				new WeaponTypeSettings("Pickaxe", 1f, 80f, 3, 5),
				new WeaponTypeSettings("Katana", 1f, 80f, 3, 5),
				new WeaponTypeSettings("SledgeHammer", 1f, 80f, 3, 6),
				new WeaponTypeSettings("EliteGolfClub", 1f, 80f, 2, 4),
				new WeaponTypeSettings("AntiqueKatana", 1f, 80f, 2, 4),
				new WeaponTypeSettings("Bow", 1f, 150f, 0, 2),
				new WeaponTypeSettings("Pistol", "GunParts", 1f, 600f, 1, 2),
				new WeaponTypeSettings("SilencedPistol", "GunParts", 1f, 600f, 1, 2),
				new WeaponTypeSettings("ModifiedPistol", "GunParts", 1f, 600f, 1, 2),
				new WeaponTypeSettings("Shotgun", "GunParts", 1f, 600f, 2, 3),
				new WeaponTypeSettings("AssaultRifle2", "GunParts", 1f, 800f, 2, 3),
				new WeaponTypeSettings("SniperRifle", "GunParts", 1f, 800f, 2, 3),
				new WeaponTypeSettings("AssaultRifle", "GunParts", 1f, 800f, 2, 3),
				new WeaponTypeSettings("SilencedSniperRifle", "GunParts", 1f, 800f, 2, 3)
			},
			ExcludedWeapons = { "ExcludedWeaponPrototypeName" }
		};
	}

	public bool EnabledForPlayerCommunity = true;

	public bool EnabledForNPCs = false;

	public bool EnabledForMeleeWeapons = true;

	public bool EnabledForAmmoWeapons = true;

	public float GlobalDurabilityDegradeRate = 1f;

	public float ChanceAnimalFatOnSkin = 0.75f;

	public float DefaultMeleeWeaponDurability = 20f;

	public float DefaultAmmoWeaponDurability = 300f;

	public bool WeaponsSpawnWithLessDurability = true;

	[XmlIgnore]
	public float MinWeaponDurabilityOnSpawnPcnt = 0.35f;

	[XmlIgnore]
	public float MaxWeaponDurabilityOnSpawnPcnt = 0.8f;

	public bool HitLocationAffectsDegradation = true;

	[XmlIgnore]
	public float HitLocationTopModifierPcnt = 1.1f;

	[XmlIgnore]
	public float HitLocationBottomModifierPcnt = 1f;

	[XmlIgnore]
	public float HitLocationHeadModifierPcnt = 0.95f;

	public bool ShowEquipmentNotificationOnWeaponBroken = true;

	public bool ShowStatusMsgOnWeaponBroken = false;

	public bool CharactersSpeakOnWeaponBroken = true;

	public bool PlaySoundOnWeaponBroken = true;

	[XmlIgnore]
	public bool SkinningReducesDurability = true;

	[XmlIgnore]
	public bool ChoppingReducesDurability = true;

	[XmlIgnore]
	public bool MiningReducesDurability = true;

	[XmlIgnore]
	public bool AssassinationReducesDurability = true;

	[XmlIgnore]
	public float SkinningDurabilityDegradePercentage = 0.05f;

	[XmlIgnore]
	public float ChoppingDurabilityDegradePercentage = 0.05f;

	[XmlIgnore]
	public float MiningDurabilityDegradePercentage = 0.05f;

	[XmlIgnore]
	public float AssassinationDurabilityDegradePercentage = 0.05f;

	public Color32 WeaponDurabilityBarColor = new Color32(byte.MaxValue, 221, 0, byte.MaxValue);

	[XmlArray(ElementName = "WeaponTypes")]
	public List<WeaponTypeSettings> WeaponTypes = new List<WeaponTypeSettings>();

	[XmlArray(ElementName = "ExcludedWeapons")]
	[XmlArrayItem(ElementName = "PrototypeName")]
	public List<string> ExcludedWeapons = new List<string>();
}
