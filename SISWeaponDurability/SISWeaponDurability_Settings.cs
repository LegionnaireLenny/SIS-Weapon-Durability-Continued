using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

// Token: 0x0200001D RID: 29
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
[Serializable]
public class SISWeaponDurability_Settings
{
	// Token: 0x0600003D RID: 61 RVA: 0x0000340C File Offset: 0x0000160C
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

	// Token: 0x04000013 RID: 19
	public bool EnabledForPlayerCommunity = true;

	// Token: 0x04000014 RID: 20
	public bool EnabledForNPCs = false;

	// Token: 0x04000015 RID: 21
	public bool EnabledForMeleeWeapons = true;

	// Token: 0x04000016 RID: 22
	public bool EnabledForAmmoWeapons = true;

	// Token: 0x04000017 RID: 23
	public float GlobalDurabilityDegradeRate = 1f;

	// Token: 0x04000018 RID: 24
	public float ChanceAnimalFatOnSkin = 0.75f;

	// Token: 0x04000019 RID: 25
	public float DefaultMeleeWeaponDurability = 20f;

	// Token: 0x0400001A RID: 26
	public float DefaultAmmoWeaponDurability = 300f;

	// Token: 0x0400001B RID: 27
	public bool WeaponsSpawnWithLessDurability = true;

	// Token: 0x0400001C RID: 28
	[XmlIgnore]
	public float MinWeaponDurabilityOnSpawnPcnt = 0.35f;

	// Token: 0x0400001D RID: 29
	[XmlIgnore]
	public float MaxWeaponDurabilityOnSpawnPcnt = 0.8f;

	// Token: 0x0400001E RID: 30
	public bool HitLocationAffectsDegradation = true;

	// Token: 0x0400001F RID: 31
	[XmlIgnore]
	public float HitLocationTopModifierPcnt = 1.1f;

	// Token: 0x04000020 RID: 32
	[XmlIgnore]
	public float HitLocationBottomModifierPcnt = 1f;

	// Token: 0x04000021 RID: 33
	[XmlIgnore]
	public float HitLocationHeadModifierPcnt = 0.95f;

	// Token: 0x04000022 RID: 34
	public bool ShowEquipmentNotificationOnWeaponBroken = true;

	// Token: 0x04000023 RID: 35
	public bool ShowStatusMsgOnWeaponBroken = false;

	// Token: 0x04000024 RID: 36
	public bool CharactersSpeakOnWeaponBroken = true;

	// Token: 0x04000025 RID: 37
	public bool PlaySoundOnWeaponBroken = true;

	// Token: 0x04000026 RID: 38
	[XmlIgnore]
	public bool SkinningReducesDurability = true;

	// Token: 0x04000027 RID: 39
	[XmlIgnore]
	public bool ChoppingReducesDurability = true;

	// Token: 0x04000028 RID: 40
	[XmlIgnore]
	public bool MiningReducesDurability = true;

	// Token: 0x04000029 RID: 41
	[XmlIgnore]
	public bool AssassinationReducesDurability = true;

	// Token: 0x0400002A RID: 42
	[XmlIgnore]
	public float SkinningDurabilityDegradePercentage = 0.05f;

	// Token: 0x0400002B RID: 43
	[XmlIgnore]
	public float ChoppingDurabilityDegradePercentage = 0.05f;

	// Token: 0x0400002C RID: 44
	[XmlIgnore]
	public float MiningDurabilityDegradePercentage = 0.05f;

	// Token: 0x0400002D RID: 45
	[XmlIgnore]
	public float AssassinationDurabilityDegradePercentage = 0.05f;

	// Token: 0x0400002E RID: 46
	public Color32 WeaponDurabilityBarColor = new Color32(byte.MaxValue, 221, 0, byte.MaxValue);

	// Token: 0x0400002F RID: 47
	[XmlArray(ElementName = "WeaponTypes")]
	public List<WeaponTypeSettings> WeaponTypes = new List<WeaponTypeSettings>();

	// Token: 0x04000030 RID: 48
	[XmlArray(ElementName = "ExcludedWeapons")]
	[XmlArrayItem(ElementName = "PrototypeName")]
	public List<string> ExcludedWeapons = new List<string>();
}
