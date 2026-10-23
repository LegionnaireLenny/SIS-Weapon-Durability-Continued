using System;
using System.Collections.Generic;
using HarmonyLib;

// Token: 0x0200000B RID: 11
[HarmonyPatch(typeof(GameImpl), "SetPrototypeLookupsFromStory")]
public class GameImpl_SetPrototypeLookupsFromStory__Patch
{
	// Token: 0x06000013 RID: 19 RVA: 0x000025A0 File Offset: 0x000007A0
	private static void AddMeleeDisassembleWorkbenchRecipe(GameImpl __instance, Story story, EquipmentPrototype equip)
	{
		bool flag = __instance == null || __instance.CurrentRecipes == null;
		if (!flag)
		{
			Recipe recipe = new Recipe();
			recipe.UniqueID = SISWeaponDurability_Config.DisassembleMeleeWeaponWorkbenchRecipeUniqueId + "_" + equip.Name;
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip.Name, false);
			recipe.NativeName = "Scrap Weapon";
			recipe.ShowRecipe = 1;
			recipe.CraftingStationType = "WorkBench";
			recipe.RecipeType = 12;
			recipe.SkillType = 5;
			recipe.SkillLevel = 1;
			recipe.ProductAmount = weaponModifiersForEquipment.MaxPartsOnBroken;
			recipe.IsDisassembly = true;
			recipe.ProductPrototypeName = SISWeaponDurability_Config.WeaponPartPrototypeName;
			recipe.SkillProgression = 1;
			recipe.CraftingTime = 15f;
			recipe.NameHash = StringUtil.JenkinsHash(SISWeaponDurability_Config.DisassembleWeaponRecipeNameKey);
			Ingredient ingredient = new Ingredient();
			ingredient.PrototypeNames = new List<string>();
			ingredient.Amount = 1;
			ingredient.PrototypeNames.Add(equip.Name);
			recipe.Ingredients.Add(ingredient);
			story.Recipes[recipe.UniqueID] = recipe;
			__instance.CurrentRecipes[recipe.UniqueID] = recipe;
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000026C4 File Offset: 0x000008C4
	private static void AddGunDisassembleWorkbenchRecipe(GameImpl __instance, Story story, EquipmentPrototype equip)
	{
		bool flag = __instance == null || __instance.CurrentRecipes == null;
		if (!flag)
		{
			Recipe recipe = new Recipe();
			recipe.UniqueID = SISWeaponDurability_Config.DisassembleGunWorkbenchRecipeUniqueId + "_" + equip.Name;
			WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip.Name, false);
			recipe.NativeName = "Scrap Weapon";
			recipe.ShowRecipe = 1;
			recipe.CraftingStationType = "WorkBench";
			recipe.RecipeType = 12;
			recipe.SkillType = 5;
			recipe.SkillLevel = 2;
			recipe.ProductAmount = weaponModifiersForEquipment.MaxPartsOnBroken + 1;
			recipe.IsDisassembly = true;
			recipe.ProductPrototypeName = weaponModifiersForEquipment.PartsPrototypeName;
			recipe.SkillProgression = 1;
			recipe.CraftingTime = 15f;
			recipe.NameHash = StringUtil.JenkinsHash(SISWeaponDurability_Config.DisassembleWeaponRecipeNameKey);
			Ingredient ingredient = new Ingredient();
			ingredient.PrototypeNames = new List<string>();
			ingredient.Amount = 1;
			ingredient.PrototypeNames.Add(equip.Name);
			recipe.Ingredients.Add(ingredient);
			story.Recipes[recipe.UniqueID] = recipe;
			__instance.CurrentRecipes[recipe.UniqueID] = recipe;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000027EC File Offset: 0x000009EC
	private static void AddRecipes(GameImpl __instance, Story story)
	{
		foreach (EquipmentPrototype equipmentPrototype in __instance.CurrentEquipmentPrototypes.Values)
		{
			bool flag = SISWeaponDurability_Utils.IsPrototypeMeleeWeapon(equipmentPrototype);
			if (flag)
			{
				GameImpl_SetPrototypeLookupsFromStory__Patch.AddMeleeDisassembleWorkbenchRecipe(__instance, story, equipmentPrototype);
			}
			else
			{
				bool flag2 = SISWeaponDurability_Utils.IsPrototypeAmmoWeapon(equipmentPrototype);
				if (flag2)
				{
					GameImpl_SetPrototypeLookupsFromStory__Patch.AddGunDisassembleWorkbenchRecipe(__instance, story, equipmentPrototype);
				}
			}
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002870 File Offset: 0x00000A70
	private static void Postfix(GameImpl __instance, Story story)
	{
		try
		{
			GameImpl_SetPrototypeLookupsFromStory__Patch.AddRecipes(__instance, story);
		}
		catch (Exception ex)
		{
			SISWeaponDurability_Config.Log("ERROR adding recipes: " + ex.Message);
		}
	}
}
