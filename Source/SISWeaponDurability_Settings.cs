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
				new WeaponTypeSettings("Pistol", 1f, 600f, 1, 2, false),
				new WeaponTypeSettings("SilencedPistol", 1f, 600f, 1, 2, false),
				new WeaponTypeSettings("ModifiedPistol", 1f, 600f, 1, 2, false),
				new WeaponTypeSettings("Shotgun", 1f, 600f, 2, 3, false),
				new WeaponTypeSettings("AssaultRifle2", 1f, 800f, 2, 3, false),
				new WeaponTypeSettings("SniperRifle", 1f, 800f, 2, 3, false),
				new WeaponTypeSettings("AssaultRifle", 1f, 800f, 2, 3, false),
				new WeaponTypeSettings("SilencedSniperRifle", 1f, 800f, 2, 3, false)
			},
			ExcludedWeapons = { "ExcludedWeaponPrototypeName" }
		};
	}

    public bool LoggingEnabled = false;
    public bool EnabledForPlayer = true;
    public bool EnabledForPlayerCommunity = true;
	public bool EnabledForNPCs = false;
	public bool EnabledForMeleeWeapons = true;
	public bool EnabledForAmmoWeapons = true;

	public bool SkinningReducesDurability = true;
	public float SkinningDurabilityDegradePercentage = 0.05f;

	public bool ChoppingReducesDurability = true;
	public float ChoppingDurabilityDegradePercentage = 0.05f;

	public bool MiningReducesDurability = true;
	public float MiningDurabilityDegradePercentage = 0.05f;

	public bool AssassinationReducesDurability = true;
	public float AssassinationDurabilityDegradePercentage = 0.05f;

	public bool WeaponsSpawnWithLessDurability = true;
	public float MinWeaponDurabilityOnSpawnPcnt = 0.35f;
	public float MaxWeaponDurabilityOnSpawnPcnt = 0.8f;

	public bool HitLocationAffectsDegradation = true;
	public float HitLocationHeadModifierPcnt = 0.95f;
	public float HitLocationTopModifierPcnt = 1.1f;
	public float HitLocationBottomModifierPcnt = 1f;

	public bool ShowEquipmentNotificationOnWeaponBroken = true;
	public bool ShowStatusMsgOnWeaponBroken = false;
	public bool CharactersSpeakOnWeaponBroken = true;
	public bool PlaySoundOnWeaponBroken = true;

	public float GlobalWeaponBaseDamageMultiplier = 1f;
	public float DeterioratedWeaponMinimumDamagePercent = 0.1f;
	public float GlobalDurabilityDegradeRate = 1f;
	public float DefaultMeleeWeaponDurability = 20f;
	public float DefaultBowDurability = 300f;
	public float DefaultGunDurability = 300f;

	public float ChanceAnimalFatOnSkinAnimal = 0.75f;
	public float ChanceAnimalFatOnSkinHuman = 0.50f;
	public float ChanceAnimalFatOnSkinZombie = 0.25f;

	public float DisassemblyTime = 15f;

	public Color32 WeaponDurabilityBarColor = new Color32(byte.MaxValue, 221, 0, byte.MaxValue);

	[XmlArray(ElementName = "WeaponTypes")]
	public List<WeaponTypeSettings> WeaponTypes = new List<WeaponTypeSettings>();

	[XmlArray(ElementName = "ExcludedWeapons")]
	[XmlArrayItem(ElementName = "PrototypeName")]
	public List<string> ExcludedWeapons = new List<string>();
}
