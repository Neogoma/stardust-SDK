using com.Neogoma.HoboDream;
using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.Mapping;
using com.Neogoma.Stardust.Datamodel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Mapper
{
    /// <summary>
    /// Class used to export data to the pointcloud API
    /// </summary>
    public class Exporter : AbstractDataUploader
    {
        /// <summary>
        /// Text element to show how many pictures were taken
        /// </summary>
        public Text pictureTakenText;

        /// <summary>
        /// Text element to show how many pictures were sucessfully uploaded
        /// </summary>
        public Text pictureSentText;

        /// <summary>
        /// Data limit reached text
        /// </summary>
        public Text dataLimitReached;

        /// <summary>
        /// Session controller to manage the different sessions
        /// </summary>
        private SessionController sessionController;

        /// <summary>
        /// Event triggered when session is initialized
        /// </summary>
        public UnityEvent sessionInitialized = new UnityEvent();

        /// <summary>
        /// Showing the session ID
        /// </summary>
        public Text idSession;

        protected override void DoOnAwake()
        {
            sessionController = SessionController.Instance;
            sessionController.onSessionCreationSucess.AddListener(SessionCreated);
            CreateSession();

        }

        private void CreateSession()
        {
            sessionController.CreateMappingSession();
        }

        private void SessionCreated(Session session)
        {
            idSession.text = session.name;
            sessionInitialized.Invoke();
            SetSession(session);

        }

        protected override void HandleRequest(string jsonResult, string key)
        {
        }


    

        protected override void HandleRequestFailed(string jsonResult, string key)
        {
          
        }


        protected override void OnDataCaptured()
        {
            pictureTakenText.text = DataCapturedCount.ToString();
        }

        protected override void OnDataSentSuccess()
        {
            pictureSentText.text = DataSentCount.ToString();
        }

        protected override void OnDataLimitReached()
        {
            dataLimitReached.gameObject.SetActive(true);
            
        }

        protected override void HandleSubEvent(IInteractionEvent e)
        {
        }

        protected override void OnDataSentFailure()
        {   
        }

        protected override InteractiveEventAction[] GetSupportedEventsForSubclass()
        {
            return null;
        }
    }
}