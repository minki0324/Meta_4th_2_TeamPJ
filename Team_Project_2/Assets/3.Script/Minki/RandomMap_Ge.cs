using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap_Ge : MonoBehaviour
{
    [SerializeField] private Terrain terrain;

    [Header("맵 크기 설정")]
    [SerializeField] private float width = 400f;    // x값
    [SerializeField] private float length = 400f;   // z값
    [SerializeField] private float height = 100f;   // y값

    [Header("맵 높이 설정")]
    [SerializeField] private float mountainHeight = 40f;
    [SerializeField] private float detailScale = 20f;
    [SerializeField] private int numberOfMountains = 8;
    [SerializeField] private int brushSize = 30;

    private void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        SetTerrain();
        SetHeight();
    }

    private void SetTerrain()
    {
        float LocalX = (width / 2) * (-1);
        float LocalZ = (length / 2) * (-1);

        terrain.transform.localPosition = new Vector3(LocalX, 0, LocalZ);
        terrain.terrainData.size = new Vector3(width, height, length);
    }

    private void SetHeight()
    {
        // TerrainData terrainData = terrain.terrainData;
        // int heightmapWidth = terrainData.heightmapResolution;
        // int heightmapHeight = terrainData.heightmapResolution;
        // 
        // float[,] heights = new float[heightmapWidth, heightmapHeight];
        // 
        // float inverseDetailScale = 1f / detailScale;
        // float inverseHeight = 1f / height;
        // float inverseBrushSizeSquared = 1f / (brushSize * brushSize);
        // 
        // for (int m = 0; m < numberOfMountains; m++)
        // {
        //     // 산의 시작 위치를 랜덤으로 설정
        //     Vector2 mountainStart = new Vector2(Random.Range(0, heightmapWidth), Random.Range(0, heightmapHeight));
        // 
        //     // 산의 방향을 랜덤으로 설정
        //     Vector2 direction = Random.insideUnitCircle.normalized;
        // 
        //     // 산을 그리기
        //     for (float i = 0; i < heightmapWidth; i += 0.1f)
        //     {
        //         int x = Mathf.RoundToInt(mountainStart.x + direction.x * i);
        //         int z = Mathf.RoundToInt(mountainStart.y + direction.y * i);
        // 
        //         if (x < 0 || x >= heightmapWidth || z < 0 || z >= heightmapHeight) continue;
        // 
        //         Vector2 mountainPoint = new Vector2(x, z);
        //         for (int brushX = -brushSize; brushX <= brushSize; brushX++)
        //         {
        //             for (int brushZ = -brushSize; brushZ <= brushSize; brushZ++)
        //             {
        //                 int posX = x + brushX;
        //                 int posZ = z + brushZ;
        // 
        //                 if (posX < 0 || posX >= heightmapWidth || posZ < 0 || posZ >= heightmapHeight) continue;
        // 
        //                 Vector2 brushPoint = new Vector2(posX, posZ);
        //                 float distanceToCenterSquared = (brushPoint - mountainPoint).sqrMagnitude;
        // 
        //                 if (distanceToCenterSquared > brushSize * brushSize) continue;
        // 
        //                 float intensity = 1.0f - Mathf.Sqrt(distanceToCenterSquared * inverseBrushSizeSquared);
        // 
        //                 float distanceToMountainStart = Vector2.Distance(brushPoint, mountainStart) / heightmapWidth;
        //                 float heightAtPoint = Mathf.PerlinNoise(posX * inverseDetailScale, posZ * inverseDetailScale) * mountainHeight * (1 - distanceToMountainStart) * intensity;
        // 
        //                 heights[posX, posZ] = Mathf.Max(heights[posX, posZ], heightAtPoint * inverseHeight);
        //             }
        //         }
        //     }
        // }
        // terrainData.SetHeights(0, 0, heights);

        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        // Compute Shader를 설정하고 실행합니다.
        // ... (Compute Shader 설정 및 실행 코드)

        // GPU에서 결과를 CPU로 가져오기 위해 Render Texture를 사용합니다.
        RenderTexture heightMapRT = new RenderTexture(heightmapWidth, heightmapHeight, 0, RenderTextureFormat.RFloat);
        heightMapRT.enableRandomWrite = true;
        heightMapRT.Create();

        // Compute Shader 결과를 Render Texture에 저장합니다.
        // ... (Compute Shader 결과 저장 코드)

        // Render Texture에서 결과를 읽어와서 Terrain의 높이맵에 적용합니다.
        Texture2D heightMapTexture = new Texture2D(heightmapWidth, heightmapHeight, TextureFormat.RFloat, false);
        RenderTexture.active = heightMapRT;
        heightMapTexture.ReadPixels(new Rect(0, 0, heightmapWidth, heightmapHeight), 0, 0);
        heightMapTexture.Apply();
        RenderTexture.active = null;

        float[,] heights = new float[heightmapWidth, heightmapHeight];
        for (int x = 0; x < heightmapWidth; x++)
        {
            for (int z = 0; z < heightmapHeight; z++)
            {
                heights[x, z] = heightMapTexture.GetPixel(x, z).r;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
