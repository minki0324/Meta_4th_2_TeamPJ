using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct TeamColor
{
    public int ColorNum;
    public Color color_c;
    public Material color_m;
    public bool isUsing;
}

public class GetColor : MonoBehaviour
{
    public static GetColor instance = null;
    [SerializeField]
    public Material[] TeamColors;

    [SerializeField]
    public TeamColor[] teamColors = new TeamColor[11];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < TeamColors.Length; i++)
        {
            teamColors[i].color_m = TeamColors[i];
            teamColors[i].color_c = TeamColors[i].color;
            teamColors[i].ColorNum = i;
            teamColors[i].isUsing = false;
        }

    }


}
