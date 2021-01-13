#if UNITY_EDITOR
using com.Neogoma.HoboDream.Language;
using com.Neogoma.HoboDream.UI.Impl.Language;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Neogoma.HoboDream.EditorTools
{
    /// <summary>
    /// EditorScript script to create a tool that will allow to quickly and automatically add the languages keys
    /// </summary>
    /// <seealso cref="UnityEditor.EditorWindow" />
    public class AutomaticLanguageAdd : EditorWindow
    {
        static AutomaticLanguageAdd window;
        static Dictionary<Text, bool> textToMapButton = new Dictionary<Text, bool>();
        static List<Text> allTextWithoutLoader = new List<Text>();
        static List<Text> allTexts;

        static Dictionary<string, bool> providerKeyToMapButton = new Dictionary<string, bool>();
        static List<ISourceCodeLanguageProvider> allLanguageKeysProvider = new List<ISourceCodeLanguageProvider>();
        static List<string> languageKeysProviderToAdd = new List<string>();

        private Vector2 scrollPos;
        static List<string> allLanguageNames;
        static int selectedLanguage = 0;
        static int previousSelectedLanguage = -1;
        static string currentSelectedLanguageName;


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [MenuItem("Tools/Neogoma/Language Manager systems")]
        static void Init()
        {
            window = (AutomaticLanguageAdd)EditorWindow.GetWindow(typeof(AutomaticLanguageAdd), true, "All texts without Language Loader");
            allTexts = EditorUtils.FindObjectsOfTypeAll<Text>();
            textToMapButton.Clear();
            allTextWithoutLoader.Clear();
            providerKeyToMapButton.Clear();
            allLanguageKeysProvider.Clear();
            languageKeysProviderToAdd.Clear();

            if (allLanguageNames == null)
            {
                List<SystemLanguage> allLanguagesValue = SystemLanguage.GetValues(typeof(SystemLanguage)).Cast<SystemLanguage>().ToList();

                allLanguageNames = new List<string>();

                foreach (SystemLanguage lang in allLanguagesValue)
                {
                    allLanguageNames.Add(lang.ToString());
                }
            }

            for (int i = 0; i < allTexts.Count; i++)
            {
                GameObject currentGameobject = allTexts[i].gameObject;
                if (currentGameobject.GetComponent<LanguageLoader>() == null)
                {
                    textToMapButton.Add(allTexts[i], false);
                    allTextWithoutLoader.Add(allTexts[i]);
                }
            }


            allLanguageKeysProvider = EditorUtils.FindObjectsOfTypeAll<ISourceCodeLanguageProvider>();
            for (int i = 0; i < allLanguageKeysProvider.Count; i++)
            {
                ISourceCodeLanguageProvider currentGameobject = allLanguageKeysProvider[i];

                List<string> allKeys = currentGameobject.GetAllProvidedKeys();

                foreach (string currentkey in allKeys)
                {
                    if (!providerKeyToMapButton.ContainsKey(currentkey))
                    {
                        providerKeyToMapButton.Add(currentkey, false);
                        languageKeysProviderToAdd.Add(currentkey);

                    }
                    else
                    {
                        Debug.Log("The key " + currentkey + " is already here");
                    }
                }
            }

            window.Show();

        }



        void OnGUI()
        {

            EditorGUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


            //Adding all text without loaders
            for (int i = 0; i < allTextWithoutLoader.Count; i++)
            {

                EditorGUILayout.BeginHorizontal();
                Text txt = allTextWithoutLoader[i];

                bool value = textToMapButton[txt];

                value = EditorGUILayout.ToggleLeft(GetName(txt.gameObject), value, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("View"))
                {
                    Selection.activeGameObject = txt.gameObject;
                }

                textToMapButton[txt] = value;
                EditorGUILayout.EndHorizontal();

            }

            foreach (string currentKey in languageKeysProviderToAdd)
            {
                EditorGUILayout.BeginHorizontal();

                bool value = providerKeyToMapButton[currentKey];
                value = EditorGUILayout.ToggleLeft(currentKey, value, GUILayout.ExpandWidth(true));
                providerKeyToMapButton[currentKey] = value;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            selectedLanguage = EditorGUILayout.Popup(selectedLanguage, allLanguageNames.ToArray());
            if (selectedLanguage != previousSelectedLanguage)
            {
                currentSelectedLanguageName = allLanguageNames[selectedLanguage];
                previousSelectedLanguage = selectedLanguage;
            }

            if (GUILayout.Button("Add the Language Loaders"))
            {
                AddScripts();
                UpdateJSonFile();
                this.Close();
            }
            EditorGUILayout.EndVertical();
        }

        private void AddScripts()
        {
            Dictionary<Text, bool>.KeyCollection allKeys = textToMapButton.Keys;
            foreach (Text key in allKeys)
            {
                if (textToMapButton[key])
                {
                    LanguageLoader loader = key.gameObject.AddComponent<LanguageLoader>();
                    loader.textKey = GetName(key.gameObject);
                    EditorUtility.SetDirty(key.gameObject);
                }
            }
            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
        }

        private void UpdateJSonFile()
        {

            List<LanguageLoader> allLoaders = EditorUtils.FindObjectsOfTypeAll<LanguageLoader>();
            List<LocalizationItem> allItems = new List<LocalizationItem>();
            foreach (LanguageLoader loader in allLoaders)
            {
                allItems.Add(new LocalizationItem(loader.textKey));
            }


            foreach (KeyValuePair<string, bool> entry in providerKeyToMapButton)
            {
                if (entry.Value)
                {
                    allItems.Add(new LocalizationItem(entry.Key));
                }
            }

            if (!Directory.Exists(Application.dataPath + "/StreamingAssets"))
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
            }


            string path = Path.Combine(Application.dataPath+ "/StreamingAssets","lang" + currentSelectedLanguageName + ".json");

            if (File.Exists(path))
            {
                string dataAsJson = File.ReadAllText(path);
                LanguageData loadedData = JsonUtility.FromJson<LanguageData>(dataAsJson);
                foreach (LocalizationItem item in allItems)
                {

                    if (!loadedData.allItems.Contains(item))
                    {
                        loadedData.allItems.Add(item);
                    }
                }

                string json = JsonUtility.ToJson(loadedData);
                json = json.Replace(",{", ",\n{");
                File.WriteAllText(path, json);
            }   
            else
            {
                LanguageData newData = new LanguageData();
                newData.allItems = allItems;
                string json = JsonUtility.ToJson(newData);
                json = json.Replace(",{", ",\n{");
                File.WriteAllText(path, json);
            }

            EditorUtility.RevealInFinder(path);
        }


        private string GetName(GameObject currentGameobject)
        {
            string name = currentGameobject.name;


            Transform parent = currentGameobject.transform.parent;

            while (parent != null)
            {
                name = parent.gameObject.name + "/" + name;
                parent = parent.parent;
            }
            name.Replace(" ", "_");

            return name;
        }
    }
}
#endif