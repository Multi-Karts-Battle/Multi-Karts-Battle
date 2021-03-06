﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 5;
    public int numOfHearts = 5;
    
    public Image[] hearts = new Image[5];
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start() {
        GameObject[] g = GameObject.FindGameObjectsWithTag("hearts");
        for (int i = 0; i < 5; i++) {
            hearts[i] = g[i].GetComponent<Image>();
        }
    }
    void Update(){

        if(health > numOfHearts){
            health = numOfHearts;
        }
        
        Debug.Log("health:" + health);
        for (int i = 0; i < hearts.Length; i++){
            Debug.Log(i);
            Debug.Log(hearts[i].sprite);
            if(i < health){
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
            if(i < numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
            Debug.Log(hearts[i].sprite);
        }
    }
}
