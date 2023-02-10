using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWobble : MonoBehaviour
{ // --FK-- 2023.02.08
        public float wobbleAmount = 0.7f;

    [SerializeField] public Transform currentCamera = null;
    Vector3 m_cameraPos;

    void Start()
    {
        currentCamera = Camera.main.transform;
        m_cameraPos = currentCamera.transform.position;
    }

    void Update(){
       
        /// szinusz függvény
        //https://gist.github.com/ftvs/5822103
        currentCamera.localPosition = m_cameraPos + new Vector3(0, Mathf.Sin(Time.time * 2) * wobbleAmount, 0);
    }

    void OnEnable(){
        m_cameraPos = currentCamera.localPosition;
    }


}
