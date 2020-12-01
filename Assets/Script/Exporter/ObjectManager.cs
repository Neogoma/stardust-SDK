using com.Neogoma.Stardust.API.Persistence;
using com.Neogoma.Stardust.Datamodel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Mapper
{
    /// <summary>
    /// example for creating object
    /// </summary>
    public class ObjectManager : MonoBehaviour
    {

        /// <summary>
        /// objectController
        /// </summary>
        private ObjectController objectController;

        /// <summary>
        /// session id text
        /// </summary>
        public Text sessionText;

        /// <summary>
        /// objects root
        /// </summary>
        public Transform parent;

        /// <summary>
        /// objects dropdown
        /// </summary>
        public Dropdown prefabDropdown;

        /// <summary>
        /// current selected bundle
        /// </summary>
        private Bundle selectedBundle;

        private Session currentSession;

        private Dictionary<int, Bundle> objectDictionary = new Dictionary<int, Bundle>();
        private Transform cam;
        
        // Start is called before the first frame update
        void Start()
        {
            objectController = ObjectController.Instance;
            
            cam = Camera.main.transform;
            objectController.objectListDownloaded.AddListener(InitializeObjects);

            
            RequestAllObjects();
        }

        public void SetupSession(Session session) {
            currentSession = session;
        }

        private void RequestAllObjects()
        {
            objectController.RequestAllObjects();
        }

        private void InitializeObjects()
        {
            List<Bundle> objectList = objectController.GetAllAvailableObjects();
            List<string> options = new List<string>();
            for (int i = 0; i < objectList.Count; i++)
            {

                options.Add(objectList[i].dlc_name);
                objectDictionary[i] = objectList[i];
            }

            if(objectDictionary.Count>0)
                selectedBundle = objectDictionary[0];

            prefabDropdown.AddOptions(options);
            prefabDropdown.onValueChanged.AddListener(delegate
            {
                ValueUpdated();
            });
        }

        private void ValueUpdated()
        {

            selectedBundle = objectDictionary[prefabDropdown.value];
        }

        public void CreateSelectedObject()
        {
            Vector3 pos = cam.position + cam.forward;
            Quaternion rot = Quaternion.LookRotation(pos - cam.position, Vector3.up);
            var local = rot.eulerAngles;

            objectController.CreateAndSaveObject(pos, Quaternion.Euler(0, local.y, 0), Vector3.one * 0.1f,currentSession , selectedBundle, parent);
        }
    }
}
