using com.Neogoma.HoboDream.Util.RouteVisualization;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.HoboDream.Util.RouteVisualization
{
    /// <summary>
    /// used to draw Bezier
    /// </summary>
    public static class BezierUtils
	{
		private static Color SPLINE_DETAILED_COLOR = new Color( 0.8f, 0.6f, 0.8f );

		
		private static Color NORMAL_END_POINT_COLOR = Color.white;
		private static Color SELECTED_END_POINT_COLOR = Color.yellow;
		private static Color SELECTED_END_POINT_CONNECTED_POINTS_COLOR = Color.green;

		private static Color AUTO_CONSTRUCT_SPLINE_BUTTON_COLOR = new Color( 0.65f, 1f, 0.65f );

		private const float SPLINE_THICKNESS = 8f;
		private const float END_POINT_SIZE = 0.075f;
		private const float END_POINT_CONTROL_POINTS_SIZE = 0.05f;

		private const string PRECEDING_CONTROL_POINT_LABEL = "  <--";
		private const string FOLLOWING_CONTROL_POINT_LABEL = "  -->";

		[MenuItem( "GameObject/Bezier Spline", priority = 35 )]
		static void NewSpline()
		{
			GameObject spline = new GameObject( "BezierSpline", typeof( BezierSpline ) );
			Undo.RegisterCreatedObjectUndo( spline, "Create Spline" );
			Selection.activeTransform = spline.transform;
		}




        /// <summary>
        /// Draws the spline detailed.
        /// </summary>
        /// <param name="spline">The spline.</param>
        public static void DrawSplineDetailed( BezierSpline spline )
		{
			if( spline.Count < 2 )
				return;

			BezierPoint endPoint0 = null, endPoint1 = null;
			for( int i = 0; i < spline.Count - 1; i++ )
			{
				endPoint0 = spline[i];
				endPoint1 = spline[i + 1];

				DrawBezier( endPoint0, endPoint1 );
			}

			if( spline.loop && endPoint1 != null )
				DrawBezier( endPoint1, spline[0] );
            
		}


        /// <summary>
        /// Draws the bezier point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="pointIndex">Index of the point.</param>
        /// <param name="isSelected">if set to <c>true</c> [is selected].</param>
        public static void DrawBezierPoint( BezierPoint point, int pointIndex, bool isSelected )
		{
			Color c = Handles.color;

			if( isSelected )
			{
				Handles.color = SELECTED_END_POINT_COLOR;
				Handles.DotHandleCap( 0, point.Position, Quaternion.identity, HandleUtility.GetHandleSize( point.Position ) * END_POINT_SIZE * 1.5f, EventType.Repaint );
			}
			else
			{
				Handles.color = NORMAL_END_POINT_COLOR;

				if( Event.current.alt || Event.current.button > 0 )
					Handles.DotHandleCap( 0, point.Position, Quaternion.identity, HandleUtility.GetHandleSize( point.Position ) * END_POINT_SIZE, EventType.Repaint );
				else if( Handles.Button( point.Position, Quaternion.identity, HandleUtility.GetHandleSize( point.Position ) * END_POINT_SIZE, END_POINT_SIZE, Handles.DotHandleCap ) )
					Selection.activeTransform = point.transform;
			}

			Handles.color = c;

			Handles.DrawLine( point.Position, point.PrecedingControlPointPosition );
			Handles.DrawLine( point.Position, point.FollowingControlPointPosition );

			if( isSelected )
				Handles.color = SELECTED_END_POINT_CONNECTED_POINTS_COLOR;
			else
				Handles.color = NORMAL_END_POINT_COLOR;

			Handles.RectangleHandleCap( 0, point.PrecedingControlPointPosition, SceneView.lastActiveSceneView.rotation, HandleUtility.GetHandleSize( point.PrecedingControlPointPosition ) * END_POINT_CONTROL_POINTS_SIZE, EventType.Repaint );
			Handles.RectangleHandleCap( 0, point.FollowingControlPointPosition, SceneView.lastActiveSceneView.rotation, HandleUtility.GetHandleSize( point.FollowingControlPointPosition ) * END_POINT_CONTROL_POINTS_SIZE, EventType.Repaint );

			Handles.color = c;

			Handles.Label( point.Position, "Point" + pointIndex );
			Handles.Label( point.PrecedingControlPointPosition, PRECEDING_CONTROL_POINT_LABEL );
			Handles.Label( point.FollowingControlPointPosition, FOLLOWING_CONTROL_POINT_LABEL );
		}

		private static void DrawBezier( BezierPoint endPoint0, BezierPoint endPoint1 )
		{
			Handles.DrawBezier( endPoint0.Position, endPoint1.Position,
								endPoint0.FollowingControlPointPosition,
								endPoint1.PrecedingControlPointPosition,
								SPLINE_DETAILED_COLOR, null, SPLINE_THICKNESS );
		}
	}
}