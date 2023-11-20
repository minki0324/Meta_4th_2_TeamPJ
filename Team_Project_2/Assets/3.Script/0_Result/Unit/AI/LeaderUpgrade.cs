using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderUpgrade : MonoBehaviour
{
    private bool ableUpgrade = false;

    private List<int> Upgrade_List;
    private int Upgrade_Num;
    private LeaderController leadercon;
    private Enemy_Upgrade enemy_upgrade;

    private void Start()
    {
        Upgrade_List = new List<int>(10) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        leadercon = GetComponent<LeaderController>();
        enemy_upgrade = GetComponent<Enemy_Upgrade>();
    }

    public void Update()
    {
        if (GameManager.instance.currentTime % 180 < 5 && GameManager.instance.currentTime > 180)  // default 3minute
        {
            ableUpgrade = true;
        }
        if (ableUpgrade && leadercon.Target.CompareTag("Base") && leadercon.isArrive(leadercon.Target)) 
        {
            int Temp = Random.Range(0, Upgrade_List.Count);
            Upgrade_Num = Upgrade_List[Temp];
            enemy_upgrade.Upgradeall(1, Upgrade_Num);

            Upgrade_List.Remove(Upgrade_Num);
            ableUpgrade = false;
        }


    }
}
