
using System.Collections.Generic;
using UnityEngine;

namespace Player.Avatar
{
    public class Hats : MonoBehaviour
    {
        public static List<GameObject> hats;


        private void Awake()
        {
            hats = new List<GameObject>();
            
            foreach (Transform child in transform)
            {
                hats.Add(child.gameObject);
            }

            foreach (GameObject hat in hats)
            {
                hat.AddComponent<HatPicker>();
            }
        }
    }
}

