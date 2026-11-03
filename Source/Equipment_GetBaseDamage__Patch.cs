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
            float minDamage = __instance.GetPrototype().Damage * SISWeaponDurability_Config.DeterioratedWeaponMinimumDamagePercent;
            float deteriorationMultiplier = Mathf.Log10(durabilityPercent * 10);
            float damage = __instance.GetPrototype().Damage * deteriorationMultiplier * SISWeaponDurability_Config.GlobalWeaponBaseDamageMultiplier;
            __result = Mathf.Clamp(damage, minDamage, __instance.GetPrototype().Damage);
            //SISWeaponDurability_Config.Log($"{__instance.GetPrototype().Name} | Durability: {currentDurability}/{maxDurabilityForEquipment} | Deterioration: x{deteriorationMultiplier} | Global: x{SISWeaponDurability_Config.GlobalWeaponBaseDamageMultiplier} | Damage: {__instance.GetPrototype().Damage} -> {__result}");
        }
    }
}
