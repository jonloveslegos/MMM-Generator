using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEntryScript : MonoBehaviour
{
    public UploadScript parentScript;
    public int myId = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveSelf()
    {
        parentScript.RemoveItem(myId);
    }
}
