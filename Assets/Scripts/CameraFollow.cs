using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /* public Transform target;

      public float smoothSpeed = 0.125f;
      public Vector3 offset;
  */
    public GameObject player;
    public bool zoomOut;
    public bool zoomIn;
    public bool zoomDone;

    void LateUpdate()
    {
        if(player.tag == "Player" /*&& zoomDone*/)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            StopCoroutine("ZoomToShip");
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, player.transform.eulerAngles.z);
        }
        else if(player.tag == "Rocket" && zoomDone)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            StopCoroutine("ZoomToShip");
        }

        if (zoomOut)
        {
            StopCoroutine("ZoomIn");
            StopCoroutine("ZoomToShip");
            StartCoroutine("ZoomOut");
            StartCoroutine("ZoomToShip");
            zoomOut = false;
        }
        else if(zoomIn)
        {
            StopCoroutine("ZoomOut");
            StopCoroutine("ZoomToShip");
            StartCoroutine("ZoomIn");
            //StartCoroutine("ZoomToShip");
            zoomIn = false;
        }    

        if(Input.GetKeyDown("."))
        {
            Camera.main.orthographicSize += 10;
        }

        if (Input.GetKeyDown(","))
        {
            Camera.main.orthographicSize -= 10;
        }
    }

    IEnumerator ZoomOut()
    {
        while (Camera.main.orthographicSize < 60)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 60, 0.02f);
            //transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, 0.01f), Mathf.Lerp(transform.position.y, player.transform.position.y, 0.01f), -10f);
            yield return null;
        }
        StopCoroutine("ZoomOut");
        yield return null;
    }

    IEnumerator ZoomToShip()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        while (dist > 0.2)
        {
            dist = Vector2.Distance(transform.position, player.transform.position);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, 0.02f), Mathf.Lerp(transform.position.y, player.transform.position.y, 0.02f), -10f);
            yield return null;
        }
        zoomDone = true;
        yield return null;
    }

    IEnumerator ZoomIn()
    {
        while (Camera.main.orthographicSize > 15)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 15, 0.01f);
            yield return null;
        }
        StopCoroutine("ZoomIn");
        yield return null;
    }
}
