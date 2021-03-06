﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discoTile : MonoBehaviour
{
    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        colors = new Color[7];
        colors[0] = new Color(0f, 115f, 155f);
        colors[1] = new Color(0f, 155f, 30f);
        colors[2] = new Color(155f, 0f, 40f);
        colors[3] = new Color(050f, 0f, 155f);
        colors[4] = new Color(155f, 124f, 0f);
        colors[5] = new Color(7, 125f, 0f);
        colors[6] = new Color(155f, 0f, 0f);
        StartCoroutine(materialChange());
    }

    IEnumerator materialChange()
    {
        while (true)
        {
            int val = Random.Range(0, 7);
            Color color = colors[val];
            this.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;

    }
}
