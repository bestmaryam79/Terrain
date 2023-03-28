using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTerrainGenerator : MonoBehaviour
{

    public int width = 256;
    public int height = 256;
    public int depth = 20;

    public float scale = 20f;           // Scale of the terrain
    public float offsetX = 100f;        // Offset x-coordinate
    public float offsetY = 100f;        // Offset y-coordinate


    public Texture2D grassTexture;      // Grass texture
    public Texture2D dirtTexture;       // Dirt texture
    public Texture2D rockTexture;       // Rock texture

    // Start is called before the first frame update
    void Start()
    {
        // Generate random offsets
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);


    }
    void Update()
    {

        Terrain terrain = GetComponent<Terrain>();  //Getting the terrain component 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);  // Generate the terrain

    }
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

    /*
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
    } */
    // Generate the heights of the terrain


    // Calculate the height at the given x and y coordinate
    /* float CalculateHeight(int x, int y)
     {
         // Calculate the x and y coordinate based on the given scale and offset
         float xCoord = (float)x / width * scale + offsetX;
         float yCoord = (float)y / depth * scale + offsetY;

         // Generate the height using Perlin noise and return it
         return Mathf.PerlinNoise(xCoord, yCoord);
    } */



    // Generate the texture
    float[,,] GenerateTexture()
    {
        float[,,] texture = new float[width, height, 3];

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
                if (noise < 0.3f)
                {
                    texture[x, y, 0] = 1f;
                    texture[x, y, 1] = 0f;
                    texture[x, y, 2] = 0f; // set to grass texture
                }
                else if (noise < 0.6f)
                {
                    texture[x, y, 0] = 0f;
                    texture[x, y, 1] = 1f;
                    texture[x, y, 2] = 0f; // set to dirt texture
                }
                else
                {
                    texture[x, y, 0] = 0f;
                    texture[x, y, 1] = 0f;
                    texture[x, y, 2] = 1f; // set to rock texture
                }
            }
        }

        // Apply the textures to the terrain
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData.splatPrototypes = new SplatPrototype[]
        {
            new SplatPrototype() {texture = grassTexture},
            new SplatPrototype() {texture = dirtTexture},
            new SplatPrototype() {texture = rockTexture}
        };

        return texture;
    }
}







/* int x, y; 
float z1, z2; 
float increment = 0.025; 
int scale = 5; 
int numRows, numColumns; 
int zScale = 200; 
int xOffset, yOffset;
float terrainIncrement  = 0.005; 
float t1, t2; 
float m1, m2; 
void setup() {
  size(1280, 720, P3D); 
  noiseSeed(10052014);
  numRows = height / scale; 
  numColumns = width / scale;
}

void draw() {
  background(255, 255, 255); 
  noiseDetail(3, 0.5); 
  translate(0, height / 2); 
  rotateX(radians(60)); 
  for (y = 0; y < numRows; y++) {
    beginShape(TRIANGLE_STRIP); 
    for (x = 0; x <= numColumns; x++) {
      z1 = abs((noise((x + xOffset) * increment, (y + yOffset) * increment, 0) * 2) - 1); 
      z2 = abs((noise((x + xOffset) * increment, (y + 1+ yOffset) * increment, 0) * 2) - 1); 
      if (z1 > 0.7) {
        fill(255);
      } else if (z1 > 0.4)
      {
        fill(112, 84, 27);
      } else if (z1 > 0.05)
      {
        fill(17, 92, 24);
      } else
      {
        fill(24, 163, 156);
      }
      t1 = noise((x + xOffset) * terrainIncrement, (y + yOffset) * terrainIncrement, 10) * 255; 
      t2 = noise((x + xOffset) * terrainIncrement, (y + 1 + yOffset) * terrainIncrement, 10) * 255;
      //fill(t1);
      if (t1 < 102) m1 = 50; 
      else if (t1 < 152) m1 = 50 + ((t1 - 102) * 3);
      else m1 = 200; 
      if (t2 < 102) m2 = 50; 
      else if (t2 < 152) m2 = 50 + ((t2 - 102) * 3);
      else m2 = 200; 
      z1 = z1 * m1; 
      z2 = z2 * m2; 

      vertex(x * scale, y * scale, z1); 
      vertex(x * scale, (y + 1) * scale, z2);
    }
    endShape();
  }
  if (keyPressed)
  {
    //println(keyCode); 
    if (key == CODED)
    {
      if (keyCode == UP)
      {
        yOffset -= 1;
      } else if (keyCode == DOWN)
      {
        yOffset += 1;
      } else if (keyCode == LEFT) 
      {
        xOffset -= 1;
      } else if (keyCode == RIGHT)
      {
        xOffset += 1;
      }
    }
  }
} 
*/