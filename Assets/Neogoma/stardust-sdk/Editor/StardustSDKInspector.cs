using UnityEditor;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    [CustomEditor(typeof(StardustSDK))]
    public class StardustSDKInspector : Editor
    {
        private SerializedProperty apiKey;
        private string lastApiKey;
        private SerializedProperty password;
        private string lastPassword;

        private void OnEnable()
        {
            apiKey = serializedObject.FindProperty("ApiKey");
            lastApiKey = apiKey.stringValue;
        }

        public override void OnInspectorGUI()
        {
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
                    EditorGUILayout.HelpBox("The API Key is mandatory", MessageType.Error);
                }



        }


    }
}
