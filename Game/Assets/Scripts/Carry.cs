using UnityEngine;

public class Carry : MonoBehaviour
{
     void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
