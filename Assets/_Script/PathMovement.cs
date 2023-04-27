using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathMovement : MonoBehaviour
{

    public bool isTimeToGap;
   [SerializeField] private bool isTop;
    private bool istakeGap = false;
    private float stepAngle;
    [SerializeField] private Transform[] all_Path;
    
    private float flt_Offset = 8.48f;
    private float flt_MinRange = 1.5f;
    private float flt_MaxRange = 3f;
    [SerializeField]private float flt_Speed;
    private int persantageOfRoad = 110;

    private float roadSpeed;
    private void Start() {

        roadSpeed = GameManager.instance.flt_RoadSpeed;
        stepAngle = GameManager.instance.flt_StepAngle;
    }
    private void Update() {
        if (!GameManager.instance.isplayerLive) {
            return;
        }
        if (flt_Speed<8) {
            flt_Speed = 3+ GameManager.instance.currentLevelIndex * roadSpeed;
        }
        else {
            flt_Speed = 8;
        }
       
        PathMotion();
    }

    private void PathMotion() {
        for (int i = 0; i < all_Path.Length; i++) {
            all_Path[i].transform.localPosition += new Vector3(-Mathf.Cos(stepAngle * Mathf.Deg2Rad), -Mathf.Sin(stepAngle * Mathf.Deg2Rad), 0) *
               flt_Speed * Time.deltaTime;
            if (all_Path[i].transform.localPosition.x < -Mathf.Cos(stepAngle * Mathf.Deg2Rad) * 2 * flt_Offset || all_Path[i].transform.localPosition.y
                < -Mathf.Sin(stepAngle * Mathf.Deg2Rad) * 2 * flt_Offset) {

              

                pathChange(i);

            }
        }
    }

    private void pathChange(int i) {
        int index = Random.Range(0, 100);
        if (index < persantageOfRoad && isTimeToGap && !istakeGap) {

           
            istakeGap = true;
            float currentRange = Random.Range(flt_MinRange,flt_MaxRange);
            if (i == 0) {
                all_Path[i].transform.localPosition = all_Path[all_Path.Length - 1].transform.localPosition + new Vector3((flt_Offset + currentRange)
                    * Mathf.Cos(stepAngle * Mathf.Deg2Rad),
                (flt_Offset + currentRange) * Mathf.Sin(stepAngle * Mathf.Deg2Rad), 0);
            }
            else {
                all_Path[i].transform.localPosition = all_Path[i - 1].transform.localPosition + new Vector3((flt_Offset + currentRange) * Mathf.Cos(stepAngle * Mathf.Deg2Rad),
               (flt_Offset + currentRange) * Mathf.Sin(stepAngle * Mathf.Deg2Rad), 0);
            }
          

        }
        else {

            WithoutGap(i);
           

        }
    }

    private void WithoutGap(int i) {

        bool isEnemy2Spawn = false;
        if (isTimeToGap && istakeGap) {
            istakeGap = false;
            PathManager.instance.SetOtherPath();
            SpwnEnemy(i);
            isEnemy2Spawn = true;
        }
        if (!isEnemy2Spawn) {
           // SpwnEnemy1(i);
        }
        if (i == 0) {
            all_Path[i].transform.localPosition = all_Path[all_Path.Length - 1].transform.localPosition + new Vector3(flt_Offset * Mathf.Cos(stepAngle * Mathf.Deg2Rad),
            flt_Offset * Mathf.Sin(stepAngle * Mathf.Deg2Rad), 0);
        }
        else {
            all_Path[i].transform.localPosition = all_Path[i - 1].transform.localPosition + new Vector3(flt_Offset * Mathf.Cos(stepAngle * Mathf.Deg2Rad),
           flt_Offset * Mathf.Sin(stepAngle * Mathf.Deg2Rad), 0);
        }
    }
    private void SpwnEnemy(int i) {
        if (isTop) {
            all_Path[i].GetComponent<SpawnObstaclesTwo>().TopSideEnemySpawn();
        }
        else {
            all_Path[i].GetComponent<SpawnObstaclesTwo>().DownSideEnemySpawn();
        }
    }
    //private void SpwnEnemy1(int i) {
    //    if (isTop) {
    //        all_Path[i].GetComponent<SpawnObstaclesTwo>().TopSideEnemy1Spawn();
    //    }
    //    else {
    //        all_Path[i].GetComponent<SpawnObstaclesTwo>().DownSideEnemy1Spawn();
    //    }
    //}
}
