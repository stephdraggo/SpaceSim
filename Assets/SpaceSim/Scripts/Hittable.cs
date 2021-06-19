using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim
{
    public abstract class Hittable : MonoBehaviour
    {
        public abstract void OnHit();

        public abstract void OnLeave();

        public abstract void OnDeath();
    }
}