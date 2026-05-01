using TMPro;
using UnityEngine;

public class BeatMover : MonoBehaviour
{
    public float moveSpeed = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
    }


    public void ShowNumber(int number)
    {
        GetComponentInChildren<TMP_Text>().text = number.ToString();
    }
}
