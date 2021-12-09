using com.Neogoma.Stardust.API.Persistence;
using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using System;
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
        /// Root for user created content
        /// </summary>
        public Transform userCreatedParent;

        /// <summary>
        /// objects dropdown
        /// </summary>
        public Dropdown prefabDropdown;

        /// <summary>
        /// distance in front of camera
        /// </summary>
        public float forwardCamera = 1;

        /// <summary>
        /// current selected bundle
        /// </summary>
        private Bundle selectedBundle;

        private Session currentSession;

        private Dictionary<int, Bundle> objectDictionary = new Dictionary<int, Bundle>();
        private Transform cam;
        private Transform currentParent;        

        // Start is called before the first frame update
        void Start()
        {

            currentParent = userCreatedParent;
            objectController = ObjectController.Instance;
            
            cam = Camera.main.transform;
            objectController.onObjectListDownloaded.AddListener(InitializeObjects);

            MapRelocationManager.Instance.onMapDownloadedSucessfully.AddListener(MapDownloadedSucessfully);            
            RequestAllObjects();
        }

        /// <summary>
        /// Setup the session
        /// </summary>
        /// <param name="session"></param>
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


        private PersistentObject lastPersistent;


        /// <summary>
        /// Creates and instanciates the selected object 
        /// </summary>
        public void CreateSelectedObject()
        {   

            Vector3 position = cam.position + cam.forward*forwardCamera;
            Quaternion rot = Quaternion.Euler(0,cam.rotation.eulerAngles.y,0);
            
            
            lastPersistent= objectController.CreateViewAndSaveModel(position, rot, Vector3.one, Guid.NewGuid().ToString(), currentSession, selectedBundle, currentParent, ObjectController.CreationSpace.World);           
            
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {

               string uuid= Guid.NewGuid().ToString();

                Debug.Log(uuid);

                Debug.Log(lastPersistent.uuid);

                GameObject view = objectController.GetViewForModel(lastPersistent);

                Debug.Log(view);

                lastPersistent.metadata = uuid;
                objectController.SaveModel(lastPersistent);
            }
        }

        private void MapDownloadedSucessfully(Session session,GameObject map)
        {
            currentParent = map.transform;
        }
    }
}
