using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControls : MonoBehaviour
{
    public GameObject target;
    private bool toggle;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            toggle = target.activeSelf;
            target.SetActive(!toggle);
        }
    }
}
