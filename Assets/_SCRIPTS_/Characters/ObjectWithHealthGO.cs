using MapNavigation;
using UnityEngine;

namespace MapRooms
{

    public class ObjectWithHealthGO : MoveableObjectGO
    {
        float startHealth;
        public float currentHealth;
        protected float healthBarOffsetY = 0.18f;

        protected override void Awake()
        {
            base.Awake();
            
            startHealth = currentHealth;
        }

        public virtual void ApplyDamange(float damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0f);

            if (currentHealth == 0f)
            {
                Die();
            }
        }

        public float HealthAsPercentage()
        {
            return currentHealth / startHealth * 100f;
        }

        public virtual void Die()
        {
            Destroy(gameObject);
        }

        public float HealthBarOffsetY()
        {
            return Screen.height * healthBarOffsetY;
        }
    }

}