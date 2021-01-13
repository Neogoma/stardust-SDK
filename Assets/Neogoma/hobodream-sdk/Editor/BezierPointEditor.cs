using com.Neogoma.HoboDream.Util.RouteVisualization;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.HoboDream.Util.RouteVisualization
{
    /// <summary>
    /// used to show the bezier function when add a point
    /// </summary>
    /// <seealso cref="UnityEditor.Editor" />
    [CustomEditor( typeof( BezierPoint ) )]
	public class BezierPointEditor : Editor
	{
		private BezierSpline spline;
		private BezierPoint point;
        private Quaternion precedingPointRotation = Quaternion.identity;
        private Quaternion followingPointRotation = Quaternion.identity;

        private bool controlPointRotationsInitialized = false;

	

		void OnEnable()
		{
			point = target as BezierPoint;
			spline = point.GetComponentInParent<BezierSpline>();

			if( spline != null && !spline.Equals( null ) )
				spline.Refresh();

			Undo.undoRedoPerformed -= OnUndoRedo;
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedo;
		}

		void OnSceneGUI()
		{
			if( spline != null && !spline.Equals( null ) )
			{


				BezierUtils.DrawSplineDetailed( spline );
				for( int i = 0; i < spline.Count; i++ )
				{
					BezierUtils.DrawBezierPoint( spline[i], i + 1, spline[i] == point );
				}
			}
			else
				BezierUtils.DrawBezierPoint( point, 0, true );

            // Draw translate handles for control points
            if (Event.current.alt)
                return;

            if (Tools.current != Tool.Move)
            {
                controlPointRotationsInitialized = false;
                return;
            }

            if (!controlPointRotationsInitialized)
            {
                precedingPointRotation = Quaternion.LookRotation(point.PrecedingControlPointPosition - point.Position);
                followingPointRotation = Quaternion.LookRotation(point.FollowingControlPointPosition - point.Position);

                controlPointRotationsInitialized = true;
            }

            EditorGUI.BeginChangeCheck();
            Vector3 position = Handles.PositionHandle(point.PrecedingControlPointPosition, Tools.pivotRotation == PivotRotation.Local ? precedingPointRotation : Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(point, "Move Control Point");
                point.PrecedingControlPointPosition = position;
            }

            EditorGUI.BeginChangeCheck();
            position = Handles.PositionHandle(point.FollowingControlPointPosition, Tools.pivotRotation == PivotRotation.Local ? followingPointRotation : Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(point, "Move Control Point");
                point.FollowingControlPointPosition = position;
            }

        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            BezierPoint.HandleMode handleMode = (BezierPoint.HandleMode)EditorGUILayout.EnumPopup("Handle Mode", point.HandleModeType);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(point, "Change Point Handle Mode");
                point.HandleModeType = handleMode;

                SceneView.RepaintAll();
            }

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            Vector3 position = EditorGUILayout.Vector3Field("Preceding Control Point Local Position", point.PrecedingControlPointLocalPosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(point, "Change Point Position");
                point.PrecedingControlPointLocalPosition = position;

                SceneView.RepaintAll();
            }

            EditorGUI.BeginChangeCheck();
            position = EditorGUILayout.Vector3Field("Following Control Point Local Position", point.FollowingControlPointLocalPosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(point, "Change Point Position");
                point.FollowingControlPointLocalPosition = position;

                SceneView.RepaintAll();
            }

           
        }


        private void OnUndoRedo()
		{
			controlPointRotationsInitialized = false;

			if( spline != null && !spline.Equals( null ) )
				spline.Refresh();

			Repaint();
		}

		
	}
}