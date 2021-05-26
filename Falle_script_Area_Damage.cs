using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falle_script_Area_Damage : MonoBehaviour
{
    private BoxCollider BoxCollide;
    private Animator animator;
    private bool attack = false;
    public List<GameObject> Enemys = new List<GameObject>();
    public GameObject Build_Block;
    public float Damage, value;
    public string Trap_Type;
    public GameObject Shockwave;

    void Start()
    {
        BoxCollide = gameObject.GetComponent<BoxCollider>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy_hitbox"))
        {
            Enemys.Add(other.gameObject);
            if (Enemys.Count <= 1)
            {
                StartCoroutine(Make_Damage());
                attack = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy_hitbox"))
        {
            Enemys.Remove(other.gameObject);
            if (Enemys.Count == 0)
            {
                attack = false;
            }
        }
    }

    public void Sell_Trap()
    {
        Money_Script mon_script;
        mon_script = GameObject.FindGameObjectWithTag("Money").gameObject.GetComponent<Money_Script>();
        mon_script.Add_Money(value);
        Grid_Cube Block;
        Block = Build_Block.GetComponent<Grid_Cube>();
        Block.Tower_Dies();
        Destroy(gameObject);
    }

    IEnumerator Make_Damage()
    {
        Global_Tower_upgrades add_script;
        add_script = GameObject.FindGameObjectWithTag("Tower Upgrades").gameObject.GetComponent<Global_Tower_upgrades>();

        animator.SetBool("Attack", true);
        for (int i = 0; i < Enemys.Count; i++)
        {
            Health_script HS;
            HS = Enemys[i].transform.Find("Health script Holder").gameObject.GetComponent<Health_script>();
            if (HS.is_ded)
            {
                Enemys.Remove(Enemys[i]);
                if (Enemys.Count == 0)
                {
                    attack = false;
                }
            }
        }
        if (attack)
        {
            foreach (GameObject Enemy in Enemys)
            {
                if (Enemy == null)
                {
                    Debug.Log("is null");
                    Enemys.Remove(Enemy);
                }
                else
                {
                    Health_script HS;
                    HS = Enemy.transform.Find("Health script Holder").gameObject.GetComponent<Health_script>();
                    HS.Make_damage(Damage + add_script.Damage_Falle + add_script.Damage_Falle_P);
                }
            }
        }
        Instantiate(Shockwave, gameObject.transform.position + new Vector3(0,1,0), Quaternion.Euler(90,0,0), gameObject.transform);
        yield return new WaitForSeconds(0.416f);
        if (attack)
        {
            StartCoroutine(Make_Damage());
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
}
