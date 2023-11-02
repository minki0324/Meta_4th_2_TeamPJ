using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail, bool useFlatShading)
	{
		AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

		int mesh_Simplification_Increment = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

		int BorderedSize = heightMap.GetLength(0);
		int meshSize = BorderedSize - 2 * mesh_Simplification_Increment;
		int meshSizeUnsimplified = BorderedSize - 2;

		float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

		int verticesPerLine = (meshSize - 1) / mesh_Simplification_Increment + 1;

		MeshData meshData = new MeshData(verticesPerLine, useFlatShading);

		int[,] vertexIndicesMap = new int[BorderedSize, BorderedSize];
		int meshVertexIndex = 0;
		int borderVertexIndex = -1;

		for (int y = 0; y < BorderedSize; y += mesh_Simplification_Increment)
		{
			for (int x = 0; x < BorderedSize; x += mesh_Simplification_Increment)
			{
				bool isBorderVertex = y == 0 || y == BorderedSize - 1 || x == 0 || x == BorderedSize - 1;
				if(isBorderVertex)
                {
					vertexIndicesMap[x, y] = borderVertexIndex;
					borderVertexIndex--;
                }
				else
                {
					vertexIndicesMap[x, y] = meshVertexIndex;
					meshVertexIndex++;
                }
			}
		}

		for (int y = 0; y < BorderedSize; y += mesh_Simplification_Increment)
		{
			for (int x = 0; x < BorderedSize; x += mesh_Simplification_Increment)
			{
				int vertexIndex = vertexIndicesMap[x, y];
				
				Vector2 percent = new Vector2((x-mesh_Simplification_Increment) / (float)meshSize, (y-mesh_Simplification_Increment) / (float)meshSize);
				float height = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
				Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

				meshData.AddVertex(vertexPosition, percent, vertexIndex);

				if (x < BorderedSize - 1 && y < BorderedSize - 1)
				{
					int a = vertexIndicesMap[x, y];
					int b = vertexIndicesMap[x + mesh_Simplification_Increment, y];
					int c = vertexIndicesMap[x, y + mesh_Simplification_Increment];
					int d = vertexIndicesMap[x + mesh_Simplification_Increment, y + mesh_Simplification_Increment];
					meshData.AddTriangle(a,d,c);
					meshData.AddTriangle(d,a,b);
				}
				vertexIndex++;
			}
		}
		meshData.ProcessMesh();

		return meshData;
	}
}

public class MeshData
{
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uvs;
	private Vector3[] bakedNormals;

	private Vector3[] borderVertices;
	private int[] borderTriangles;

	private int triangleIndex;
	private int borderTriangleIndex;

	private bool useFlatShading;

	public MeshData(int verticesPerLine, bool useFlatShading)
	{
		this.useFlatShading = useFlatShading;
		vertices = new Vector3[verticesPerLine * verticesPerLine];
		uvs = new Vector2[verticesPerLine * verticesPerLine];
		triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

		borderVertices = new Vector3[verticesPerLine * 4 + 4];
		borderTriangles = new int[24 * verticesPerLine];
	}

	public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
		if(vertexIndex < 0)
        {
			borderVertices[-vertexIndex - 1] = vertexPosition;

        }
		else
        {
			vertices[vertexIndex] = vertexPosition;
			uvs[vertexIndex] = uv;
        }
    }

	public void AddTriangle(int a, int b, int c)
	{
		if(a<0 || b<0 || c<0)
        {
			borderTriangles[borderTriangleIndex] = a;
			borderTriangles[borderTriangleIndex + 1] = b;
			borderTriangles[borderTriangleIndex + 2] = c;
			borderTriangleIndex += 3;
		}
		else
        {
			triangles[triangleIndex] = a;
			triangles[triangleIndex + 1] = b;
			triangles[triangleIndex + 2] = c;
			triangleIndex += 3;
        }
	}

	private Vector3[] CalculateNormals()
    {
		Vector3[] vertexNormals = new Vector3[vertices.Length];
		int trianglesCount = triangles.Length / 3;

		for(int i = 0; i < trianglesCount; i++)
        {
			int normalTrianglesIndex = i * 3;
			int vertexIndexA = triangles[normalTrianglesIndex];
			int vertexIndexB = triangles[normalTrianglesIndex + 1];
			int vertexIndexC = triangles[normalTrianglesIndex + 2];

			Vector3 triangleNormal = SurfaceNomalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			vertexNormals[vertexIndexA] += triangleNormal;
			vertexNormals[vertexIndexB] += triangleNormal;
			vertexNormals[vertexIndexC] += triangleNormal;
        }

		int borderTrianglesCount = borderTriangles.Length / 3;

		for (int i = 0; i < borderTrianglesCount; i++)
		{
			int normalTrianglesIndex = i * 3;
			int vertexIndexA = borderTriangles[normalTrianglesIndex];
			int vertexIndexB = borderTriangles[normalTrianglesIndex + 1];
			int vertexIndexC = borderTriangles[normalTrianglesIndex + 2];

			Vector3 triangleNormal = SurfaceNomalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			if(vertexIndexA >= 0)
            {
				vertexNormals[vertexIndexA] += triangleNormal;
            }
			if(vertexIndexB >= 0)
            {
				vertexNormals[vertexIndexB] += triangleNormal;
            }
			if(vertexIndexC >= 0)
            {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }

		for (int i = 0; i < vertexNormals.Length; i++)
        {
			vertexNormals[i].Normalize();
        }

		return vertexNormals;
    }

	private Vector3 SurfaceNomalFromIndices(int indexA, int indexB, int indexC)
    {
		Vector3 PointA = (indexA < 0) ? borderVertices[-indexA - 1] : vertices[indexA];
		Vector3 PointB = (indexB < 0) ? borderVertices[-indexB - 1] : vertices[indexB];
		Vector3 PointC = (indexC < 0) ? borderVertices[-indexC - 1] : vertices[indexC];

		Vector3 sideAB = PointB - PointA;
		Vector3 sideAC = PointC - PointA;

		return Vector3.Cross(sideAB, sideAC).normalized;
    }

	public void ProcessMesh()
    {
		if(useFlatShading)
        {
			FlatShading();
        }
		else
        {
			BakeNormals();
        }
    }

	private void BakeNormals()
    {
		bakedNormals = CalculateNormals();
    }

	private void FlatShading()
    {
		Vector3[] flatShadedVertices = new Vector3[triangles.Length];
		Vector2[] flatShadedUvs = new Vector2[triangles.Length];

		for(int i = 0; i < triangles.Length; i++)
        {
			flatShadedVertices[i] = vertices[triangles[i]];
			flatShadedUvs[i] = uvs[triangles[i]];
			triangles[i] = i;
        }

		vertices = flatShadedVertices;
		uvs = flatShadedUvs;
    }

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		if(useFlatShading)
        {
			mesh.RecalculateNormals();
        }
		else
        {
			mesh.normals = bakedNormals;
        }

		return mesh;
	}
}
