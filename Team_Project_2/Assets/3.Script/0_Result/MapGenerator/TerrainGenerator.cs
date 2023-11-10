using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using ProceduralNoiseProject;
using Pathfinding.Util;

namespace SimpleProceduralTerrainProject
{
    /*
        1. 랜덤 터레인 생성 (각종 노이즈 로직들 기반으로 랜덤한 높이의 언덕 생성)
        2. 랜덤 지형으로 생성된 터레인에 랜덤한 위치에 베이스, 플래그 생성
        3. 베이스 > 플래그로 가는 길 에이스타 알고리즘을 통해 경로를 파악하여 해당 위치에 도로 텍스춰 입히기
    */
    public class TerrainGenerator : MonoBehaviour
    {

        [SerializeField]
        private GameObject Terra;

        //Prototypes
        public Texture2D[] m_splat; // 텍스춰 배열
        public float[] m_splatTileSizes; // 타일 사이즈 값들을 저장할 배열
        public Texture2D[] m_detail;
        public GameObject[] m_tree;

        //noise settings
        [Header("Noise 셋팅")]
        public int m_seed = 0;
        public float m_groundFrq = 0.02f;
        public float m_treeFrq = 0.01f;
        public float m_detailFrq = 0.01f;

        //Terrain settings
        [Header("Terrain 셋팅")]
        public int m_tilesX = 2; // x축에 있는 테레인 타일의 개수
        public int m_tilesZ = 2; // z축에 있는 테레인 타일의 개수
        public float m_pixelMapError = 5f; // 숫자 낮아질수록 디테일 상승하지만 속도 느려짐
        public float m_baseMapDist = 100f; // 저해상도 기본 맵이 그려질 거리. 성능을 향상시키려면 이 값을 줄이세요.
        private List<Terrain> terrainList;

        //Terrain data settings
        [Header("TerrainData 셋팅")]
        public AnimationCurve animationCurve;
        public int m_heightMapSize = 50; // 더 높은 숫자는 더 디테일한 높이 맵을 생성할 것입니다
        public int m_alphaMapSize = 512; // 이것은 스플랫 텍스처가 어떻게 혼합될지를 제어하는 컨트롤 맵입니다
        public int m_terrainSize = 512;
        public int m_terrainHeight = 256;
        public int m_detailMapSize = 128; // 디테일(풀) 레이어의 해상도

        //Tree settings
        [Header("Tree 셋팅")]
        public int m_treeSpacing = 32; // 나무 간의 간격
        public float m_treeDistance = 2000.0f; // 나무가 그려지지 않을 거리
        public float m_treeBillboardDistance = 400.0f; // 나무 메시가 나무 빌보드로 변할 거리
        public float m_treeCrossFadeLength = 20.0f; // 나무가 빌보드로 변하면서 변형이 메시와 일치하도록 회전합니다. 높은 숫자는 이 전환을 더 부드럽게 만듭니다.
        public int m_treeMaximumFullLODCount = 400; // 특정 영역에서 그려질 최대 나무 수

        //Detail settings
        [Header("Detail 셋팅")]
        public int m_detailObjectDistance = 400; // 디테일이 더이상 그려지지 않을 거리
        public float m_detailObjectDensity = 4.0f; // 패치 내에서 더 밀도 높은 세부사항을 생성
        public int m_detailResolutionPerPatch = 32; // 세부 패치의 크기 높아질수록 해상도 상승 단 배치, 드로우콜도 상승 
        public float m_wavingGrassStrength = 0.4f;
        public float m_wavingGrassAmount = 0.2f;
        public float m_wavingGrassSpeed = 0.4f;
        public Color m_wavingGrassTint = Color.white;
        public Color m_grassHealthyColor = Color.white;
        public Color m_grassDryColor = Color.white;

        [Header("Road 셋팅")]
        private NNInfo[] positionNode;

        //Base Settings
        [Header("Base 셋팅")]
        public GameObject[] Base_PreFabs;
        public int Ply_Num;
        public int Base_Num = 0;
        List<GameObject> baseCampPositions = new List<GameObject>();

