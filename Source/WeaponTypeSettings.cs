using System;

[Serializable]
public class WeaponTypeSettings
{
	public WeaponTypeSettings()	{ }

	public WeaponTypeSettings(string weaponType, float degradeRate = 1f, float maxDurability = 15f, int minPartsOnBroken = 0, int maxPartsOnBroken = 3, bool isMeleeWeapon = true)
	{
		WeaponType = weaponType;
		DegradeRate = degradeRate;
		MaxDurability = maxDurability;
		MinPartsOnBroken = minPartsOnBroken;
		MaxPartsOnBroken = maxPartsOnBroken;
		PartsPrototypeName = isMeleeWeapon ? SISWeaponDurability_Config.WeaponPartPrototypeName : SISWeaponDurability_Config.GunPartPrototypeName;
	}

	public static WeaponTypeSettings GetDefaultDurability(Equipment equipment)
	{
		if (equipment is Gun)
		{
			return GetDefaultGunDurability();
		}
		else if (equipment is Bow)
		{

			return GetDefaultBowDurability();
		}
		else
		{
			return GetDefaultMeleeWeaponDurability();
		}
	}

	public static WeaponTypeSettings GetDefaultDurability(EquipmentPrototype equipment)
	{
		if (SISWeaponDurability_Utils.IsPrototypeGun(equipment))
		{
			return GetDefaultGunDurability();
		}
		else if (SISWeaponDurability_Utils.IsPrototypeBow(equipment))
		{

			return GetDefaultBowDurability();
		}
		else
		{
			return GetDefaultMeleeWeaponDurability();
		}
	}

	public static WeaponTypeSettings GetDefaultMeleeWeaponDurability()
	{
		return new WeaponTypeSettings
		{
			WeaponType = "MeleeWeapon",
			MaxDurability = 20f
		};
	}

	public static WeaponTypeSettings GetDefaultBowDurability()
	{
		return new WeaponTypeSettings
		{
			WeaponType = "Bow",
			MaxDurability = 300f
		};
	}

	public static WeaponTypeSettings GetDefaultGunDurability()
	{
		return new WeaponTypeSettings
		{
			WeaponType = "Gun",
			MaxDurability = 300f
		};
	}

	public string WeaponType = "MeleeWeapon";
	public float DegradeRate = 1f;
	public float MaxDurability = 20f;
	public int MinPartsOnBroken = 0;
	public int MaxPartsOnBroken = 3;
	public string PartsPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
}
