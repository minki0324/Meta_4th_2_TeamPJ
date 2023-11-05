using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avartar_DropDown : MonoBehaviour
{

    [Header("Dropdown")]
    public Dropdown dropdown;

    private void SetDropdownOptionsExample() // Dropdown 格废 积己
    {
        dropdown.options.Clear();

        Dropdown.OptionData option_head = new Dropdown.OptionData();
        Dropdown.OptionData option_body = new Dropdown.OptionData();
        Dropdown.OptionData option_weapon = new Dropdown.OptionData();


        option_head.text = "赣府";
        option_body.text = "个";
        option_weapon.text = "公扁";


        dropdown.options.Add(option_head);
        dropdown.options.Add(option_body);
        dropdown.options.Add(option_weapon);

    }
}
