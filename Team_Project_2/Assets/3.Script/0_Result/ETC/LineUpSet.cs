using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineUpSet : MonoBehaviour
{

    [SerializeField]
    //���ּ��� ��ư�� 6��
    private Button[] buttons;
    //��ŸƮ��ư
    [SerializeField] Button startButton;
    //�� ��ư���� üũ�̹���
    private GameObject[] Checkbox;
    //���� ���ξ� ��������Ʈ �ε���
    public List<int> lineupIndexs = new List<int>();
    [Header("Sword > Heavy > Archer > Priest > Spear > Halberdier > Default")]
    //���ֽ�������Ʈ �迭
    [SerializeField] private Sprite[] unitSprite_array;
    //���ξ� UI ��������
    [SerializeField] private GameObject lineupUI;
    //���ξ� UI �� �ִ� ���� ���� ���ֵ�(�̹���)
    private Image[] lineupSprite;
    private bool isCanStart;
    private Color originalColor;

    #region ���� ���� ��ư
    [SerializeField]
    private Button Swordman_Btn;

    [SerializeField]
    private Button Knight_Btn;

    [SerializeField]
    private Button Archer_Btn;

    [SerializeField]
    private Button Prist_Btn;

    [SerializeField]
    private Button Spearman_Btn;

    [SerializeField]
    private Button Halberdier_Btn;


    #endregion



    void Start()
    {

        lineupSprite = lineupUI.GetComponentsInChildren<Image>();
        buttons = GetComponentsInChildren<Button>();
        Checkbox = new GameObject[buttons.Length]; //
        originalColor = buttons[1].colors.normalColor; // ���� ���� ����

        for (int i = 0; i < buttons.Length; i++)
        {
            Checkbox[i] = buttons[i].transform.GetChild(0).gameObject;
        }


        lineupIndexs.Add(0);



        Init_Button();
    }
    private void Update()
    {

        for (int i = 0; i < 3; i++)
        {
            try
            {
                // ���� ���� ���� i��° ��������Ʈ = �������� �ε��� ��°�� ���ֽ�������Ʈ
                lineupSprite[i].sprite = unitSprite_array[lineupIndexs[i]];
            }
            catch
            {
                // lineupIndex �� ����Ʈ�� ��������� Default ��������Ʈ �Ҵ�
                lineupSprite[i].sprite = unitSprite_array[6];
            }
        }
        if (lineupIndexs.Count == 3)
        {
            isCanStart = true;
        }
        if (isCanStart)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
    private void Init_Button()
    {

        Swordman_Btn = gameObject.transform.GetChild(0).GetComponent<Button>();
        Knight_Btn = gameObject.transform.GetChild(1).GetComponent<Button>();
        Archer_Btn = gameObject.transform.GetChild(2).GetComponent<Button>();
        Prist_Btn = gameObject.transform.GetChild(3).GetComponent<Button>();
        Spearman_Btn = gameObject.transform.GetChild(4).GetComponent<Button>();
        Halberdier_Btn = gameObject.transform.GetChild(5).GetComponent<Button>();


        Set_Buttons(ref Swordman_Btn, GameManager.instance.isCanUse_SwordMan);
        Set_Buttons(ref Knight_Btn, GameManager.instance.isCanUse_Knight);
        Set_Buttons(ref Archer_Btn, GameManager.instance.isCanUse_Archer);
        Set_Buttons(ref Spearman_Btn, GameManager.instance.isCanUse_SpearMan);
        Set_Buttons(ref Halberdier_Btn, GameManager.instance.isCanUse_Halberdier);
        Set_Buttons(ref Prist_Btn, GameManager.instance.isCanUse_Prist);

    }

    private void Set_Buttons(ref Button button, bool iscanuse)
    {
        if (iscanuse)
        {
            button.enabled = true;
            button.GetComponent<Image>().color = Color.white;
        }
        else
        {
            button.enabled = false;

            button.GetComponent<Image>().color = Color.black;
        }
    }



    public void ButtonClicked(int buttonIndex)
    {
        if (lineupIndexs.Count < 3)
        {
            //ī��Ʈ�� 2�����ϰ�� �߰�
            SetLineup(buttonIndex);

        }
        else
        {

            //ī��Ʈ�� 3�̻��϶� �߰�����
            //���õ��ִ� ���ֵ鸸 ����Ʈ���� ��������.
            for (int i = 0; i < lineupIndexs.Count; i++)
            {
                if (lineupIndexs[i] == buttonIndex)
                {
                    SetLineup(buttonIndex);
                    break;
                }
            }

        }

    }
    private void SetLineup(int buttonIndex)
    {
        if (buttonIndex >= 1 && buttonIndex < Checkbox.Length)
        {
            //üũ�� ��������� ���ְ� �ȵ�������� ����
            bool isActive = !Checkbox[buttonIndex].gameObject.activeSelf;
            Checkbox[buttonIndex].gameObject.SetActive(isActive);
            if (isActive)
            {
                //üũǥ���ϴ� ���ÿ� ���� ��������Ʈ �ε����� �߰�
                lineupIndexs.Add(buttonIndex);

            }
            else
            {

                Debug.Log("����Ʈ����");
                //üũǪ�� ���ÿ� ���� ��������Ʈ �ε����� ����
                lineupIndexs.Remove(buttonIndex);
                //Color c = buttons[buttonIndex].colors.selectedColor;
                // c   = originalColor;
            }
        }
    }

    public void UnitSetToPlayer()
    {
        GameManager.instance.unit0 = GameManager.instance.units[lineupIndexs[0]];
        GameManager.instance.unit1 = GameManager.instance.units[lineupIndexs[1]];
        GameManager.instance.unit2 = GameManager.instance.units[lineupIndexs[2]];

        //�̽������� resultPanel �ʱ�ȭ
        GameManager.instance.Result = FindObjectOfType<ToggleMenu>().transform.GetChild(4).gameObject;
        GameManager.instance.Result.SetActive(false);
        GameManager.instance.txt = GameManager.instance.Result.transform.GetChild(10).GetComponent<Text>();
    }

    public void Normal_Mode()
    {
        GameManager.instance.GameSpeed = 0;
    }

    public void Speed_Mode()
    {
        GameManager.instance.GameSpeed = 1;
    }
}


