using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeTexture : MonoBehaviour
{
    // Variables to control the size and depth of the terrain
    public int width = 256;
    public int height = 256;
    public int depth = 20;


    public float scale = 20f;           // Scale of the terrain
    public float offsetX = 100f;        // Offset x-coordinate
    public float offsetY = 100f;        // Offset y-coordinate

    // Start is called before the first frame update
    void Start()
    {
        // Generate random offsets
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        
    }

    // Generate the terrain and update it every frame
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();  //Getting the terrain component 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);  // Generate the terrain
    }

    // Generate the terrain
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the heightmap resolution and size of the terrain data
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        // Generate the heights of the terrain and set it to the terrain data
        float[,] heights = GenerateHeights();
        terrainData.SetHeights(0, 0, heights);

        // Generate the texture and set it to the terrain data
        float[,,] texture = GenerateTexture();
        terrainData.baseMapResolution = width + 1;
        terrainData.alphamapResolution = width + 1;
        terrainData.SetAlphamaps(0, 0, texture);

        return terrainData;
    }

    // Generate the heights of the terrain using Perlin noise
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        // Loop through each x and y coordinate to generate the heights
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale + offsetX;
                float yCoord = (float)y / height * scale + offsetY;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                heights[x, y] = sample;
            }
        }

        return heights;
    }

    // Generate the texture using Perlin noise
    float[,,] GenerateTexture()
    {
        float[,,] texture = new float[width, height, 2];


        // Loop through each x and y coordinate to generate the texture
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate the x and y coordinate based on the given scale and offset
                float xCoord = (float)x / width * scale + offsetX;
                float yCoord = (float)y / height * scale + offsetY;

                // Generate the texture using Perlin noise
                float noise = Mathf.PerlinNoise(xCoord, yCoord);

                // Set the texture based on the noise value
                if (noise < 0.2f)
                {
                    texture[x, y, 0] = 1f;
                    texture[x, y, 1] = 0f;
                }
                else if (noise < 0.5f)
                {
                    texture[x, y, 0] = 0f;
                    texture[x, y, 1] = 1f;
                }
                else
                {
                    texture[x, y, 0] = 1f;
                    texture[x, y, 1] = 1f;
                }
            }
        }

        return texture;
    }
}