        public GameObject[] flag;
        public int flag_Num;
        List<GameObject> flagPositions_List = new List<GameObject>();
        private Dictionary<string, List<Vector3>> cachedPaths = new Dictionary<string, List<Vector3>>();

        public GameObject[] Leader_Prefabs;
        public GameObject start_Btn;

        //Private
        private FractalNoise m_groundNoise, m_mountainNoise, m_treeNoise, m_detailNoise;
        private Terrain[,] m_terrain;
        private SplatPrototype[] m_splatPrototypes;
        private TreePrototype[] m_treeProtoTypes;
        private DetailPrototype[] m_detailProtoTypes;
        private Vector2 m_offset;

        private void Awake()
        {
            int seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(seed);
            m_seed = Random.Range(0, 100);
            terrainList = new List<Terrain>();
            InitializeTerrain();
        }
        #region 베이스부터 깃발까지 도로 까는 메소드들
        // 가장 가까운 깃발 찾는 메소드
        private Vector3? FindNearestFlag(Vector3 position)
        {
            Vector3? nearestFlag = null;
            float minDistance = float.MaxValue;

            foreach (var flagPos in flagPositions_List)
            {
                float distance = Vector3.Distance(position, flagPos.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestFlag = flagPos.transform.position;
                }
            }

            return nearestFlag;
        }


        private void Start()
        {
            Debug.Log(GameManager.instance.Color_Index);
            Debug.Log(GameManager.instance.T1_Color);
            Debug.Log(GameManager.instance.T2_Color);
            Debug.Log(GameManager.instance.T3_Color);
        }


        // baseCampPositions에서 각 베이스 캠프 위치에 대해 가장 가까운 플래그를 찾아 경로 계산
        private void FindPathsFromBasesToFlags()
        {
            foreach (var basePos in baseCampPositions)
            {
                var nearestFlag = FindNearestFlag(basePos.transform.position);
                if (nearestFlag.HasValue)
                {
                    var path = Pathfinding(basePos.transform.position + Vector3.forward, nearestFlag.Value);
                    DrawRoad(path);
                }
            }
        }

        // 플래그 간의 경로를 찾는 메소드
        private void FindPathsBetweenFlags()
        {
            for (int i = 0; i < flagPositions_List.Count; i++)
            {
                // j를 i + 1로 시작하여, i와 j가 같지 않게 하고, 각 플래그 쌍을 한 번만 비교하도록 함
                for (int j = i + 1; j < flagPositions_List.Count; j++)
                {
                    //int j = (i + 1) % flagPositions_List.Count;
                    Vector3 startFlag = flagPositions_List[i].transform.position + Vector3.forward;
                    Vector3 endFlag = flagPositions_List[j].transform.position + Vector3.forward;
                    
                    string pathKey = i.ToString() + "-" + j.ToString();  // 경로를 식별하는 고유한 키 생성

                    // 이미 탐색한 경로인지 확인
                    if (!cachedPaths.ContainsKey(pathKey))
                    {
                        var path = Pathfinding(startFlag, endFlag);
                        if (path != null && path.Count > 0)
                        {
                            cachedPaths[pathKey] = path;  // 경로를 캐시에 저장
                            DrawRoad(path);
                        }
                        // 반대 방향의 경로도 저장해야 한다면 여기에 추가
                        string reversePathKey = j.ToString() + "-" + i.ToString();
                        if (!cachedPaths.ContainsKey(reversePathKey))
                        {
                            cachedPaths[reversePathKey] = path;  // 경로를 캐시에 저장
                        }
                    }
                }
            }
            
        }

