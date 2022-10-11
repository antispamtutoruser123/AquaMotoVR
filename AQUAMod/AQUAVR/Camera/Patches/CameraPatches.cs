using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;


namespace AQUAVR
{

    [HarmonyPatch]
    public class CameraPatches
    {

        public static GameObject DummyCamera, VRCamera, VRPlayer;
        public static GameObject newUI;
        public static Vector3 startpos, startrot, offset;
        public static bool fpmode = false;

        private static readonly string[] canvasesToIgnore =
{
        "com.sinai.unityexplorer_Root", // UnityExplorer.
        "com.sinai.unityexplorer.MouseInspector_Root", // UnityExplorer.
        "IntroCanvas"
    };
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LoadingScreen), "Awake")]
        private static void LoadingScreen(LoadingScreen __instance)
        {
            __instance.gameObject.transform.localScale = new Vector3(.5f, .5f, 1f);
            __instance.transform.localPosition += new Vector3(0, 1.5f, 02f);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(IntroSequence), "Start")]
        private static void SplashScreen(IntroSequence __instance)
        {
            __instance.gameObject.transform.localScale = new Vector3(.1f, .1f, 1f);

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(JetskiPlayer), "Start")]
        private static void getinput(JetskiPlayer __instance)
        {
            
                CameraManager.Recenter();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(JetskiPlayer), "Update")]
        private static void getkeyboardinput(JetskiPlayer __instance)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                DummyCamera.transform.position += new Vector3(0, 0.1f, 0);
                startpos += new Vector3(0, 0.1f, 0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                DummyCamera.transform.position -= new Vector3(0, 0.1f, 0);
                startpos -= new Vector3(0, 0.1f, 0);
            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(JetskiCamera), "Awake")]
        private static void CreatCamera(JetskiCamera __instance)
        {
            if (__instance.gameObject.name == "PlayerCamera(Clone)" && !DummyCamera)
            {
                DummyCamera = new GameObject("DummyCamera");
                VRCamera = __instance.transform.Find("pCamera").gameObject;
                VRCamera.GetComponent<Camera>().nearClipPlane = .01f;
                VRCamera.GetComponent<Camera>().farClipPlane = 10000f;
                
                DummyCamera.transform.parent = __instance.transform;
                VRPlayer = __instance.gameObject;
                DummyCamera.transform.localPosition = new Vector3(0.0817f, -2.2828f, 4.738f)- VRPlayer.transform.position+ new Vector3(0,.4f,0);
              //  DummyCamera.transform.localPosition = new Vector3(0.0817f, -2.0828f, 4.738f);
              //  DummyCamera.transform.eulerAngles = new Vector3(359.6745f, 358.8231f, 4.313f);

                VRCamera.transform.parent = DummyCamera.transform;

                startpos = new Vector3(-.1f, 1.36f, -.22f);
                startrot = new Vector3(358.6f, 354.9f, .37f);
                offset = startpos - VRCamera.transform.localPosition;

                if (GameObject.Find("mesh_DeepBlueBullhead"))
                {
                    startpos += new Vector3(0, .2f, 0);
                    DummyCamera.transform.localPosition += new Vector3(0, .2f, 0);
                }
                // CameraManager.Recenter();
            }
        }

            [HarmonyPrefix]
        [HarmonyPatch(typeof(JetskiCamera), "SetMode_FirstPerson")]
        private static void RecenterCamera(JetskiCamera __instance)
        {
            CameraManager.Recenter();
            fpmode = !fpmode;

         /*   if(fpmode)
            DummyCamera.transform.localPosition = new Vector3(.0477f,-1.7957f,.532f);
          else
                DummyCamera.transform.localPosition = new Vector3(.0477f, -1.8957f, 4.8265f);
         */
        }
           


        [HarmonyPrefix]
        [HarmonyPatch(typeof(CanvasScaler), "OnEnable")]
        private static void MoveIntroCanvas(CanvasScaler __instance)
        {
            if (IsCanvasToIgnore(__instance.name)) return;

            var uicamera = GameObject.Find("UICamera_Main");
            if (uicamera)
            {
                uicamera.GetComponent<Camera>().nearClipPlane = .01f;
                uicamera.GetComponent<Camera>().farClipPlane = 10000f;

            }

                Logs.WriteInfo($"Hiding Canvas:  {__instance.name}");
            var canvas = __instance.gameObject.GetComponent<Canvas>();

            if (canvas.renderMode != RenderMode.WorldSpace)
            {
                canvas.renderMode = RenderMode.WorldSpace;
              
            }




            //  AttachedUi.Create<StaticUi>(canvas, 1f);
        }

  
        private static bool IsCanvasToIgnore(string canvasName)
        {
            foreach (var s in canvasesToIgnore)
                if (Equals(s, canvasName))
                    return true;
            return false;
        }

    }
}

