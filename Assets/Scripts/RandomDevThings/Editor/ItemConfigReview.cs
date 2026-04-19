using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CraftingAPI;
using NPC;
using UnityEditor;
using UnityEngine;

namespace RandomDevThings
{
    public class ItemConfigReviewWindow : EditorWindow
    {
        private List<NPCCharacter> _npcs = new();
        private List<ItemConfig> _items = new();
        private Dictionary<string, string> _itemNpcPairs = new();
        private Vector2 _scrollValue = new(0, 1);

        [MenuItem("Tools/ItemConfigReview")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<ItemConfigReviewWindow>();
            wnd.titleContent = new GUIContent("ItemConfigReview");
        }
 
        private void OnGUI()
        {
            if (GUILayout.Button("Load Items"))
            {
                _items = Resources.LoadAll<ItemConfig>("ItemConfigData").ToList();
                Debug.Log("Loaded " + _items.Count + " items");
                _npcs = Resources.LoadAll<NPCCharacter>("NPCs").ToList();
                Debug.Log("Loaded " + _npcs.Count + " npcs");
                _itemNpcPairs = new();
                for (int i = 0; i < _items.Count; i++)
                {
                    var item = _items[i];
                    Debug.Log("Loading item: " + item.name);
                    var npcList = _npcs.Where(npc => npc.DesiredItem != null && npc.DesiredItem.ItemName.Equals(item.ItemName));
                    if (npcList != null && npcList.Count() > 0)
                    {
                        _itemNpcPairs.Add(item.name, string.Join(", ", npcList.Select(character => character.name)));
                    }
                    else
                    {
                        _itemNpcPairs.Add(item.name, "NONE");
                    }
                }
            }

            if(GUILayout.Button("Save to CSV"))
            {
                StringBuilder sb = new();
                sb.AppendLine("InternalName;Name;SpriteAssigned;IngredientsAssigned;NPCsAssignedTo");
                
                for (int i = 0; i < _items.Count; i++)
                {
                    string ingredients = "NONE";
                    string spriteName = "NONE";
                    var item = _items[i];
                    if (item.Recipe.Count > 0)
                    {
                        ingredients = string.Join(",", item.Recipe.Select(ingredient => ingredient.ItemName));
                    }
                    if (item.ItemIconBackground != null)
                    {
                        spriteName = item.ItemIconBackground.name;
                    }

                    string npcs = "NONE";
                    if(_itemNpcPairs.TryGetValue(item.name, out string val))
                    {
                        npcs = val;
                    }

                    sb.AppendLine($"{item.name};{item.ItemName};{spriteName};{ingredients};{npcs}");
                }

                var folder = Application.streamingAssetsPath;
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, "itemconfig.csv");
                using (var writer = new StreamWriter(filePath, false))
                {
                    writer.Write(sb.ToString());
                }
            }

            _scrollValue = GUILayout.BeginScrollView(_scrollValue);

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("InternalName", GUILayout.Width(50));
            GUILayout.Label("Name", GUILayout.Width(50));
            GUILayout.Label("Sprite Assigned", GUILayout.Width(100));
            GUILayout.Label("Ingredients Assigned", GUILayout.ExpandWidth(true));
            GUILayout.Label("NPCS Assigned To", GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < _items.Count; i++)
            {
                string ingredients = "NONE";
                string spriteName = "NONE";
                var item = _items[i];
                if (item.Recipe.Count > 0)
                {
                    ingredients = string.Join(", ", item.Recipe.Select(ingredient => ingredient.ItemName));
                }
                if (item.ItemIconBackground != null)
                {
                    spriteName = item.ItemIconBackground.name;
                }

                string npcs = "NONE";
                if(_itemNpcPairs.TryGetValue(item.name, out string val))
                {
                    npcs = val;
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(item.name, GUILayout.Width(50));
                GUILayout.Label(item.ItemName, GUILayout.Width(50));
                GUILayout.Label(spriteName, GUILayout.Width(100));
                GUILayout.Label(ingredients, GUILayout.ExpandWidth(true));
                GUILayout.Label(npcs, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        
    }
}