        private void DrawRoad(List<Vector3> pathPositions)
        {
            // 각 지형에 대해 반복
            foreach (var terrain in terrainList)
            {
                TerrainData terrainData = terrain.terrainData;
                // 알파맵의 해상도를 가져옵니다.
                int alphaMapWidth = terrainData.alphamapWidth;
                int alphaMapHeight = terrainData.alphamapHeight;
                int alphaMapLayers = terrainData.alphamapLayers;

                // 현재 알파맵을 가져옵니다.
                float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, alphaMapWidth, alphaMapHeight);

                // 경로를 따라 반복하면서 도로 텍스처를 적용합니다.
                foreach (Vector3 position in pathPositions)
                {
                    // 월드 좌표를 알파맵 좌표로 변환합니다.
                    int mapX = Mathf.FloorToInt((position.x - terrain.transform.position.x) / terrainData.size.x * alphaMapWidth);
                    int mapZ = Mathf.FloorToInt((position.z - terrain.transform.position.z) / terrainData.size.z * alphaMapHeight);

                    // 도로 폭을 설정합니다.
                    int roadWidth = 5;  // 이 값은 도로의 폭에 따라 조절할 수 있습니다.

                    // 도로의 폭만큼 알파맵을 수정합니다.
                    for (int x = mapX - roadWidth / 2; x <= mapX + roadWidth / 2; x++)
                    {
                        for (int z = mapZ - roadWidth / 2; z <= mapZ + roadWidth / 2; z++)
                        {
                            if (x >= 0 && x < alphaMapWidth && z >= 0 && z < alphaMapHeight)
                            {
                                // 도로 텍스처 이외의 모든 레이어를 비활성화합니다.
                                for (int i = 0; i < alphaMapLayers; i++)
                                {
                                    alphaMap[z, x, i] = 0;
                                }

                                // 도로 텍스처를 활성화합니다.
                                alphaMap[z, x, 0] = 1;  // 여기서 '0'은 도로 텍스처 레이어를 가정합니다.
                            }
                        }
                    }
                }

                // 알파맵을 지형 데이터에 적용합니다.
                terrainData.SetAlphamaps(0, 0, alphaMap);
            }
        }

        public void initRoad()
        {
            AstarPath.active.Scan();
            foreach (var terrain in terrainList)
            {
                FillAlphaMap(terrain.terrainData);
            }

            // 도로 그리기 메소드 호출
            // 이를 위해선 먼저 Pathfinding 메소드에서 도로 경로를 생성해야 합니다.
            FindPathsFromBasesToFlags();
            FindPathsBetweenFlags();
            for (int i = 0; i < terrainList.Count; i++)
            {
                terrainList[i].GetComponent<TerrainCollider>().enabled = false;
                terrainList[i].GetComponent<TerrainCollider>().enabled = true;
            }
            GameManager.instance.isLive = true;
            Terra.GetComponent<InitNavMesh>().GenerateNavmesh();
            start_Btn.SetActive(false);

        }
        #endregion
        #region 길찾기 알고리즘
        private List<Vector3> Pathfinding(Vector3 start, Vector3 end)
        {
            NNInfo startNode = AstarPath.active.GetNearest(start);
            NNInfo endNode = AstarPath.active.GetNearest(end);
            
            ABPath path = ABPath.Construct(startNode.position, endNode.position);

            // 경로 계산 시작
            AstarPath.StartPath(path);
            Debug.Log(path.vectorPath.Count);

            // 경로 계산이 끝날 때까지 기다립니다.
            path.BlockUntilCalculated();

            // 경로 상의 점들을 List<Vector3> 형태로 반환합니다.
            List<Vector3> pathPositions = path.vectorPath;

            Draw.Debug.Polyline(pathPositions, Color.red);

            return pathPositions;
        }
        #endregion
        #region 랜덤 터레인 생성
        void InitializeTerrain()
        {
            m_groundNoise = new FractalNoise(new PerlinNoise(m_seed, m_groundFrq), 6, 1.0f, 0.1f);
            m_treeNoise = new FractalNoise(new PerlinNoise(m_seed + 1, m_treeFrq), 6, 1.0f);
            m_detailNoise = new FractalNoise(new PerlinNoise(m_seed + 2, m_detailFrq), 6, 1.0f);

            m_heightMapSize = Mathf.ClosestPowerOfTwo(m_heightMapSize) + 1;
            m_alphaMapSize = Mathf.ClosestPowerOfTwo(m_alphaMapSize);
            m_detailMapSize = Mathf.ClosestPowerOfTwo(m_detailMapSize);

            if (m_detailResolutionPerPatch < 8)
                m_detailResolutionPerPatch = 8;

            float[,] htmap = new float[m_heightMapSize, m_heightMapSize];

            m_terrain = new Terrain[m_tilesX, m_tilesZ];

            m_offset = new Vector2(-m_terrainSize * m_tilesX * 0.5f, -m_terrainSize * m_tilesZ * 0.5f);

            CreateProtoTypes();

            for (int x = 0; x < m_tilesX; x++)
            {
                for (int z = 0; z < m_tilesZ; z++)
                {
                    FillHeights(htmap, x, z);

                    TerrainData terrainData = new TerrainData();
                    


                    terrainData.heightmapResolution = m_heightMapSize;
                    terrainData.SetHeights(0, 0, htmap);
                    terrainData.size = new Vector3(m_terrainSize, m_terrainHeight, m_terrainSize);
                    terrainData.splatPrototypes = m_splatPrototypes;
                    terrainData.treePrototypes = m_treeProtoTypes;
                    terrainData.detailPrototypes = m_detailProtoTypes;
                    FillAlphaMap(terrainData);



                    m_terrain[x, z] = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();

                    m_terrain[x, z].transform.SetParent(Terra.transform);
                    m_terrain[x, z].transform.position = new Vector3(m_terrainSize * x + m_offset.x, 0, m_terrainSize * z + m_offset.y);
                    m_terrain[x, z].heightmapPixelError = m_pixelMapError;
                    m_terrain[x, z].basemapDistance = m_baseMapDist;
                    m_terrain[x, z].gameObject.tag = "Ground";
                    m_terrain[x, z].castShadows = false;

                   

                    FillTreeInstances(m_terrain[x, z], x, z);
                    FillDetailMap(m_terrain[x, z], x, z);
                    terrainList.Add(m_terrain[x, z]);
                }
            }
            
            SpawnBaseCamps(SpawnFlags(flag_Num, 75f, 45f));

            for (int x = 0; x < m_tilesX; x++)
            {
                for (int z = 0; z < m_tilesZ; z++)
                {
                    Terrain right = null;
                    Terrain left = null;
                    Terrain bottom = null;
                    Terrain top = null;

                    if (x > 0) left = m_terrain[(x - 1), z];
                    if (x < m_tilesX - 1) right = m_terrain[(x + 1), z];

                    if (z > 0) bottom = m_terrain[x, (z - 1)];
                    if (z < m_tilesZ - 1) top = m_terrain[x, (z + 1)];

                    m_terrain[x, z].SetNeighbors(left, top, right, bottom);

                }
            }
        }

