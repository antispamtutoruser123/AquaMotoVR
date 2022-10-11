using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AQUAVR
{
    class CameraManager : MonoBehaviour
    {
        static int cameramode;
        static Vector3 roffsetprev= Vector3.zero;
        static Vector3 prevpos = Vector3.zero;

        public static void RecenterRotation()
        {
            if (!CameraPatches.VRCamera) return;
            var angleOffset = CameraPatches.VRPlayer.transform.eulerAngles.y - CameraPatches.VRCamera.transform.eulerAngles.y;
            CameraPatches.DummyCamera.transform.RotateAround(CameraPatches.DummyCamera.transform.position, Vector3.up, angleOffset);
            CameraPatches.DummyCamera.transform.eulerAngles = Vector3.up * CameraPatches.DummyCamera.transform.eulerAngles.y;
           // var hud = GameObject.Find("HUD: Player");
           // hud.transform.RotateAround(CameraPatches.DummyCamera.transform.position, Vector3.up, angleOffset);
           // GameObject.Find("Card_PauseScreen").transform.RotateAround(CameraPatches.VRCamera.transform.position, Vector3.up, angleOffset);
        

        }

        public static void Recenter()
        {
            Logs.WriteInfo($"LLLL: RECENTERING");
            if (!CameraPatches.VRCamera) return;

            Vector3 offset = CameraPatches.startpos - CameraPatches.VRCamera.transform.localPosition;

            RecenterRotation();

            CameraPatches.DummyCamera.transform.Translate(offset - prevpos);

            prevpos = offset;

         /*   switch (cameramode)
            {
                case 0:  // recenter
                    cameramode = 1;
                    break;
                case 1:  // switch to 3rd person
                    CameraPatches.DummyCamera.transform.localPosition = new Vector3(0, 1f, 0) - offset;
                    cameramode = 0;
                    break;
                default:
                    break;
            }
         */
        }
     
    }
}
