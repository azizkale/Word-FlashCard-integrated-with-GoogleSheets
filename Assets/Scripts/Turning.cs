using System.Collections;
using UnityEngine;


public class Turning : MonoBehaviour
{
    public GameObject appCube;
    string m_Path;
    void Start()
    {
     

    }

    

   IEnumerator turnTheCard(GameObject go)
    {
        if (go.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            for (int i = 0; i <= 10; i++)
            {
                //go.transform.Rotate(new Vector3(0, i*18, 0));
                go.transform.rotation = Quaternion.Euler(0, 180, 0);
                yield return new WaitForSeconds(0.00001f);
            }
        }

        if (go.transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            //for (int i = 0; i <= 10; i--)
            //{
                //go.transform.Rotate(new Vector3(0, i*18, 0));
                go.transform.rotation = Quaternion.Euler(0, 0, 0);
                yield return new WaitForSeconds(0.00001f);
            //}
        }
    }
   

   
    public void turnToBack()
    {
       appCube.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void turnToFront()
    {
        appCube.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
