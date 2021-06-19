using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim
{
    public abstract class Hittable : MonoBehaviour
    {
        
        public string Description => description;

        [SerializeField,TextArea]
        private string description;
        public abstract void OnHit();

        public abstract void OnLeave();

        public abstract void OnDeath();
    }
}