using UnityEditor;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    [CustomEditor(typeof(APIAutoLogin))]
    public class AutoLoginCustomInspector:Editor
    {
        private SerializedProperty email;
        private string lastEmail;
        private SerializedProperty password;
        private string lastPassword;

        private void OnEnable()
        {
            email = serializedObject.FindProperty("email");
            lastEmail = email.stringValue;
            password = serializedObject.FindProperty("password");
            lastPassword = password.stringValue;
        }

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.PropertyField();
            lastEmail= EditorGUILayout.TextField("Email", lastEmail);

            if (lastEmail.CompareTo(email.stringValue) != 0)
            {
                email.stringValue = lastEmail;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);

               
            }

             if (string.IsNullOrEmpty(lastEmail))
                {
                    EditorGUILayout.HelpBox("Please fill in the mail", MessageType.Error);
                }

            lastPassword = EditorGUILayout.PasswordField("User password", lastPassword);

            if (lastPassword.CompareTo(password.stringValue)!=0)
            {
                password.stringValue = lastPassword;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);

                
            }

            if (string.IsNullOrEmpty(lastPassword))
            {
                EditorGUILayout.HelpBox("Please fill in the password", MessageType.Error);
            }


        }


    }
}
