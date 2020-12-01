using com.Neogoma.HoboDream;
using com.Neogoma.HoboDream.Utils.Reset;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static com.Neogoma.HoboDream.Framework.MaterialEditor.MaterialStateEditor;

namespace com.Neogoma.HoboDream.Framework.MaterialEditor
{
    /// <summary>
    /// Editor to manage different states for the material
    /// 编辑器管理材料的不同状态
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MaterialStateEditor : MonoBehaviour, IResetObjectListener
    {
        public List<ShaderPropertiesList> states = new List<ShaderPropertiesList>();

        private const string NORMAL_MAP = "_NORMALMAP";
        private const string METALLIC_GLOSS = "_METALLICGLOSSMAP";

        private InteractiveEventAction[] allaction = { InteractiveEventAction.RESET };

        private List<ShaderPropertyValue> originalShaderPropertiesValues = new List<ShaderPropertyValue>();
        private Material mat;

        public enum ShaderParameterType
        {
            Color, TexEnv, Vector, Float
        }

        private void Start()
        {
            mat = GetComponent<Renderer>().material;
            SetUpInit();
        }

        public void SetStateToPlay(int stateIndex)
        {
            mat.EnableKeyword(NORMAL_MAP);
            mat.EnableKeyword(METALLIC_GLOSS);
            try
            {
                List<ShaderPropertyValue> allValues = states[stateIndex].values;
                for (int i = 0; i < allValues.Count; i++)
                {
                    ShaderPropertyValue value = allValues[i];
                    switch (value.type)
                    {
                        case ShaderParameterType.Color:
                            mat.SetColor(value.valueName, value.colorValue);
                            break;

                        case ShaderParameterType.TexEnv:
                            mat.SetTexture(value.valueName, value.textureValue);
                            break;

                        case ShaderParameterType.Vector:
                            mat.SetVector(value.valueName, value.vectorValue);
                            break;

                        default:
                            mat.SetFloat(value.valueName, value.floatValue);
                            break;

                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning("Trying to acess unreachable state index");
                Debug.LogWarning(e.StackTrace);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogWarning("Trying to acess unreachable state index");
                Debug.LogWarning(e.StackTrace);
            }
        }

        public void SetUpInit()
        {
            for (int i = 0; i < states.Count; ++i)
            {

                ShaderPropertiesList currentState = states[i];
                List<ShaderPropertyValue> values = currentState.values;

                for (int j = 0; j < values.Count; j++)
                {
                    ShaderPropertyValue currentValue = values[j];


                    if (!originalShaderPropertiesValues.Contains(currentValue))
                    {
                        ShaderPropertyValue newValue = new ShaderPropertyValue();
                        newValue.valueName = currentValue.valueName;
                        newValue.type = currentValue.type;
                        switch (currentValue.type)
                        {
                            case ShaderParameterType.Color:
                                newValue.colorValue = mat.GetColor(currentValue.valueName);
                                break;

                            case ShaderParameterType.TexEnv:
                                newValue.textureValue = mat.GetTexture(currentValue.valueName) as Texture2D;
                                break;

                            case ShaderParameterType.Vector:
                                newValue.vectorValue = mat.GetVector(currentValue.valueName);
                                break;

                            default:
                                newValue.floatValue = mat.GetFloat(currentValue.valueName);
                                break;

                        }

                        originalShaderPropertiesValues.Add(newValue);
                    }
                }


            }
        }

        public void ResetInteractive()
        {
            mat.EnableKeyword(NORMAL_MAP);
            mat.EnableKeyword(METALLIC_GLOSS);

            for (int i = 0; i < originalShaderPropertiesValues.Count; i++)
            {
                ShaderPropertyValue value = originalShaderPropertiesValues[i];
                switch (value.type)
                {
                    case ShaderParameterType.Color:
                        mat.SetColor(value.valueName, value.colorValue);
                        break;

                    case ShaderParameterType.TexEnv:
                        mat.SetTexture(value.valueName, value.textureValue);
                        break;

                    case ShaderParameterType.Vector:
                        mat.SetVector(value.valueName, value.vectorValue);
                        break;

                    default:
                        mat.SetFloat(value.valueName, value.floatValue);
                        break;

                }
            }
        }

        public void HandleEvent(IInteractionEvent e)
        {
            ResetInteractive();
        }

        public InteractiveEventAction[] GetSupportedEvents()
        {
            return allaction;
        }


#if UNITY_EDITOR

        public List<ShaderInformations> GetAllShaderProperties()
        {
            Shader shader = GetComponent<Renderer>().sharedMaterial.shader;

            List<ShaderInformations> allInfos = new List<ShaderInformations>();


            int propertyCount = ShaderUtil.GetPropertyCount(shader);

            for (int i = 0; i < propertyCount; i++)
            {

                string description = ShaderUtil.GetPropertyDescription(shader, i);

                string name = ShaderUtil.GetPropertyName(shader, i);

                ShaderUtil.ShaderPropertyType type = ShaderUtil.GetPropertyType(shader, i);

                ShaderInformations shaderInfo = new ShaderInformations(name, description, type);


                if (type == ShaderUtil.ShaderPropertyType.Range)
                {
                    shaderInfo.SetRange(ShaderUtil.GetRangeLimits(shader, i, 0), ShaderUtil.GetRangeLimits(shader, i, 1), ShaderUtil.GetRangeLimits(shader, i, 2));
                }

                allInfos.Add(shaderInfo);
            }

            return allInfos;
        }

        public void AddValuesToList(List<ShaderPropertyValue> allValues)
        {
            if (allValues.Count == 0)
            {
                return;
            }

            ShaderPropertiesList newList = new ShaderPropertiesList();
            for (int i = 0; i < allValues.Count; i++)
            {
                newList.values.Add(new ShaderPropertyValue(allValues[i]));
            }
            states.Add(newList);
        }

#endif


    }


#if UNITY_EDITOR

    public class ShaderInformations
    {

        private string propertyName;
        public string Name
        {
            get
            {
                return propertyName;
            }
        }

        private string propertyDescription;
        public string Description
        {
            get
            {
                return propertyDescription;
            }
        }

        private ShaderUtil.ShaderPropertyType propertyType;
        public ShaderUtil.ShaderPropertyType Type
        {
            get
            {
                return propertyType;
            }
        }

        private float min;
        public float Min
        {
            get
            {
                return min;
            }
        }

        private float max;
        public float Max
        {
            get
            {
                return max;
            }
        }

        private float defaultValue;
        public float DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        public ShaderInformations(string propertyName, string propertyDescription, ShaderUtil.ShaderPropertyType propertyType)
        {
            this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.propertyDescription = propertyDescription ?? throw new ArgumentNullException(nameof(propertyDescription));
            this.propertyType = propertyType;
        }

        public void SetRange(float defaultValue, float min, float max)
        {
            this.defaultValue = defaultValue;
            this.min = min;
            this.max = max;
        }
    }
#endif

    [Serializable]
    public class ShaderPropertiesList
    {
        public List<ShaderPropertyValue> values = new List<ShaderPropertyValue>();
    }

    [Serializable]
    public class ShaderPropertyValue
    {
        public string valueName;

        public string stringVal = "";

        public bool boolValue;

        public float floatValue;

        public Texture2D textureValue;

        public Vector4 vectorValue = new Vector4();

        public Color colorValue = Color.white;

        public ShaderParameterType type;

        public ShaderPropertyValue()
        {

        }

        public ShaderPropertyValue(ShaderPropertyValue toCopy)
        {
            this.valueName = string.Copy(toCopy.valueName) ?? throw new ArgumentNullException(nameof(valueName));
            this.stringVal = string.Copy(toCopy.stringVal) ?? throw new ArgumentNullException(nameof(stringVal));
            this.boolValue = toCopy.boolValue;
            this.floatValue = toCopy.floatValue;
            this.vectorValue = new Vector4(toCopy.vectorValue.x, toCopy.vectorValue.y, toCopy.vectorValue.z, toCopy.vectorValue.w);
            this.colorValue = new Color(toCopy.colorValue.r, toCopy.colorValue.g, toCopy.colorValue.b, toCopy.colorValue.a);
            this.textureValue = toCopy.textureValue;


            this.type = toCopy.type;
        }
    }
}