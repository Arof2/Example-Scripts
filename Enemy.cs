using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region variables
    Health_script Health_script;
    private GameObject core_of_heart, Health_Holder;
    public GameObject aim_Gameobject;
    public NavMeshAgent agent;
    public Animator animator;
    private Material material;
    private Color Startcolor;
    public float distance;
    public float Damage_Value;
    public float Health = 20, Death_Value = 10;
    private bool Is_attacking = false;
    private float getting_damage;
    private bool colorchange;
    private bool lerp = false;
    private bool has_been_hit, really_is_ded = true;
    public bool can_make_damage, can_make_damage_private = false;
    private bool reached_core = false;
    #endregion

    void Start()
    {
        Health_Holder = transform.Find("Health script Holder").gameObject;
        Health_script = Health_Holder.GetComponent<Health_script>();
        Startcolor = new Color32(85, 0, 86, 1);
        material = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        material.color = Startcolor;
        core_of_heart = GameObject.FindGameObjectWithTag("core");
        Search_Aim();
        agent.SetDestination(aim_Gameobject.transform.position);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("shot") && Is_attacking == false && has_been_hit == false)
        {
            StartCoroutine(aktivate_lerp());
        }
        if (other.gameObject.CompareTag("shot"))
        {
            material.color = new Color32(255, 15, 15, 1);
            colorchange = true;
        }
    }
    void Update()
    {
        if (Health_script.is_ded == false)
        {
            if (Vector3.Distance(agent.destination, transform.position) < agent.stoppingDistance)
            {                                                                   
                reached_core = true;
                can_make_damage_private = true;
            }
        }

        if(aim_Gameobject == null)
        {
            can_make_damage = false;
            can_make_damage_private = false;
            animator.SetBool("Attack", false);
            Is_attacking = false;
            reached_core = false;
            agent.enabled = true;
            Search_Aim();
            agent.SetDestination(aim_Gameobject.transform.position);
        }

        if(Health_script.is_ded && really_is_ded)
        {
            Dies();
        }

        if (lerp == true)
        {
            transform.position += -transform.forward * distance * Time.deltaTime;
        }

        if (can_make_damage && can_make_damage_private)
        {
            Make_Damage();
            StartCoroutine(damage_cooldown());
        }

        if (colorchange)
        {
            material.color = Color.Lerp(material.color, Startcolor, 0.03f);
        }

        if (material.color == Startcolor)
        {
            colorchange = false;
        }

        if(reached_core && Is_attacking == false)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            animator.SetBool("Attack", true);
            Is_attacking = true;
        }

        Test_update();
    }

    #region search aims
    void Test_update()
    {
        if (aktualisiere_path.update_path == true && Health_script.is_ded == false)
        {
            NavMeshPath pfad = new NavMeshPath();
            agent.CalculatePath(core_of_heart.transform.position, pfad);
            if (pfad.status == NavMeshPathStatus.PathPartial)
            {
                GameObject[] Gos;
                Gos = GameObject.FindGameObjectsWithTag("wall");
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject Go in Gos)
                {
                    Vector3 diff = Go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = Go;
                        distance = curDistance;
                    }
                }
                agent.SetDestination(closest.transform.position);
                aim_Gameobject = closest;
                agent.stoppingDistance = 0.75f;
            }
            else
            {
                GameObject aim = GameObject.FindGameObjectWithTag("core");
                agent.SetDestination(aim.transform.position);
                aim_Gameobject = aim;
            }
        }
    }

    void Search_Aim()
    {
        if (Health_script.is_ded == false)
        {
            NavMeshPath pfad = new NavMeshPath();
            agent.CalculatePath(core_of_heart.transform.position, pfad);
            if (pfad.status == NavMeshPathStatus.PathPartial)
            {
                GameObject[] Gos;
                Gos = GameObject.FindGameObjectsWithTag("wall");
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject Go in Gos)
                {
                    Vector3 diff = Go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = Go;
                        distance = curDistance;
                    }
                }
                aim_Gameobject = closest;
                agent.stoppingDistance = 0.75f;
            }
            else
            {
                GameObject aim = GameObject.FindGameObjectWithTag("core");
                aim_Gameobject = aim;
            }
        }
    }
    #endregion

    void Dies()
    {
        really_is_ded = false;
        Destroy(agent);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        animator.SetTrigger("ded");
        Destroy(gameObject, 1.5f);
        transform.Find("Max Health").gameObject.SetActive(false);
        transform.Find("Max Health 2").gameObject.SetActive(false);
        Money_Script MS = GameObject.FindGameObjectWithTag("Money").GetComponent<Money_Script>();
        MS.Add_Money(Death_Value);
    }

    void Make_Damage()
    {
        Health_script script = aim_Gameobject.GetComponent<Health_script>();
        script.Make_damage(Damage_Value);
    }

    #region Time Funktions
    IEnumerator aktivate_lerp()
    {
        StartCoroutine(cant_get_hit());
        lerp = true;
        yield return new WaitForSeconds(0.3f);
        lerp = false;
    }

    IEnumerator cant_get_hit()
    {
        has_been_hit = true;
        yield return new WaitForSeconds(1.3f);
        has_been_hit = false;
    }

    IEnumerator damage_cooldown()
    {
        can_make_damage_private = false;
        yield return new WaitForSeconds(0.5f);
        can_make_damage_private = true;
    }
    #endregion
}