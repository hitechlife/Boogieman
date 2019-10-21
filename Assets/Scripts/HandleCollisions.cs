﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class HandleCollisions : MonoBehaviour
{
    //TODO: add in particle system
    private LineRenderer lr;
    [SerializeField] private ParticleSystem goodParticlePrefab;
    [SerializeField] private ParticleSystem badParticlePrefab;
    [SerializeField] private LineConfig config;
    private ControllerObject controller;
    private ControllerObject otherController;
    [SerializeField] private controllerSet controllers;
    [SerializeField] private IntEvent ScorePoint;

    private Vector3[] points;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        points = new Vector3[0];
        lr = GetComponent<LineRenderer>();
        if (lr == null) Debug.LogError("Why isn't there a line renderer");
    }


    public void Setup(Vector3[] new_points,ControllerObject new_controller)
    {
        this.points = new_points;
        lr.positionCount = points.Length;
        lr.SetPositions(points);

        controller = new_controller;
        if (controller == controllers.leftHand)
            otherController = controllers.rightHand;
        if (controller == controllers.rightHand)
            otherController = controllers.rightHand;
        
        StartCoroutine(CheckCollisions());
    }

    //TODO: delete and reset points array when removing points
    IEnumerator CheckCollisions()
    {
        while (true)
        {
            for (int i = points.Length - 1; i >= 0; --i)
            {
                var point = points[i] + transform.position;
                if (CheckController(controller, point, config.correctHandGrace))
                {
                    Instantiate(goodParticlePrefab, point,Quaternion.identity);
                    DestroyRest(i + 1);
                    ScorePoint.Invoke(1);
                    yield return new WaitForSeconds(.05f);
                    break;
                }
                if (otherController != null) //our controller is not the head
                {
                    if (CheckController(otherController, point, config.incorrectHandGrace))
                    {
                        DestroyRest(i);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    void DestroyRest(int j)
    {
        int drop = j;

        for (; j < points.Count(); j++)
        {
            Instantiate(badParticlePrefab, points[j] + transform.position, Quaternion.identity);
        }

        points = points.Take(drop).ToArray();
        lr.SetPositions(points);
    }

    bool CheckController(ControllerObject checkController, Vector3 position,float dist)
    {
        return Vector3.Distance(checkController.pos, position) < dist;
    }
}