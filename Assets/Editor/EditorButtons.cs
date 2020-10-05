using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(MapVariation))]
public class HackerButtons : Editor
{
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Variation!"))
            {
                (target as MapVariation).ChangeMap();
            }
        }


    }

