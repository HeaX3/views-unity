using System;
using UnityEditor;

namespace Views.UI.Editor
{
    [CustomEditor(typeof(CircularImage))]
    public class CircularImageEditor : UnityEditor.UI.ImageEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); //Draw inspector UI of ImageEditor

            var image = (CircularImage) target;
            
            Undo.RecordObject(image, image.gameObject.name);
            
            var previousStartAngle = image.StartAngle;
            var previousLength = image.Length;
            var previousCircleWidth = image.CircleWidth;
            var previousBorderWidth = image.BorderWidth;
            var previousMergeTips = image.MergeTips;
            var previousFillClockwise = image.FillClockwise;
            var precision = image.Precision;

            image.StartAngle = EditorGUILayout.FloatField("Start", previousStartAngle);
            image.Length = EditorGUILayout.FloatField("Length", previousLength);
            image.CircleWidth = EditorGUILayout.Slider("Circle Width", previousCircleWidth, 0, 1);
            image.BorderWidth = EditorGUILayout.Slider("Border Width", previousBorderWidth, 0, 0.5f);
            image.MergeTips = EditorGUILayout.Toggle("Merge Tips", previousMergeTips);
            image.FillClockwise = EditorGUILayout.Toggle("Fill Clockwise", previousFillClockwise);
            image.Precision = EditorGUILayout.IntSlider("Precision", precision, 2, 256);

            if (Math.Abs(image.StartAngle - previousStartAngle) > 0.001f ||
                Math.Abs(image.Length - previousLength) > 0.001f ||
                Math.Abs(image.CircleWidth - previousCircleWidth) > 0.001f ||
                Math.Abs(image.BorderWidth - previousBorderWidth) > 0.001f ||
                image.MergeTips != previousMergeTips ||
                image.FillClockwise != previousFillClockwise ||
                image.Precision != precision)
            {
                image.SetAllDirty();
            
                EditorUtility.SetDirty(image);
            }
        }
    }
}