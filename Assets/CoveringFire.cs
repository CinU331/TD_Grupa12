using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoveringFire : MonoBehaviour
{
    private bool isStarted = false;
    private float maxTime = 1;
    private float initTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted)
        {
            if(Time.time - initTime >= maxTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void StartDestruction(float aTime)
    {
        if(aTime > 0)
        {
            isStarted = true;
            maxTime = aTime;
            initTime = Time.time;
        }
    }
}
