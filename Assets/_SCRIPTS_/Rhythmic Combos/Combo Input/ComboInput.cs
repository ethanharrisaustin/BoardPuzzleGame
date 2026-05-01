using System.Collections.Generic;
using UnityEngine;


namespace Combat
{
    public class ComboInput : MonoBehaviour
    {
        public ObjectPool objectPool;
        public BasicMetronomeObject basicMetronomeObject;
        RB_ComboGhosting comboGhosting;

        public bool makingCombo = false;
        public List<CombatInput> currentCombatInputs = new List<CombatInput>();

        float resetTimer = 0f;

        void Awake()
        {
            comboGhosting = GetComponent<RB_ComboGhosting>();
        }

        void OnEnable()
        {
            Input.onCombat1 += Combat1;
            Input.onCombat2 += Combat2;
            Input.onCombat3 += Combat3;
            Input.onCombat4 += Combat4;
        }

        void OnDisable()
        {
            Input.onCombat1 -= Combat1;
            Input.onCombat2 -= Combat2;
            Input.onCombat3 -= Combat3;
            Input.onCombat4 -= Combat4;
        }

        void Update()
        {
            if (!makingCombo) return;

            resetTimer += Time.deltaTime;

            if (resetTimer > 1f)
            {
                makingCombo = false;
                currentCombatInputs.Clear();
            }
        }

        void RhythmTimer()
        {
            
        }

        void Combat1()
        {
            SpawnCombatInput(0);
        }

        void Combat2()
        {
            SpawnCombatInput(1);
        }

        void Combat3()
        {
            SpawnCombatInput(2);
        }

        void Combat4()
        {
            SpawnCombatInput(3);
        }

        void SpawnCombatInput(int combatIndex)
        {
            resetTimer = 0f;

            UI_CombatInput combatInput = objectPool.SpawnObject().GetComponent<UI_CombatInput>();

            combatInput.SpawnBeat(combatIndex, basicMetronomeObject);

            makingCombo = true;

            currentCombatInputs.Add(new CombatInput(new int[] { combatIndex }, MusicRhythmTimer.instance.Accuracy()));

            bool isValidCombo, finishedCombo;
            Combo potentialCombo;
            comboGhosting.ShowComboGhosting(currentCombatInputs, out isValidCombo, out finishedCombo, out potentialCombo);

            finishedCombo = currentCombatInputs.Count >= 4; // TO DO: check why 'out finishedCombo' from comboGhosting.ShowComboGhosting() isn't working!

            if (finishedCombo)
            {
                // We can use this to attack an enemy
                CombatAttack combatAttack = new CombatAttack(potentialCombo, currentCombatInputs);

                MagicOrbSpawner.instance.SpawnMagicOrbs(combatAttack);   

                makingCombo = false;
                currentCombatInputs.Clear();    

                return;  
            }

            if (!isValidCombo)
            {
                makingCombo = false;
                currentCombatInputs.Clear();
            }
        }
    }
}