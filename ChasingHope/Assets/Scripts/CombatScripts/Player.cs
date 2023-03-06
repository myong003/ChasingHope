using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float currHope;
    public float maxHope;
    public float hopeRate;

    private bool isRegenHope;

    void Awake()
    {
        maxHope = 10f;
        currHope = 0f;
        hopeRate = .01f;
    }
    
    // void Start()
    // {
    //     currHope = 0f;    
    // }

    void Update()
    {
        if (currHope < maxHope && !isRegenHope) 
        {
            StartCoroutine(RegenHope());
        }
    }

    private IEnumerator RegenHope() 
    {
        isRegenHope = true;
        while (currHope < maxHope)
        {
            currHope += hopeRate;
            yield return new WaitForSeconds (0.01f);
        }
        isRegenHope = false;
    }

    public void reduceHope(float amount) 
    {
        currHope -= amount;
    }
}
