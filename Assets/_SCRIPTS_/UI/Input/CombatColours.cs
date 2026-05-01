using UnityEngine;

namespace Combat
{
    public enum Element { Earth, Water, Air, Fire }

    public class CombatColours : MonoBehaviour
    {
        #region Variables

        public static CombatColours instance;

        [System.Serializable]
        public class InputsElements 
        {
            public string name;
            public enum InputGroupName { ArrowKeys, Xbox, Playstation, NitendoSwitch, OtherController }
            public InputGroupName inputGroupName;
            public Element[] elements; 
            public Sprite[] combatSprites;
            public Color[] combatColours;
            public bool changeColourOfSprites;
        }

        public InputsElements[] inputsElements;

        static InputsElements c_inputElements;

        #endregion

        #region Monobehaviour methods

        void OnEnable()
        {
            Input.OnControlSchemeChangedEvent += OnControlSchemeChanged;

            OnControlSchemeChanged(Input.Main().currentControlScheme);
        }

        void OnDisable()
        {
            Input.OnControlSchemeChangedEvent -= OnControlSchemeChanged;
        }

        #endregion

        #region Getting Current InputElements
        
        void OnControlSchemeChanged(Input.ControlScheme controlScheme)
        {
            c_inputElements = GetInputsElements(controlScheme);
        }

        InputsElements GetInputsElements(Input.ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                case Input.ControlScheme.Controller: return GetInputsElements(InputsElements.InputGroupName.OtherController);
                case Input.ControlScheme.XboxController: return GetInputsElements(InputsElements.InputGroupName.Xbox);
                case Input.ControlScheme.PlaystationController: return GetInputsElements(InputsElements.InputGroupName.Playstation);
                case Input.ControlScheme.NitendoSwitch: return GetInputsElements(InputsElements.InputGroupName.NitendoSwitch);
                default: return GetInputsElements(InputsElements.InputGroupName.ArrowKeys);
            }
        }

        InputsElements GetInputsElements(InputsElements.InputGroupName inputGroupName)
        {
            for (int i = 0; i < inputsElements.Length; ++i)
            {
                if (inputsElements[i].inputGroupName == inputGroupName) return inputsElements[i];
            }
            return null;
        }

        #endregion
        
        #region Get

        public static void GetElementFromInput(int combatInput, out int element, out Sprite inputIconSprite, out Color elementColour, out Color inputIconColour)
        {
            element = (int)c_inputElements.elements[combatInput];
            inputIconSprite = c_inputElements.combatSprites[combatInput];
            elementColour = c_inputElements.combatColours[combatInput];
            inputIconColour = c_inputElements.changeColourOfSprites ? elementColour : Color.white;
        }

        public static void GetElementFromInput(int combatInput, out Element element, out Sprite inputIconSprite, out Color elementColour, out Color inputIconColour)
        {
            element = c_inputElements.elements[combatInput];
            inputIconSprite = c_inputElements.combatSprites[combatInput];
            elementColour = c_inputElements.combatColours[combatInput];
            inputIconColour = c_inputElements.changeColourOfSprites ? elementColour : Color.white;
        }

        public static Color GetElementColour(Element element)
        {
            for (int i = 0; i < c_inputElements.elements.Length; ++i)
            {
                if (c_inputElements.elements[i] == element)
                {
                    return c_inputElements.combatColours[i];
                }
            }

            return Color.paleTurquoise;
        }

        #endregion

        #region Archived
        
        /* 

        /// <summary>
        /// Grab the correct combat sprite for xbox, keyboard or ps4 controller.
        /// </summary>
        public static Sprite GetCombatSprite(int combatInput)
        {
            switch (Input.instance.currentControlScheme)
            {
                case Input.ControlScheme.PlaystationController: return GetCombatSprite(combatInput, instance.combatSpritesPS5);
                case Input.ControlScheme.XboxController: return GetCombatSprite(combatInput, instance.combatSpritesXbox);
                case Input.ControlScheme.NitendoSwitch: return GetCombatSprite(combatInput, instance.combatSpritesSwitch);
                case Input.ControlScheme.Controller: return GetCombatSprite(combatInput, instance.combatSpritesControllerGeneral);
                default: return GetCombatSprite(combatInput, instance.combatSpritesArrowKeys);
            }
        }

        static Sprite GetCombatSprite(int combatInput, Sprite[] sprites)
        {
            return sprites[combatInput];
        }

        /// <summary>
        /// Grab the correct combat sprite for xbox, keyboard or ps4 controller.
        /// </summary>
        public static Color GetCombatColour(int combatInput)
        {
            switch (Input.instance.currentControlScheme)
            {
                case Input.ControlScheme.PlaystationController: return GetCombatColour(combatInput, instance.combatColoursPS5);
                case Input.ControlScheme.XboxController: return GetCombatColour(combatInput, instance.combatColoursXbox);
                default: return GetCombatColour(combatInput, instance.combatColoursGeneral);
            }
        }

        static Color GetCombatColour(int combatInput, Color[] colours)
        {
            return colours[combatInput];
        }

        */

        #endregion
    }
}