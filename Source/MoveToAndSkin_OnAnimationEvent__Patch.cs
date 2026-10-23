using System;
using HarmonyLib;

// TODO: This function throws errors occasionally
[HarmonyPatch(typeof(MoveToAndSkin), "OnAnimationEvent")]
public class MoveToAndSkin_OnAnimationEvent__Patch
{
	private static void Prefix(MoveToAndSkin __instance, Character character, Goal parent, AnimEvent animEvent)
    {
        if (__instance == null ||
            character == null ||
            parent == null ||
            parent.GetTargetCharacter() == null ||
            animEvent == null ||
            animEvent.EventType != AnimationEventType.FinishSkinning)
        {
            return;
        }

        Character targetCharacter = parent.GetTargetCharacter();
        if (targetCharacter == null || 
            targetCharacter.SkinnedAmount >= 1.0 || 
            !(targetCharacter is Animal) ||
            SISWeaponDurability_Utils.NextFloat(character) >= SISWeaponDurability_Config.ChanceAnimalFatOnSkin)
        {
            return;
        }

        // TODO: Human check is redundant since the function returns if the target isn't an animal
        // Add option to get fat from humans
        int num = 0;
        if (targetCharacter is Human)
        {
            num = SISWeaponDurability_Utils.NextBetweenInts(character, 0, 3, 1);
        }
        else if (targetCharacter is Animal animal)
        {
            if (animal.IsSmallAnimal())
            {
                num = SISWeaponDurability_Utils.NextBetweenInts(character, 1, 2, 1);
            }
            else
            {
                num = SISWeaponDurability_Utils.NextBetweenInts(character, 2, 4, 1);
            }
        }

        if (num > 0)
        {
            Equipment equipment = Equipment.Spawn(GameImpl.Instance.FindEquipmentPrototypeByName(SISWeaponDurability_Config.AnimalFatPrototypeName), num);
            character.Inventory.Add(character, equipment);
            NotificationManager.Instance.AddEquipmentNotification(null, character, equipment, num);
        }
    }
}
