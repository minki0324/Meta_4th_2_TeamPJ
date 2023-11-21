using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    /*
        토글 했을 때 메뉴창들 켜고 끌 수 있는 스크립트 
    */
    [SerializeField] private WorldMap worldMap;
    [SerializeField] private GameObject[] Menu;
    [SerializeField] private Optioin_Panel option;

    public bool isMenuOpen = false;
    private void Awake()
    {
        Menu[2] = Optioin_Panel.instance.gameObject;
        option = Menu[2].GetComponent<Optioin_Panel>();
        Menu[2].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        Menu[2].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        Menu[2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        if (!isMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Togglemenu(Menu[0]);    // 업그레이드
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                worldMap.Change_ColorChip();
                Togglemenu(Menu[1]);    // 월드맵
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Togglemenu(Menu[2]);    // 옵션
                option.OptionPanel_On();

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = 0; i < Menu.Length; i++)
                {
                    Menu[i].SetActive(false);
                }
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isMenuOpen = false;
                GameManager.instance.Resume();
            }
        }
    }

    public void Togglemenu(GameObject menu)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menu.SetActive(true);
        isMenuOpen = true;
        GameManager.instance.Stop();
    }

    public void Close_Menu()
    {
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isMenuOpen = false;
        GameManager.instance.Resume();
    }
}
