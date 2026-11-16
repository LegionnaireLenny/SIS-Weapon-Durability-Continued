## SIS-Weapon-Durability-Continued
This is an update to SweetCaptn's mod, SIS Weapon Durability https://steamcommunity.com/sharedfiles/filedetails/?id=3487805258, that fixes a few bugs, adds a couple features and several new configuration options. This mod replaces the original mod, so do not use them together. This was tested on the publicbeta version.

## Changelog
#### Version 1.2.1
- Fixed crash when punching zombies

#### Version 1.2.0
- Added an option to toggle degradation for the player
- Assassination, chopping, mining, and skinning check if degradation is enabled for the character (previously it always degraded the item regardless of settings, even for NPCs)
- Improved debug logging (it actually works now)
- Refactoring, code improvements, and cleanup

#### Version 1.1.0
- Fixed bug that prevented chopping trees from degrading axes (previously it checked if a hunting knife was equipped instead of an axe)
- Fixed bug that prevented repairing firearms if the character didn't also have duct tape in their inventory
- Disassembling ranged weapons now gives parts based on the weapon type instead of defaulting to junk parts when the weapon is not defined in Settings_WeaponDurability.xml or Settings_WeaponDurabilityAddon.xml
- Changed the 0.5x multiplier to weapon base damage to 1x and made it a setting in Settings_WeaponDurability.xml (it wasn't mentioned anywhere, so I assume it was unintentional)
- Changed deterioration to decrease base weapon damage along a logarithmic curve instead of linearly (e.g. at 80%, 50%, 30% durability, weapon damage reduced by ~10%, ~30%, and 50% respectively)
- Added option to get animal fat from skinning humans
- Added option to get animal fat from skinning zombies
- Added option to configure weapon disassembly time
- Separated DefaultAmmoWeaponDurability into DefaultGunDurability and DefaultBowDurability
- Exposed additional settings to XML serialization and added them to the default Settings_WeaponDurability.xml
- Added additional error handling to mitigate crashing when skinning
- Refactoring, code improvements, and cleanup