        // 텍스춰
        void CreateProtoTypes()
        {
            int numSplat = m_splat.Length;
            int numDetail = m_detail.Length;
            int numTree = m_tree.Length;

            m_splatPrototypes = new SplatPrototype[numSplat];
            m_detailProtoTypes = new DetailPrototype[numDetail];
            m_treeProtoTypes = new TreePrototype[numTree];

            for (int i = 0; i < numSplat; i++)
            {
                m_splatPrototypes[i] = new SplatPrototype();
                m_splatPrototypes[i].texture = m_splat[i];
                m_splatPrototypes[i].tileSize = new Vector2(m_splatTileSizes[i], m_splatTileSizes[i]);
            }

            for (int i = 0; i < numTree; i++)
            {
                m_treeProtoTypes[i] = new TreePrototype();
                m_treeProtoTypes[i].prefab = m_tree[i];
            }

            for (int i = 0; i < numDetail; i++)
            {
                m_detailProtoTypes[i] = new DetailPrototype();
                m_detailProtoTypes[i].prototypeTexture = m_detail[i];
                m_detailProtoTypes[i].renderMode = DetailRenderMode.GrassBillboard;
                m_detailProtoTypes[i].healthyColor = m_grassHealthyColor;
                m_detailProtoTypes[i].dryColor = m_grassDryColor;
            }
        }

