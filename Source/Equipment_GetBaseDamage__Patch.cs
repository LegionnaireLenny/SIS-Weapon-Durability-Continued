using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Equipment), "GetBaseDamage")]
public class Equipment_GetBaseDamage__Patch
{
	private static void Postfix(ref float __result, Equipment __instance)
	{
		float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance);
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() < maxDurabilityForEquipment)
		{
            float currentDurability = __instance.GetDampness();
            float durabilityPercent = currentDurability / maxDurabilityForEquipment;
            int reductionIncrements = SISWeaponDurability_Config.DurabilityDamageReductionThresholdCount;

            __result = __instance.GetPrototype().Damage * (Mathf.Ceil(durabilityPercent * reductionIncrements) / reductionIncrements) * SISWeaponDurability_Config.GlobalWeaponBaseDamageMultiplier;
            SISWeaponDurability_Config.Log($"{__instance.GetPrototype().Name} | Durability: {currentDurability}/{maxDurabilityForEquipment} | Damaged: x{(Mathf.Ceil(durabilityPercent * reductionIncrements) / reductionIncrements)} | Global: x{SISWeaponDurability_Config.GlobalWeaponBaseDamageMultiplier} | Damage: {__instance.GetPrototype().Damage} -> {__result}");
        }
    }
}
