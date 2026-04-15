using System;
using System.Collections.Generic;
using CraftingAPI;
using UnityEditor;
using UnityEngine;

namespace RandomDevThings
{
    [Serializable]
    public struct PotionSpriteBundle
    {
        public Sprite background;
        public Sprite middle;
        public Sprite foreground;
    }

    public class PotionDataGeneratorWindow : EditorWindow
    {
        private List<PotionSpriteBundle> _spriteBundles = new();
        private Vector2 _scrollValue = new(0, 1);

        [MenuItem("Tools/Potion-no-jutsu")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<PotionDataGeneratorWindow>();
            wnd.titleContent = new GUIContent("Potion-no-jutsu");
        }
 
        private void OnGUI()
        {
            if (GUILayout.Button("Add Item"))
            {
                _spriteBundles.Add(new());
            }

            _scrollValue = GUILayout.BeginScrollView(_scrollValue, GUILayout.ExpandWidth(true));
            for (int i = 0; i < _spriteBundles.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");

                PotionSpriteBundle item = _spriteBundles[i];

                item.background = (Sprite)EditorGUILayout.ObjectField("Background", item.background, typeof(Sprite), false);
                item.middle = (Sprite)EditorGUILayout.ObjectField("Middle", item.middle, typeof(Sprite), false);
                item.foreground = (Sprite)EditorGUILayout.ObjectField("Foreground", item.foreground, typeof(Sprite), false);

                if (GUILayout.Button("Remove"))
                {
                    _spriteBundles.RemoveAt(i);
                    break;
                }
                
                _spriteBundles[i] = item;

                EditorGUILayout.EndVertical();
            }

            GUILayout.EndScrollView();

            if (GUILayout.Button("Potion-no-jutsu!"))
            {
                var itemData = Resources.LoadAll<ItemConfig>("ItemConfigData");

                foreach (var item in itemData)
                {
                    if (item.ItemIconBackground != null)
                    {
                        continue; // already set up, ignore it
                    }

                    var bundle = _spriteBundles[UnityEngine.Random.Range(0, _spriteBundles.Count)];
                    item.ForceUpdateSpriteAssets(bundle.background, bundle.middle, bundle.foreground);

                    Color unityColour = new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

                    item.ForceUpdatePotionContentsTint(unityColour);

                    EditorUtility.SetDirty(item);
                }

                AssetDatabase.SaveAssets();
            }
        }
    }
}