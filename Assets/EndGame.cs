﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject me;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
