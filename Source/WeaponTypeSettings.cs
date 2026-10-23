using System;

// Token: 0x0200001B RID: 27
[Serializable]
public class WeaponTypeSettings
{
	public WeaponTypeSettings()
	{
	}

	public WeaponTypeSettings(string weaponType, float degradeRate = 1f, float maxDurability = 15f, int minPartsOnBroken = 0, int maxPartsOnBroken = 3)
	{
		this.WeaponType = weaponType;
		this.DegradeRate = degradeRate;
		this.MaxDurability = maxDurability;
		this.MinPartsOnBroken = minPartsOnBroken;
		this.MaxPartsOnBroken = maxPartsOnBroken;
		this.PartsPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
	}

	public WeaponTypeSettings(string weaponType, string partsPrototypeName, float degradeRate = 1f, float maxDurability = 15f, int minPartsOnBroken = 0, int maxPartsOnBroken = 3)
	{
		this.WeaponType = weaponType;
		this.DegradeRate = degradeRate;
		this.MaxDurability = maxDurability;
		this.MinPartsOnBroken = minPartsOnBroken;
		this.MaxPartsOnBroken = maxPartsOnBroken;
		this.PartsPrototypeName = partsPrototypeName;
	}

	public static WeaponTypeSettings GetDefault()
	{
		return new WeaponTypeSettings();
	}

	public static WeaponTypeSettings GetDefaultMeleeWeapon()
	{
		return new WeaponTypeSettings();
	}

	public static WeaponTypeSettings GetDefaultAmmoWeapon()
	{
		return new WeaponTypeSettings
		{
			MaxDurability = 400f
		};
	}

	public string WeaponType = "MeleeWeapon";

	public float DegradeRate = 1f;

	public float MaxDurability = 15f;

	public int MinPartsOnBroken = 0;

	public int MaxPartsOnBroken = 3;

	public string PartsPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
}
