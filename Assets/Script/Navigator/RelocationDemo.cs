using com.Neogoma.HoboDream;
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
    public class RelocationDemo : AbstractMapManager
    {


        public Text downloadingData;

        public Text matchingResult;

        public Dropdown mapList;

        public Button locateMeButton;

        public UnityEvent showResultsEvent = new UnityEvent();

        private SessionController sessionController;


        protected override void DoOnAwake()
        {
            sessionController = SessionController.Instance;
            LoginController.Instance.onLoginSucess.AddListener(RequireMapList);
            mapList.onValueChanged.AddListener(OnMapSelected);
            sessionController.onAllSessionsRetrieved.AddListener(MapListDownloaded);
        }


        protected override InteractiveEventAction[] GetSupportedEventsForSubclass()
        {
            return null;
        }

        protected override void OnMapDownloaded(GameObject map)
        {
            downloadingData.gameObject.SetActive(false);
            locateMeButton.gameObject.SetActive(true);
        }

        protected override void OnMapStartDownloading()
        {
            downloadingData.gameObject.SetActive(true);
        }

        protected override void OnPositionMatched(MatchingPosition positionMatched)
        {

            ShowMatchResults("Located sucessfully!", Color.green);
        }

        protected override void OnPositionMatchFailed()
        {
            ShowMatchResults("Failed to locate", Color.red);
        }

        protected override void OnRequestFailed(string jsonResult, string key)
        {
        }

        protected override void OnRequestSucess(string jsonResult, string key)
        {
        }



        private void MapListDownloaded(Session[] allSessions)
        {

            List<string> mapListDatas = new List<string>();
            mapListDatas.Add("NONE");
            for (int i = 0; i < allSessions.Length; i++)
            {

                mapListDatas.Add(allSessions[i].id);

            }

            mapList.AddOptions(mapListDatas);
            mapList.gameObject.SetActive(true);
        }

        private void ShowMatchResults(string text, Color color)
        {
            matchingResult.color = color;
            matchingResult.text = text;
            matchingResult.gameObject.SetActive(true);
            StartCoroutine(TextDissapearCoroutine());
            showResultsEvent.Invoke();
        }

        private void RequireMapList()
        {
            sessionController.GetAllSessionsReady();
        }

        private void OnMapSelected(int val)
        {

            mapList.interactable = false;
            GetDataForMap(int.Parse(mapList.options[val].text));


        }

        private IEnumerator TextDissapearCoroutine()
        {
            yield return new WaitForSeconds(3);
            matchingResult.gameObject.SetActive(false);
        }

        protected override void OnHandleEvent(IInteractionEvent eve)
        {

        }
    }
}