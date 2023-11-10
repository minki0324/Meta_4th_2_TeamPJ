using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    enum state
    {
        empty,
        upgrade,
        worldmap,
        option
    }

    /*
        토글 했을 때 메뉴창들 켜고 끌 수 있는 스크립트 
    */
    [SerializeField] private GameObject[] Menu;

    [SerializeField] private GameObject Upgrade;
    [SerializeField] private GameObject Option;
    [SerializeField] private GameObject WorldMap;

    public bool isMenuOpen = false;

    void Update()
    {
        if(!isMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Togglemenu(Upgrade);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                Togglemenu(WorldMap);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Togglemenu(Option);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 0; i < Menu.Length; i++)
                {
                    Menu[i].SetActive(false);
                    isMenuOpen = false;
                    GameManager.instance.Resume();
                }
            }
        }
    }

    private void Togglemenu(GameObject menu)
    {
        menu.SetActive(true);
        isMenuOpen = true;
        GameManager.instance.Stop();
    }
}
