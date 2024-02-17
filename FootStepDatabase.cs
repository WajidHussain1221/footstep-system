using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FootstepDatabase contains list of all the registered surfaces that have footsteps applied
/// </summary>

namespace  CRAFTIZON.Utils.EFS
{


    [CreateAssetMenu(menuName = "Footstep System/Footstep Database", fileName = "New Footstep Database" )] 
    public class FootStepDatabase : ScriptableObject
    {   

        

        public string  varientName;

        // List (list)  of all the surfaces that we want footstep sound to apply to....
        public List<FootStepSurface>  registeredSurfaces; 

        // IDK , remove this probably
        void OnValidate(){

            varientName =   this.name;

        }


        /// <summary>
        /// This searches the list of registered surfaces provided a texutre, 
        /// Iterating over all the surfaces and all the textures  linked to thesse surfaces
        ///  make sense of a surface with a  texture given to as argument to this function.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>FootStepSurface</returns>

        public FootStepSurface SearchSurfaceByTexture(Texture texture){

            // Creating an instance of our return type so we can modify it as we go
            // and return it respectfully at the end instead of returning it in every scope.
            FootStepSurface  surface =  new FootStepSurface ();

            // Iterating through all the surfaces
            for (int  surfaceIndex = 0 ;   surfaceIndex   < registeredSurfaces.Count ;  surfaceIndex ++ )
            {       

                // Iterating through all the textures in current surface we are in
                for (int textureIndex = 0; textureIndex < registeredSurfaces[surfaceIndex].dedicatedTextures.Length; textureIndex++)
                {  
                    // comparing if current texture is the texture we are looking for

                    Texture t =    registeredSurfaces[surfaceIndex].dedicatedTextures[textureIndex]; 
                    if (t ==  texture)
                    {
                        // if texture is found , it means the object in upper iterattion  is the correct  surface object
                    // we are looking for, if so we assign the surface to the surface var

                        surface =  registeredSurfaces[surfaceIndex];
                        // end the for loop
                        break;
                    }
                }
            }

            // throw the surface we found
            return  surface;
        }

    }

    /// <summary>
    /// This struct defines a surface  that  the footstep system applies to.
    /// </summary>

    [System.Serializable]
    public struct  FootStepSurface {


        // This memeber has no use case, just keeps the inspector nice and tidy
        public string surfaceName;

        // List (array) of textures that  are linked to  'this' surface.  These textures can later refer back to this instance.
        public Texture[]   dedicatedTextures;
    
        // List (array)   of audio clips of footsteps that are played when the player steps on object with this texture.
        public AudioClip[]  footStepSounds;

    }
}