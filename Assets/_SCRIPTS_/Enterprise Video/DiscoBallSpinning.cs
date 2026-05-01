using UnityEngine;

public class DiscoBallSpinning : MonoBehaviour
{
    public float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + Time.deltaTime * speed, transform.eulerAngles.z);
    }
}
