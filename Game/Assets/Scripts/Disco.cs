using UnityEngine;

public class Disco : MonoBehaviour
{
    private GameObject disco;
    void Start()
    {
        disco = gameObject.transform.GetChild(0).gameObject;
        disco.SetActive(false);
    }

    void Update()
    {
        if (AudioRecorder.Instance.isReplaying)
        {
            disco.SetActive(true);
        }
        else
        {
            disco.SetActive(false);
        }
    }
}
