using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UI_CombatInput : RB_Beat
    {
        [Space, Header("Combat Sprites")]

        public Sprite[] combatSpritesXbox;
        public Sprite[] combatSpritesPS5;
        public Sprite[] combatSpritesSwitch;
        public Sprite[] combatSpritesControllerGeneral;
        public Sprite[] combatSpritesArrowKeys;

        [Space, Header("Combat Colours")]

        public Color[] combatColoursXbox;
        public Color[] combatColoursPS5;
        public Color[] combatColoursGeneral;

        [Space, Header("References")]

        public Image combatAccuracyLineImg, controllerIconImg;

        public override void SpawnBeat(int combatInput, BasicMetronomeObject basicMetronomeObject)
        {
            base.SpawnBeat(0, basicMetronomeObject); // Make it new bar scale

            Sprite sprite;
            Color elementColour, spriteColour;
            CombatColours.GetElementFromInput(combatInput, out int _, out sprite, out elementColour, out spriteColour);

            combatAccuracyLineImg.color = elementColour;
            controllerIconImg.sprite = sprite;
            controllerIconImg.color = spriteColour;
        }
    }
}