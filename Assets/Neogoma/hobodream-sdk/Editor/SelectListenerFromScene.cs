using com.Neogoma.HoboDream;
using com.Neogoma.HoboDream.Impl;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Neogoma.HoboDream.EditorTools.EditorUtils;

namespace Neogoma.HoboDream.EditorTools
{
    /// <summary>
    /// This window will select all the Listeners in the scene to add it to the interactive Listener
    /// </summary>
    /// <seealso cref="UnityEditor.EditorWindow" />
    public class SelectListenerFromScene : EditorWindow
    {
        private List<ScriptAndAssociatedGameobject<IInteractiveElementListener>> allListeners;

        private AbstractInteractive interactive;

        private Vector2 scrollPos;


        private void OnEnable()
        {
            allListeners = EditorUtils.FindObjectsOfTypeAllWithGameobject<IInteractiveElementListener>();
        }

        private void OnGUI()
        {
            //the list contains all listeners which have been added
            List<GameObjectAndListenerNames> allListenerInInteractive = interactive.allListenersToAdd;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("LISTENER NAME");
            GUILayout.Label("GAMEOBJECT NAME");
            GUILayout.Label("CLICK TO VIEW GAMEOBJECT");
            GUILayout.Label("ADD/REMOVE");

            EditorGUILayout.EndHorizontal();



            foreach (ScriptAndAssociatedGameobject<IInteractiveElementListener> listenerWithGameobject in allListeners)
            {
                EditorGUILayout.BeginHorizontal();

                string[] explodedName = listenerWithGameobject.listener.GetType().ToString().Split('.');

                //listener class name
                GUILayout.Label(explodedName[explodedName.Length-1]);

                //listener gameobject name
                GUILayout.Label(listenerWithGameobject.gameObject.name);

                if (GUILayout.Button("View Gameobject"))
                {
                    EditorGUIUtility.PingObject(listenerWithGameobject.gameObject);
                }

                bool hasBeenFound = false;

                
                //Check if interface has already been added
                foreach (GameObjectAndListenerNames go in allListenerInInteractive)
                {
                    if (go.listenerClassesToAdd.Contains(listenerWithGameobject.listener.GetType().ToString()) && go.gameObject == listenerWithGameobject.gameObject)
                    {
                        hasBeenFound = true;

                        if (GUILayout.Button("Remove listener"))
                        {
                            interactive.allListenersToAdd.Remove(go);
                            EditorUtility.SetDirty(interactive);
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                            break;
                        }
                    }
                }

                //If interface was nod added 
                if (!hasBeenFound)
                {

                    if (GUILayout.Button("Add listener"))
                    {
                        GameObjectAndListenerNames newGo = null;

                        foreach (GameObjectAndListenerNames go in allListenerInInteractive)
                        {
                            if (go.gameObject == listenerWithGameobject.gameObject)
                            {
                                newGo = go;
                                break;
                            }
                        }

                        if (newGo == null)
                        {
                            newGo = new GameObjectAndListenerNames();
                            newGo.gameObject = listenerWithGameobject.gameObject;
                            allListenerInInteractive.Add(newGo);
                        }
                        newGo.listenerClassesToAdd.Add(listenerWithGameobject.listener.GetType().ToString());
                        EditorUtility.SetDirty(interactive);

                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());


                    }

                }


                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

        }

        public void SetInteractive(AbstractInteractive interactive)
        {
            this.interactive = interactive;
        }
    }

}