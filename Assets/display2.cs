using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class display2 : MonoBehaviour
{
	[SerializeField] Camera cameraDisplay2;
	// Start is called before the first frame update
	void Start()
    {
		cameraDisplay2.targetDisplay = 1;
		Display.displays[1].Activate();//activate secondary display
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
