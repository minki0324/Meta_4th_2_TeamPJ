using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

using ProceduralNoiseProject;

namespace SimpleProceduralTerrainProject
{

    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField]
        NavMeshSurface[] surfaces;


        //Prototypes
        public Texture2D[] m_splat; // �ؽ��� �迭
        public float[] m_splatTileSizes; // Ÿ�� ������ ������ ������ �迭
        public Texture2D[] m_detail;
        public GameObject[] m_tree;

        //noise settings
        [Header("Noise ����")]
        public int m_seed = 0;
        public float m_groundFrq = 0.02f;
        public float m_treeFrq = 0.01f;
        public float m_detailFrq = 0.01f;

        //Terrain settings
        [Header("Terrain ����")]
        public int m_tilesX = 2; // x�࿡ �ִ� �׷��� Ÿ���� ����
        public int m_tilesZ = 2; // z�࿡ �ִ� �׷��� Ÿ���� ����
        public float m_pixelMapError = 5f; // ���� ���������� ������ ��������� �ӵ� ������
        public float m_baseMapDist = 100f; // ���ػ� �⺻ ���� �׷��� �Ÿ�. ������ ����Ű���� �� ���� ���̼���.

        //Terrain data settings
        [Header("TerrainData ����")]
        public AnimationCurve animationCurve;
        public int m_heightMapSize = 50; // �� ���� ���ڴ� �� �������� ���� ���� ������ ���Դϴ�
        public int m_alphaMapSize = 50; // �̰��� ���÷� �ؽ�ó�� ��� ȥ�յ����� �����ϴ� ��Ʈ�� ���Դϴ�
        public int m_terrainSize = 512;
        public int m_terrainHeight = 256;
        public int m_detailMapSize = 128; // ������(Ǯ) ���̾��� �ػ�

        //Tree settings
        [Header("Tree ����")]
        public int m_treeSpacing = 32; // ���� ���� ����
        public float m_treeDistance = 2000.0f; // ������ �׷����� ���� �Ÿ�
        public float m_treeBillboardDistance = 400.0f; // ���� �޽ð� ���� ������� ���� �Ÿ�
        public float m_treeCrossFadeLength = 20.0f; // ������ ������� ���ϸ鼭 ������ �޽ÿ� ��ġ�ϵ��� ȸ���մϴ�. ���� ���ڴ� �� ��ȯ�� �� �ε巴�� ����ϴ�.
        public int m_treeMaximumFullLODCount = 400; // Ư�� �������� �׷��� �ִ� ���� ��

        //Detail settings
        [Header("Detail ����")]
        public int m_detailObjectDistance = 400; // �������� ���̻� �׷����� ���� �Ÿ�
        public float m_detailObjectDensity = 4.0f; // ��ġ ������ �� �е� ���� ���λ����� ����
        public int m_detailResolutionPerPatch = 32; // ���� ��ġ�� ũ�� ���������� �ػ� ��� �� ��ġ, ��ο��ݵ� ��� 
        public float m_wavingGrassStrength = 0.4f;
        public float m_wavingGrassAmount = 0.2f;
        public float m_wavingGrassSpeed = 0.4f;
        public Color m_wavingGrassTint = Color.white;
        public Color m_grassHealthyColor = Color.white;
        public Color m_grassDryColor = Color.white;

        //Base Settings
        public GameObject[] Base_PreFabs;
        public int Ply_Num;
        public int Base_Num = 0;
        List<Vector3> baseCampPositions = new List<Vector3>();

        //Private
        private FractalNoise m_groundNoise, m_mountainNoise, m_treeNoise, m_detailNoise;
        private Terrain[,] m_terrain;
        private SplatPrototype[] m_splatPrototypes;
        private TreePrototype[] m_treeProtoTypes;
        private DetailPrototype[] m_detailProtoTypes;
        private Vector2 m_offset;

        void Start()
        {
            int seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(seed);

            m_seed = Random.Range(0, 100);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                InitializeTerrain();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GenerateNavmesh();
            }
        }

        private void GenerateNavmesh()
        {

            surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

            foreach (var s in surfaces)
            {
                s.RemoveData();
                s.BuildNavMesh();
            }

        }



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

            //this will center terrain at origin
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
                    m_terrain[x, z].transform.position = new Vector3(m_terrainSize * x + m_offset.x, 0, m_terrainSize * z + m_offset.y);
                    m_terrain[x, z].heightmapPixelError = m_pixelMapError;
                    m_terrain[x, z].basemapDistance = m_baseMapDist;
                    m_terrain[x, z].gameObject.tag = "Ground";
                    m_terrain[x, z].castShadows = false;

                  
                    FillTreeInstances(m_terrain[x, z], x, z);
                    FillDetailMap(m_terrain[x, z], x, z);
                }
            }
            SpawnBaseCamps();
            RemoveTreesFromBases();

            //Set the neighbours of terrain to remove seams.
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

        void SpawnBaseCamps()
        {
            int numPlayers = Ply_Num; // �÷��̾� ��
            int maxAttempts = 100; // �ִ� �õ� Ƚ��, ���� ���� ������ ���� ����
            List<GameObject> baseCamps = new List<GameObject>();

           
            for (int i = 0; i < numPlayers; i++)
            {
                bool validPositionFound = false;
                Vector3 baseCampPosition = Vector3.zero;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    // �߾ӿ��� 250x250 ���� ���� ������ ��ġ ����
                    float posX = Random.Range(-200f, 200f);
                    float posZ = Random.Range(-200f, 200f);

                    // ���� ��ǥ�踦 �׷��� �迭 �ε����� ��ȯ
                    int terrainIndexX = Mathf.FloorToInt((posX + m_tilesX * m_terrainSize * 0.5f) / m_terrainSize);
                    int terrainIndexZ = Mathf.FloorToInt((posZ + m_tilesZ * m_terrainSize * 0.5f) / m_terrainSize);

                    // ��ġ�� �׷��� �迭 ���� �ִ��� Ȯ��
                    if (terrainIndexX >= 0 && terrainIndexX < m_tilesX && terrainIndexZ >= 0 && terrainIndexZ < m_tilesZ)
                    {
                        Terrain terrain = m_terrain[terrainIndexX, terrainIndexZ];
                        
                    
                        baseCampPosition = new Vector3(posX, 10, posZ);
                        bool tooCloseToOtherBases = false;

                        foreach (Vector3 otherBasePosition in baseCampPositions)
                        {
                            if (Vector3.Distance(baseCampPosition, otherBasePosition) < 170f)
                            {
                                tooCloseToOtherBases = true;
                                break;
                            }
                        }

                        if (!tooCloseToOtherBases)
                        {
                            validPositionFound = true;
                            break;
                        }
                    }
                }

                if (validPositionFound)
                {
                    // ���̽� ķ�� ��ȯ
                    GameObject baseCamp =  Instantiate(Base_PreFabs[i % Base_PreFabs.Length], baseCampPosition, Quaternion.identity);
                                       
                    baseCamps.Add(baseCamp);
                    baseCampPositions.Add(baseCampPosition);
                    // ���̽� ķ���� ������ �ٶ󺸵��� ȸ�� ����
                    Vector3 lookDirection = Vector3.zero - baseCamp.transform.position;
                    lookDirection.y = 0f; // ������Ʈ�� �������� ȸ����Ű���� y ���� 0���� ����
                    Quaternion rotation = Quaternion.LookRotation(lookDirection.normalized);
                    baseCamp.transform.rotation = rotation;

                    // ColorSet ��ũ��Ʈ�� RecursiveSearchAndSetTexture �޼��� ȣ���Ͽ� �÷� ����
                    ColorSet colorSet = baseCamp.GetComponentInChildren<ColorSet>();
                    if (colorSet != null)
                    {
                        colorSet.RecursiveSearchAndSetTexture(baseCamp.transform, GameManager.instance.Color_Index);
                    }
                    else
                    {
                        Debug.Log("��");
                    }
                }
                else
                {
                    Debug.Log("Failed to find a valid position for base camp. Base camp spawning failed.");
                    // ���� ���̽� ķ�� ����
                    RemoveBaseCamps(baseCamps);
                    SpawnBaseCamps();
                }
            }
        }

        void RemoveBaseCamps(List<GameObject> baseCamps)
        {
            foreach (GameObject baseCamp in baseCamps)
            {
                Destroy(baseCamp);
            }

            baseCamps.Clear(); // ���̽� ķ�� ��� �ʱ�ȭ
            baseCampPositions.Clear(); // ���̽� ķ�� ��ġ ��� �ʱ�ȭ
        }

        void RemoveTreesFromBases()
        {
            // ��ȯ�� ���̽� ķ�� ��ġ���� ���� ����
            foreach (Vector3 baseCampPosition in baseCampPositions)
            {
                // ���̽� ķ���� ��ġ�� �׷��� ã��
                int x = (int)(baseCampPosition.x / m_terrainSize);
                int z = (int)(baseCampPosition.z / m_terrainSize);
                Terrain terrain = m_terrain[x, z];

                // ���̽� ķ�� �ֺ��� ���� ����
                RemoveTrees(terrain, baseCampPosition.x, baseCampPosition.z, 10);
            }
        }

        void RemoveTrees(Terrain terrain, float posX, float posZ, float radius)
        {
            List<TreeInstance> trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
            Vector3 terrainPosition = terrain.transform.position;
            float terrainSize = terrain.terrainData.size.x;

            for (int i = trees.Count - 1; i >= 0; i--)
            {
                Vector3 treeWorldPosition = Vector3.Scale(trees[i].position, terrain.terrainData.size) + terrainPosition;

                if (Vector3.Distance(new Vector3(posX, 0, posZ), new Vector3(treeWorldPosition.x, 0, treeWorldPosition.z)) < radius)
                {
                    trees.RemoveAt(i);
                }
            }

            terrain.terrainData.treeInstances = trees.ToArray();
        }


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

            for(int i = 0; i < numDetail; i++)
            {
                m_detailProtoTypes[i] = new DetailPrototype();
                m_detailProtoTypes[i].prototypeTexture = m_detail[i];
                m_detailProtoTypes[i].renderMode = DetailRenderMode.GrassBillboard;
                m_detailProtoTypes[i].healthyColor = m_grassHealthyColor;
                m_detailProtoTypes[i].dryColor = m_grassDryColor;
            }
        }

        void FillHeights(float[,] htmap, int tileX, int tileZ)
        {
            // ������ ũ��� ���̸��� ũ���� ������ ���.
            float ratio = (float)m_terrainSize / (float)m_heightMapSize;

            // ���̸��� �� �ȼ��� ���� �ݺ�.
            for (int x = 0; x < m_heightMapSize; x++)
            {
                // ���� �ȼ��� ���� ��ǥ�� ���.
                for (int z = 0; z < m_heightMapSize; z++)
                {
                    float worldPosX = (x + tileX * (m_heightMapSize - 1)) * ratio;
                    float worldPosZ = (z + tileZ * (m_heightMapSize - 1)) * ratio;

                    float heightMultiplier = animationCurve.Evaluate(htmap[z, x]);
                    htmap[z, x] = (m_groundFrq + heightMultiplier) * m_groundNoise.Amplitude + m_groundNoise.Sample2D(worldPosX, worldPosZ);
                }
            }
        }

        void FillAlphaMap(TerrainData terrainData)
        {
            // m_alphaMapSize x m_alphaMapSize ũ���� 2���� ���÷��� ���̾ ���� 3���� �迭�� ����.
            float[,,] map = new float[m_alphaMapSize, m_alphaMapSize, 2];

            // ���ĸ��� ä��� ���� �� ��ǥ�� ��ȸ.
            for (int x = 0; x < m_alphaMapSize; x++)
            {
                for (int z = 0; z < m_alphaMapSize; z++)
                {
                    // ���� ��ǥ�� ����ȭ�� ��(0.0 ~ 1.0)���� ��ȯ.
                    // �̷��� �ϸ� ��ǥ�� ������ ��� �κ��� ����Ű���� ���� �� �� ����.
                    float normX = x * 1.0f / (m_alphaMapSize - 1);
                    float normZ = z * 1.0f / (m_alphaMapSize - 1);

                    // ����ȭ�� ��ǥ���� ��絵�� ���. 
                    // ��絵�� ������ ��ȯ�Ǹ�, ���� ������ 0������ 90��.
                    float angle = terrainData.GetSteepness(normX, normZ);

                    // ��絵�� ���� ���� ���� ������ 0���� 1 ������ ������ ��ȯ.
                    // ������ Ŭ���� 1�� ���������, ������ �������� 0�� �������.
                    float frac = angle / 90.0f;
                    map[z, x, 0] = frac;
                    map[z, x, 1] = 1.0f - frac;

                }
            }
            // �׷��� �������� ���ĸ� �ػ󵵸� ����.
            terrainData.alphamapResolution = m_alphaMapSize;
            // ���� ���ĸ� ���� �׷��ο� ����.
            terrainData.SetAlphamaps(0, 0, map);
        }

        void FillTreeInstances(Terrain terrain, int tileX, int tileZ)
        {
            // ���� �������� �õ� ���� 0���� �ʱ�ȭ. 
            // �̷��� �ϸ� ���α׷��� �ٽ� ������ ������ ������ ����� ���� �� �ֽ��ϴ�.
            Random.InitState(0);

            // ���� ��ü�� ��ȸ�ϸ鼭 Ʈ�� �ν��Ͻ��� �߰�.
            for (int x = 0; x < m_terrainSize; x += m_treeSpacing)
            {
                for (int z = 0; z < m_terrainSize; z += m_treeSpacing)
                {
                    // ������ �� ���� ���̿� ���� ������ ���.
                    float unit = 1.0f / (m_terrainSize - 1);

                    // Ʈ���� ��ġ�� �������� �����ϱ� ���� �������� ���.
                    float offsetX = Random.value * unit * m_treeSpacing;
                    float offsetZ = Random.value * unit * m_treeSpacing;

                    // Ʈ���� ��ġ�� ����ȭ�� ��ǥ��� ��ȯ.
                    float normX = x * unit + offsetX;
                    float normZ = z * unit + offsetZ;

                    // ��絵�� ����մϴ�. �� ���� 0������ 90�� ����.
                    float angle = terrain.terrainData.GetSteepness(normX, normZ);

                    // ��絵�� 0���� 1 ������ ������ ��ȯ.
                    float frac = angle / 90.0f;

                    // ��簡 �ϸ��� ���������� Ʈ���� �ɱ�.
                    if (frac < 0.5f) 
                    {
                        // Ʈ���� ���� ��ǥ�� ���.
                        float worldPosX = x + tileX * (m_terrainSize - 1);
                        float worldPosZ = z + tileZ * (m_terrainSize - 1);

                        // ������ �Լ��� ����Ͽ� Ʈ���� �е��� ����.
                        float noise = m_treeNoise.Sample2D(worldPosX, worldPosZ);
                        float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                        // Ʈ���� ���̸� ���� �����Ϳ��� ������.
                        if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                        {
                            TreeInstance temp = new TreeInstance();
                            temp.position = new Vector3(normX, ht, normZ);
                            temp.prototypeIndex = Random.Range(0, 3);
                            temp.widthScale = 1;
                            temp.heightScale = 1;
                            temp.color = Color.white;
                            temp.lightmapColor = Color.white;

                            // Ʈ�� �ν��Ͻ��� ������ �߰�.
                            terrain.AddTreeInstance(temp);
                        }
                    }

                }
            }
            // Ʈ�� ���� ������ ����.
            terrain.treeDistance = m_treeDistance;
            terrain.treeBillboardDistance = m_treeBillboardDistance;
            terrain.treeCrossFadeLength = m_treeCrossFadeLength;
            terrain.treeMaximumFullLODCount = m_treeMaximumFullLODCount;

        }

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

    }
}