        // 높이
        void FillHeights(float[,] htmap, int tileX, int tileZ)
        {
            // 지형의 크기와 높이맵의 크기의 비율을 계산.
            float ratio = (float)m_terrainSize / (float)m_heightMapSize;

            // 높이맵의 각 픽셀에 대해 반복.
            for (int x = 0; x < m_heightMapSize; x++)
            {
                // 현재 픽셀의 월드 좌표를 계산.
                for (int z = 0; z < m_heightMapSize; z++)
                {
                    float worldPosX = (x + tileX * (m_heightMapSize - 1)) * ratio;
                    float worldPosZ = (z + tileZ * (m_heightMapSize - 1)) * ratio;

                    float heightMultiplier = animationCurve.Evaluate(htmap[z, x]);
                    htmap[z, x] = (m_groundFrq + heightMultiplier) * m_groundNoise.Amplitude + m_groundNoise.Sample2D(worldPosX, worldPosZ);
                }
            }
        }

        // 알파맵
        void FillAlphaMap(TerrainData terrainData)
        {
            // m_alphaMapSize x m_alphaMapSize 크기의 2개의 스플랫맵 레이어를 위한 3차원 배열을 생성.
            float[,,] map = new float[m_alphaMapSize, m_alphaMapSize, 2];

            // 알파맵을 채우기 위해 각 좌표를 순회.
            for (int x = 0; x < m_alphaMapSize; x++)
            {
                for (int z = 0; z < m_alphaMapSize; z++)
                {
                    // 지형 좌표를 정규화된 값(0.0 ~ 1.0)으로 변환.
                    // 이렇게 하면 좌표가 지형의 어느 부분을 가리키는지 쉽게 알 수 있음.
                    float normX = x * 1.0f / (m_alphaMapSize - 1);
                    float normZ = z * 1.0f / (m_alphaMapSize - 1);

                    // 정규화된 좌표에서 경사도를 계산. 
                    // 경사도는 각도로 반환되며, 값의 범위는 0도에서 90도.
                    float angle = terrainData.GetSteepness(normX, normZ);

                    // 경사도를 알파 블렌딩 값의 범위인 0에서 1 사이의 값으로 변환.
                    // 각도가 클수록 1에 가까워지고, 각도가 작을수록 0에 가까워짐.
                    float frac = angle / 90.0f;
                    map[z, x, 0] = frac;
                    map[z, x, 1] = 1.0f - frac;

                }
            }
            // 테레인 데이터의 알파맵 해상도를 설정.
            terrainData.alphamapResolution = m_alphaMapSize;
            // 계산된 알파맵 값을 테레인에 적용.
            terrainData.SetAlphamaps(0, 0, map);
        }

        // 나무
        void FillTreeInstances(Terrain terrain, int tileX, int tileZ)
        {
            // 난수 생성기의 시드 값을 0으로 초기화. 
            // 이렇게 하면 프로그램을 다시 실행할 때마다 동일한 결과를 얻을 수 있습니다.
            Random.InitState(0);

            // 지형 전체를 순회하면서 트리 인스턴스를 추가.
            for (int x = 0; x < m_terrainSize; x += m_treeSpacing)
            {
                for (int z = 0; z < m_terrainSize; z += m_treeSpacing)
                {
                    // 지형의 한 변의 길이에 대한 역수를 계산.
                    float unit = 1.0f / (m_terrainSize - 1);

                    // 트리의 위치를 무작위로 조정하기 위한 오프셋을 계산.
                    float offsetX = Random.value * unit * m_treeSpacing;
                    float offsetZ = Random.value * unit * m_treeSpacing;

                    // 트리의 위치를 정규화된 좌표계로 변환.
                    float normX = x * unit + offsetX;
                    float normZ = z * unit + offsetZ;

                    // 경사도를 계산합니다. 이 값은 0도에서 90도 사이.
                    float angle = terrain.terrainData.GetSteepness(normX, normZ);

                    // 경사도를 0에서 1 사이의 값으로 변환.
                    float frac = angle / 90.0f;

                    // 경사가 완만한 지역에서만 트리를 심기.
                    if (frac < 0.5f)
                    {
                        // 트리의 월드 좌표를 계산.
                        float worldPosX = x + tileX * (m_terrainSize - 1);
                        float worldPosZ = z + tileZ * (m_terrainSize - 1);

                        // 노이즈 함수를 사용하여 트리의 밀도를 결정.
                        float noise = m_treeNoise.Sample2D(worldPosX, worldPosZ);
                        float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                        // 트리의 높이를 지형 데이터에서 가져옴.
                        if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                        {
                            TreeInstance temp = new TreeInstance();
                            temp.position = new Vector3(normX, ht, normZ);
                            temp.prototypeIndex = Random.Range(0, 3);
                            temp.widthScale = 1;
                            temp.heightScale = 2;
                            temp.color = Color.white;
                            temp.lightmapColor = Color.white;

                            // 트리 인스턴스를 지형에 추가.
                            terrain.AddTreeInstance(temp);
                        }
                    }

                }
            }
            // 트리 관련 설정을 지정.
            terrain.treeDistance = m_treeDistance;
            terrain.treeBillboardDistance = m_treeBillboardDistance;
            terrain.treeCrossFadeLength = m_treeCrossFadeLength;
            terrain.treeMaximumFullLODCount = m_treeMaximumFullLODCount;

        }

