using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "GetBaseDamage")]
public class Equipment_GetBaseDamage__Patch
{
	private static void Postfix(ref float __result, Equipment __instance)
	{
		float maxDurabilityForEquipment = SISWeaponDurability_Config.GetMaxDurabilityForEquipment(__instance.GetPrototype().Name, __instance is AmmoWeapon);
        if ((__instance is MeleeWeapon || __instance is AmmoWeapon) && __instance.GetDampness() < maxDurabilityForEquipment)
		{
            // It looks like weapon damage is capped at half the normal damage even at full durability, probably unintended, also annoying
            // Probably will lerp damage proportional to durability with a step of 5 or 10 so damage doesn't drop with every use
            float dampness = __instance.GetDampness();
			__result = __instance.GetPrototype().Damage * (dampness / maxDurabilityForEquipment) * 0.5f;
            SISWeaponDurability_Config.Log($"Current durability: {dampness} | Max durability: {maxDurabilityForEquipment} | Base Damage: {__instance.GetPrototype().Damage} | Modified Damage: {__result}");
        }
    }
}
