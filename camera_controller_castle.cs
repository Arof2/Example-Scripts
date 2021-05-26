using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller_castle : MonoBehaviour
{
    private Vector3 Aim_position;
    private Quaternion Aim_rotation;
    public bool is_active = false;
    public GameObject cam;
    public float speed = 5;

    void Update()
    {
        if(is_active == true)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, Aim_position, Time.deltaTime * speed);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Aim_rotation, Time.deltaTime * speed);
        }
    }


    public void Set_Aim_position(Vector3 new_Aim_position)
    {
        Aim_position = new_Aim_position;
    }

    public void Set_aim_Quaternion(Quaternion new_Aim_rotation)
    {
        Aim_rotation = new_Aim_rotation;
    }

    public void Set_active_State(bool new_state)
    {
        is_active = new_state;
    }
}
