using com.Neogoma.Stardust.API.Persistence;
using com.Neogoma.Stardust.Graph;
using com.Neogoma.Stardust.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        private GameObject locationInstance;
        private PathFindingManager pathfindingManager;
        private int selectedTargetIndex;
        private Dictionary<int, ITarget> indexToTarget = new Dictionary<int, ITarget>();

        private void Start()
        {
            pathfindingManager = PathFindingManager.Instance;
            pathfindingManager.onNavigationDatasReady.AddListener(PathFindingReady);
            pathfindingManager.onPathCalculated.AddListener(Delta);
            targetSelectionDropDown.onValueChanged.AddListener(OnTargetSelected);
        }

        private void Delta(IGraphNode arg0)
        {
            Debug.Log(arg0.GetCoordnates());
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

                    locationInstance.transform.position = target.GetCoordnates();

                }
            }
            catch (KeyNotFoundException nokey)
            {
                pathfindingManager.ClearPath();
            }
        }

        private void PathFindingReady(List<ITarget> allTargets)
        {
            targetSelectionDropDown.ClearOptions();

            List<string> allTargetNames = new List<string>();
            allTargetNames.Add("No target");
            for (int i = 0; i < allTargets.Count; i++)
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
        }
    }
}
