using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    #region 타이틀 패널 
    [Header("Title Panel")]
    [SerializeField]
    private GameObject Title_Panel;

    [SerializeField]
    private GameObject Login_Panel;

    [SerializeField]
    private GameObject Btn_Panel;

    //로그인 패널
    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Button Confirm_Btn;

    //버튼 패널
    [SerializeField]
    private Button Ready_Btn;

    [SerializeField]
    private Button Upgrade_Btn;

    [SerializeField]
    private Button Option_Btn;

    [SerializeField]
    private Button Exit_Btn;



    //뒤로가기
    [SerializeField]
    private Button BackButton;
    #endregion



    #region Setup 패널(Ready 패널)

    [Header("Setup Panel")]
    [SerializeField]
    private GameObject Setup_Panel;

    [SerializeField]
    private Image Map_img;


    [SerializeField]
    private Button TeamColor1_Btn;

    [SerializeField]
    private Button TeamColor2_Btn;

    [SerializeField]
    private Button TeamColor3_Btn;

    [SerializeField]
    private Button TeamColor4_Btn;

    [SerializeField]
    private Button Original_Btn;

    [SerializeField]
    private Button TimeAttack_Btn;

    [SerializeField]
    private Button GameStart_Btn;


    [SerializeField]
    private Text Team1_Text;

    [SerializeField]
    private Text Team2_Text;

    [SerializeField]
    private Text Team3_Text;

    [SerializeField]
    private Text Team4_Text;


    [SerializeField]
    private Text ID1_Text;

    [SerializeField]
    private Text ID2_Text;

    [SerializeField]
    private Text ID3_Text;

    [SerializeField]
    private Text ID4_Text;

    #endregion



    #region 업그레이드 패널
    [Header("Avatar Panel")]
    [SerializeField]
    private GameObject Upgrade_Panel;

    private enum DropDownMenu
    {
        Head = 0,
        Body,
        Weapon

    }

    [SerializeField]
    private DropDownMenu menu_list;


    #endregion



    #region 옵션 패널
    [Header("Shop Panel")]
    [SerializeField]
    private GameObject Option_Panel;
    #endregion



    #region 기타
    [Header("기타")]
    [SerializeField]
    private GameObject Leader_A;

    [SerializeField]
    private GameObject Leader_B;

    private DataManager dataManager;
    private ScriptsData scriptsData;

    public bool isLogined = false;
    public string id = "sunny";

    public int timer = 3;

    #endregion



    private void Awake()
    {
        dataManager = new DataManager();
        scriptsData = dataManager.Load("Scripts");
    }

    private void Start()
    {
        BackButton = transform.GetChild(0).GetComponent<Button>();

        Title_Panel = transform.GetChild(0).gameObject;
        Setup_Panel = transform.GetChild(1).gameObject;
        Upgrade_Panel = transform.GetChild(2).gameObject;
        Option_Panel = transform.GetChild(3).gameObject;


        Init_FuntionUI();

        TitlePanel_On();

             
    }

    private void Init_FuntionUI()
    {
        //시작 시 초기화할 함수들

        //Title_Panel = transform.GetChild(0).gameObject;
        BackButton.onClick.AddListener(TitlePanel_On);

        Exit_Btn.onClick.AddListener(Exit);
    }


    public void GameStart_Btn_Clicekd()
    {
        //메인씬
        Debug.Log("Game Start");
    }

    public void TitlePanel_On()
    {
        //패널 on/off
        Title_Panel.SetActive(true);
        Setup_Panel.SetActive(false);
        Upgrade_Panel.SetActive(false);
        Option_Panel.SetActive(false);

    
        //오브젝트 값 연결
        Login_Panel = Title_Panel.transform.GetChild(1).gameObject;
        Btn_Panel = Title_Panel.transform.GetChild(2).gameObject;

        Ready_Btn = Btn_Panel.transform.GetChild(0).GetComponent<Button>();
        Upgrade_Btn = Btn_Panel.transform.GetChild(0).GetComponent<Button>();
        Option_Btn = Btn_Panel.transform.GetChild(0).GetComponent<Button>();
        Exit_Btn = Btn_Panel.transform.GetChild(0).GetComponent<Button>();


        Ready_Btn.onClick.AddListener(SetupPanel_On);
        Upgrade_Btn.onClick.AddListener(UpgradePanel_On);
        Option_Btn.onClick.AddListener(OptionPanel_On);
        Exit_Btn.onClick.AddListener(Exit);

        CheckLogin();
    }

    public void SetupPanel_On()
    {
        Setup_Panel.SetActive(true);
        Upgrade_Panel.SetActive(false);
        Option_Panel.SetActive(false);

        

        GameObject Selection = Setup_Panel.transform.GetChild(0).gameObject;

        TeamColor1_Btn = Selection.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        TeamColor2_Btn = Selection.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        TeamColor3_Btn = Selection.transform.GetChild(2).GetChild(0).GetComponent<Button>();
        TeamColor4_Btn = Selection.transform.GetChild(3).GetChild(0).GetComponent<Button>();

        Map_img = Selection.transform.GetChild(4).GetComponent<Image>();
        Original_Btn = Selection.transform.GetChild(5).GetComponent<Button>();
        Original_Btn = Selection.transform.GetChild(6).GetComponent<Button>();
        GameStart_Btn = Selection.transform.GetChild(7).GetComponent<Button>();


        Team1_Text = Selection.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        Team2_Text = Selection.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        Team3_Text = Selection.transform.GetChild(2).GetChild(1).GetComponent<Text>();
        Team4_Text = Selection.transform.GetChild(3).GetChild(1).GetComponent<Text>();

        ID1_Text = Selection.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        ID2_Text = Selection.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        ID3_Text = Selection.transform.GetChild(2).GetChild(2).GetComponent<Text>();
        ID4_Text = Selection.transform.GetChild(3).GetChild(2).GetComponent<Text>();


        Original_Btn.onClick.AddListener(OriginalBtn_Clicked);
        TimeAttack_Btn.onClick.AddListener(TimeAttackBtn_Clicked);
        GameStart_Btn.onClick.AddListener(GameStart_Btn_Clicekd);

        TeamColor1_Btn.onClick.AddListener(Change_Color);



    }

    private void Change_Color()
    {
        TeamColor1_Btn.image.color = Color.black;
    }



    public void UpgradePanel_On()
    {
        Setup_Panel.SetActive(false);
        Upgrade_Panel.SetActive(true);
        Option_Panel.SetActive(false);

    }

    public void OptionPanel_On()
    {
        Setup_Panel.SetActive(false);
        Upgrade_Panel.SetActive(false);
        Option_Panel.SetActive(true);
    }

    public void CheckLogin()
    {
        //로그인 확인
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
        //추후에 로그인 파일 만들어서 비교할 것
        //임시 아이디 sunny
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

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }


    public void OriginalBtn_Clicked()
    {
        //맵이미지 Original로 변경
    }

    public void TimeAttackBtn_Clicked()
    {
        //맵이미지 Original로 변경
    }
}
