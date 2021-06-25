using com.Neogoma.HoboDream.Network;
using System;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    [CustomEditor(typeof(StardustSDK))]
    public class StardustSDKInspector : Editor, IJSonRequestListener
    {
        private SerializedProperty apiKey;
        private string lastApiKey;
        private string serverVersion;


        private void OnEnable()
        {
            serverVersion = StardustSDK.SDKVersion;
            apiKey = serializedObject.FindProperty("ApiKey");
            lastApiKey = apiKey.stringValue;      

        }

        public override void OnInspectorGUI()
        {
            if (serverVersion.CompareTo(StardustSDK.SDKVersion) == 0)
            {
                EditorGUILayout.HelpBox("SDK Version " + StardustSDK.SDKVersion, MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("SDK Version " + StardustSDK.SDKVersion + "\nServer version"+serverVersion+" please update your SDK", MessageType.Warning);
            }

            if (GUILayout.Button("Open documentation"))
            {
                Application.OpenURL("https://neogoma.github.io/stardust-SDK-doc/");
            }

            //EditorGUILayout.PropertyField();
            lastApiKey = EditorGUILayout.TextField("ApiKey", lastApiKey);

            if (lastApiKey.CompareTo(apiKey.stringValue) != 0)
            {
                apiKey.stringValue = lastApiKey;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);


            }

            if (string.IsNullOrEmpty(lastApiKey))
            {
                EditorGUILayout.HelpBox("Please enter an API key (unless you initialize it programatically)", MessageType.Error);
            }
           
        }

        public void RequestSucess(string jsonResult, string key)
        {
            throw new System.NotImplementedException();
        }

        public void RequestFailed(string error, string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
