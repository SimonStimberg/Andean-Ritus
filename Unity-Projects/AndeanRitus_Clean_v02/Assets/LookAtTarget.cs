using System;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
     public Transform target;

      void Update()
      {
           if(target != null)
           {
                transform.LookAt(target);
				transform.Rotate(90, 0, 0, Space.Self);
           }
      }


}
