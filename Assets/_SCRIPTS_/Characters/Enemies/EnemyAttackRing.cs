using DG.Tweening;
using MapRooms;
using UnityEngine;

namespace Combat
{
    public class EnemyAttackRing : MonoBehaviour
    {
        [SerializeField] MeshRenderer outlineRenderer;
        [SerializeField] MeshRenderer fillRenderer;

        MaterialPropertyBlock outlineMat, fillMat;

        int maxBeat;
        int currentBeat;

        [HideInInspector] public EnemyGO targetEnemy;

        void Awake()
        {
            outlineMat = new();
            fillMat = new();

            outlineRenderer.GetPropertyBlock(outlineMat);
            fillRenderer.GetPropertyBlock(fillMat);
        }

        public void SetElement(EnemyGO enemyGO, Element element)
        {
            targetEnemy = enemyGO;

            Color elementColour = CombatColours.GetElementColour(element);

            Color paleColour = BringToWhite(elementColour, 0.2f);

            // Set the material parameters
            outlineMat.SetColor("_BaseColor", paleColour);
            outlineMat.SetColor("_EmissionColor", paleColour * Mathf.LinearToGammaSpace(1f));

            fillMat.SetColor("_BaseColor", elementColour);
            fillMat.SetColor("_EmissionColor", elementColour * Mathf.LinearToGammaSpace(1f));

            outlineRenderer.SetPropertyBlock(outlineMat);
            fillRenderer.SetPropertyBlock(fillMat);
        }

        Color BringToWhite(Color current, float increaseAmount)
        {
            return new Color(
                BringToOne(current.r, increaseAmount), 
                BringToOne(current.g, increaseAmount), 
                BringToOne(current.b, increaseAmount)
            );
        }

        float BringToOne(float current, float increaseAmount)
        {
            return Mathf.Clamp(current + increaseAmount, 0f, 1f);
        }

        public void SetMaxBeat(int maxBeat)
        {
            this.maxBeat = maxBeat;
            currentBeat = 0;

            ScaleFill();
        }

        public bool IncreaseBeat()
        {
            currentBeat++;

            ScaleFill();

            return currentBeat >= maxBeat;
        }

        void ScaleFill()
        {
            fillRenderer.transform.DOKill(false);

            fillRenderer.transform.DOScale(CurrentFillScale(), 0.15f).SetEase(Ease.OutBack);
        }

        Vector3 CurrentFillScale()
        {
            float fillAmount = 1f + 2f * (float)(currentBeat) / (float)(maxBeat - 1);

            return new Vector3(fillAmount, 1f, fillAmount);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}