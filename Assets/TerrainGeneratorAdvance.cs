using UnityEngine;

public class TerrainGeneratorAdvance : MonoBehaviour
{
    public int width = 256;             // Width of the terrain
    public int height = 256;            // Height of the terrain
    public int depth = 20;              // Depth of the terrain

    public float scale = 20f;           // Scale of the terrain
    public float offsetX = 100f;        // Offset of the x-coordinate
    public float offsetY = 100f;        // Offset of the y-coordinate

    // Start is called before the first frame update
    void Start()
    {
        // Generate random offsets
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }
    void Update()
    {
        // Get the terrain component and generate the terrain
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

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
    // Generate the terrain data
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the heightmap resolution and size of the terrain data
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        // Generate the heights of the terrain and set it to the terrain data
        terrainData.SetHeights(0, 0, GenerateHeights());

        // Generate the texture and set it to the terrain data
        terrainData.baseMapResolution = width + 1;
        terrainData.alphamapResolution = width + 1;
        terrainData.SetAlphamaps(0, 0, GenerateTexture());

        return terrainData;
    }

    // Generate the texture
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

/*
This script generates a terrain using Perlin noise. 
It does this by generating random offsets, 
    setting the heightmap resolution and size of the terrain, 
    generating the heights of the terrain using Perlin noise, 
    and setting the heights to the terrain data. 
The height of each point in the terrain is calculated based on the x and y coordinate, the scale, and the offset using Perlin noise.
*/
/*
In this implementation, the GenerateTexture method generates a texture based on Perlin noise and sets it to the terrainData using the SetAlphamaps method. 
The texture is generated based on the noise value at each coordinate, and it sets the texture to either fully grass, fully dirt, or a mix of both based on the noise value.
*/