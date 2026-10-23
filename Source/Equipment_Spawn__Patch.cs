using System;
using HarmonyLib;

[HarmonyPatch(typeof(Equipment), "Spawn", new Type[]
{
	typeof(EquipmentPrototype),
	typeof(int)
})]
public class Equipment_Spawn__Patch
{
	private static void Postfix(ref Equipment __result, Equipment __instance, EquipmentPrototype prototype, int amount)
    {
        if (!SISWeaponDurability_Config.WeaponsSpawnWithLessDurability || !(__result is MeleeWeapon || __result is AmmoWeapon))
        {
            return;
        }

        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(prototype.Name, __result is AmmoWeapon);
        float num = Session.Instance.DeterministicRand.Next((int)(weaponModifiersForEquipment.MaxDurability * 0.3f), (int)(weaponModifiersForEquipment.MaxDurability * 0.9f));
        if (__result.InventoryOwner is WorkBench)
        {
            num = weaponModifiersForEquipment.MaxDurability;
            SISWeaponDurability_Config.Log($"InventoryOwner is WorkBench, Durability = {num}");
        }
        SISWeaponDurability_Config.Log($"Spawning {prototype.Name};{num};1");
        __result.SetDampness(num);
    }
}
