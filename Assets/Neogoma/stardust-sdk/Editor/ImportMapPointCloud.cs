using com.Neogoma.Stardust.DataRendering;
using com.Neogoma.Stardust.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.Stardust.API.CustomsEditor
{
    public class ImportMapPointCloud : EditorWindow
    {
        private static ImportMapPointCloud window;
        private static EditorPCDownloader pointCloudDownloader;
        private Material pointCloudMaterial;
        private static string mapUUID;
        private bool currentlyDownloading = false;
        private static float progressValue;
        private static PCLDisplayer pclDisplayer;
        private GameObject previousPointCloud;

        [MenuItem("Stardust SDK/Import point cloud")]

        static void Init()
        {
            window = (ImportMapPointCloud)EditorWindow.GetWindow(typeof(ImportMapPointCloud), true, "Import point cloud from server");
            
        }

        void OnGUI()
        {
            //EditorGUILayout.BeginVertical();
            mapUUID = EditorGUILayout.TextField("Map ID", mapUUID);

            if (currentlyDownloading)
            {                
                string info = "Loading point cloud in scene";
                if (progressValue < 0.5f)
                    info = "Downloading point cloud from server";

                EditorUtility.DisplayProgressBar("Loading the point cloud", info, progressValue);
            }
            else
            {

                StardustSDK sdk = GameObject.FindObjectOfType<StardustSDK>();

                if (sdk == null)
                {

                    EditorGUILayout.HelpBox("You need a StardustComponents in scene.", MessageType.Warning);

                }else if (string.IsNullOrEmpty(sdk.ApiKey))
                {
                    EditorGUILayout.HelpBox("You need to setup the API key in the StardustComponents prefab.", MessageType.Error);
                    if (GUILayout.Button("Find the StardustComponent"))
                    {
                        Selection.activeObject = sdk.gameObject;
                    }
                }
                else if (!currentlyDownloading && GUILayout.Button("Import into editor"))
                {
                    if (pointCloudDownloader == null)
                    {
                        pointCloudDownloader = new EditorPCDownloader();
                        pointCloudMaterial=GetPCMaterial();
                    }
                    currentlyDownloading = true;

                    if(previousPointCloud!=null)
                        GameObject.DestroyImmediate(previousPointCloud);

                    pointCloudDownloader.DownloadPointCloud(StardustSDK.Instance.ApiKey, mapUUID);
                    EditorCoroutineUtility.StartCoroutineOwnerless(UpdateProgress());
                    EditorCoroutineUtility.StartCoroutineOwnerless(DisplayPointCloudWhenDone());


                }
            }
        }

        private IEnumerator DisplayPointCloudWhenDone()
        {
            yield return new WaitUntil(() => pointCloudDownloader.DownloadFinished);



            if (pclDisplayer == null)
            {
                pclDisplayer = new PCLDisplayer(pointCloudMaterial);

            }

            if(pointCloudDownloader.Points.Count>0)
                previousPointCloud = pclDisplayer.CreatePointCloudMesh(mapUUID, pointCloudDownloader.Points);

            currentlyDownloading = false;
            EditorUtility.ClearProgressBar();
            Close();

        }

        private IEnumerator UpdateProgress()
        {
            yield return new WaitForFixedUpdate();
            progressValue = pointCloudDownloader.Progress;
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