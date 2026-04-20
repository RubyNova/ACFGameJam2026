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
    public class NPCReviewWindow : EditorWindow
    {
        private List<NPCCharacter> _npcs = new();
        private Vector2 _scrollValue = new(0, 1);
        private Vector2 _scrollValueTwo = new(0, 1);

        [MenuItem("Tools/NPCReview")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<NPCReviewWindow>();
            wnd.titleContent = new GUIContent("NPC Review");
        }
 
        private void OnGUI()
        {
            if (GUILayout.Button("Load NPCs"))
            {
                _npcs = Resources.LoadAll<NPCCharacter>("NPCs").ToList();
                Debug.Log("Loaded " + _npcs.Count + " npcs");
            }

            if(GUILayout.Button("Save to CSV"))
            {
                StringBuilder sb = new();
                sb.AppendLine("InternalName;Name;IdleSpriteAssigned;DesiredItem;Arrival;Departure;AnimationsAssigned");
                
                for (int i = 0; i < _npcs.Count; i++)
                {
                    var npc = _npcs[i];
                    string spriteName = npc.IdleSprite != null ? npc.IdleSprite.name : "NONE";
                    string item = npc.DesiredItem != null ? npc.DesiredItem.name : "NONE";
                    string arriv = npc.ArrivalSequence != null ? npc.ArrivalSequence.name : "NONE";
                    string depart = npc.DepartingSequence != null ? npc.DepartingSequence.name : "NONE";
                    string anims = npc.AnimController != null && npc.SpawnInAnimationClip != null ? "Y" : "N";
                    sb.AppendLine($"{npc.name};{npc.Name};{npc.IdleSprite};{item};{arriv};{depart};{anims}");
                }

                var folder = Application.streamingAssetsPath;
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, "npc_config.csv");
                using (var writer = new StreamWriter(filePath, false))
                {
                    writer.Write(sb.ToString());
                }
            }

            _scrollValue = GUILayout.BeginScrollView(_scrollValue);

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("InternalName", GUILayout.Width(100));
            GUILayout.Label("Name", GUILayout.Width(100));
            GUILayout.Label("Idle Sprite Assigned", GUILayout.Width(100));
            GUILayout.Label("DesiredItem", GUILayout.Width(100));
            GUILayout.Label("Arrival", GUILayout.Width(100));
            GUILayout.Label("Departure", GUILayout.Width(100));
            GUILayout.Label("AnimationsAssigned", GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < _npcs.Count; i++)
            {
                var npc = _npcs[i];
                string spriteName = npc.IdleSprite != null ? npc.IdleSprite.name : "NONE";
                string item = npc.DesiredItem != null ? npc.DesiredItem.name : "NONE";
                string arriv = npc.ArrivalSequence != null ? npc.ArrivalSequence.name : "NONE";
                string depart = npc.DepartingSequence != null ? npc.DepartingSequence.name : "NONE";
                string anims = npc.AnimController != null && npc.SpawnInAnimationClip != null ? "Y" : "N";

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(npc.name, GUILayout.Width(100));
                GUILayout.Label(npc.Name, GUILayout.Width(100));
                GUILayout.Label(spriteName, GUILayout.Width(100));
                GUILayout.Label(item, GUILayout.Width(100));
                GUILayout.Label(arriv, GUILayout.Width(100));
                GUILayout.Label(depart, GUILayout.Width(100));
                GUILayout.Label(anims, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (_npcs == null || _npcs.Count == 0)
            {
                return;
            }

            _scrollValueTwo = GUILayout.BeginScrollView(_scrollValueTwo);
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("ImpossibleNPCInternalName");
            EditorGUILayout.EndHorizontal();

            foreach (var npc in _npcs)
            {
                if (npc.DesiredItem == null || npc.DesiredItem.Recipe == null || npc.DesiredItem.Recipe.Count == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(npc.name);
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
        }
        
    }
}