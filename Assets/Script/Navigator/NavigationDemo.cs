using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using com.Neogoma.Stardust.Graph;
using com.Neogoma.Stardust.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Demo for a navigation use case
    /// </summary>
    public class NavigationDemo:MonoBehaviour
    {

        /// <summary>
        /// Dropdown to select the targets
        /// </summary>
        public Dropdown targetSelectionDropDown;

        /// <summary>
        /// Prefab to display on the navigation place
        /// </summary>
        public GameObject locationPrefab;

        /// <summary>
        /// The target reached hint
        /// </summary>
        public GameObject targetReachedHint;

        public UnityEvent targetReached = new UnityEvent();

        private GameObject locationInstance;
        private PathFindingManager pathfindingManager;
        private int selectedTargetIndex;
        private Dictionary<int, ITarget> indexToTarget = new Dictionary<int, ITarget>();
        private Transform mainCameraTransform;
        public ITarget[] targets;

        private void Start()
        {
            mainCameraTransform = Camera.main.transform;
            pathfindingManager = PathFindingManager.Instance;
            pathfindingManager.onNavigationDatasReady.AddListener(PathFindingReady);            
            targetSelectionDropDown.onValueChanged.AddListener(OnTargetSelected);
            MapRelocationManager.Instance.onPositionFound.AddListener(PositionFound);
        }

        private void PositionFound(RelocationResults arg0, CoordinateSystem arg1)
        {
            targetSelectionDropDown.gameObject.SetActive(targets.Length>0); 
        }

        /// <summary>
        /// Will go to the target selected in the dropdown
        /// </summary>
        public void GoToSelectedTarget()
        {
            try
            {
                ITarget target = indexToTarget[selectedTargetIndex];
                pathfindingManager.ShowPathToTarget(target,1f);

                if (locationPrefab != null)
                {
                    if (locationInstance == null)
                    {
                        locationInstance = GameObject.Instantiate(locationPrefab);
                    }

                    locationInstance.transform.position = target.GetCoordinates();
                    locationInstance.SetActive(true);

                }
                StartCoroutine(ReachTarget());
            }
            catch (KeyNotFoundException nokey)
            {
                pathfindingManager.ClearPath();
            }
        }

        private void PathFindingReady(ITarget[] allTargets)
        {
            targets = allTargets;
            targetSelectionDropDown.ClearOptions();

            List<string> allTargetNames = new List<string>();
            allTargetNames.Add("No target");
            for (int i = 0; i < allTargets.Length; i++)
            {
                string targetName = allTargets[i].GetTargetName();
                allTargetNames.Add(targetName);                
                indexToTarget.Add(i+1, allTargets[i]);
            }
            targetSelectionDropDown.AddOptions(allTargetNames);
            

        }

        private void OnTargetSelected(int val)
        {
            this.selectedTargetIndex = val;
            if (val == 0)
            { 
                pathfindingManager.ClearPath();
                locationInstance.SetActive(false);
            
            }
        }


        IEnumerator ReachTarget()
        {
            yield return new WaitForFixedUpdate();
            if (locationInstance != null)
            {
                Vector3 currentPos = new Vector3(mainCameraTransform.position.x, locationInstance.transform.position.y, mainCameraTransform.position.z);
                
                if (Vector3.Distance(currentPos, locationInstance.transform.position) < 1f)
                {

                    targetReached.Invoke();
                    yield return new WaitForSeconds(3f);
                    locationInstance.gameObject.SetActive(false);
                    targetReachedHint.gameObject.SetActive(false);
                    
                    StopAllCoroutines();

                }
                else
                {
                    StartCoroutine(ReachTarget());
                }
            }


        }

        
    }
}
