using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class SISWeaponDurability_Utils
{
	// Token: 0x06000052 RID: 82 RVA: 0x000044D0 File Offset: 0x000026D0
	public static int GetAmountPartsToRepair(Equipment item)
	{
		return 1;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000044E4 File Offset: 0x000026E4
	public static void LoadProtos()
	{
		SISWeaponDurability_Utils.DuctTape = GameImpl.Instance.FindEquipmentPrototypeByName("DuctTape");
		SISWeaponDurability_Utils.GunRepairKit = GameImpl.Instance.FindEquipmentPrototypeByName("GunRepairKit");
		SISWeaponDurability_Utils.Glue = GameImpl.Instance.FindEquipmentPrototypeByName("Glue");
		SISWeaponDurability_Utils.JunkParts = GameImpl.Instance.FindEquipmentPrototypeByName("JunkParts");
		SISWeaponDurability_Utils.GunParts = GameImpl.Instance.FindEquipmentPrototypeByName("GunParts");
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004558 File Offset: 0x00002758
	public static bool IsPrototypeMeleeWeapon(EquipmentPrototype equip)
	{
		return equip.CanBeEquipped && 
			(equip.TypeName == BaseObjectType.MeleeWeapon ||
            equip.TypeName == BaseObjectType.HuntingKnife ||
            equip.TypeName == BaseObjectType.Axe || 
			equip.TypeName == BaseObjectType.Shovel || 
			equip.TypeName == BaseObjectType.Pickaxe);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000045B0 File Offset: 0x000027B0
	public static void SetStatusBarMsgWithTimeout(string msg, float timeout)
	{
		HudBehaviour.Instance.StatusBarMsg = msg;
		HudBehaviour.Instance.StatusBarMsgTimeout = timeout;
		HudBehaviour.Instance.StatusBarMsgShowForOneFrame = false;
		HudBehaviour.Instance.StatusBarMsgAction = default(AvailableAction);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000045E4 File Offset: 0x000027E4
	public static bool IsPrototypeAmmoWeapon(EquipmentPrototype equip)
	{
		return equip.CanBeEquipped && 
			(equip.TypeName == BaseObjectType.Pistol || 
			equip.TypeName == BaseObjectType.Shotgun || 
			equip.TypeName == BaseObjectType.AssaultRifle || 
			equip.TypeName == BaseObjectType.SniperRifle || 
			equip.TypeName == BaseObjectType.Bow || 
			equip.TypeName == BaseObjectType.Gun || 
			equip.InjuryType == InjuryType.Bullet || 
			equip.InjuryType == InjuryType.Arrow);
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004650 File Offset: 0x00002850
	public static float ReduceDurability(Equipment equip, float amount = 1f)
	{
		bool flag = amount == 0f || equip == null;
		float num;
		if (flag)
		{
			num = -1f;
		}
		else
		{
			float num2 = Mathf.Max(equip.GetDampness() - amount, 0f);
			bool flag2 = equip.IsAuthoritative();
			if (flag2)
			{
				equip.SetDampness(num2);
				SISWeaponDurability_Config.Log("Reduced durability for " + equip.GetPrototype().Name + ", dur = " + num2.ToString());
				bool flag3 = num2 == 0f;
				if (flag3)
				{
					SISWeaponDurability_Utils.BreakEquipment(equip);
				}
			}
			num = num2;
		}
		return num;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000046E3 File Offset: 0x000028E3
	public static void PlaySound(AudioClip clip, Vector3 pos, float volume = 1f)
	{
		SISWeaponDurability_Utils.PlaySound(clip, pos, volume, 1f, false, 1f, 3f);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000046FF File Offset: 0x000028FF
	public static void PlaySoundFromList(List<AudioClip> clips, Vector3 pos, float volume = 1f)
	{
		SISWeaponDurability_Utils.PlaySound(clips[MathUtil.NonDeterministicRand.Next(clips.Count)], pos, volume, 1f, false, 1f, 3f);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00004730 File Offset: 0x00002930
	public static void PlaySound(AudioClip clip, Vector3 pos, float volume, float spatialBlend, bool realisticRolloff, float minRolloffDist, float maxRolloffDist)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.spatialBlend = spatialBlend;
		audioSource.volume = volume;
		audioSource.minDistance = minRolloffDist;
		audioSource.maxDistance = maxRolloffDist;
		if (realisticRolloff)
		{
			AudioSourceExtensions.RealisticRolloff(audioSource);
		}
		else
		{
			audioSource.rolloffMode = AudioRolloffMode.Linear;
		}
		audioSource.Play();
		global::UnityEngine.Object.Destroy(gameObject, clip.length);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000047B4 File Offset: 0x000029B4
	public static void BreakEquipment(Equipment equip)
	{
		bool flag = equip.GetAsCharacter() == null;
		if (!flag)
		{
			bool flag2 = equip.GetAsCharacter() == Hud.Instance.LocalControlledCharacter;
			bool flag3 = equip.GetAsCharacter().Alive && !equip.GetAsCharacter().IsSleepingOrUnconscious();
			if (flag3)
			{
				bool flag4 = flag2;
				if (flag4)
				{
					bool showStatusMsgOnWeaponBroken = SISWeaponDurability_Config.ShowStatusMsgOnWeaponBroken;
					if (showStatusMsgOnWeaponBroken)
					{
						string text = GameImpl.Translate(SISWeaponDurability_Config.SpeechTranslateKey).Replace("%1", GameImpl.Translate(equip.GetPrototype().GetNameKey()));
						SISWeaponDurability_Utils.SetStatusBarMsgWithTimeout(text, 2f);
					}
				}
				else
				{
					bool charactersSpeakOnWeaponBroken = SISWeaponDurability_Config.CharactersSpeakOnWeaponBroken;
					if (charactersSpeakOnWeaponBroken)
					{
						Speech speech = GameImpl.Instance.FindSpeechByUniqueID(SISWeaponDurability_Config.SpeechWeaponBrokenUniqueId);
						equip.GetAsCharacter().Speak(speech);
					}
				}
			}
			Character asCharacter = equip.GetAsCharacter();
			bool playSoundOnWeaponBroken = SISWeaponDurability_Config.PlaySoundOnWeaponBroken;
			if (playSoundOnWeaponBroken)
			{
				try
				{
					SISWeaponDurability_Utils.PlaySound(SISWeaponDurability_Config.WeaponBreakSound, asCharacter.Pos, 1f);
				}
				catch
				{
					SISWeaponDurability_Config.Log("ERROR playing sound for weapon broken.");
				}
			}
			bool flag5 = SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && Session.Instance.GetLocalPlayerCharacter() == asCharacter;
			if (flag5)
			{
				NotificationManager.Instance.AddEquipmentNotification(equip.GetAsCharacter(), null, equip, 1);
			}
			bool flag6 = equip is AmmoWeapon;
			if (flag6)
			{
				((AmmoWeapon)equip).Unload(asCharacter);
			}
			asCharacter.Inventory.Remove(asCharacter, equip);
			EquipmentPrototype equipmentPrototype = SISWeaponDurability_Utils.JunkParts;
			bool flag7 = equip is AmmoWeapon && equip.GetInjuryType() == 0;
			if (flag7)
			{
				equipmentPrototype = SISWeaponDurability_Utils.GunParts;
			}
			bool flag8 = equipmentPrototype != null;
			if (flag8)
			{
				int weaponMinPartsOnBroken = SISWeaponDurability_Config.GetWeaponMinPartsOnBroken(equip.GetPrototype().Name);
				int weaponMaxPartsOnBroken = SISWeaponDurability_Config.GetWeaponMaxPartsOnBroken(equip.GetPrototype().Name);
				int num = SISWeaponDurability_Utils.NextBetweenInts(asCharacter, weaponMinPartsOnBroken, weaponMaxPartsOnBroken, 1);
				bool flag9 = num > 0 && asCharacter != null;
				if (flag9)
				{
					Equipment equipment = Equipment.Spawn(equipmentPrototype, num);
					asCharacter.Inventory.Add(asCharacter, equipment);
					bool flag10 = SISWeaponDurability_Config.ShowEquipmentNotificationOnWeaponBroken && Session.Instance.GetLocalPlayerCharacter() == asCharacter;
					if (flag10)
					{
						NotificationManager.Instance.AddEquipmentNotification(null, asCharacter, equipment, num);
					}
				}
			}
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000049F8 File Offset: 0x00002BF8
	public static float NextFloat(Character character)
	{
		int num = (int)PredictedObjectManager.Instance.GetCurrentTime(character.IsPredicted()).Ticks + character.Id * 9982;
		return MathUtil.RandomFloat((float)num);
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004A3C File Offset: 0x00002C3C
	public static int NextBetweenInts(Character character, int min, int max, int seedMult = 1)
	{
		int num = (int)PredictedObjectManager.Instance.GetCurrentTime(character.IsPredicted()).Ticks + character.Id * 9982 * seedMult;
		return MathUtil.RandomInt(num, min, max + 1);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004A84 File Offset: 0x00002C84
	public static int NextBetweenInts(bool isPredicted, int min, int max, int seedMult = 1)
	{
		int num = (int)PredictedObjectManager.Instance.GetCurrentTime(isPredicted).Ticks * 3854 * seedMult;
		return MathUtil.RandomInt(num, min, max + 1);
	}

	// Token: 0x0400006A RID: 106
	public static EquipmentPrototype DuctTape;

	// Token: 0x0400006B RID: 107
	public static EquipmentPrototype GunRepairKit;

	// Token: 0x0400006C RID: 108
	public static EquipmentPrototype Glue;

	// Token: 0x0400006D RID: 109
	public static EquipmentPrototype JunkParts;

	// Token: 0x0400006E RID: 110
	public static EquipmentPrototype GunParts;

	// Token: 0x0400006F RID: 111
	public static Color32 Pristine = new Color32(114, 181, 13, byte.MaxValue);

	// Token: 0x04000070 RID: 112
	public static Color32 Used = new Color32(224, 213, 0, byte.MaxValue);

	// Token: 0x04000071 RID: 113
	public static Color32 Damaged = new Color32(224, 142, 0, byte.MaxValue);

	// Token: 0x04000072 RID: 114
	public static Color32 VeryDamaged = new Color32(224, 41, 0, byte.MaxValue);

	// Token: 0x04000073 RID: 115
	private static global::System.Random random = new global::System.Random();
}
