using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    #region Ÿ��Ʋ �г� 
    [Header("Title Panel")]
    [SerializeField]
    private GameObject Title_Panel;

    [SerializeField]
    private GameObject Login_Panel;

    [SerializeField]
    private GameObject Btn_Panel;

    [SerializeField]
    private Button Confirm_Btn;

    [SerializeField]
    private Button BackButton;

    [SerializeField]
    private Button GameStart_Btn;

    [SerializeField]
    private Button Shop_Btn;

    [SerializeField]
    private Button Avatar_Btn;

    [SerializeField]
    private InputField inputField;



    #endregion

    #region �ƹ�Ÿ �г�
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
    private Dropdown dropdown_Avatar;


    [SerializeField]
    private List<Button> AvatarBtn_List;
    #endregion



    #region �� �г�
    [Header("Shop Panel")]
    [SerializeField]
    private GameObject Shop_Panel;

    [SerializeField]
    private Dropdown dropdown_Shop;

    [SerializeField]
    private Text Spec_Text;



    #endregion

    [Header("��Ÿ")]
    [SerializeField]
    private GameObject Leader_A;

    [SerializeField]
    private GameObject Leader_B;


    private DataManager dataManager;
    private ScriptsData scriptsData;

    public bool isLogined = false;
    public string id = "sunny";

    public int timer = 3;

    private void Awake()
    {
        dataManager = new DataManager();
        scriptsData = dataManager.Load("Scripts");

    }


    private void Start()
    {

        BackButton = transform.GetChild(0).GetComponent<Button>();
        Title_Panel = transform.GetChild(1).gameObject;
        Shop_Panel = transform.GetChild(2).gameObject;
        Avatar_Panel = transform.GetChild(3).gameObject;


        TitlePanel_On();

        Init_FuntionUI();       
    }
    private void Update()
    {
  
    }




    private void Init_FuntionUI()
    {
        //���� �� �ʱ�ȭ�� �Լ���
        BackButton.onClick.AddListener(TitlePanel_On);
        Avatar_Btn.onClick.AddListener(AvatarPanel_On);
        Shop_Btn.onClick.AddListener(ShopPanel_On);

    }


    #region ��
    private void SetDropDownOption_Shop()
    {
        dropdown_Shop.options.Clear();
        

        Dropdown.OptionData option_Healer = new Dropdown.OptionData();
        Dropdown.OptionData option_Halberdier = new Dropdown.OptionData();
        Dropdown.OptionData option_Paladin = new Dropdown.OptionData();

        option_Healer.text = "ȸ����";
        option_Halberdier.text = "������";
        option_Paladin.text = "��ġ..��..?";

        dropdown_Shop.options.Add(option_Healer);
        dropdown_Shop.options.Add(option_Halberdier);
        dropdown_Shop.options.Add(option_Paladin);        

    }

    private void Shop_Function()
    {
        switch (dropdown_Shop.value)
        {
            case 0:
                Debug.Log("ȸ����");
                Spec_Text.text = scriptsData.Scripts[0].Script;

                break;

            case 1:
                Debug.Log("������");

                Spec_Text.text = scriptsData.Scripts[1].Script;
                break;

            case 2:
                Debug.Log("��ġ��");
                Spec_Text.text = scriptsData.Scripts[2].Script;
                break;
        }
      
    }



    
    #endregion



    #region �ƹ�Ÿ
    private void SetDropdownOption_Avatar()
    {
        dropdown_Avatar.options.Clear();

        Dropdown.OptionData option_head = new Dropdown.OptionData();
        Dropdown.OptionData option_body = new Dropdown.OptionData();
        Dropdown.OptionData option_weapon = new Dropdown.OptionData();


        option_head.text = "�Ӹ�";
        option_body.text = "��";
        option_weapon.text = "����";


        dropdown_Avatar.options.Add(option_head);
        dropdown_Avatar.options.Add(option_body);
        dropdown_Avatar.options.Add(option_weapon);

    }


    //update�� �ֱ�
    private void DropDownSelection_Avatar()
    {
        if (dropdown_Avatar.value == 0)
        {
            Debug.Log("111111");
        }
        else if (dropdown_Avatar.value == 1)
        {
            Debug.Log("2222222");
        }
        else if (dropdown_Avatar.value == 2)
        {
            Debug.Log("3333333");
        }
    }

    #endregion





    public void GameStart_Btn_Clicekd()
    {
        //���ξ�
        Debug.Log("Game Start");
    }



    public void TitlePanel_On()
    {
        //�г� on/off
        Title_Panel.SetActive(true);
        Avatar_Panel.SetActive(false);
        Shop_Panel.SetActive(false);


        BackButton.enabled = false;
    
        //������Ʈ �� ����
        Login_Panel = Title_Panel.transform.GetChild(1).gameObject;
        Btn_Panel = Title_Panel.transform.GetChild(2).gameObject;

        GameStart_Btn = Title_Panel.transform.GetChild(2).GetChild(0).GetComponent<Button>();
        Shop_Btn = Title_Panel.transform.GetChild(2).GetChild(1).GetComponent<Button>();
        Avatar_Btn = Title_Panel.transform.GetChild(2).GetChild(2).GetComponent<Button>();

        CheckLogin();
    }

    public void ShopPanel_On()
    {
        

        Title_Panel.SetActive(false);
        Avatar_Panel.SetActive(false);
        Shop_Panel.SetActive(true);

        BackButton.enabled = true;


        dropdown_Shop = Shop_Panel.transform.GetChild(0).GetChild(0).GetComponent<Dropdown>();
        Spec_Text = Shop_Panel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();


        
        SetDropDownOption_Shop();
        dropdown_Shop.onValueChanged.AddListener(delegate { Shop_Function(); });
    }


    public void AvatarPanel_On()
    {
        SetDropdownOption_Avatar();
        Avatar_Panel.SetActive(true);
        Title_Panel.SetActive(false);
        Shop_Panel.SetActive(false);

        BackButton.enabled = true; ;


    }


    public void CheckLogin()
    {
        //�α��� Ȯ��
        if (!isLogined)
        {
            Btn_Panel.SetActive(false);
            Login_Panel.SetActive(true);

            inputField = Login_Panel.transform.GetChild(0).GetComponent<InputField>();
            Confirm_Btn = Login_Panel.transform.GetChild(1).GetComponent<Button>();
            Confirm_Btn.onClick.AddListener(Login);
        }
        else
        {
            Btn_Panel.SetActive(true);
            Login_Panel.SetActive(false);
        }
    }

    public void Login()
    {
        //���Ŀ� �α��� ���� ���� ���� ��
        //�ӽ� ���̵� sunny
        if(inputField.text == id)
        {
            isLogined = true;
        }
        else
        {
            isLogined = false;
        }
        CheckLogin();
    }

}
