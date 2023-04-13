using com.Neogoma.Stardust.API.Mapping;
using UnityEngine;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Mapper
{
    /// <summary>
    /// manage the navigation target creation
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class TargetCreationManager : MonoBehaviour
    {

        /// <summary>
        /// The name field
        /// </summary>
        public InputField nameField;

        /// <summary>
        /// The create target BTN
        /// </summary>
        public Button createTargetBtn;


        /// <summary>
        /// The target prefab
        /// </summary>
        public GameObject targetPrefab;

        /// <summary>
        /// Root for user created content
        /// </summary>
        public Transform userCreatedParent;

        

        /// <summary>
        /// distance in front of camera
        /// </summary>
        private const float forwardCamera = 0.5f;

        private MapDataUploader mapDataUploader;
        private Transform cam;


        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main.transform;
            mapDataUploader = MapDataUploader.Instance;
        
            
            createTargetBtn.onClick.AddListener(CreateTarget);
           
            
        }

        private void CreateTarget()
        {
            
            string name = nameField.text;
            if (!string.IsNullOrEmpty(name.TrimStart()))
            {
                GameObject target = Instantiate(targetPrefab);

                Vector3 pos = cam.position + cam.forward*forwardCamera;
                Quaternion rot = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
                target.transform.position = pos;
                target.transform.rotation = rot;
                target.transform.localScale = Vector3.one;
                target.transform.SetParent(userCreatedParent);
                
                mapDataUploader.SaveTarget(mapDataUploader.CoordinateSystem.ConvertFromUnityToMapCoordnate(pos), name);
            }


            nameField.text = string.Empty;


        }

     

      



    }
}
