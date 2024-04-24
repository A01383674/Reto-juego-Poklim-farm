using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float parallaxEffect;
    void Start()
    {
        
    }

    void Update()
    {
        float tempDistance = mainCamera.transform.position.x * (1 - parallaxEffect);
        float distance = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (tempDistance > startPos + length)
        {
            startPos += length;
        }
        else if(tempDistance < startPos - length)
        {
            startPos -= length;
        }
    }
}
