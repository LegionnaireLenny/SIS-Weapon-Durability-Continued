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
            (targetCharacter is Human && targetCharacter.Zombie && SISWeaponDurability_Config.ChanceAnimalFatOnSkinZombie <= 0) ||
            (targetCharacter is Human && targetCharacter.Zombie && SISWeaponDurability_Utils.NextFloat(character) >= SISWeaponDurability_Config.ChanceAnimalFatOnSkinZombie) ||
            (targetCharacter is Human && !targetCharacter.Zombie && SISWeaponDurability_Config.ChanceAnimalFatOnSkinHuman <= 0) ||
            (targetCharacter is Human && !targetCharacter.Zombie && SISWeaponDurability_Utils.NextFloat(character) >= SISWeaponDurability_Config.ChanceAnimalFatOnSkinHuman) ||
            (targetCharacter is Animal && SISWeaponDurability_Config.ChanceAnimalFatOnSkinAnimal <= 0) ||
            (targetCharacter is Animal && SISWeaponDurability_Utils.NextFloat(character) >= SISWeaponDurability_Config.ChanceAnimalFatOnSkinAnimal))
        {
            return;
        }

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
