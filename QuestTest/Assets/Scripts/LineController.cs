// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class LineController : MonoBehaviour
// {
//     public int positionsBeforeThrow;
//     public int positionsAfterThrow;
//
//     private Vector3[] BeforeThrowPositions;
//     private int BeforeIndex;
//
//     private Vector3[] AfterThrowPositions;
//     private int afterIndex;
//
//
//     public MiniGameController miniGameController;
//    
//     public LineRenderer beforeLR;
//     public LineRenderer afterLR;
//     private bool AfterThrow = false;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         BeforeThrowPositions = new Vector3[positionsBeforeThrow];
//         AfterThrowPositions = new Vector3[positionsAfterThrow];
//
//
//         miniGameController.OnThrowEvent += OnThrow;
//     }
//
//     private void OnThrow(object sender, DiscThrowEventArgs e)
//     {
//         AfterThrow = true;
//     }
//
//     private void FixedUpdate()
//     {
//         if (!AfterThrow)
//         {
//             BeforeThrowPositions[BeforeIndex] = transform.position;
//             BeforeIndex = (BeforeIndex + 1) % positionsBeforeThrow;
//         }
//         else
//         {
//             if (afterIndex < positionsAfterThrow)
//             {
//                 AfterThrowPositions[afterIndex] = transform.position;
//                 afterIndex++;
//             }
//             else
//             {
//                 RenderLine();
//             }
//         }
//     }
//
//     private void RenderLine()
//     {
//         beforeLR.positionCount = positionsBeforeThrow;
//         afterLR.positionCount = positionsAfterThrow;
//
//         for (int i = 0; i < positionsBeforeThrow; i++)
//         {
//             beforeLR.SetPosition(i, BeforeThrowPositions[(BeforeIndex + i) % positionsBeforeThrow]);
//         }
//
//         for (int i = 0; i < positionsAfterThrow; i++)
//         {
//             afterLR.SetPosition(i, AfterThrowPositions[i]);
//         }
//
//         ResetArrays();
//
//         AfterThrow = false;
//     }
//
//     private void ResetArrays()
//     {
//         BeforeIndex = 0;
//         afterIndex = 0;
//
//         for (int i = 0; i < positionsBeforeThrow; i++)
//         {
//             BeforeThrowPositions[i] = Vector3.zero;
//         }
//
//         for (int i = 0; i < positionsAfterThrow; i++)
//         {
//             AfterThrowPositions[i] = Vector3.zero;
//         }
//     }
// }
//
// public enum LineControllerState
// {
//     WaitingForThrow,
// }