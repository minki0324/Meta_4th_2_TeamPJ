using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("Title Panel")]
    [SerializeField]
    private GameObject Title_Panel;

    [SerializeField]
    private Button GameStart_Btn;

    [SerializeField]
    private Button Avatar_Btn;


    [Header("Avatar Panel")]
    [SerializeField]
    private GameObject Avatar_Panel;



    private enum DropDownMenu
    {
        Head = 0,
        Body,
        Weapon

    }

    [SerializeField]
    private DropDownMenu menu_list;


    [SerializeField]
    private Dropdown dropdown;


    [SerializeField]
    private List<Button> AvatarBtn_List;


    private void Start()
    {
        SetDropdownOption();



        Title_Panel.SetActive(true);
        Avatar_Panel.SetActive(false);
    }
    private void Update()
    {
        DropDown_Selection();
    }


    // Dropdown ��� ����
    private void SetDropdownOption()
    {
        dropdown.options.Clear();

        Dropdown.OptionData option_head = new Dropdown.OptionData();
        Dropdown.OptionData option_body = new Dropdown.OptionData();
        Dropdown.OptionData option_weapon = new Dropdown.OptionData();


        option_head.text = "�Ӹ�";
        option_body.text = "��";
        option_weapon.text = "����";


        dropdown.options.Add(option_head);
        dropdown.options.Add(option_body);
        dropdown.options.Add(option_weapon);

    }


    //update�� �ֱ�
    private void DropDown_Selection()
    {
        if (dropdown.value == 0)
        {
            Debug.Log("111111");
        }
        else if (dropdown.value == 1)
        {
            Debug.Log("2222222");
        }
        else if (dropdown.value == 2)
        {
            Debug.Log("3333333");
        }
    }


    public void GameStart_Btn_Clicekd()
    {
       //���ξ�
    }

    public void Avatar_Btn_Clicked()
    {
        Avatar_Panel.SetActive(true);
        SetDropdownOption();
    }



    public void Avatar_1_Btn()
    {
        switch(menu_list)
        {
            
        }
    }

    public void Avatar_2_Btn()
    {

    }

    public void Avatar_3_Btn()
    {

    }

    public void Avatar_4_Btn()
    {

    }

}
