using com.Neogoma.Stardust.API;
using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Demo for a relocation use case
    /// </summary>
    public class RelocationDemo:MonoBehaviour
    {
        /// <summary>
        /// Text used to show the download picture status
        /// </summary>
        public Text downloadingData;

        /// <summary>
        /// Text used to show the results of the server matching
        /// </summary>
        public Text matchingResult;

        /// <summary>
        /// Dropdown to select the map
        /// </summary>
        public Dropdown mapSelectionDropDown;

        /// <summary>
        /// Name of the selected session
        /// </summary>
        public Text sessionName;

        /// <summary>
        /// Button to locate the user
        /// </summary>
        public Button locateMeButton;

        public UnityEvent showResultsEvent = new UnityEvent();
        
        private SessionController sessionController;
        private MapRelocationManager relocationManager;
        private Dictionary<int, Session> indexToSession = new Dictionary<int, Session>();
        
        private Session selectedSession;

        public void Awake()
        {
            sessionController = SessionController.Instance;            
            mapSelectionDropDown.onValueChanged.AddListener(OnMapSelected);
            sessionController.onAllSessionsRetrieved.AddListener(MapListDownloaded);
            RequireMapList();

            relocationManager = MapRelocationManager.Instance;
            relocationManager.onMapDownloadedSucessfully.AddListener(OnMapDownloaded);
            relocationManager.onMapDownloadStarted.AddListener(OnMapStartDownloading);
            relocationManager.onPositionFound.AddListener(OnPositionMatched);
            relocationManager.onLocationNotFound.AddListener(OnPositionMatchFailed);
            
        }


      
        private void OnMapDownloaded(Session session,GameObject map)
        {
            downloadingData.gameObject.SetActive(false);
            locateMeButton.gameObject.SetActive(true);
        }

        private void OnMapStartDownloading()
        {
            downloadingData.gameObject.SetActive(true);
        }


        private void OnPositionMatched(RelocationResults positionMatched,CoordinateSystem newCoords)
        {
            
            ShowMatchResults("Located sucessfully!", Color.green);
            locateMeButton.gameObject.SetActive(true);

        }

        private void OnPositionMatchFailed()
        {
            ShowMatchResults("Failed to locate", Color.red);
            locateMeButton.gameObject.SetActive(true);
        }


        private void MapListDownloaded(Session[] allSessions)
        {
            indexToSession.Clear();
            List<string> mapListDatas = new List<string>();
            mapListDatas.Add("NONE");

            for (int i = 0; i < allSessions.Length; i++)
            {
                Session currentSession = allSessions[i];
                mapListDatas.Add(currentSession.name);


                indexToSession.Add(i+1, currentSession);

            }

            mapSelectionDropDown.AddOptions(mapListDatas);
            mapSelectionDropDown.gameObject.SetActive(true);
        }

        private void ShowMatchResults(string text, Color color)
        {
            matchingResult.color = color;
            matchingResult.text = text;
            matchingResult.gameObject.SetActive(true);
            StartCoroutine(HideTextCoroutine());
            showResultsEvent.Invoke();
        }

        private void RequireMapList()
        {
            sessionController.GetAllSessionsReady();
        }

        private void OnMapSelected(int val)
        {
            selectedSession = indexToSession[val];

            if (selectedSession != null)
            {
                mapSelectionDropDown.gameObject.SetActive(false);
                relocationManager.GetDataForMap(selectedSession);
                sessionName.text = selectedSession.name;

            }            
        }        

        private IEnumerator HideTextCoroutine()
        {
            yield return new WaitForSeconds(3);
            matchingResult.gameObject.SetActive(false);
        }

    }
}