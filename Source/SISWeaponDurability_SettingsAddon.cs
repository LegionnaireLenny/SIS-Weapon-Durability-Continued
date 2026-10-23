using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
[Serializable]
public class SISWeaponDurability_SettingsAddon
{
	[XmlArray(ElementName = "WeaponTypes")]
	public List<WeaponTypeSettings> WeaponTypes = new List<WeaponTypeSettings>();
}
