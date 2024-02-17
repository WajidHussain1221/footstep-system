using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is master class of the system, assign this to a child of your player
/// This class requres surface determination class and audio source component attached to this same object for it to function
/// </summary>

namespace  CRAFTIZON.Utils.EFS
{
    [RequireComponent(typeof(SurfaceDetermination))]
    [RequireComponent(typeof(AudioSource))]
    public class FootstepSystem : MonoBehaviour
    {   



        #region  Singletone

        public static FootstepSystem  Instance;

        void Awake(){Instance =  this;}

        #endregion




        public FootStepDatabase footStepDatabase;
        private SurfaceDetermination surfaceDetermination;

        [Range(0 , 10)]
        public float detectionHeight = 5;

        public LayerMask detectionLayer;


        [Range(0 , 10)]
        public float speed = 0.75f;  

        private float nextFootstepTime;

        [SerializeField ]private bool play;

        FootStepSurface currentSurface;

        private AudioSource audioSource;








        
        public void StartFootStepSounds(){
            play =  true;
        }
        public void StopFootStepSounds(){
            play = false;
        }
    
        void Start()
        {   
            audioSource =  GetComponent<AudioSource>();
            nextFootstepTime = Time.time + speed;
        }

        
        void Update()
        {
            SetFootstepData();

            if(play)
            {
                if (Time.time >= nextFootstepTime)
                {
                
                    int randomIndex = Random.Range(0, currentSurface.footStepSounds.Length);
                    audioSource.clip = currentSurface.footStepSounds[randomIndex];
                    audioSource.Play();

                    nextFootstepTime = Time.time + speed;
                }       
            }
        }


        public void SetFootstepData (){

            surfaceDetermination =  GetComponent<SurfaceDetermination>();

            HitPointSurfaceInfo  hitPoint =  GetSurfaceBeneath (transform ,  detectionHeight);
            Texture texture  =  null; 
            if(hitPoint.IsTerrain())
            {
                texture = surfaceDetermination.GetTextureAtPosition (hitPoint.terrain , hitPoint.point);
            }
            else
            {
                if(hitPoint.IsMesh())
                texture = surfaceDetermination.GetTextureOnMesh(hitPoint.meshRenderer);
            }  
            if(texture != null)
            {
                currentSurface =  footStepDatabase.SearchSurfaceByTexture (texture);

            }
        

        }


        private HitPointSurfaceInfo  GetSurfaceBeneath(Transform refrencePoint, float range){

            HitPointSurfaceInfo  surfaceInfo =  new HitPointSurfaceInfo();
            
            RaycastHit  hit; 

            if (Physics.Raycast(refrencePoint.position , refrencePoint.forward ,   out hit , range , detectionLayer))
            {   
                if(hit.transform !=  null)
                {

                    hit.transform.TryGetComponent<Terrain>(out Terrain terrain);
                    hit.transform.TryGetComponent<MeshRenderer>(out MeshRenderer  mesh);

                    Vector3  point = hit.point;

                    surfaceInfo =  new HitPointSurfaceInfo {
                        terrain =  terrain,
                        point =  point,
                        meshRenderer =  mesh

                    };
                    


                }
            }


            return surfaceInfo;

        }


        void OnDrawGizmosSelected(){

            Gizmos.color =  Color.red;

            Gizmos.DrawLine(transform.position,  new Vector3( 0,0,   detectionHeight));

        }
    }


    public class HitPointSurfaceInfo  {

        public Terrain terrain;
        public Vector3 point;


        public MeshRenderer meshRenderer;



        public bool IsTerrain(){

            return  terrain !=  null;
        }

        public bool IsMesh(){
            return  meshRenderer !=  null;
        }

    
    }
}