        // 디테일맵
        void FillDetailMap(Terrain terrain, int tileX, int tileZ)
        {
            //each layer is drawn separately so if you have a lot of layers your draw calls will increase 
            int[,] detailMap0 = new int[m_detailMapSize, m_detailMapSize];
            int[,] detailMap1 = new int[m_detailMapSize, m_detailMapSize];
            int[,] detailMap2 = new int[m_detailMapSize, m_detailMapSize];

            float ratio = (float)m_terrainSize / (float)m_detailMapSize;

            Random.InitState(0);

            for (int x = 0; x < m_detailMapSize; x++)
            {
                for (int z = 0; z < m_detailMapSize; z++)
                {
                    detailMap0[z, x] = 0;
                    detailMap1[z, x] = 0;
                    detailMap2[z, x] = 0;

                    float unit = 1.0f / (m_detailMapSize - 1);

                    float normX = x * unit;
                    float normZ = z * unit;

                    // Get the steepness value at the normalized coordinate.
                    float angle = terrain.terrainData.GetSteepness(normX, normZ);

                    // Steepness is given as an angle, 0..90 degrees. Divide
                    // by 90 to get an alpha blending value in the range 0..1.
                    float frac = angle / 90.0f;

                    if (frac < 0.5f)
                    {
                        float worldPosX = (x + tileX * (m_detailMapSize - 1)) * ratio;
                        float worldPosZ = (z + tileZ * (m_detailMapSize - 1)) * ratio;

                        float noise = m_detailNoise.Sample2D(worldPosX, worldPosZ);

                        if (noise > 0.0f)
                        {
                            float rnd = Random.value;
                            //Randomly select what layer to use
                            if (rnd < 0.33f)
                                detailMap0[z, x] = 1;
                            else if (rnd < 0.66f)
                                detailMap1[z, x] = 1;
                            else
                                detailMap2[z, x] = 1;
                        }
                    }

                }
            }
            terrain.terrainData.wavingGrassStrength = m_wavingGrassStrength;
            terrain.terrainData.wavingGrassAmount = m_wavingGrassAmount;
            terrain.terrainData.wavingGrassSpeed = m_wavingGrassSpeed;
            terrain.terrainData.wavingGrassTint = m_wavingGrassTint;
            terrain.detailObjectDensity = m_detailObjectDensity;
            terrain.detailObjectDistance = m_detailObjectDistance;
            terrain.terrainData.SetDetailResolution(m_detailMapSize, m_detailResolutionPerPatch);
            terrain.terrainData.SetDetailLayer(0, 0, 0, detailMap0);
            terrain.terrainData.SetDetailLayer(0, 0, 1, detailMap1);
            terrain.terrainData.SetDetailLayer(0, 0, 2, detailMap2);
        }
        #endregion
        #region 깃발, 베이스 생성 메소드
        private List<Vector3> SpawnFlags(int numberOfFlags, float range, float minDistanceBetweenFlags)
        {
            int maxAttempts = 100; // 최대 시도 횟수, 무한 루프 방지를 위해 설정
            List<Vector3> flagPositions = new List<Vector3>();
            List<GameObject> flags = new List<GameObject>();

            for (int i = 0; i < numberOfFlags; i++)
            {
                bool validPositionFound = false;
                Vector3 flagPosition = Vector3.zero;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    // 50x50 범위 내의 랜덤한 위치 생성
                    float posX = Random.Range(-range / 2f, range / 2f);
                    float posZ = Random.Range(-range / 2f, range / 2f);
                    flagPosition = new Vector3(posX, 10, posZ); // 높이는 적절히 조정

                    bool tooCloseToOtherFlags = flagPositions.Any(existingPosition => Vector3.Distance(flagPosition, existingPosition) < minDistanceBetweenFlags);

                    if (!tooCloseToOtherFlags)
                    {
                        validPositionFound = true;
                        break;
                    }
                }

                if (validPositionFound)
                {
                    flagPositions.Add(flagPosition);
                    GameObject flaG = Instantiate(flag[0], flagPosition, Quaternion.identity);
                    flags.Add(flaG);
                    flagPositions_List.Add(flaG);
                }
                else
                {
                    Debug.LogError("Failed to find a valid position for a flag.");
                    RemoveFlag(flags);
                    SpawnFlags(numberOfFlags, range, minDistanceBetweenFlags);
                }
            }

