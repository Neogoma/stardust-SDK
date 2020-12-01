using com.Neogoma.HoboDream.Util;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Neogoma.HoboDream.EditorTools
{
    public class CreateDatabaseConfigurationWindow:EditorWindow
    {

        private static CreateDatabaseConfigurationWindow window;

        private string developmentUrl;
        private string productionUrl;
        private string preProductionUrl;
        private string localUrl;
        private bool useLocal = true;
        private bool usePreproduction = true;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [MenuItem("Tools/Neogoma/Create API Configuration")]
        static void Init()
        {
            window = (CreateDatabaseConfigurationWindow)EditorWindow.GetWindow(typeof(CreateDatabaseConfigurationWindow), true, "Database configuration");
            
        }

        private void OnGUI()
        {
            localUrl = EditorGUILayout.TextField("Local URL", localUrl);

            developmentUrl = EditorGUILayout.TextField("Development URL", developmentUrl);
            preProductionUrl = EditorGUILayout.TextField("Preproduction URL URL", preProductionUrl);
            productionUrl = EditorGUILayout.TextField("Production URL", productionUrl);
            usePreproduction =  EditorGUILayout.Toggle("Use Preproduction", usePreproduction);
            useLocal = EditorGUILayout.Toggle("Use Local", useLocal);

            if (GUILayout.Button("Create configuration"))
            {

                if (!Directory.Exists(Application.dataPath + "/StreamingAssets"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                }

                string path = Application.dataPath + "/StreamingAssets/dbconfig.json";
                DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();
                databaseConfiguration.development_url = developmentUrl;
                databaseConfiguration.preproduction_url = preProductionUrl;
                databaseConfiguration.production_url = productionUrl;
                databaseConfiguration.local_url = localUrl;
                databaseConfiguration.use_preprod = "0";
                databaseConfiguration.use_local = "0";

                
                if (usePreproduction)
                    databaseConfiguration.use_preprod = "1";

                if(useLocal)
                    databaseConfiguration.use_local = "1";

                string json = JsonUtility.ToJson(databaseConfiguration);
                File.WriteAllText(path, json);
                EditorUtility.RevealInFinder(path);


            }

        }
    }

    
}
