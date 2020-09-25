using com.Neogoma.HoboDream;
using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.Mapping;
using com.Neogoma.Stardust.Datamodel;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Updater
{
    public class Updater : AbstractDataUploader
    {
        public Text pictureTaken;

        public Text pictureSent;

        public Text dataLimitReachedText;



        public Dropdown mapSelectionDropdown;


        public UnityEvent mapSelected = new UnityEvent();


        private SessionController sessionController;

        private Dictionary<string, Session> idToSession = new Dictionary<string, Session>();

        protected override void DoOnAwake()
        {
            sessionController = SessionController.Instance;
            mapSelectionDropdown.onValueChanged.AddListener(delegate { ValueUpdated(); });            
            sessionController.onAllSessionsRetrieved.AddListener(AllSessionsRetrieved);

            GetDatas();
        }


        protected override void HandleRequestFailed(string jsonResult, string key)
        {

        }

        protected override void HandleSubEvent(IInteractionEvent e)
        {

        }


        private void AllSessionsRetrieved(Session[] allSessions)
        {
            //InitializeCameraProvider();
            mapSelectionDropdown.ClearOptions();
            List<string> mapList = new List<string>();

            for (int i = 0; i < allSessions.Length; i++)
            {

                idToSession.Add(allSessions[i].id, allSessions[i]);
                mapList.Add(allSessions[i].id);
            }

            mapSelectionDropdown.AddOptions(mapList);

            if(allSessions.Length>0)
             ValueUpdated();
        }


        private void GetDatas() {
            sessionController.GetAllSessionsReady();
        }

        private void ValueUpdated()
        {
            string selection = mapSelectionDropdown.options[mapSelectionDropdown.value].text;
            Session selectedSession = idToSession[selection];
            SetSession(selectedSession);
            pictureSent.text = selectedSession.PicturesNumber.ToString();
            pictureTaken.text = selectedSession.PicturesNumber.ToString();
            mapSelected.Invoke();
        }
        protected override void OnDataCaptured()
        {
            pictureTaken.text = DataCapturedCount.ToString();
        }

        protected override void OnDataSentSuccess()
        {
            pictureSent.text = DataSentCount.ToString();
        }

        protected override void HandleRequest(string jsonResult, string key)
        {
//nothing to do

        }

        protected override void OnDataLimitReached()
        {
            dataLimitReachedText.gameObject.SetActive(true);
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