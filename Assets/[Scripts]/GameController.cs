using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit, 1000, LayerMask.NameToLayer("Enemy")));
        //    {
        //        Debug.Log("you hit me");
        //    }
        //}
    }
}
