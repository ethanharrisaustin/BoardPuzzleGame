using UnityEngine;
 #if UNITY_EDITOR
using UnityEditor;
#endif
namespace MapRooms
{
    public class LightUpFloorTileGO : FloorTileGO
    {
        [Space]
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] Color offColour;
        [SerializeField] Color litUpEmission;
        [SerializeField] float litUpEmissionAmount;

        MaterialPropertyBlock mpb;

        Material material;

        protected override void Awake()
        {
            base.Awake();

            material = new Material(meshRenderer.material);

            meshRenderer.material = material;

            mpb = new();
        }

        public override void SetValues(string[] values)
        {
            try
            {
                #if UNITY_EDITOR
                EditorUtility.SetDirty(gameObject);
                #endif
                
                // Off colour
                float red = float.Parse(values[0]);
                float green = float.Parse(values[1]);
                float blue = float.Parse(values[2]);

                // Emissive colour
                float lightUpRed = float.Parse(values[3]);
                float lightUpGreen = float.Parse(values[4]);
                float lightUpBlue = float.Parse(values[5]);

                // Intensity
                float intensity = float.Parse(values[6]);

                // Create the colours here
                offColour = new Color(red, green, blue);
                litUpEmission = new Color(lightUpRed, lightUpGreen, lightUpBlue);
                litUpEmissionAmount = intensity;

                if (mpb == null) mpb = new();
                
                meshRenderer.GetPropertyBlock(mpb);
                
                // Set the material parameters
                mpb.SetColor("_BaseColor", offColour);
                mpb.SetColor("_EmissionColor", litUpEmission * Mathf.LinearToGammaSpace(intensity));

                meshRenderer.SetPropertyBlock(mpb);
            }
            catch
            {
                Debug.Log("Setting values of LightUpFloorTileGO Failed.");
            }
        }

        public override void GetValues(out string[] values)
        {
            try
            {
                values = new string[7];
                
                // Set values for off colour
                float red = offColour.r;
                float green = offColour.g;
                float blue = offColour.b;

                // Set values for emissive colour
                float lightUpRed = litUpEmission.r;
                float lightUpGreen = litUpEmission.g;
                float lightUpBlue = litUpEmission.b;
                
                // Set values
                values[0] = red.ToString();
                values[1] = green.ToString();
                values[2] = blue.ToString();

                values[3] = lightUpRed.ToString();
                values[4] = lightUpGreen.ToString();
                values[5] = lightUpBlue.ToString();

                values[6] = litUpEmissionAmount.ToString();
            }
            catch
            {
                Debug.Log("Getting values of LightUpFloorTileGO Failed.");

                values = new string[0];
            }
        }
    }
}