
using UnityEngine;

namespace Player.Avatar
{
    public class HatPicker : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            Debug.Log(gameObject.name);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        private void OnMouseExit()
        {
            transform.localScale = Vector3.one;
        }

        public void OnMouseDown()
        {
            int hatIndex = Hats.hats.IndexOf(gameObject);
            PlayerStats.Instance.HatIndex = hatIndex;
        }
    }
}

