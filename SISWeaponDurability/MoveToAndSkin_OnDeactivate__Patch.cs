using System;
using HarmonyLib;

// Token: 0x02000018 RID: 24
[HarmonyPatch(typeof(MoveToAndSkin), "OnDeactivate")]
public class MoveToAndSkin_OnDeactivate__Patch
{
	// Token: 0x06000032 RID: 50 RVA: 0x000031C4 File Offset: 0x000013C4
	private static void Postfix(MoveToAndSkin __instance, Character character, Goal parent)
	{
		bool success = __instance.Success;
		if (success)
		{
			SISWeaponDurability_Config.Log("Skinning successful");
			bool skinningReducesDurability = SISWeaponDurability_Config.SkinningReducesDurability;
			if (skinningReducesDurability)
			{
				WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(character.EquippedItem.GetPrototype().Name, false);
				bool flag = character.EquippedItem is HuntingKnife;
				if (flag)
				{
					SISWeaponDurability_Utils.ReduceDurability(character.EquippedItem, SISWeaponDurability_Config.SkinningDurabilityDegradePercentage * weaponModifiersForEquipment.MaxDurability);
				}
			}
		}
	}
}
