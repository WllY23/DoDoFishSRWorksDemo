﻿using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class GameStatePufferFish : IState, GameStateManager.IGameStateUpdateRealWorldCameraImage
    {
        GameStateManager manager;
        public void EnterState(IState oldState, StatePatternBase statePatternBase)
        {
            manager = statePatternBase as GameStateManager;
            GameManager.Instance.StartFishDemo();

            //Set touch fish hand            
            GameManager.Instance.SetHandTouchMode();
            SRWorkHand.GetDynamicHand().gameObject.layer = HandTouchFish.HandTouchLayer;
        }

        public void LeaveState()
        {
            SRWorkHand.Instance.CloseDetectHand();
            GameManager.Instance.ClosePufferFishDemo();
        }

        public string Name()
        {
            return "GameStatePufferFish";
        }

        public void UpdateState()
        {
            if (GameManager.Instance.ViveIsGrip() &&
                GameManager.Instance.IsFishIdle() //fish idle, player can 'grip' to switch game, otherwise 'grip' is pick up gun.
                                                  //the pick up gun behaviour is handle by ViveInputUtility.
                )
            {
                manager.SwitchState(GameStateManager.GameState.ROBOT);
            }
            else if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
            {
                if (GameManager.Instance.IsFishIdle())
                {
                    GameManager.Instance.FairyStick.gameObject.SetActive(!GameManager.Instance.FairyStick.gameObject.activeSelf);
                }
            }
        }

        public void LRealWorldCamRTRefresh(RenderTexture rt)
        {
            List<MeshRenderer> renders = new List<MeshRenderer>(GameManager.Instance.PaintBallWallGameObj.GetComponentsInChildren<MeshRenderer>());
            renders.AddRange(GameManager.Instance.hidingDoor.GetComponentsInChildren<MeshRenderer>());
            foreach (MeshRenderer mr in renders)
            {
                if (mr.gameObject.tag == "PaintBallWall_Lift" ||
                    mr.gameObject.tag == "PaintBallWall_Fix")
                {
                    mr.material.SetTexture("_ReflectTextureL", rt);
                }
            }
        }

        public void RRealWorldCamRTRefresh(RenderTexture rt)
        {
            List<MeshRenderer> renders = new List<MeshRenderer>(GameManager.Instance.PaintBallWallGameObj.GetComponentsInChildren<MeshRenderer>());
            renders.AddRange(GameManager.Instance.hidingDoor.GetComponentsInChildren<MeshRenderer>());
            foreach (MeshRenderer mr in renders)
            {
                if (mr.gameObject.tag == "PaintBallWall_Lift" ||
                    mr.gameObject.tag == "PaintBallWall_Fix")
                {
                    mr.material.SetTexture("_ReflectTextureR", rt);
                }
            }
        }
    }
}