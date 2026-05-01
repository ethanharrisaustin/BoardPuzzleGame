using UnityEngine;
using DG.Tweening;

namespace MapRooms
{
    public class BrazierGO : LightGO
    {
        [SerializeField] float coalLightMinIntensity = 0.5f;
        [SerializeField] float roomLightMinIntensity = 0.5f;
        [SerializeField] int offset = 0;
        [Space]
        [SerializeField] Light coalLight;
        [SerializeField] Light roomLight;

        float coalLightIntensity, roomLightIntensity;

        int numExtraValues = 3;
        public override void GetValues(out string[] values)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif

            string[] lightGOValues;
            base.GetValues(out lightGOValues);

            string[] finalValues = new string[lightGOValues.Length + numExtraValues];

            for (int i = 0; i < lightGOValues.Length; ++i) finalValues[i] = lightGOValues[i];

            finalValues[finalValues.Length - 3] = coalLightMinIntensity.ToString();
            finalValues[finalValues.Length - 2] = roomLightMinIntensity.ToString();
            finalValues[finalValues.Length - 1] = offset.ToString();

            values = finalValues;
        }

        public override void SetValues(string[] values)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif

            string[] lightGOValues = new string[values.Length - numExtraValues];

            for (int i = 0; i < lightGOValues.Length; ++i) lightGOValues[i] = values[i];

            base.SetValues(lightGOValues);

            if (float.TryParse(values[values.Length - 3], out var _coalLightMinIntensity)) 
                coalLightMinIntensity = _coalLightMinIntensity;

            if (float.TryParse(values[values.Length - 2], out var _roomLightMinIntensity)) 
                roomLightMinIntensity = _roomLightMinIntensity;

            if (int.TryParse(values[values.Length - 1], out var _offset)) 
                offset = _offset;
        }

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            coalLightIntensity = coalLight.intensity;
            roomLightIntensity = roomLight.intensity;

            for (int i = 0; i < offset; ++i) showIntensity = !showIntensity;
        }

        bool showIntensity = true;

        protected override void Update()
        {
            base.Update();
            
            if (!MusicRhythmTimer.BeatIncreased()) return;
            
            showIntensity = !showIntensity;

            coalLight.DOKill(false);
            roomLight.DOKill(false);

            float multiplier = showIntensity ? 1f : coalLightMinIntensity;
            coalLight.DOIntensity(coalLightIntensity * multiplier, 0.2f);

            multiplier = showIntensity ? 1f : roomLightMinIntensity;
            roomLight.DOIntensity(roomLightIntensity * multiplier, 0.2f);
        }
    }
}