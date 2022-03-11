using com.Neogoma.Stardust.API.Mapping;
using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using Neogoma.Stardust.Demo.Mapper;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Demo to allow updating datas on another position than 0,0,0
    /// </summary>
    public class UpdateAfterRelocateDemo : MonoBehaviour
    {
        #region Data upload panel
       

        //Text to show current map picture count
        public Text mapPicturesCount;

        //Text to show current picture taken count
        public Text pictureTakenCount;

        #endregion

        /// <summary>
        /// Object list dropdown
        /// </summary>
        public Dropdown objectList;

        public Image dataUploadProgress;

        public UnityEvent positionFound = new UnityEvent();
        
        private int pictureTaken=0;
        
        private ObjectManager objectManager;

        public void Awake()
        {
            //Setup listeners for relocation
            MapRelocationManager.Instance.onMapDownloadedSucessfully.AddListener(MapDownloaded);
            MapRelocationManager.Instance.onPositionFound.AddListener(PositionFound);


            //Setup listeners for data upload
            MapDataUploader.Instance.onDataSentSucessfully.AddListener(PictureUploadSucceed);
            MapDataUploader.Instance.onQueueUpdated.AddListener(PictureTaken);
            
            MapDataUploader.Instance.onRequestProgress.AddListener(RequestProgress);

            objectManager = GetComponent<ObjectManager>();
        }

        private void RequestProgress(float arg0)
        {
            dataUploadProgress.fillAmount = arg0;
            
        }

        private void PositionFound(RelocationResults positionMatched,CoordinateSystem newCoords)
        {
           
            MapDataUploader.Instance.UpdateCoordinateSystem(newCoords);
           
            objectList.gameObject.SetActive(true);
            positionFound.Invoke();
        }


        private void MapDownloaded(Session session,GameObject map)
        {
            MapDataUploader.Instance.SetSession(session);
            InitializeSessionData(session);            
            objectManager.SetupSession(session);
        }

        private void InitializeSessionData(Session session)
        {
            mapPicturesCount.text = session.PicturesNumber.ToString();

        }

        private void PictureUploadSucceed(int count)
        {
            mapPicturesCount.text = count.ToString();
        }

        private void PictureTaken(int count)
        {

            pictureTakenCount.text = count.ToString();

            if (count == 0)
            {
                dataUploadProgress.fillAmount = 0;
            }
        }
            
    }
}