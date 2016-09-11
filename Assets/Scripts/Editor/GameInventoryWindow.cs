using UnityEngine;
using UnityEditor;

public class GameInventoryWindow : EditorWindow {

    private static TextAsset xmlDBAsset;
    public Vector2 scrollPosition;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Game Inventory")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GameInventoryWindow window = (GameInventoryWindow)EditorWindow.GetWindow(typeof(GameInventoryWindow));
        window.titleContent = new GUIContent("Game Inventory");
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        xmlDBAsset = (TextAsset)EditorGUILayout.ObjectField("XML File DB", xmlDBAsset, typeof(TextAsset), true);
        EditorGUILayout.Space();

        EditorGUILayout.Separator();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (xmlDBAsset != null)
        {
            GameInventoryManager gameInventory = GameInventoryManager.Load(xmlDBAsset);
            GUILayout.Label(string.Format("Item XML DB Item : {0}" , gameInventory.GameItems.Count));

            GUILayout.BeginVertical();
            foreach (GameItem item in gameInventory.GameItems)
            {
                Texture2D myTexture = (Texture2D)item.WeaponSprite;
                                
                GUILayout.Box(myTexture, GUILayout.Width(100), GUILayout.Height(100)) ;
                GUILayout.Label(item.Name);

                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();

        }
        else
        {
            EditorGUILayout.LabelField("No XML DB Item In Database");
        }

        EditorGUILayout.EndScrollView();

    }
}
