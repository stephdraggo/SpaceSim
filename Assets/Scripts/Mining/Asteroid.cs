using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Mining
{
    public class Asteroid : Hittable
    {
        [SerializeField] private ResourceType resource;

        public ResourceType Resource => resource;

        [SerializeField] private int resourceSize;

        private float miningTime;

        private bool beingHit;

        private void Awake()
        {
            switch (resource)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnHit()
        {
            beingHit = true;
            StartCoroutine(MiningCheck());
        }

        public override void OnLeave()
        {
            beingHit = false;
        }

        public override void OnDeath()
        {
            //play explosion particles
            
            Destroy(gameObject);
        }

        private IEnumerator MiningCheck()
        {
            float counter = miningTime;
            while (counter >= 0)
            {
                counter += Time.deltaTime;
                if (!beingHit) counter = 0;
                yield return null;
            }

            if (beingHit) Mine();
        }

        private void Mine()
        {
            resourceSize--;

            if (resourceSize <= 0) OnDeath();
        }
    }

    public enum ResourceType
    {
        Copper,
        Iron,
        Diamond,
    }
}