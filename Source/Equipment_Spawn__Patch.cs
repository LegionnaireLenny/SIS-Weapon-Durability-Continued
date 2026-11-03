using HarmonyLib;
using System;

[HarmonyPatch(typeof(Equipment), "Spawn", new Type[]
{
    typeof(EquipmentPrototype),
    typeof(int)
})]
public class Equipment_Spawn__Patch
{
    private static void Postfix(Equipment __instance, EquipmentPrototype prototype, int amount, ref Equipment __result)
    {
        if (!SISWeaponDurability_Config.WeaponsSpawnWithLessDurability || !(__result is MeleeWeapon || __result is AmmoWeapon))
        {
            return;
        }

        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(__result);
        int minDurability = (int)(weaponModifiersForEquipment.MaxDurability * SISWeaponDurability_Config.MinWeaponDurabilityOnSpawnPcnt);
        int maxDurability = (int)(weaponModifiersForEquipment.MaxDurability * SISWeaponDurability_Config.MaxWeaponDurabilityOnSpawnPcnt);
        float num = Session.Instance.DeterministicRand.Next(minDurability, maxDurability);
        if (__result.InventoryOwner is WorkBench)
        {
            num = weaponModifiersForEquipment.MaxDurability;
            SISWeaponDurability_Config.Log($"InventoryOwner is WorkBench, Durability = {num}");
        }
        SISWeaponDurability_Config.Log($"Spawning {__result.GetPrototype().Name};{num};1");
        __result.SetDampness(num);
    }
}
