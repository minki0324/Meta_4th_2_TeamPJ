using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;




public struct TeamColor
{
    public int ColorNum;
    public Color color_c;
    public Material color_m;
    public bool isUsing;
}

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

    [SerializeField]
    private Button SignUp_Btn;

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
    [Header("Upgrade Panel")]
    private ScrollRect Upgrade_Panel;

    [SerializeField]
    private List<GameObject> Upgrade_Items;

    [SerializeField]
    private Text Coin_Text;

    #endregion



    #region 옵션 패널
    [Header("Option Panel")]

    [SerializeField]
    private Optioin_Panel Option_Panel;



    #endregion


    #region 회원가입 패널
    [SerializeField]
    private GameObject SignUp_Panel;

    [SerializeField]
    private InputField SignUp_inputField;

    [SerializeField]
    private Button SignUp_Confirm_Btn;

    [SerializeField]
    private Button SignUp_Cancel_Btn;
    #endregion


    #region 경고 패널
    [SerializeField]
    private GameObject Warning_Panel;
    #endregion


    #region 기타
    [Header("기타")]

    //배경관련
    [SerializeField]
    private GameObject Leader_A;

    [SerializeField]
    private GameObject Leader_B;


   // public float mouseSensitivity;

    private float MouseY;
    private float MouseX;


    //json 관련
    private DataManager dataManager;
    private ScriptsData scriptsData;
    private Player_Data playerData;

    //로그인 관련
    public bool isLogined = false;

    public string id = "sunny";

    public int playerID_index;

    //소리 관련
    public AudioMixer audioMixer;

   

    //팀관련
    [Header("팀 컬러")]
    [SerializeField]
    private Material[] TeamColors;

    [SerializeField]
    public TeamColor[] teamColors = new TeamColor[11];


    //GameManager에 보낼 팀 별 색상 번호, ColorSet 인덱스와 동일한 숫자
    public int Team1_Color;
    public int Team2_Color;
    public int Team3_Color;
    public int Team4_Color;




    #endregion




    private void Awake()
    {
        // gameObject.SetActive(true);
        
   
        dataManager = new DataManager();
        scriptsData = dataManager.Load("Scripts");
        playerData = dataManager.Load_playerData("playerData");

       
    }

    private void Start()
    {
      

        Screen.SetResolution(1920, 1080, true);

        Init_FuntionUI();


       
        TitlePanel_On();

     
    }



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Login();
        }
    }

    private void Init_FuntionUI()
    {
        //시작 시 초기화할 것들

        BackButton = transform.GetChild(0).GetComponent<Button>();
        BackButton.onClick.AddListener(BackBtn_Clicked);
        BackButton.gameObject.SetActive(false);

        Title_Panel = transform.GetChild(1).gameObject;
        Setup_Panel = transform.GetChild(2).gameObject;
        Upgrade_Panel = transform.GetChild(3).GetComponent<ScrollRect>();
        Option_Panel = transform.GetChild(4).GetComponent<Optioin_Panel>();
        SignUp_Panel = transform.GetChild(5).gameObject;
        Warning_Panel = transform.GetChild(6).gameObject;



        
        //구조체 배열 초기화
        for (int i = 0; i < TeamColors.Length; i++)
        {
            teamColors[i].color_m = TeamColors[i];
            teamColors[i].color_c = TeamColors[i].color;
            teamColors[i].ColorNum = i;
            teamColors[i].isUsing = false;
        }


       
    }

    public void GameStart_Btn_Clicekd()
    {
        GameManager.instance.Color_Index = Team1_Color;
        GameManager.instance.T1_Color = Team2_Color;
        GameManager.instance.T2_Color = Team3_Color;
        GameManager.instance.T3_Color = Team4_Color;

        SceneManager.LoadScene(1);
    }

    public void TitlePanel_On()
    {
        //패널 on/off
        Title_Panel.SetActive(true);
        Setup_Panel.SetActive(false);
        Upgrade_Panel.gameObject.SetActive(false);
        Option_Panel.gameObject.SetActive(false);
        SignUp_Panel.SetActive(false);
        Warning_Panel.SetActive(false);

        Title_Panel.GetComponent<Image>().raycastTarget = false;
      
        BackButton.gameObject.SetActive(false);
        BackButton.enabled = false;

        //오브젝트 값 연결
        Login_Panel = Title_Panel.transform.GetChild(1).gameObject;
        Btn_Panel = Title_Panel.transform.GetChild(2).gameObject;

        Ready_Btn = Btn_Panel.transform.GetChild(0).GetComponent<Button>();
        Upgrade_Btn = Btn_Panel.transform.GetChild(1).GetComponent<Button>();
        Option_Btn = Btn_Panel.transform.GetChild(2).GetComponent<Button>();
        Exit_Btn = Btn_Panel.transform.GetChild(3).GetComponent<Button>();


        

        Ready_Btn.onClick.AddListener(SetupPanel_On);
        Upgrade_Btn.onClick.AddListener(UpgradePanel_On);
        Option_Btn.onClick.AddListener(OptionPanel_On);
        Exit_Btn.onClick.AddListener(Exit);

       
        CheckLogin();
    }

    public void SetupPanel_On()
    {
        Setup_Panel.SetActive(true);
        Upgrade_Panel.gameObject.SetActive(false);
        Option_Panel.gameObject.SetActive(false);
        SignUp_Panel.SetActive(false);
        Warning_Panel.SetActive(false);

        BackButton.gameObject.SetActive(true);
        BackButton.enabled = true;

        GameObject Selection = Setup_Panel.transform.GetChild(0).gameObject;

        #region 오브젝트 연결
        TeamColor1_Btn = Selection.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        TeamColor2_Btn = Selection.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        TeamColor3_Btn = Selection.transform.GetChild(2).GetChild(0).GetComponent<Button>();
        TeamColor4_Btn = Selection.transform.GetChild(3).GetChild(0).GetComponent<Button>();

        Map_img = Selection.transform.GetChild(4).GetComponent<Image>();
        Original_Btn = Selection.transform.GetChild(5).GetComponent<Button>();
        TimeAttack_Btn = Selection.transform.GetChild(6).GetComponent<Button>();
        GameStart_Btn = Selection.transform.GetChild(7).GetComponent<Button>();


        Team1_Text = TeamColor1_Btn.transform.GetChild(0).GetComponent<Text>();
        Team2_Text = TeamColor2_Btn.transform.GetChild(0).GetComponent<Text>();
        Team3_Text = TeamColor3_Btn.transform.GetChild(0).GetComponent<Text>();
        Team4_Text = TeamColor4_Btn.transform.GetChild(0).GetComponent<Text>(); 

        ID1_Text = TeamColor1_Btn.transform.GetChild(1).GetComponent<Text>();
        ID2_Text = TeamColor2_Btn.transform.GetChild(1).GetComponent<Text>();
        ID3_Text = TeamColor3_Btn.transform.GetChild(1).GetComponent<Text>();
        ID4_Text = TeamColor4_Btn.transform.GetChild(1).GetComponent<Text>();

        #endregion


        #region 버튼 함수 호출
        Original_Btn.onClick.AddListener(OriginalBtn_Clicked);
        TimeAttack_Btn.onClick.AddListener(TimeAttackBtn_Clicked);
        GameStart_Btn.onClick.AddListener(GameStart_Btn_Clicekd);

        TeamColor1_Btn.onClick.AddListener(delegate { Change_Team_Color(TeamColor1_Btn, ref Team1_Color); });
        TeamColor2_Btn.onClick.AddListener(delegate { Change_Team_Color(TeamColor2_Btn, ref Team2_Color); });
        TeamColor3_Btn.onClick.AddListener(delegate { Change_Team_Color(TeamColor3_Btn, ref Team3_Color); });
        TeamColor4_Btn.onClick.AddListener(delegate { Change_Team_Color(TeamColor4_Btn, ref Team4_Color); });

        #endregion

        ID1_Text.text = id;

        TeamColor1_Btn.image.color = teamColors[8].color_c;
        TeamColor2_Btn.image.color = teamColors[2].color_c;
        TeamColor3_Btn.image.color = teamColors[5].color_c;
        TeamColor4_Btn.image.color = teamColors[10].color_c;

        teamColors[8].isUsing = true;
        teamColors[2].isUsing = true;
        teamColors[5].isUsing = true;
        teamColors[10].isUsing = true;

        Team1_Color = teamColors[8].ColorNum;
        Team2_Color = teamColors[2].ColorNum;
        Team3_Color = teamColors[5].ColorNum;
        Team4_Color = teamColors[10].ColorNum;



    }

    public void Change_Team_Color(Button button, ref int teamNum)
    {
        Debug.Log("컬러체인지");
      
        for (int i = teamNum; i < teamColors.Length; i++)
        {
            if (button.image.color == teamColors[i].color_c)
            {
                for (int j = 0; j < teamColors.Length; j++)
                {
                    if (i + j >= 10)
                    {
                        if (teamColors[i + j - 10].isUsing == false)
                        {
                            button.image.color = teamColors[i + j - 10].color_c;
                            teamColors[i + j - 10].isUsing = true;
                            teamColors[i].isUsing = false;
                            teamNum = i + j - 10;
                            break;
                        }
                    }
                    else
                    {
                        if (teamColors[i + j].isUsing == false)
                        {
                            button.image.color = teamColors[i + j].color_c;
                            teamColors[i + j].isUsing = true;
                            teamColors[i].isUsing = false;
                            teamNum = i + j;
                            break;
                        }
                    }
                }
                break;
            }
        }
    }




    public void UpgradePanel_On()
    {
        Setup_Panel.SetActive(false);
        Upgrade_Panel.gameObject.SetActive(true);
        Option_Panel.gameObject.SetActive(false);
        SignUp_Panel.SetActive(false);
        Warning_Panel.SetActive(false);

        BackButton.gameObject.SetActive(true);
        BackButton.enabled = true;

        Coin_Text = Upgrade_Panel.transform.GetChild(2).GetComponent<Text>();
        Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";

        Init_Scroll_Item();


    }

    private void Init_Scroll_Item()
    {
       
        for (int i = 0; i < 7; i++)
        {
            GameObject a = Upgrade_Panel.transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
            Upgrade_Items.Add(a);
        }

        for(int i = 0; i<Upgrade_Items.Count; i++)
        {
            Upgrade_Items[i].transform.GetChild(1).GetComponent<Text>().text = scriptsData.Scripts[i].Title;
            Upgrade_Items[i].transform.GetChild(2).GetComponent<Text>().text = scriptsData.Scripts[i].Script;
            Upgrade_Items[i].transform.GetChild(3).GetComponent<Text>().text = $"{scriptsData.Scripts[i].Price}c";
        }

        #region 구매여부
        if (GameManager.instance.isCanUse_SwordMan)
        {
            Upgrade_Items[0].GetComponent<Button>().enabled = false;
            Upgrade_Items[0].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[0].GetComponent<Button>().enabled = true;
            Upgrade_Items[0].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[0].Price)
                {
                    GameManager.instance.isCanUse_SwordMan = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[0].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[0].GetComponent<Button>().enabled = false;
                    Upgrade_Items[0].GetComponent<Image>().color = Color.black;
                    
                }
            });    
        }

        if (GameManager.instance.isCanUse_Knight)
        {
            Upgrade_Items[1].GetComponent<Button>().enabled = false;
            Upgrade_Items[1].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[0].GetComponent<Button>().enabled = true;
            Upgrade_Items[0].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[1].Price)
                {
                    GameManager.instance.isCanUse_Knight = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[1].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[1].GetComponent<Button>().enabled = false;
                    Upgrade_Items[1].GetComponent<Image>().color = Color.black;

                }
            });
        }

        if (GameManager.instance.isCanUse_Archer)
        {
            Upgrade_Items[2].GetComponent<Button>().enabled = false;
            Upgrade_Items[2].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[2].GetComponent<Button>().enabled = true;
            Upgrade_Items[2].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[2].Price)
                {
                    GameManager.instance.isCanUse_Archer = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[2].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[2].GetComponent<Button>().enabled = false;
                    Upgrade_Items[2].GetComponent<Image>().color = Color.black;

                }
            });
        }

        if (GameManager.instance.isCanUse_SpearMan)
        {
            Upgrade_Items[3].GetComponent<Button>().enabled = false;
            Upgrade_Items[3].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[3].GetComponent<Button>().enabled = true;
            Upgrade_Items[3].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[3].Price)
                {
                    GameManager.instance.isCanUse_SpearMan = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[3].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[3].GetComponent<Button>().enabled = false;
                    Upgrade_Items[3].GetComponent<Image>().color = Color.black;

                }
            });
        }

        if (GameManager.instance.isCanUse_Halberdier)
        {
            Upgrade_Items[4].GetComponent<Button>().enabled = false;
            Upgrade_Items[4].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[4].GetComponent<Button>().enabled = true;
            Upgrade_Items[4].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[4].Price)
                {
                    GameManager.instance.isCanUse_SwordMan = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[4].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[4].GetComponent<Button>().enabled = false;
                    Upgrade_Items[4].GetComponent<Image>().color = Color.black;

                }
            });
        }

        if (GameManager.instance.isCanUse_Prist)
        {
            Upgrade_Items[5].GetComponent<Button>().enabled = false;
            Upgrade_Items[5].GetComponent<Image>().color = Color.black;
        }
        else
        {
            Upgrade_Items[5].GetComponent<Button>().enabled = true;
            Upgrade_Items[5].GetComponent<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[5].Price)
                {
                    GameManager.instance.isCanUse_SwordMan = true;
                    GameManager.instance.PlayerCoin -= scriptsData.Scripts[5].Price;
                    Coin_Text.text = $"COIN : {GameManager.instance.PlayerCoin.ToString()}c";
                    Upgrade_Items[5].GetComponent<Button>().enabled = false;
                    Upgrade_Items[5].GetComponent<Image>().color = Color.black;

                }
            });
        }
        #endregion



    }

    public void BuyBtn_Clicked(Button button, bool isHave, int solidernum)
    {
        if(isHave)
        {
            button.enabled = false;
            button.GetComponent<Image>().color = Color.black;
        }
        else
        {
            button.enabled = true;
            button.onClick.AddListener(() =>
            {
                if (GameManager.instance.PlayerCoin >= scriptsData.Scripts[solidernum].Price)
                {
                    
                    button.enabled = false;
                    button.GetComponent<Image>().color = Color.black;
                }

            });
        }
      
    }


    public void OptionPanel_On()
    {
        
        Setup_Panel.SetActive(false);
        Upgrade_Panel.gameObject.SetActive(false);
        //Option_Panel.SetActive(true);
        Option_Panel.gameObject.SetActive(true);
        Option_Panel.OptionPanel_On();
        SignUp_Panel.SetActive(false);
        Warning_Panel.SetActive(false);

        BackButton.gameObject.SetActive(true);
        BackButton.enabled = true;
    }


    public void SignUpPanel_On()
    {
        //오브젝트 연결
        SignUp_Panel.SetActive(true);
        SignUp_inputField = SignUp_Panel.transform.GetChild(1).GetComponent<InputField>();
        SignUp_Confirm_Btn = SignUp_Panel.transform.GetChild(2).GetComponent<Button>();
        SignUp_Cancel_Btn = SignUp_Panel.transform.GetChild(3).GetComponent<Button>();

        //버튼 이벤트
        SignUp_Confirm_Btn.onClick.AddListener(Check_SignUp);
        SignUp_Cancel_Btn.onClick.AddListener(() => {
            SignUp_Panel.SetActive(false);
            inputField.enabled = true;
            inputField.ActivateInputField();
            SignUp_inputField.text = "";
        });

        inputField.enabled = false;
        SignUp_inputField.ActivateInputField();
    }

    public void WarningPanel_Off()
    {

        Warning_Panel.SetActive(false);

    }



    public void Check_SignUp()
    {

       
        for(int i = 0; i< playerData.playerData.Count; i++)
        {
            if (playerData.playerData[i].ID != SignUp_inputField.text || SignUp_inputField.text != "")
            {
                dataManager.Save_playerData(SignUp_inputField.text, 0, true, true, true, false, false, false);
            }
            else
            {
                Warning_Panel.SetActive(true);
            }
        }

      

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
            SignUp_Btn = Login_Panel.transform.GetChild(2).GetComponent<Button>();

            Confirm_Btn.onClick.AddListener(Login);
            SignUp_Btn.onClick.AddListener(SignUpPanel_On);

            //시작시 inputField에 커서
            inputField.ActivateInputField();
        }
        else
        {
            Btn_Panel.SetActive(true);
            Login_Panel.SetActive(false);
        }

       
    }

    public void Login()
    {
        
        for (int i = 0; i < playerData.playerData.Count; i++)
        {
            if(playerData.playerData[i].ID == inputField.text)
            {
                playerID_index = i;
                GameManager.instance.PlayerID = playerData.playerData[i].ID;
                GameManager.instance.PlayerCoin = playerData.playerData[i].Coin;
                GameManager.instance.isCanUse_SwordMan = playerData.playerData[i].SwordMan;
                GameManager.instance.isCanUse_Knight = playerData.playerData[i].Knight;
                GameManager.instance.isCanUse_Archer = playerData.playerData[i].Archer;
                GameManager.instance.isCanUse_SpearMan = playerData.playerData[i].SpearMan;
                GameManager.instance.isCanUse_Halberdier = playerData.playerData[i].Halberdier;
                GameManager.instance.isCanUse_Prist = playerData.playerData[i].Prist;



                Debug.Log("로그인됨ㅎㅎ");

                isLogined = true;
            }
      
        }

        //if(inputField.text == id)
        //{
        //    isLogined = true;   
        //}
        //else
        //{
        //    isLogined = false;
        //}

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
        Debug.Log("오리지널 맵 이미지");
    }

    public void TimeAttackBtn_Clicked()
    {
        //맵이미지 타임어택으로 변경
        Debug.Log("타임어택 맵 이미지");
    }

    
    public void BackBtn_Clicked()
    {
        Debug.Log("Backbutton 클릭됨");
        TitlePanel_On();
    }


}
