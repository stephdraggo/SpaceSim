using System;
using System.Collections;
using System.Collections.Generic;
using SpaceSim.UI;
using UnityEngine;

namespace SpaceSim.Mining
{
    public class Asteroid : Hittable
    {
        [SerializeField] private ResourceType resource;

        public ResourceType Resource => resource;

        public int resourceSize;

        private float miningTime;

        private bool beingHit;

        private void Awake() {
            beingHit = false;
            switch (Resource)
            {
                case ResourceType.Copper:
                    miningTime = 0.2f;
                    break;
                case ResourceType.Iron:
                    miningTime = 0.5f;
                    break;
                case ResourceType.Diamond:
                    miningTime = 3;
                    break;
                case ResourceType.Light:
                    miningTime = float.MaxValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnHit()
        {
            if (!beingHit) {
                beingHit = true;
                StartCoroutine(MiningCheck());
            }
        }

        public override void OnLeave()
        {
            beingHit = false;
        }

        public override void OnDeath()
        {
            //play explosion particles?
            
            AsteroidManager.Instance.OnRockDeath(this);
            
           
        }

        private IEnumerator MiningCheck()
        {
            float counter = miningTime;
            while (counter >= 0)
            {
                counter -= Time.deltaTime;
                if (!beingHit) counter = miningTime;
                yield return null;
            }

            if (beingHit) {
                beingHit = false;
                Mine();
            }
        }

        private void Mine()
        {
            resourceSize--;

            CanvasManager.Instance.UpdateResource(Resource, 1);

            if (resourceSize <= 0) OnDeath();
            else if(beingHit) OnHit();
        }
    }

    public enum ResourceType
    {
        Copper,
        Iron,
        Diamond,
        Light,
    }
}