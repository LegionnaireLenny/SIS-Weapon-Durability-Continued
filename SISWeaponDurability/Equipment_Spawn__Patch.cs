using System;
using HarmonyLib;

// Token: 0x0200001F RID: 31
[HarmonyPatch(typeof(Equipment), "Spawn", new Type[]
{
	typeof(EquipmentPrototype),
	typeof(int)
})]
public class Equipment_Spawn__Patch
{
	// Token: 0x06000050 RID: 80 RVA: 0x000043BC File Offset: 0x000025BC
	private static void Postfix(ref Equipment __result, Equipment __instance, EquipmentPrototype prototype, int amount)
	{
		bool flag = !SISWeaponDurability_Config.WeaponsSpawnWithLessDurability;
		if (!flag)
		{
			bool flag2 = __result is MeleeWeapon || __result is AmmoWeapon;
			if (flag2)
			{
				WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(prototype.Name, __result is AmmoWeapon);
				float num = (float)Session.Instance.DeterministicRand.Next((int)((float)((int)weaponModifiersForEquipment.MaxDurability) * 0.3f), (int)(weaponModifiersForEquipment.MaxDurability * 0.9f));
				bool flag3 = __result.InventoryOwner is WorkBench;
				if (flag3)
				{
					num = weaponModifiersForEquipment.MaxDurability;
					SISWeaponDurability_Config.Log("InventoryOwner is WorkBench, Durability = " + num.ToString());
				}
				SISWeaponDurability_Config.Log(string.Concat(new string[]
				{
					"Spawning ",
					prototype.Name,
					";",
					num.ToString(),
					";",
					1.ToString()
				}));
				__result.SetDampness(num);
			}
		}
	}
}
