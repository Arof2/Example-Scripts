using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camera : MonoBehaviour
{
    public GameObject Build_Menu, Enemy_holder, am_weitestem_weg, platzhalter_am_weitesten_weg, Scann_auf_object;
    private GameObject last_Object_in_way;
    public Transform position_core_of_heart;
    public Scrollbar scrollbar_camera;
    public Quaternion aim_rotation;
    public float speed = 5, speed2 = 5, x_cord_value = 0.55f, y_cord_value = 0.4f;
    private Quaternion start_rotation;
    private Vector3 aim_position, start_position;
    public bool can_switch_aimposition = true, looking_at_obstacle;
    private float distance, max_distance;

    void Start()
    {
        start_rotation = transform.rotation;
        start_position = transform.position;
        aim_position = transform.position;
    }

    void Update()
    {
        if (modes.mode_build == true)
        {
            if ((mouse.hit_gameobject.CompareTag("Build_Block") || mouse.hit_gameobject.CompareTag("wall") || mouse.hit_gameobject.CompareTag("tower") || mouse.hit_gameobject.CompareTag("Falle")) && can_switch_aimposition)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    aim_position = new Vector3(mouse.hit_gameobject.transform.position.x, mouse.hit_gameobject.transform.position.y + 2, mouse.hit_gameobject.transform.position.z - 4);
                    aim_rotation = Quaternion.Euler(0, 0, 0);
                    can_switch_aimposition = false;
                }
            }
        }
        else
        {
            if(am_weitestem_weg == null)
            {
                am_weitestem_weg = platzhalter_am_weitesten_weg;
            }
            max_distance = 0;
            foreach(Transform T in Enemy_holder.transform)
            {
                distance = Vector3.Distance(T.transform.position, position_core_of_heart.position);
                if(distance > max_distance)
                {
                    max_distance = distance;
                    am_weitestem_weg = T.gameObject;
                }
            }
            if(scrollbar_camera.value <= 0.4f)
            {
                x_cord_value = 0.4f;
                y_cord_value = 0.5f;
            }
            else
            {
                x_cord_value = scrollbar_camera.value;
                if(scrollbar_camera.value >= 0.8f)
                {
                    y_cord_value = 0.9f;
                }
                else
                {
                    y_cord_value = scrollbar_camera.value + 0.1f;
                }
            }
            float y_cord = 0.4f * distance;
            float x_cord = x_cord_value * distance;
            if(y_cord > 11 + scrollbar_camera.value * 4)
            {
                y_cord = 11 + scrollbar_camera.value * 4;
            }
            if (y_cord < 4 + scrollbar_camera.value * 2)
            {
                y_cord = 4 + scrollbar_camera.value * 2;
            }
            if(x_cord < 5 + scrollbar_camera.value * 2)
            {
                x_cord = 5 + scrollbar_camera.value * 2;
            }
            aim_position = Vector3.Lerp(am_weitestem_weg.transform.position, position_core_of_heart.position, 0.5f) + new Vector3(x_cord, y_cord, 0);
            aim_rotation = Quaternion.LookRotation(Vector3.Lerp(am_weitestem_weg.transform.position, position_core_of_heart.position, 0.5f) - transform.position, Vector3.up);
        }
 
        transform.position = Vector3.Lerp(transform.position, aim_position, speed2 * 0.01f);
        transform.rotation = Quaternion.Lerp(transform.rotation, aim_rotation, speed * 0.01f);

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 6);

        if(hit.collider != null && hit.transform.gameObject.CompareTag("Ground") && !looking_at_obstacle)
        {
            looking_at_obstacle = true;
            last_Object_in_way = hit.transform.gameObject;
            last_Object_in_way.transform.Find("Level Mesh").GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
        if (looking_at_obstacle && hit.collider == null)
        { 
            looking_at_obstacle = false;
            last_Object_in_way.transform.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        }
    }

    public void Return_to_Start_position()
    {
        aim_position = start_position;
        aim_rotation = start_rotation;
        can_switch_aimposition = true;
    }
}
