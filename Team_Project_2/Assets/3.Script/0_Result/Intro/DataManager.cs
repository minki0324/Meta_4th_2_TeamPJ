using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Text;


#region �÷��̾� ������
[System.Serializable]
public class Player_Data
{
    public List<PlayerData> playerData;
}
[System.Serializable]
public class PlayerData
{
    public string ID;
    public int Coin;
    public bool SwordMan;
    public bool Knight;
    public bool Archer;
    public bool SpearMan;
    public bool Halberdier;
    public bool Prist;
}


#endregion

public class DataManager : MonoBehaviour
{
    #region �̱���..?
    //public static DataManager instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(instance.gameObject);
    //    }
    //    DontDestroyOnLoad(this.gameObject);

    //}

    #endregion



    //public string path = "C:/Users/KGA/Documents/GitHub/Meta_4th_2_TeamPJ/Team_Project_2/JsonFiles/";
    public string filename = "playerData.json";
    Player_Data player_Data = new Player_Data();

    public void Save_playerData(string id, int coin, bool swordman, bool knight, bool archer, bool spearMan, bool halberdier, bool priest)
    {
       
        PlayerData player = new PlayerData();
        player.ID = id;
        player.Coin = coin;
        player.SwordMan = swordman;
        player.Knight = knight;
        player.Archer = archer;
        player.SpearMan = spearMan;
        player.Halberdier = halberdier;
        player.Prist = priest;
      
        string path = Application.streamingAssetsPath;
        filename = Path.Combine(path, filename);
       


        string loadJson = File.ReadAllText(filename);    //json������ ���� �ҷ�����

        player_Data = JsonConvert.DeserializeObject<Player_Data>(loadJson); //Ǯ��~
        Debug.Log(path);

        player_Data.playerData.Add(player); //���� �߰��ϰ�~

        string data = JsonUtility.ToJson(player_Data);  //�ٽ� �ְ�~
        File.WriteAllText(filename, data);
    }






    public Player_Data Load_playerData(string filename)
    {
        //if (!filename.Contains(".json"))
        //{
        //    filename += ".json";
        //}

        string path = Application.streamingAssetsPath;
        string _filename = path + "/" + filename;
       
        string ReadData = File.ReadAllText(_filename);


        Player_Data playerdata = new Player_Data();

        playerdata = JsonConvert.DeserializeObject<Player_Data>(ReadData);

        return playerdata;
    }


}
