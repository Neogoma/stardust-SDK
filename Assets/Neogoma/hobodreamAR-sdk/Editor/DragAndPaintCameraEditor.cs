using com.Neogoma.Hobodream.ARFramework.Utils.Common.Impl.AR;
using UnityEditor;

namespace com.Neogoma.HoboDream.AR.EditorTools
{
    [CustomEditor(typeof(DragCameraToCreateObjects))]
    public class DragAndPaintCameraEditor : Editor
    {
        private SerializedProperty materialProperty;

        private SerializedProperty pencilColorProperty;

        private SerializedProperty pencilSizeProperty;

        private SerializedProperty rootProperty;

        private SerializedProperty brushProperty;

        private void OnEnable()
        {
            materialProperty = serializedObject.FindProperty("material");
            pencilColorProperty = serializedObject.FindProperty("pencilColor");
            pencilSizeProperty = serializedObject.FindProperty("pencilSize");
            rootProperty = serializedObject.FindProperty("drawingRoot");
            brushProperty = serializedObject.FindProperty("meshBrush");


        }

        public override void OnInspectorGUI()
        {
            DragCameraToCreateObjects camerapaint = (DragCameraToCreateObjects)target;

            EditorGUILayout.PropertyField(brushProperty);

            bool hasBrush = camerapaint.meshBrush != null;

            if (!hasBrush)
            {
                EditorGUILayout.PropertyField(materialProperty);

            }

            EditorGUILayout.PropertyField(pencilColorProperty);

            EditorGUILayout.PropertyField(pencilSizeProperty);

            EditorGUILayout.PropertyField(rootProperty);

            serializedObject.ApplyModifiedProperties();


        }
    }
}