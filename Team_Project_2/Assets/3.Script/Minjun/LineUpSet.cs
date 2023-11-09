using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineUpSet : MonoBehaviour
{
    [SerializeField]
    private GameObject Sol1;
    [SerializeField]
    private Button[] buttons;
    private GameObject[] Checkbox;
    private GameObject[] check;
    public List<int> lineupIndexs = new List<int>();
    [SerializeField]private Sprite[] unit;
    private List<Sprite> unit_Sprite = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        

        buttons = GetComponentsInChildren<Button>();
        Checkbox = new GameObject[buttons.Length]; //
        for (int i = 0; i < buttons.Length; i++)
        {
            Checkbox[i] = buttons[i].transform.GetChild(0).gameObject;
            Debug.Log(Checkbox[i]);
        }

        
        lineupIndexs.Add(0);
        unit_Sprite.Add(unit[0]);




    }
    private void Update()
    {
    }

    public void ButtonClicked(int buttonIndex)
    {
        if(lineupIndexs.Count < 3)
        {
            SetLineup(buttonIndex);



            

        }
        else
        {
            for (int i = 0; i < lineupIndexs.Count; i++)
            {
               if(lineupIndexs[i] == buttonIndex)
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
            bool isActive = !Checkbox[buttonIndex].gameObject.activeSelf;
            Checkbox[buttonIndex].gameObject.SetActive(isActive);

            if (isActive)
            {
                lineupIndexs.Add(buttonIndex);
            }
            else
            {
                lineupIndexs.Remove(buttonIndex);
            }
        }
    }

}
