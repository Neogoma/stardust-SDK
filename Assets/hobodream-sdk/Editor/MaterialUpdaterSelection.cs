#if UNITY_EDITOR
using com.Neogoma.HoboDream.Framework.MaterialEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.Neogoma.HoboDream.EditorTools
{
    /// <summary>
    /// Editor script used to add a menu to every material state editor in order to select which items to update
    /// </summary>
    /// <seealso cref="UnityEditor.Editor" />
    [CustomEditor(typeof(MaterialStateEditor))]
    public class MaterialUpdaterSelection : Editor
    {
        private float currentValue = 0;

        private List<Color> allColors = new List<Color>();

        private List<float> allSliderMin = new List<float>();
        private List<float> allSliderMax = new List<float>();
        private List<float> allSliderValues = new List<float>();

        private List<float> allFloats = new List<float>();

        private List<Vector4> allVectors = new List<Vector4>();

        private List<bool> allChecked = new List<bool>();

        private List<Texture2D> allTextures = new List<Texture2D>();

        public bool toogleGroup = true;

        private List<ShaderPropertyValue> allPropertiesValues = new List<ShaderPropertyValue>();

        private void OnEnable()
        {
            allColors.Clear();
            allSliderMin.Clear();
            allSliderMax.Clear();
            allSliderValues.Clear();
            allTextures.Clear();

            for (int i = 0; i < 100; i++)
            {
                if (i < 10)
                {
                    allColors.Add(Color.white);
                    allVectors.Add(new Vector4());

                    allTextures.Add(new Texture2D(0, 0));
                }
                else if (i < 20)
                {
                    allSliderValues.Add(0);
                    allSliderMax.Add(1);
                    allSliderMin.Add(0);
                }
                else if (i < 30)
                {
                    allFloats.Add(0);
                }
                else
                {
                    allChecked.Add(false);
                }
            }

        }



        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            allPropertiesValues.Clear();

            MaterialStateEditor monobehaviorPropertyEditor = (MaterialStateEditor)target;
            List<ShaderInformations> infos = monobehaviorPropertyEditor.GetAllShaderProperties();



            GUILayout.BeginVertical();

            int colorCounter = 0;
            int sliderCounter = 0;
            int floatCounter = 0;
            int vectorCounter = 0;
            int toogleCounter = 0;
            int textureCounter = 0;

            foreach (ShaderInformations informations in infos)
            {
                GUILayout.BeginHorizontal();

                string label = informations.Description;

                if (string.IsNullOrEmpty(label))
                {
                    label = informations.Name;
                }

                allChecked[toogleCounter] = EditorGUILayout.BeginToggleGroup(label, allChecked[toogleCounter]);

                ShaderPropertyValue value = new ShaderPropertyValue();
                value.valueName = informations.Name;

                switch (informations.Type)
                {

                    case ShaderUtil.ShaderPropertyType.Color:
                        value.type = MaterialStateEditor.ShaderParameterType.Color;
                        break;

                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        value.type = MaterialStateEditor.ShaderParameterType.TexEnv;
                        break;

                    case ShaderUtil.ShaderPropertyType.Vector:
                        value.type = MaterialStateEditor.ShaderParameterType.Vector;
                        break;

                    default:
                        value.type = MaterialStateEditor.ShaderParameterType.Float;
                        break;
                }



                switch (informations.Type)
                {
                    case ShaderUtil.ShaderPropertyType.Color:

                        if (colorCounter >= allColors.Count)
                        {
                            allColors.Add(new Color(1, 1, 0, 1));
                        }

                        allColors[colorCounter] = EditorGUILayout.ColorField("", allColors[colorCounter]);
                        value.colorValue = allColors[colorCounter];

                        colorCounter++;



                        break;

                    case ShaderUtil.ShaderPropertyType.Range:
                        allSliderMax[sliderCounter] = informations.Max;
                        allSliderMin[sliderCounter] = informations.Min;
                        allSliderValues[sliderCounter] = EditorGUILayout.Slider(allSliderValues[sliderCounter], allSliderMin[sliderCounter], allSliderMax[sliderCounter]);
                        value.floatValue = allSliderValues[sliderCounter];

                        sliderCounter++;


                        break;

                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        GUILayout.BeginVertical();
                        var style = new GUIStyle(GUI.skin.label);
                        style.alignment = TextAnchor.UpperCenter;
                        style.fixedWidth = 70;
                        GUILayout.Label(name, style);
                        allTextures[textureCounter] = (Texture2D)EditorGUILayout.ObjectField(allTextures[textureCounter], typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                        value.textureValue = allTextures[textureCounter];
                        textureCounter++;
                        GUILayout.EndVertical();

                        break;

                    case ShaderUtil.ShaderPropertyType.Vector:
                        allVectors[vectorCounter] = EditorGUILayout.Vector4Field(informations.Description, allVectors[vectorCounter]);
                        value.vectorValue = allVectors[vectorCounter];
                        vectorCounter++;
                        break;

                    default:
                        allFloats[floatCounter] = EditorGUILayout.FloatField(allFloats[floatCounter]);
                        value.floatValue = allFloats[floatCounter];
                        floatCounter++;
                        break;
                }

                if (allChecked[toogleCounter])
                {
                    allPropertiesValues.Add(value);
                }

                toogleCounter++;

                EditorGUILayout.EndToggleGroup();
                GUILayout.EndHorizontal();



            }

            if (GUILayout.Button("Create a new material state"))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "Confirm the creation of a new state please", "Fuck yeah!"))
                {
                    monobehaviorPropertyEditor.AddValuesToList(allPropertiesValues);
                }
            }

            GUILayout.EndVertical();
        }

    }


}
#endif