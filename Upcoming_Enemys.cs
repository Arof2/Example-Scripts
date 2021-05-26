using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upcoming_Enemys : MonoBehaviour
{
    public GameObject Scroll_Rect, Start_Level_G, Prefab;
    private Start_Level Start_Level_Script;
    public List<Sprite> Enemy_Spirtes = new List<Sprite>();
    public List<GameObject> All_Enemys = new List<GameObject>();

    public void Awake()
    {
        Start_Level_Script = Start_Level_G.GetComponent<Start_Level>();
    }

    public void update_next_Enemys()
    {
        foreach(Transform T in Scroll_Rect.transform)
        {
            Destroy(T.gameObject);
        }
        List<Level> Levels = new List<Level>();
        if(Start_Level_Script.momentanes_Level == 1 && Start_Level_Script.Selected_Level != Start_Level_Script.Level_scripts.List.Count)
        {
            foreach (GameObject G in Start_Level_Script.Level_scripts.List[Start_Level_Script.Selected_Level].List)
            {
                Levels.Add(G.GetComponent<Level>());
            }
        }
        if(Start_Level_Script.momentanes_Level == 2 && Start_Level_Script.Selected_Level != Start_Level_Script.Level2_scripts.List.Count)
        {
            foreach (GameObject G in Start_Level_Script.Level2_scripts.List[Start_Level_Script.Selected_Level].List)
            {
                Levels.Add(G.GetComponent<Level>());
            }
        }
        List<int> Count = new List<int>();
        List<bool> Type = new List<bool>();
        for (int i = 0; i < All_Enemys.Count; i++) Count.Add(0);
        for (int i = 0; i < All_Enemys.Count; i++) Type.Add(false);
        foreach (Level L in Levels)
        {
            foreach (GameObject G in L.Enemys)
            {
                string s = G.name.Substring(0,7);
                switch (s)
                {
                    case "Enemy 1":
                        if(Type[0] == false)
                        {
                            Type[0] = true;
                        }
                        Count[0]++;

                        break;
                    case "Enemy 2":
                        if (Type[1] == false)
                        {
                            Type[1] = true;
                        }
                        Count[1]++;

                        break;
                    case "Enemy 3":
                        if (Type[2] == false)
                        {
                            Type[2] = true;
                        }
                        Count[2]++;

                        break;
                    case "Fernkam":
                        if (Type[3] == false)
                        {
                            Type[3] = true;
                        }
                        Count[3]++;

                        break;
                }
            }
        }

        for(int i = 0; i < All_Enemys.Count; i++)
        {
            if(Type[i])
            {
                GameObject Object = Instantiate(Prefab, Vector3.zero, Quaternion.identity, Scroll_Rect.transform);
                GameObject child1_Icon = Object.transform.GetChild(0).gameObject;
                GameObject child2_Text = Object.transform.GetChild(1).gameObject;

                Image Icon = child1_Icon.GetComponent<Image>();
                Text text_count = child2_Text.GetComponent<Text>();

                Icon.sprite = Enemy_Spirtes[i];
                text_count.text = Count[i].ToString();
            }
        }
    }
}