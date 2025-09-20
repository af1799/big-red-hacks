using UnityEngine;

public class Spin : MonoBehaviour
{
    private float rotationSpeed = 70f;
    public void SpinRecord()
    {
        if (rotationSpeed == 0)
        {
            rotationSpeed = 70f;
        }
        else
        {
            rotationSpeed = 0;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
