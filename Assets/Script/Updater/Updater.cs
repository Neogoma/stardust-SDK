using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.Mapping;
using com.Neogoma.Stardust.Datamodel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Updater
{
    /// <summary>
    /// Demo for update use case
    /// </summary>
    public class Updater: MonoBehaviour
    {
        /// <summary>
        /// Text showing picture taken
        /// </summary>
        public Text pictureTaken;

        /// <summary>
        /// Text showing pictures sucesffully sent
        /// </summary>
        public Text pictureSent;

        /// <summary>
        /// Text showing data limit reached
        /// </summary>
        public Text dataLimitReachedText;

        /// <summary>
        /// Dropdown to select map
        /// </summary>
        public Dropdown mapSelectionDropdown;

        private SessionController sessionController;
        private MapDataUploader dataUploader;
        private Dictionary<string, Session> idToSession = new Dictionary<string, Session>();

        public void Awake()
        {
            sessionController = SessionController.Instance;
            mapSelectionDropdown.onValueChanged.AddListener(delegate { ValueUpdated(); });            
            sessionController.onAllSessionsRetrieved.AddListener(AllSessionsRetrieved);

            GetDatas();

            dataUploader = MapDataUploader.Instance;
            dataUploader.onQueueUpdated.AddListener(OnDataCaptured);
            dataUploader.onDataSentSucessfully.AddListener(OnDataSentSuccess);
            dataUploader.onDatalimitReached.AddListener(OnDataLimitReached);
        }


        private void AllSessionsRetrieved(Session[] allSessions)
        {
            //InitializeCameraProvider();
            mapSelectionDropdown.ClearOptions();
            List<string> mapList = new List<string>();
            mapList.Add("None");
            for (int i = 0; i < allSessions.Length; i++)
            {

                idToSession.Add(allSessions[i].name, allSessions[i]);
                mapList.Add(allSessions[i].name);
            }

            mapSelectionDropdown.AddOptions(mapList);

            
        }


        private void GetDatas() {
            sessionController.GetAllSessionsReady();
        }

        private void ValueUpdated()
        {
            string selection = mapSelectionDropdown.options[mapSelectionDropdown.value].text;

            if (selection.CompareTo("None") == 0)
            {
                return;
            }
            Session selectedSession = idToSession[selection];
            dataUploader.SetSession(selectedSession);
            pictureSent.text = selectedSession.PicturesNumber.ToString();
            pictureTaken.text = "0";
            dataUploader.SetUpdateBatch();            
        }
        private void OnDataCaptured(int count)
        {
            pictureTaken.text = count.ToString();
        }

        private void OnDataSentSuccess(int count)
        {
            pictureSent.text = count.ToString();
            
        }

        private void OnDataLimitReached()
        {
            dataLimitReachedText.gameObject.SetActive(true);
        }
    }
}