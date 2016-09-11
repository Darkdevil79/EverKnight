using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("GameInventory")]
public class GameInventoryManager
{

    [XmlArray("GameItems")]
    [XmlArrayItem("GameItem")]
    public List<GameItem> GameItems;

    public static GameInventoryManager Load(TextAsset xmlText)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameInventoryManager));
        StringReader reader = new StringReader(xmlText.text);
        GameInventoryManager GameItems = serializer.Deserialize(reader) as GameInventoryManager;

        reader.Close();
        return GameItems;
    }
}


