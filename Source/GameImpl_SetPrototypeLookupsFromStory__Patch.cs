using System;
using System.Collections.Generic;
using HarmonyLib;

[HarmonyPatch(typeof(GameImpl), "SetPrototypeLookupsFromStory")]
public class GameImpl_SetPrototypeLookupsFromStory__Patch
{
	private static void AddMeleeDisassembleWorkbenchRecipe(GameImpl __instance, Story story, EquipmentPrototype equip)
    {
        if (__instance == null || __instance.CurrentRecipes == null)
        {
            return;
        }

        Recipe recipe = new Recipe();
        recipe.UniqueID = SISWeaponDurability_Config.DisassembleMeleeWeaponWorkbenchRecipeUniqueId + "_" + equip.Name;
        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip.Name, false);
        recipe.NativeName = "Scrap Weapon";
        recipe.ShowRecipe = ShowRecipe.IfAllIngredientsFound;
        recipe.CraftingStationType = "WorkBench";
        recipe.RecipeType = RecipeType.Workbench;
        recipe.SkillType = SkillType.Construction;
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

    private static void AddGunDisassembleWorkbenchRecipe(GameImpl __instance, Story story, EquipmentPrototype equip)
    {
        if (__instance == null || __instance.CurrentRecipes == null)
        {
            return;
        }

        Recipe recipe = new Recipe();
        recipe.UniqueID = SISWeaponDurability_Config.DisassembleGunWorkbenchRecipeUniqueId + "_" + equip.Name;
        WeaponTypeSettings weaponModifiersForEquipment = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip.Name, false);
        recipe.NativeName = "Scrap Weapon";
        recipe.ShowRecipe = ShowRecipe.IfAllIngredientsFound;
        recipe.CraftingStationType = "WorkBench";
        recipe.RecipeType = RecipeType.Workbench;
        recipe.SkillType = SkillType.Construction;
        recipe.SkillLevel = 2;
        recipe.ProductAmount = weaponModifiersForEquipment.MaxPartsOnBroken + 1;
        recipe.IsDisassembly = true;
        recipe.ProductPrototypeName = equip.InjuryType == InjuryType.Arrow ? SISWeaponDurability_Config.WeaponPartPrototypeName : SISWeaponDurability_Config.GunPartPrototypeName;
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

    private static void AddRecipes(GameImpl __instance, Story story)
	{
		foreach (EquipmentPrototype equipmentPrototype in __instance.CurrentEquipmentPrototypes.Values)
		{
            if (SISWeaponDurability_Utils.IsPrototypeMeleeWeapon(equipmentPrototype))
			{
				GameImpl_SetPrototypeLookupsFromStory__Patch.AddMeleeDisassembleWorkbenchRecipe(__instance, story, equipmentPrototype);
			}
			else if (SISWeaponDurability_Utils.IsPrototypeAmmoWeapon(equipmentPrototype))
			{
				GameImpl_SetPrototypeLookupsFromStory__Patch.AddGunDisassembleWorkbenchRecipe(__instance, story, equipmentPrototype);
			}
		}
	}

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
