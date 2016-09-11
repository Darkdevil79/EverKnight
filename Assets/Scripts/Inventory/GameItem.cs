using System.Xml.Serialization;
using UnityEngine;

public class GameItem {

    [XmlAttribute("Name")]
    public string Name;

    public Texture WeaponSprite;

    public GameItem()
    {
        WeaponSprite = Resources.Load<Sprite>("Weapons/Weapons_ZombieAxe").texture;
    }

}
