using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Manager : MonoBehaviour
{
    public static Save_Manager instance;
    public Tutorial_Manager Tutorial_Script;
    public Tutorial_Castle Tutorial_Script_Castle;
    public Money_Script MS;
    public Global_Tower_upgrades unlocked_Towers;
    public Save_Türme Türme;
    public Start_Level Level;
    public Save_Tower_Guns Tower_Guns;
    public Health_script core_health_script;
    public go_to_castle go_to_ca;
    public P_Money_Script P_Money;
    public Phasen_Manager Phasen;
    public Switch_Level1to2 switchLevel;
    public Switch_Level_Move_TG SwitchTG;

    public List<Button_Info> Buttons;
    public List<PShop_Button> P_Shop_Buttonss;
    public GameObject parent_of_P_Shop_Buttons;

    void Awake()
    {
        instance = this;
    }

    public void Load()
    {
        Debug.Log("Load");

        Player_Data data = SaveSystem.Load();

        if(data.momentanes_L == 2)
        {
            switchLevel.Switch_Level();
            SwitchTG.Move_Tower_Guns_to_Level2();
            Türme.get_Build_Blocks();
            Level.momentanes_Level = 2;
        }

        P_Money.P_Points = data.P_Points;
        MS.LoadMoney(data.Money);

        // Global upgrades
        unlocked_Towers.Load_Towers(data.unlocked_Tower, data.unlocked_Wall, data.unlocked_trap);
        unlocked_Towers.unlocked_Weaponised_Wall = data.unlocked_weaponised_Wall;
        unlocked_Towers.unlocked_Armoured_Wall = data.unlocked_Armoured_Wall;
        unlocked_Towers.unlocked_Area_Damage_Tower = data.unlocked_Area_Damage_Tower;
        unlocked_Towers.unlocked_Singel_Damage_Tower = data.unlocked_Singel_Damage_Tower;
        unlocked_Towers.unlocked_Singel_Damage_Trap = data.unlocked_Singel_Damage_Tower;
        unlocked_Towers.unlocked_Area_Damage_Trap = data.unlocked_Area_Damage_Trap;
        unlocked_Towers.Damage_Tower_P = data.Damage_Tower_P;
        unlocked_Towers.Damage_Falle_P = data.Damage_Falle_P;
        unlocked_Towers.Damage_Core_Guns_P = data.Damage_Core_Guns_P;
        //Load Buttons
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].Load_Button(data.Buttons[i]);
        }
        foreach (Transform G in parent_of_P_Shop_Buttons.transform)
        {
            for(int i = 0; i < G.childCount;i++)
            {
                PShop_Button P = G.GetChild(i).GetComponent<PShop_Button>();
                P.unlocked = data.P_shop_Buttons_unlocked[i];
                P.current_state = data.P_shop_Buttons_current_state[i];
            }
        }
        
        Tutorial_Script.Made_Tutorial = data.made_tutorial;
        Tutorial_Script_Castle.made_tutorial = data.made_tutorial_castle; 

        if(data.phase_enemy)
        {
            Level.Load_Level(data.Level_stat);
        }
        else
        {
            Level.Load_Level(data.Level_stat + 1);
        }

        Level.Started_First_Level = data.Started_F_Level;

        if (data.momentanes_L != 2)
            Tower_Guns.Load_Towerguns(data.Tower_Gun_1, data.Tower_Gun_2);

        core_health_script.Health = data.core_health;
        core_health_script.is_ded = false;

        go_to_ca.current_Level = data.Current_level_go_to_castle;

        
        // Load Towers

        List<Vector3> Position_Wall_S = new List<Vector3>();
        List<Vector3> Position_Türme_S = new List<Vector3>();
        List<Vector3> Position_Traps_S = new List<Vector3>();

        for (int i = 0;i < data.Position_Wall_P_x.Count;i++)
        {
            Position_Wall_S.Add(new Vector3(data.Position_Wall_P_x[i],data.Position_Wall_P_y[i],data.Position_Wall_P_z[i]));
        }
        for (int i = 0; i < data.Position_Towers_P_x.Count; i++)
        {
            Position_Türme_S.Add(new Vector3(data.Position_Towers_P_x[i], data.Position_Towers_P_y[i], data.Position_Towers_P_z[i]));
        }
        for (int i = 0; i < data.Position_Traps_P_x.Count; i++)
        {
            Position_Traps_S.Add(new Vector3(data.Position_Traps_P_x[i], data.Position_Traps_P_y[i], data.Position_Traps_P_z[i]));
        }

        Türme.Position_Towers = Position_Türme_S;
        Türme.Position_Wall = Position_Wall_S;
        Türme.Position_Traps = Position_Traps_S;

        Türme.Walls = data.Walls_P;
        Türme.Towers = data.Towers_P;
        Türme.Traps = data.Traps_P;

        Türme.BuildBlock_Tower = data.BuildBlock_Tower_P;
        Türme.BuildBlock_Trap = data.BuildBlock_Trap_P;
        Türme.BuildBlock_Wall = data.BuildBlock_Wall_P;

        Türme.Value_Towers = data.Value_Towers_p;
        Türme.Value_Traps = data.Value_Traps_P;
        Türme.Value_Walls = data.Value_Walls_P;

        Türme.Load_Towers();
        Phasen.Phase_Castle();
    }

    public void Save()
    {
        Debug.Log("Save");
        Tower_Guns.Save_Towerguns();
        Türme.Save_Towers();
        Player_Data pd = new Player_Data();
        SaveSystem.Save(pd);
    }
}
