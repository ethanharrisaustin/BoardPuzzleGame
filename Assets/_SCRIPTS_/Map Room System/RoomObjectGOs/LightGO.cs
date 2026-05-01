#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MapRooms
{
    public class LightGO : RoomObjectGO
    {
        public override string ObjectFlyInCategory()
        {
            return "LightGO";
        }
        
        protected int valuesPerLight = 11;
        public override void GetValues(out string[] values)
        {
            Light[] lights = GetLights();

            values = new string[lights.Length * (valuesPerLight + 1) + 1];

            for (int light = 0; light < lights.Length; ++light)
            {
                int i = light * valuesPerLight;

                values[i] = ((int)lights[light].type).ToString(); 
                values[i + 1] = lights[light].intensity.ToString();
                values[i + 2] = lights[light].innerSpotAngle.ToString();
                values[i + 3] = lights[light].spotAngle.ToString();
                values[i + 4] = lights[light].range.ToString();

                values[i + 5] = lights[light].color.r.ToString();
                values[i + 6] = lights[light].color.g.ToString();
                values[i + 7] = lights[light].color.b.ToString();

                values[i + 8] = lights[light].transform.localEulerAngles.x.ToString();
                values[i + 9] = lights[light].transform.localEulerAngles.y.ToString();
                values[i + 10] = lights[light].transform.localEulerAngles.z.ToString();
            }
        }

        public override void SetValues(string[] values)
        {
            Light[] lights = GetLights();

            int intendedLength = lights.Length * (valuesPerLight + 1) + 1;

            if (values.Length != intendedLength)
            {
                Debug.LogError("Lights bug");
                return;
            }

            for (int x = 0; x < lights.Length; ++x)
            {
                int i = x * valuesPerLight;

                Light light = lights[x];

                #if UNITY_EDITOR
                EditorUtility.SetDirty(light);
                #endif

                if (int.TryParse(values[i], out int lightType)) light.type = (LightType)lightType;
                if (float.TryParse(values[i + 1], out float intensity)) light.intensity = intensity;
                if (float.TryParse(values[i + 2], out float innerSpotAngle)) light.innerSpotAngle = innerSpotAngle;
                if (float.TryParse(values[i + 3], out float spotAngle)) light.spotAngle = spotAngle;
                if (float.TryParse(values[i + 4], out float range)) light.range = range;
                
                Color color = new Color(1f,1f,1f);
                if (float.TryParse(values[i + 5], out float r)) color.r = r;
                if (float.TryParse(values[i + 6], out float g)) color.g = g;
                if (float.TryParse(values[i + 7], out float b)) color.b = b;
                light.color = color;

                Vector3 eulerAngles = new Vector3(light.transform.localEulerAngles.x, light.transform.localEulerAngles.y, light.transform.localEulerAngles.z);
                if (float.TryParse(values[i + 8], out float eulerX)) eulerAngles.x = eulerX;
                if (float.TryParse(values[i + 9], out float eulerY)) eulerAngles.y = eulerY;
                if (float.TryParse(values[i + 10], out float eulerZ)) eulerAngles.z = eulerZ;
                light.transform.localEulerAngles = eulerAngles;
            }
        }

        Light[] GetLights()
        {
            return GetComponentsInChildren<Light>();
        }
    }
}