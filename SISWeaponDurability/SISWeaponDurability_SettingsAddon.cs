using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

// Token: 0x0200001C RID: 28
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
[Serializable]
public class SISWeaponDurability_SettingsAddon
{
	// Token: 0x04000012 RID: 18
	[XmlArray(ElementName = "WeaponTypes")]
	public List<WeaponTypeSettings> WeaponTypes = new List<WeaponTypeSettings>();
}
