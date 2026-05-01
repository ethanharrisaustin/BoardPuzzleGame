using MapNavigation;
using UnityEngine;
using MapRooms;

namespace Combat
{
    public class MagicOrb : MonoBehaviour
    {
        public ObjectWithHealthGO target;

        public float directionChangeSpeed = 5f;
        public float moveSpeed = 5f;
        public float dirChangeSpeedIncreaseSpeed = 2f;
        public float disappearSpeed = 8f;
        
        float c_directionChangeSpeed, c_moveSpeed;

        Vector3 direction;

        Material material;

        float delayForSpawn = 0f;

        [HideInInspector] public float strength;

        void Awake()
        {
            material = new Material(GetComponentInChildren<MeshRenderer>().material);
            GetComponentInChildren<MeshRenderer>().material = material;
        }

        public void Spawn(Color orbColour, ObjectWithHealthGO objectWithHealthGO, float strength, float delayForSpawn)
        {
            material.SetColor("_BaseColor", orbColour);
            material.SetColor("_EmissionColor", orbColour *  Mathf.LinearToGammaSpace(2f) * 10f);

            target = objectWithHealthGO;

            Quaternion playerRot = Quaternion.Euler(PlayerGO.instance.transform.eulerAngles.x, PlayerGO.instance.transform.eulerAngles.y + Random.Range(-45f, 45f), PlayerGO.instance.transform.eulerAngles.z);

            direction = playerRot * Vector3.forward;

            c_moveSpeed = moveSpeed * Random.Range(0.8f, 1.2f);
            c_directionChangeSpeed = directionChangeSpeed * Random.Range(0.8f, 1.2f);

            this.delayForSpawn = delayForSpawn;
            this.strength = strength;

            transform.position = PlayerGO.instance.GetCenterPosition();
            transform.localScale = Vector3.one;
        }

        void Update()
        {
            if (delayForSpawn > 0f)
            {
                delayForSpawn -= Time.deltaTime;
            }

            if (target == null)
            {
                Disappear();
                return;
            }

            Vector3 targetDirection = (target.GetCenterPosition() - transform.position).normalized;

            direction = Vector3.MoveTowards(direction, targetDirection, Time.deltaTime * c_directionChangeSpeed);

            transform.position += direction * Time.deltaTime * c_moveSpeed;

            c_directionChangeSpeed += Time.deltaTime * dirChangeSpeedIncreaseSpeed;

            if (Vector3.Distance(target.GetCenterPosition(), transform.position) < 0.2f)
            {
                Explode();
            }
        }

        void Explode()
        {
            target.ApplyDamange(Mathf.Max(strength, 1f));

            gameObject.SetActive(false);
        }

        void Disappear()
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * disappearSpeed);

            if (transform.localScale == Vector3.zero)
            {
                gameObject.SetActive(false);
            }
        }
    }
}