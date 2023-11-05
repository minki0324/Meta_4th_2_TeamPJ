using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avartar_DropDown : MonoBehaviour
{

    [Header("Dropdown")]
    public Dropdown dropdown;

    private void SetDropdownOptionsExample() // Dropdown ��� ����
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
}
