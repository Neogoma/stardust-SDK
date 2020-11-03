using com.Neogoma.HoboDream;
using com.Neogoma.HoboDream.UI.Impl.Buttons;
using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.PersistenceObject;
using com.Neogoma.Stardust.Datamodel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Mapper
{
    /// <summary>
    /// example for creating object
    /// </summary>
    public class ObjectManager : MonoBehaviour, IInteractiveElementListener
    {
        /// <summary>
        /// create object button
        /// </summary>
        public SimpleCommonButton createObjBtn;

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

        private Dictionary<int, Bundle> objectDictionary = new Dictionary<int, Bundle>();
        private Transform cam;
        private InteractiveEventAction[] actions = { InteractiveEventAction.CLICK};
        // Start is called before the first frame update
        void Start()
        {
            objectController = ObjectController.Instance;
            createObjBtn.AddInteractiveListener(this);
            cam = Camera.main.transform;
            objectController.objectListDownloaded.AddListener(InitializeObjects);

            RequestAllObjects();
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
        public InteractiveEventAction[] GetSupportedEvents()
        {
            return actions;
        }

        public void HandleEvent(IInteractionEvent e)
        {
            if (e.GetEventType() == InteractiveEventAction.CLICK)
            {
                Vector3 pos = cam.position + cam.forward;
                Quaternion rot = Quaternion.LookRotation(pos-cam.position,Vector3.up);
                var local = rot.eulerAngles;

                PersistenceModel persistenceModel = new PersistenceModel(pos,Quaternion.Euler(0,local.y,0), sessionText.text, Vector3.one*0.1f,  selectedBundle);


                //create obj
                objectController.CreateInstance(persistenceModel,parent);
                objectController.SaveObjectInstance(persistenceModel);
            }
        }

       
        

       
    }
}
