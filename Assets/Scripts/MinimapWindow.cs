using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimap
{
    public class MinimapWindow : MonoBehaviour
    {
        public static MinimapWindow instance;
        public bool a = true;
        // Start is called before the first frame update

        private void Update()
        {
            /*if(Input.GetKeyDown("m") && a)
            {
                Show();
                a = false;
            }
            if (Input.GetKeyDown("m") && !a)
            {
                Hide();
                a = true;
            }*/
        }

        private void Awake()
        {
            instance = this;
            instance.gameObject.SetActive(false);
        }

        public static void Show()
        {
            instance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            instance.gameObject.SetActive(false);
        }

    }
}
