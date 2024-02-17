using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  CRAFTIZON.Utils.EFS
{
/// <summary>
/// The surface determination class has easy functions to help determine what texture the player object is standing on
/// currently, contains function for both terrain and normal mesh textures
/// </summary>
    public class SurfaceDetermination : MonoBehaviour
    {   
        

            /// <summary>
            /// This function retrives  the main texture of first material from a mesh renderer componenent
            /// </summary>
            /// <param name="mesh"></param>
            /// <returns>MeshRenderer</returns>
        


            public  Texture GetTextureOnMesh(MeshRenderer mesh)
            {

                Texture texture =  null;
            
                texture =  mesh.materials[0].mainTexture;

                return  texture;

            }
        
            /// <summary>
            /// This function returns a texure from terrain at a given point in word space.
            /// This is done by getting the  alph-maps in terrain data model from the terrain  
            /// </summary>
            /// <param name="_terrain"></param>
            /// <param name="_position"></param>
            /// <returns></returns>
            public Texture GetTextureAtPosition(Terrain _terrain ,  Vector3 _position)
            {
                
                // Initialize variables, dont teach me good code practices

                Texture texture = null; 
                Terrain terrain =  _terrain;

                // Getting the terrain  data model from terrrain 

                TerrainData terrainData = terrain.terrainData;

                Vector3 worldPosition = _position;

                // converting world position to local position that can be used to refer to  a specfic point on terrain

                Vector3 terrainLocalPosition = worldPosition - terrain.transform.position;

                // Use the X,Y from vector3 because we are working on a flat X and Y  plane

                Vector2 normalizedPos = new Vector2(
                    terrainLocalPosition.x / terrainData.size.x,
                    terrainLocalPosition.z / terrainData.size.z
                );

                int textureX = (int)(normalizedPos.x * terrainData.alphamapWidth);
                int textureY = (int)(normalizedPos.y * terrainData.alphamapHeight);

                float[,,] splatmapData = terrainData.GetAlphamaps(textureX, textureY, 1, 1);

                float maxTextureValue = Mathf.NegativeInfinity;
                int maxTextureIndex = 0;

                // Iterating thorugh the alpha-maps  

                for (int i = 0; i < splatmapData.GetLength(2); i++)
                {
                    if (splatmapData[0, 0, i] > maxTextureValue)
                    {
                        maxTextureValue = splatmapData[0, 0, i];
                        maxTextureIndex = i;
                    }
                }
                
                // Oncee we have the terrain layers and the 'most intense' layer at a point  >  _position
                // we can use the index of the layer to later acces the diffuse texture on this layer
                TerrainLayer selectedTexture = terrainData.terrainLayers[maxTextureIndex];

                // ASSign the  diffuse texture on layer to the texture variable we created at top to finnaly throw it
                texture = selectedTexture.diffuseTexture;
                    
                // Throw the found texture and have fun with ot
                return texture;

            }

    }
}