using com.Neogoma.Stardust.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    [CustomEditor(typeof(StardustSDK))]
    public class StardustSDKInspector : Editor
    {
        private SerializedProperty apiKey;
        private string lastApiKey;


        private void OnEnable()
        {
            apiKey = serializedObject.FindProperty("ApiKey");
            lastApiKey = apiKey.stringValue;
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.HelpBox("SDK Version "+StardustSDK.SDKVersion, MessageType.Info);

            if (GUILayout.Button("Open documentation"))
            {
                Application.OpenURL("https://neogoma.github.io/stardust-SDK-doc/");
            }

            //EditorGUILayout.PropertyField();
            lastApiKey = EditorGUILayout.TextField("Api Key", lastApiKey);

            if (lastApiKey.CompareTo(apiKey.stringValue) != 0)
            {
                apiKey.stringValue = lastApiKey;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);


            }

            if (string.IsNullOrEmpty(lastApiKey))
            {
                EditorGUILayout.HelpBox("The API Key is necessary for the SDK to function properly", MessageType.Error);
            }            
        }
    }
}
