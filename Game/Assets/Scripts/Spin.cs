using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    private float rotationSpeed = 70f;
    private float seconds = 1.5f;
    public AudioSource audioSource;
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

    public void ReverseRecordForSeconds()
    {
        if (AudioRecorder.Instance.GetMaxLength().Count != 0)
        {
            StartCoroutine(ReverseAndRestore(seconds));
        }
    }

    private IEnumerator ReverseAndRestore(float seconds)
    {
        audioSource.Play();
        rotationSpeed = -5f * rotationSpeed;
        yield return new WaitForSeconds(seconds);
        rotationSpeed = -rotationSpeed / 5f;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
