using System;

// Token: 0x0200001B RID: 27
[Serializable]
public class WeaponTypeSettings
{
	// Token: 0x06000036 RID: 54 RVA: 0x00003258 File Offset: 0x00001458
	public WeaponTypeSettings()
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000032A8 File Offset: 0x000014A8
	public WeaponTypeSettings(string weaponType, float degradeRate = 1f, float maxDurability = 15f, int minPartsOnBroken = 0, int maxPartsOnBroken = 3)
	{
		this.WeaponType = weaponType;
		this.DegradeRate = degradeRate;
		this.MaxDurability = maxDurability;
		this.MinPartsOnBroken = minPartsOnBroken;
		this.MaxPartsOnBroken = maxPartsOnBroken;
		this.PartsPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003328 File Offset: 0x00001528
	public WeaponTypeSettings(string weaponType, string partsPrototypeName, float degradeRate = 1f, float maxDurability = 15f, int minPartsOnBroken = 0, int maxPartsOnBroken = 3)
	{
		this.WeaponType = weaponType;
		this.DegradeRate = degradeRate;
		this.MaxDurability = maxDurability;
		this.MinPartsOnBroken = minPartsOnBroken;
		this.MaxPartsOnBroken = maxPartsOnBroken;
		this.PartsPrototypeName = partsPrototypeName;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000033A4 File Offset: 0x000015A4
	public static WeaponTypeSettings GetDefault()
	{
		return new WeaponTypeSettings();
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000033BC File Offset: 0x000015BC
	public static WeaponTypeSettings GetDefaultMeleeWeapon()
	{
		return new WeaponTypeSettings();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000033D4 File Offset: 0x000015D4
	public static WeaponTypeSettings GetDefaultAmmoWeapon()
	{
		return new WeaponTypeSettings
		{
			MaxDurability = 400f
		};
	}

	// Token: 0x0400000C RID: 12
	public string WeaponType = "MeleeWeapon";

	// Token: 0x0400000D RID: 13
	public float DegradeRate = 1f;

	// Token: 0x0400000E RID: 14
	public float MaxDurability = 15f;

	// Token: 0x0400000F RID: 15
	public int MinPartsOnBroken = 0;

	// Token: 0x04000010 RID: 16
	public int MaxPartsOnBroken = 3;

	// Token: 0x04000011 RID: 17
	public string PartsPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
}