            return flagPositions;
        }

        void SpawnBaseCamps(List<Vector3> flagPositions)
        {
            int numPlayers = Ply_Num; // 플레이어 수
            int maxAttempts = 100; // 최대 시도 횟수, 무한 루프 방지를 위해 설정
            List<GameObject> baseCamps = new List<GameObject>();

            for (int i = 0; i < numPlayers; i++)
            {
                bool validPositionFound = false;
                Vector3 baseCampPosition = Vector3.zero;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    // 중앙에서 200x200 범위 내의 랜덤한 위치 생성
                    float posX = Random.Range(-170f, 170f);
                    float posZ = Random.Range(-170f, 170f);

                    // 월드 좌표계를 테레인 배열 인덱스로 변환
                    int terrainIndexX = Mathf.FloorToInt((posX + m_tilesX * m_terrainSize * 0.5f) / m_terrainSize);
                    int terrainIndexZ = Mathf.FloorToInt((posZ + m_tilesZ * m_terrainSize * 0.5f) / m_terrainSize);

                    // 위치가 테레인 배열 내에 있는지 확인
                    if (terrainIndexX >= 0 && terrainIndexX < m_tilesX && terrainIndexZ >= 0 && terrainIndexZ < m_tilesZ)
                    {
                        Terrain terrain = m_terrain[terrainIndexX, terrainIndexZ];


                        baseCampPosition = new Vector3(posX, 10, posZ);
                        bool tooCloseToOtherBases = false;
                        bool tooClostToFlag = false;

                        foreach (GameObject flagPosition in flagPositions_List)
                        {
                            if (Vector3.Distance(baseCampPosition, flagPosition.transform.position) < 100f)
                            {
                                tooClostToFlag = true;
                                break;
                            }
                        }

                        foreach (GameObject otherBasePosition in baseCampPositions)
                        {
                            if (Vector3.Distance(baseCampPosition, otherBasePosition.transform.position) < 170f)
                            {
                                tooCloseToOtherBases = true;
                                break;
                            }
                        }


                        if (!tooCloseToOtherBases && !tooClostToFlag)
                        {
                            validPositionFound = true;
                            break;
                        }
                    }
                }

                if (validPositionFound)
                {
                    // 베이스 캠프 소환
                    GameObject baseCamp = Instantiate(Base_PreFabs[i % Base_PreFabs.Length], baseCampPosition, Quaternion.identity);

                    
                    baseCamps.Add(baseCamp);
                    baseCampPositions.Add(baseCamp);
                    // 베이스 캠프를 원점을 바라보도록 회전 설정
                    Vector3 lookDirection = Vector3.zero - baseCamp.transform.position;
                    lookDirection.y = 0f; // 오브젝트를 수평으로 회전시키려면 y 값을 0으로 설정
                    Quaternion rotation = Quaternion.LookRotation(lookDirection.normalized);
                    baseCamp.transform.rotation = rotation;
                }
                else
                {
                    Debug.Log("Failed to find a valid position for base camp. Base camp spawning failed.");
                    // 기존 베이스 캠프 제거
                    RemoveBaseCamps(baseCamps);
                    SpawnBaseCamps(flagPositions);
                }
            }
            ColorSetting(baseCamps);
            SpawnPlayer(baseCamps);
        }

        void RemoveBaseCamps(List<GameObject> baseCamps)
        {
            foreach (GameObject baseCamp in baseCamps)
            {
                Destroy(baseCamp);
            }

            baseCamps.Clear(); // 베이스 캠프 목록 초기화
            baseCampPositions.Clear(); // 베이스 캠프 위치 목록 초기화
        }

        void RemoveFlag(List<GameObject> flags)
        {
            foreach (GameObject flag in flags)
            {
                Destroy(flag);
            }

            flags.Clear(); // 베이스 캠프 목록 초기화
            flagPositions_List.Clear(); // 베이스 캠프 위치 목록 초기화
        }

        void ColorSetting(List<GameObject> baseCamps)
        {
            for(int i = 0; i < baseCamps.Count; i++)
            {
                if(baseCamps[i].gameObject.layer == 0)
                {
                    baseCamps[i].gameObject.layer = i + 6;
                }
            }

            for(int i = 0; i < baseCamps.Count; i++)
            {
                switch (baseCamps[i].gameObject.layer)
                {
                    case (int)TeamLayerIdx.Player:
                        ColorManager.instance.RecursiveSearchAndSetBuilding(baseCamps[i].transform, GameManager.instance.Color_Index);
                        baseCamps[i].GetComponentInChildren<Flag>().Change_Flag_Color(GameManager.instance.Color_Index);
                        break;
                    case (int)TeamLayerIdx.Team1:
                        ColorManager.instance.RecursiveSearchAndSetBuilding(baseCamps[i].transform, GameManager.instance.T1_Color);
                        baseCamps[i].GetComponentInChildren<Flag>().Change_Flag_Color(GameManager.instance.T1_Color);
                        break;
                    case (int)TeamLayerIdx.Team2:
                        ColorManager.instance.RecursiveSearchAndSetBuilding(baseCamps[i].transform, GameManager.instance.T2_Color);
                        baseCamps[i].GetComponentInChildren<Flag>().Change_Flag_Color(GameManager.instance.T2_Color);
                        break;
                    case (int)TeamLayerIdx.Team3:
                        ColorManager.instance.RecursiveSearchAndSetBuilding(baseCamps[i].transform, GameManager.instance.T3_Color);
                        baseCamps[i].GetComponentInChildren<Flag>().Change_Flag_Color(GameManager.instance.T3_Color);
                        break;
                }
            }
        }

        void SpawnPlayer(List<GameObject> baseCamps)
        {
            GameObject Leader;
            for (int i = 0; i < baseCamps.Count; i++)
            {
                switch (baseCamps[i].gameObject.layer)
                {
                    case 6:
                        Leader = Instantiate(Leader_Prefabs[0], baseCamps[i].transform.position, Quaternion.identity);
                        ColorManager.instance.RecursiveSearchAndSetUnit(Leader.transform, GameManager.instance.Color_Index);
                        Leader.gameObject.layer = 6;
                        break;
                    case 7:
                        Leader = Instantiate(Leader_Prefabs[1], baseCamps[i].transform.position, Quaternion.identity);
                        ColorManager.instance.RecursiveSearchAndSetUnit(Leader.transform, GameManager.instance.T1_Color);
                        Leader.gameObject.layer = 7;
                        break;
                    case 8:
                        Leader = Instantiate(Leader_Prefabs[1], baseCamps[i].transform.position, Quaternion.identity);
                        ColorManager.instance.RecursiveSearchAndSetUnit(Leader.transform, GameManager.instance.T2_Color);
                        Leader.gameObject.layer = 8;
                        break;
                    case 9:
                        Leader = Instantiate(Leader_Prefabs[1], baseCamps[i].transform.position, Quaternion.identity);
                        ColorManager.instance.RecursiveSearchAndSetUnit(Leader.transform, GameManager.instance.T3_Color);
                        Leader.gameObject.layer = 9;
                        break;
                }
            }
        }
        #endregion
    }
}


