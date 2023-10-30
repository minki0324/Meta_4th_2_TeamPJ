using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap_Ge : MonoBehaviour
{
    [SerializeField] private Terrain terrain;

    [Header("맵 크기 설정")]
    [SerializeField] private float width = 400f;    // x값
    [SerializeField] private float length = 400f;   // z값
    [SerializeField] private float height = -1f;   // y값

    private void Awake()
    {
        SetTerrain();
    }

    private void SetTerrain()
    {
        float LocalX = (width / 2) * (-1);
        float LocalZ = (length / 2) * (-1);

        terrain.transform.localPosition = new Vector3(LocalX, 0, LocalZ);
        terrain.terrainData.size = new Vector3(width, height, length);
    }
}
