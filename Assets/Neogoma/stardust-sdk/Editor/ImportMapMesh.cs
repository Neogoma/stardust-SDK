using com.Neogoma.Stardust.DataRendering;
using com.Neogoma.Stardust.Utils;
using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    public class ImportMapMesh : EditorWindow
    {
        private static ImportMapMesh window;
        private static EditorMeshDownloader meshDownloader;

        private static string mapUUID;
        private bool currentlyDownloading = false;
        private static float progressValue;
        private GameObject displayedMesh;

        [MenuItem("Stardust SDK/Import mesh (SME and above)")]

        static void Init()
        {
            window = (ImportMapMesh)EditorWindow.GetWindow(typeof(ImportMapMesh), true, "Import mesh from server");

        }

        void OnGUI()
        {
            //EditorGUILayout.BeginVertical();
            mapUUID = EditorGUILayout.TextField("Map ID", mapUUID);

            if (currentlyDownloading)
            {
                string info = "Loading mesh in scene";

                EditorUtility.DisplayProgressBar("Loading the mesh", info, progressValue);
            }
            else
            {

                StardustSDK sdk = GameObject.FindObjectOfType<StardustSDK>();

                if (sdk == null)
                {

                    EditorGUILayout.HelpBox("You need a StardustComponents in scene.", MessageType.Warning);

                }
                else if (string.IsNullOrEmpty(sdk.ApiKey))
                {
                    EditorGUILayout.HelpBox("You need to setup the API key in the StardustComponents prefab.", MessageType.Error);
                    if (GUILayout.Button("Find the StardustComponent"))
                    {
                        Selection.activeObject = sdk.gameObject;
                    }
                }
                else if (!currentlyDownloading && GUILayout.Button("Import into editor"))
                {
                    if (meshDownloader == null)
                    {
                        meshDownloader = new EditorMeshDownloader();

                    }
                    currentlyDownloading = true;

                    if (displayedMesh != null)
                    {
                        Debug.Log(displayedMesh.name);
                        GameObject.Destroy(displayedMesh);
                    }

                    meshDownloader.DownloadMesh(StardustSDK.Instance.ApiKey, mapUUID);
                    EditorCoroutineUtility.StartCoroutineOwnerless(UpdateProgress());
                    EditorCoroutineUtility.StartCoroutineOwnerless(DisplayPointCloudWhenDone());


                }
            }
        }

        private IEnumerator DisplayPointCloudWhenDone()
        {
            yield return new WaitUntil(() => meshDownloader.DownloadFinished);

            displayedMesh = Importer.LoadFromBytes(meshDownloader.Data);
            displayedMesh.name ="Mesh "+mapUUID;
            EditorUtility.ClearProgressBar();
            Close();

        }

        private IEnumerator UpdateProgress()
        {
            yield return new WaitForFixedUpdate();
            progressValue = meshDownloader.Progress;
            if (currentlyDownloading)
                EditorCoroutineUtility.StartCoroutineOwnerless(UpdateProgress());
        }

        private static Material GetPCMaterial()
        {
            var guids = AssetDatabase.FindAssets("t:Material");

            var materials = new List<UnityEngine.Material>();
            foreach (var g in guids)
            {
                var m = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(g)).OfType<UnityEngine.Material>();
                materials.AddRange(m);
            }

            foreach (var m in materials)
            {
                if (m.ToString().Contains("PCLMat"))
                    return m;
            }

            return null;
        }
    }
}