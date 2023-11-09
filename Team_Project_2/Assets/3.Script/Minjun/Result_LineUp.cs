using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_LineUp : MonoBehaviour
{
    private Image[] images;
    [SerializeField]private Sprite[] sprites;
    private List<Image> lineUp = new List<Image>();
    [SerializeField] private LineUpSet lineUpSet;
    // Update is called once per frame
    private void Awake()
    {
        
        images = GetComponentsInChildren<Image>();
     
    }
    void Update()
    {
        //for (int i = 0; i < lineUp.Count; i++)
        //{
        //    if (lineUp[i] == null)
        //    {
        //        images[i].sprite = sprites[0];
        //    }
        //    else
        //    {
        //        images[i].sprite = sprites[lineUpSet.lineupIndexs[i]];
        //    }




        //}
        Debug.Log(lineUpSet.lineupIndexs.Count);
        if (lineUpSet.lineupIndexs.Count > 0)
        {
            for (int i = 0; i < lineUpSet.lineupIndexs.Count; i++)
            {

                images[i].sprite = sprites[lineUpSet.lineupIndexs[i]];
            }
        }
        
        //else if(lineUpSet.lineupIndexs.Count < 3)
        //{

        //}
    }
}
