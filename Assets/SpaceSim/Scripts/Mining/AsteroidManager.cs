using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BigBoi;
using SpaceSim.Light;
using Rand = UnityEngine.Random;

namespace SpaceSim.Mining
{
    public class AsteroidManager : MonoBehaviour
    {
        #region Variables

        //--------------static-----------------------
        public static AsteroidManager Instance;
        public static bool Ready = false;

        //--------------public properties------------
        public ObjectPooling<Asteroid> Asteroids => asteroids;
        //--------------serialised-------------------
        [SerializeField, Min(1)]
        private int spawnCount;
        [SerializeField]
        private Orbit[] orbiters;
        [SerializeField]
        private int maxOrbiters;
        [SerializeField, Range(0, 1)]
        private float orbiterChance;
        [SerializeField]
        private Asteroid[] spawnableRocks;

        //unity headers are weird okay
        //basically the bounds displayed in the editor are multiplied by 10 right before being used in the script
        [Header("editor 10 = script 100")]
        [Header("Bounds are multiplied by 10")]
        [SerializeField]
        private MinMaxField xBounds;
        [SerializeField]
        private MinMaxField yBounds, zBounds;

        //-------------private-----------------------
        private ObjectPooling<Asteroid> asteroids = new ObjectPooling<Asteroid>();

        #endregion

        private void Awake() {
            Instance = this;
            Ready = false;
        }

        private void Start() {
            for (int i = 0; i < spawnCount; i++) {
                SpawnRock();
            }

            Ready = true;
        }

        #region Other Methods

        /// <summary>
        /// Removes dead rock and spawns a new one
        /// </summary>
        public void OnRockDeath(Asteroid rock) {
            asteroids.Despawn(rock);
            SpawnRock();
        }

        /// <summary>
        /// Spawns new from random prefab at random location
        /// </summary>
        private void SpawnRock() {
            //using ints helps to make the rocks spawn more spread out
            Vector3Int spawnPos = new Vector3Int(xBounds.RandomInt(), yBounds.RandomInt(), zBounds.RandomInt());
            spawnPos *= 10;
            int index = Rand.Range(0, spawnableRocks.Length);

            Asteroid newRock = asteroids.Spawn(spawnableRocks[index], spawnPos, Quaternion.identity);
            
            AddOrbiter(newRock);
            
            newRock.transform.SetParent(transform);

            newRock.resourceSize = Rand.Range(1, 10);
        }

        private void AddOrbiter(Asteroid rock) {
            for (int i = 0; i < maxOrbiters; i++) {
                float randValue = Rand.Range(0f, 1f);
                if (randValue <= orbiterChance) {
                    Transform parent = rock.transform;
                    Vector3 spawnPos = parent.position + Vector3.one;

                    int index = Rand.Range(0, orbiters.Length - 1);
                    Orbit newOrbit = Instantiate(orbiters[index], spawnPos, Quaternion.identity, parent);
                }
                else return;
            }
        }

        #endregion
    }
}