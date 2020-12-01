using com.Neogoma.HoboDream.Impl;
using UnityEditor;
using UnityEngine;

namespace Neogoma.HoboDream.EditorTools
{
    [CustomEditor(typeof(AbstractInteractive),true)]
    public class RegisterToInteractive : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Add Listener from scene"))
            {
                SelectListenerFromScene window = (SelectListenerFromScene)EditorWindow.GetWindow(typeof(SelectListenerFromScene),true, "Interactive listeners management");
                AbstractInteractive monobehaviorPropertyEditor = (AbstractInteractive)target;
                window.SetInteractive(monobehaviorPropertyEditor);
                window.Show();
            }
            
            base.OnInspectorGUI();
        }
    }
}