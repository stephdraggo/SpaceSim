using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Light
{
    [RequireComponent(typeof(SphereCollider))]
    public class LightBalls : MonoBehaviour
    {
        #region Variables
        //-------------serialised--------------
        [SerializeField]
        private Orbit prefab;
        
        
        #endregion
        void Start() { }

        void Update() { }
    }
}