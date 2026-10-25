using System;
using System.Collections.Generic;
using HarmonyLib;

[HarmonyPatch(typeof(GameImpl), "SetPrototypeLookupsFromStory")]
public class GameImpl_SetPrototypeLookupsFromStory__Patch
{
	private static void Postfix(GameImpl __instance, Story story)
	{
		try
		{
            if (__instance == null || __instance.CurrentRecipes == null || story == null)
            {
                SISWeaponDurability_Config.Log("ERROR adding recipes, gameplay instance or story is null");
                return;
            }

            foreach (EquipmentPrototype equip in __instance.CurrentEquipmentPrototypes.Values)
            {
                string id;
                int skillLevel;
                int productAmount;
                WeaponTypeSettings settings = SISWeaponDurability_Config.GetWeaponModifiersForEquipment(equip);
                if (SISWeaponDurability_Utils.IsPrototypeMeleeWeapon(equip) || SISWeaponDurability_Utils.IsPrototypeBow(equip))
                {
                    id = $"{SISWeaponDurability_Config.DisassembleMeleeWeaponWorkbenchRecipeUniqueId}_{equip.Name}";
                    skillLevel = 1;
                    productAmount = settings.MaxPartsOnBroken;
                }
                else if (SISWeaponDurability_Utils.IsPrototypeGun(equip))
                {
                    id = $"{SISWeaponDurability_Config.DisassembleGunWorkbenchRecipeUniqueId}_{equip.Name}";
                    skillLevel = 2;
                    productAmount = settings.MaxPartsOnBroken + 1;
                }
                else
                {
                    continue;
                }

                Recipe recipe = new Recipe
                {
                    UniqueID = id,
                    NativeName = "Scrap Weapon",
                    ShowRecipe = ShowRecipe.IfAllIngredientsFound,
                    CraftingStationType = "WorkBench",
                    RecipeType = RecipeType.Workbench,
                    SkillType = SkillType.Construction,
                    SkillLevel = skillLevel,
                    ProductAmount = productAmount,
                    IsDisassembly = true,
                    ProductPrototypeName = SISWeaponDurability_Utils.GetDisassemblyPrototypeName(equip),
                    SkillProgression = 1,
                    CraftingTime = SISWeaponDurability_Config.DisassemblyTime,
                    NameHash = StringUtil.JenkinsHash(SISWeaponDurability_Config.DisassembleWeaponRecipeNameKey)
                };
                Ingredient ingredient = new Ingredient
                {
                    PrototypeNames = new List<string>(),
                    Amount = 1
                };
                ingredient.PrototypeNames.Add(equip.Name);
                recipe.Ingredients.Add(ingredient);
                story.Recipes[recipe.UniqueID] = recipe;
                __instance.CurrentRecipes[recipe.UniqueID] = recipe;
            }
        }
        catch (Exception ex)
		{
			SISWeaponDurability_Config.Log("ERROR adding recipes: " + ex.Message);
		}
	}
}
