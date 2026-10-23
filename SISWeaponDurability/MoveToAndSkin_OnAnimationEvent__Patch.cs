using System;
using HarmonyLib;

// Token: 0x02000015 RID: 21
[HarmonyPatch(typeof(MoveToAndSkin), "OnAnimationEvent")]
public class MoveToAndSkin_OnAnimationEvent__Patch
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002FA4 File Offset: 0x000011A4
	private static void Prefix(MoveToAndSkin __instance, Character character, Goal parent, AnimEvent animEvent)
	{
		bool flag = animEvent.EventType == 51;
		if (flag)
		{
			Character targetCharacter = parent.GetTargetCharacter();
			bool flag2 = targetCharacter != null && targetCharacter is Animal && (double)targetCharacter.SkinnedAmount < 1.0;
			if (flag2)
			{
				bool flag3 = SISWeaponDurability_Utils.NextFloat(character) < SISWeaponDurability_Config.ChanceAnimalFatOnSkin;
				if (flag3)
				{
					int num = 0;
					Animal animal = targetCharacter as Animal;
					bool flag4 = animal != null && !animal.IsSmallAnimal();
					if (flag4)
					{
						num = SISWeaponDurability_Utils.NextBetweenInts(character, 2, 4, 1);
					}
					else
					{
						Animal animal2 = targetCharacter as Animal;
						bool flag5 = animal2 != null && animal2.IsSmallAnimal();
						if (flag5)
						{
							num = SISWeaponDurability_Utils.NextBetweenInts(character, 1, 2, 1);
						}
						else
						{
							bool flag6 = targetCharacter is Human;
							if (flag6)
							{
								num = SISWeaponDurability_Utils.NextBetweenInts(character, 0, 3, 1);
							}
						}
					}
					bool flag7 = num > 0;
					if (flag7)
					{
						Equipment equipment = Equipment.Spawn(GameImpl.Instance.FindEquipmentPrototypeByName(SISWeaponDurability_Config.AnimalFatPrototypeName), num);
						character.Inventory.Add(character, equipment);
						NotificationManager.Instance.AddEquipmentNotification(null, character, equipment, num);
					}
				}
			}
		}
	}
}
