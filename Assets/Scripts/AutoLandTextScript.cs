using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLandTextScript : MonoBehaviour
{
    public bool autoLand;
    public Text autoLandText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        autoLandText.gameObject.SetActive(false);
    }

    public static void Show()
    {
        //instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
       // instance.gameObject.SetActive(false);
    }
}
