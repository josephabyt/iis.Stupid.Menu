using BepInEx;
using ExitGames.Client.Photon;
using GorillaLocomotion.Climbing;
using GorillaLocomotion.Swimming;
using GorillaTagScripts;
using HarmonyLib;
using iiMenu.Classes;
using iiMenu.Menu;
using iiMenu.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static iiMenu.Classes.RigManager;
using static iiMenu.Menu.Main;

namespace iiMenu.Mods
{
    public class Movement
    {
        public static void ChangeInvisType()
        {
            invisType++;
            if (invisType > 2.5)
            {
                invisType = 0;
            }
            string[] invisibleTypeNames = new string[] {
                "Toggle",
                "Hold",
                "Reverse Hold"
            };
            GetIndex("visType").overlapText = "Change Invisible Type <color=grey>[</color><color=green>" + invisibleTypeNames[invisType] + "</color><color=grey>]</color>";
        }        

        public static void ChangePlatformType()
        {
            platformMode++;
            if (platformMode > 11)
            {
                platformMode = 0;
            }

            string[] platformNames = new string[] {
                "Normal",
                "Invisible",
                "Rainbow",
                "Random Color",
                "Noclip",
                "Glass",
                "Snowball",
                "Water Balloon",
                "Rock",
                "Present",
                "Mentos",
                "Fish Food"
            };

            GetIndex("Change Platform Type").overlapText = "Change Platform Type <color=grey>[</color><color=green>" + platformNames[platformMode] + "</color><color=grey>]</color>";
        }

        public static void ChangePlatformShape()
        {
            platformShape++;
            if (platformShape > 6)
            {
                platformShape = 0;
            }

            string[] platformShapes = new string[] {
                "Sphere",
                "Cube",
                "Cylinder",
                "Legacy",
                "Small",
                "Long",
                "1x1"
            };

            GetIndex("Change Platform Shape").overlapText = "Change Platform Shape <color=grey>[</color><color=green>" + platformShapes[platformShape] + "</color><color=grey>]</color>";
        }

        public static GameObject CreatePlatform()
        {
            GameObject platform = null;
            if (platformShape == 0)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                platform.transform.localScale = new Vector3(0.333f, 0.333f, 0.333f);
            }
            if (platformShape == 1)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.localScale = new Vector3(0.333f, 0.333f, 0.333f);
            }
            if (platformShape == 2)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                platform.transform.localScale = new Vector3(0.333f, 0.333f, 0.333f);
            }
            if (platformShape == 3)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
            }
            if (platformShape == 4)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.localScale = new Vector3(0.025f, 0.15f, 0.2f);
            }
            if (platformShape == 5)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.localScale = new Vector3(0.025f, 0.3f, 0.8f);
            }
            if (platformShape == 6)
            {
                platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (platformMode != 5)
            {
                platform.GetComponent<Renderer>().material.color = GetBGColor(0f);
            }
            if (platformMode == 1)
            {
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 2)
            {
                float h = (Time.frameCount / 180f) % 1f;
                platform.GetComponent<Renderer>().material.color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
            }
            if (platformMode == 3)
            {
                platform.GetComponent<Renderer>().material.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 128);
            }
            if (platformMode == 4)
            {
                UpdateClipColliders(false);
            }
            if (platformMode == 5)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 29;
                if (glass == null)
                {
                    glass = new Material(Shader.Find("GUI/Text Shader"));
                    glass.color = new Color32(145, 187, 255, 100);
                }
                platform.GetComponent<Renderer>().material = glass;
            }
            if (platformMode == 6)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 32;
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 7)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 204;
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 8)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 231;
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 9)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 240;
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 10)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 249;
                platform.GetComponent<Renderer>().enabled = false;
            }
            if (platformMode == 11)
            {
                platform.AddComponent<GorillaSurfaceOverride>().overrideIndex = 252;
                platform.GetComponent<Renderer>().enabled = false;
            }

            FixStickyColliders(platform);

            if (GetIndex("Platform Outlines").enabled)
            {
                GameObject gameObject = null;
                if (platformShape == 2)
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                }
                if (platformShape == 1)
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                if (platformShape == 0)
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
                if (gameObject == null)
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                gameObject.transform.parent = platform.transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.95f, 1.05f, 1.05f);
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = buttonDefaultA;
                array[0].time = 0f;
                array[1].color = buttonDefaultB;
                array[1].time = 0.5f;
                array[2].color = buttonDefaultA;
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();
            }
            return platform;
        }

        public static GameObject leftplat = null;
        public static GameObject rightplat = null;
        public static void Platforms()
        {
            if (leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = CreatePlatform();
                    var leftHandTransform = TrueLeftHand();
                    leftplat.transform.position = leftHandTransform.position;
                    leftplat.transform.rotation = leftHandTransform.rotation;
                    if (GetIndex("Stick Long Arms").enabled)
                    {
                        leftplat.transform.position += GorillaTagger.Instance.leftHandTransform.forward * (armlength - 0.917f);
                    }
                    if (GetIndex("Multiplied Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        MultipliedLongArms();
                        leftplat.transform.position = TrueLeftHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                    if (GetIndex("Vertical Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        VerticalLongArms();
                        leftplat.transform.position = TrueLeftHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                    if (GetIndex("Horizontal Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        HorizontalLongArms();
                        leftplat.transform.position = TrueLeftHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                }
                else
                {
                    if (platformMode != 5)
                    {
                        leftplat.GetComponent<Renderer>().material.color = GetBGColor(0f);
                    }
                    if (platformMode == 2)
                    {
                        float h = (Time.frameCount / 180f) % 1f;
                        leftplat.GetComponent<Renderer>().material.color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    }
                    if (platformMode == 3)
                    {
                        leftplat.GetComponent<Renderer>().material.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 128);
                    }
                }
            }
            else
            {
                if (leftplat != null)
                {
                    if (GetIndex("Platform Gravity").enabled)
                    {
                        leftplat.AddComponent(typeof(Rigidbody));
                        UnityEngine.Object.Destroy(leftplat.GetComponent<Collider>());
                        UnityEngine.Object.Destroy(leftplat, 2f);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(leftplat);
                    }
                    leftplat = null;
                    if (platformMode == 4 && rightplat == null)
                    {
                        UpdateClipColliders(true);
                    }
                }
            }

            if (rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = CreatePlatform();
                    var rightHandTransform = TrueRightHand();
                    rightplat.transform.position = rightHandTransform.position;
                    rightplat.transform.rotation = rightHandTransform.rotation;
                    if (GetIndex("Stick Long Arms").enabled)
                    {
                        rightplat.transform.position += GorillaTagger.Instance.rightHandTransform.forward * (armlength - 0.917f);
                    }
                    if (GetIndex("Multiplied Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        MultipliedLongArms();
                        rightplat.transform.position = TrueRightHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                    if (GetIndex("Vertical Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        VerticalLongArms();
                        rightplat.transform.position = TrueRightHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                    if (GetIndex("Horizontal Long Arms").enabled)
                    {
                        Vector3 legacyPosL = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position;
                        Vector3 legacyPosR = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position;
                        HorizontalLongArms();
                        rightplat.transform.position = TrueRightHand().position;
                        GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = legacyPosL;
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = legacyPosR;
                    }
                }
                else
                {
                    if (platformMode != 5)
                    {
                        rightplat.GetComponent<Renderer>().material.color = GetBGColor(0f);
                    }
                    if (platformMode == 2)
                    {
                        float h = (Time.frameCount / 180f) % 1f;
                        rightplat.GetComponent<Renderer>().material.color = UnityEngine.Color.HSVToRGB(h, 1f, 1f);
                    }
                    if (platformMode == 3)
                    {
                        rightplat.GetComponent<Renderer>().material.color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 128);
                    }
                }
            }
            else
            {
                if (rightplat != null)
                {
                    if (GetIndex("Platform Gravity").enabled)
                    {
                        rightplat.AddComponent(typeof(Rigidbody));
                        UnityEngine.Object.Destroy(rightplat.GetComponent<Collider>());
                        UnityEngine.Object.Destroy(rightplat, 2f);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(rightplat);
                    }
                    rightplat = null;
                    if (platformMode == 4 && leftplat == null)
                    {
                        UpdateClipColliders(true);
                    }
                }
            }
        }

        public static void TriggerPlatforms()
        {
            bool lt = leftGrab;
            bool rt = rightGrab;
            leftGrab = leftTrigger > 0.5f;
            rightGrab = rightTrigger > 0.5f;
            Platforms();
            leftGrab = lt;
            rightGrab = rt;
        }

        public static void Frozone()
        {
            if (leftGrab)
            {
                GameObject lol = GameObject.CreatePrimitive(PrimitiveType.Cube);
                lol.GetComponent<Renderer>().material.color = GetBGColor(0f);
                lol.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                lol.transform.localPosition = TrueLeftHand().position + (TrueLeftHand().right * 0.05f);
                lol.transform.rotation = TrueLeftHand().rotation;

                lol.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                UnityEngine.Object.Destroy(lol, 1);
            }

            if (rightGrab)
            {
                GameObject lol = GameObject.CreatePrimitive(PrimitiveType.Cube);
                lol.GetComponent<Renderer>().material.color = GetBGColor(0f);
                lol.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                lol.transform.localPosition = TrueRightHand().position + (TrueRightHand().right * -0.05f);
                lol.transform.rotation = TrueRightHand().rotation;

                lol.AddComponent<GorillaSurfaceOverride>().overrideIndex = 61;
                UnityEngine.Object.Destroy(lol, 1);
            }

            GorillaTagger.Instance.bodyCollider.enabled = !(leftGrab || rightGrab);
        }

        public static void ChangeSpeedBoostAmount()
        {
            speedboostCycle++;
            if (speedboostCycle > 3)
            {
                speedboostCycle = 0;
            }

            float[] jspeedamounts = new float[] { 2f, 7.5f, 9f, 200f };
            jspeed = jspeedamounts[speedboostCycle];

            float[] jmultiamounts = new float[] { 0.5f, /*1.25f*/1.1f, 2f, 10f };
            jmulti = jmultiamounts[speedboostCycle];

            string[] speedNames = new string[] { "Slow", "Normal", "Fast", "Ultra Fast" };
            GetIndex("Change Speed Boost Amount").overlapText = "Change Speed Boost Amount <color=grey>[</color><color=green>" + speedNames[speedboostCycle] + "</color><color=grey>]</color>";
        }

        public static void PlatformSpam()
        {
            if (rightGrab)
            {
                GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(platform.GetComponent<BoxCollider>());
                platform.GetComponent<Renderer>().material.color = bgColorA;
                platform.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                platform.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                platform.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                platform.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                UnityEngine.Object.Destroy(platform, 1f);
                PhotonNetwork.RaiseEvent(69, new object[2] { platform.transform.position, platform.transform.rotation }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        public static void PlatformGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true))
                {
                    GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(platform.GetComponent<BoxCollider>());
                    platform.GetComponent<Renderer>().material.color = bgColorA;
                    platform.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    platform.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platform.transform.position = NewPointer.transform.position;
                    platform.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
                    UnityEngine.Object.Destroy(platform, 1f);
                    PhotonNetwork.RaiseEvent(69, new object[2] { platform.transform.position, platform.transform.rotation }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                }
            }
        }

        public static void ChangeFlySpeed()
        {
            flySpeedCycle++;
            if (flySpeedCycle > 4)
            {
                flySpeedCycle = 0;
            }

            float[] speedamounts = new float[] { 5f, 10f, 30f, 60f, 0.5f };
            flySpeed = speedamounts[flySpeedCycle];

            string[] speedNames = new string[] { "Slow", "Normal", "Fast", "Extra Fast", "Extra Slow" };
            GetIndex("Change Fly Speed").overlapText = "Change Fly Speed <color=grey>[</color><color=green>" + speedNames[flySpeedCycle] + "</color><color=grey>]</color>";
        }

        public static void ChangeArmLength()
        {
            longarmCycle++;
            if (longarmCycle > 4)
            {
                longarmCycle = 0;
            }

            float[] lengthAmounts = new float[] { 0.75f, 1.1f, 1.25f, 1.5f, 2f };
            armlength = lengthAmounts[longarmCycle];

            string[] lengthNames = new string[] { "Shorter", "Unnoticable", "Normal", "Long", "Extreme" };
            GetIndex("Change Arm Length").overlapText = "Change Arm Length <color=grey>[</color><color=green>" + lengthNames[longarmCycle] + "</color><color=grey>]</color>";
        }

        public static void Fly()
        {
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void TriggerFly()
        {
            if (rightTrigger > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void NoclipFly()
        {
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (noclip == false)
                {
                    noclip = true;
                    UpdateClipColliders(false);
                }
            } else
            {
                if (noclip == true)
                {
                    noclip = false;
                    UpdateClipColliders(true);
                }
            }
        }

        public static void JoystickFly()
        {
            Vector2 joy = leftJoystick;

            if (Mathf.Abs(joy.x) > 0.3 || Mathf.Abs(joy.y) > 0.3)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * (joy.y * flySpeed)) + (GorillaTagger.Instance.headCollider.transform.right * Time.deltaTime * (joy.x * flySpeed));
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void BarkFly()
        {
            ZeroGravity();

            var rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
            Vector2 xz = leftJoystick;
            float y = rightJoystick.y;

            Vector3 inputDirection = new Vector3(xz.x, y, xz.y);
            var playerForward = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.forward;
            playerForward.y = 0;
            var playerRight = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.right;
            playerRight.y = 0;

            var velocity = inputDirection.x * playerRight + y * Vector3.up + inputDirection.z * playerForward;
            velocity *= GorillaLocomotion.GTPlayer.Instance.scale * flySpeed;
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.12875f);
        }

        public static void VelocityBarkFly()
        {
            if ((Mathf.Abs(leftJoystick.x) > 0.3 || Mathf.Abs(leftJoystick.y) > 0.3) || (Mathf.Abs(rightJoystick.x) > 0.3 || Mathf.Abs(rightJoystick.y) > 0.3))
            {
                BarkFly();
            }
        }

        public static void HandFly()
        {
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += TrueRightHand().forward * Time.deltaTime * flySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void SlingshotFly()
        {
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * (flySpeed * 2);
            }
        }

        public static void ZeroGravitySlingshotFly()
        {
            if (rightPrimary)
            {
                ZeroGravity();
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
            }
        }

        public static void WASDFly()
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0.067f, 0f);

            bool W = UnityInput.Current.GetKey(KeyCode.W);
            bool A = UnityInput.Current.GetKey(KeyCode.A);
            bool S = UnityInput.Current.GetKey(KeyCode.S);
            bool D = UnityInput.Current.GetKey(KeyCode.D);
            bool Space = UnityInput.Current.GetKey(KeyCode.Space);
            bool Ctrl = UnityInput.Current.GetKey(KeyCode.LeftControl);

            if (Mouse.current.rightButton.isPressed)
            {
                Transform parentTransform = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent;
                Quaternion currentRotation = parentTransform.rotation;
                Vector3 euler = currentRotation.eulerAngles;

                if (startX < 0)
                {
                    startX = euler.y;
                    subThingy = Mouse.current.position.value.x / UnityEngine.Screen.width;
                }
                if (startY < 0)
                {
                    startY = euler.x;
                    subThingyZ = Mouse.current.position.value.y / UnityEngine.Screen.height;
                }

                float newX = startY - ((((Mouse.current.position.value.y / UnityEngine.Screen.height) - subThingyZ) * 360) * 1.33f);
                float newY = startX + ((((Mouse.current.position.value.x / UnityEngine.Screen.width) - subThingy) * 360) * 1.33f);

                newX = (newX > 180f) ? newX - 360f : newX;
                newX = Mathf.Clamp(newX, -90f, 90f);

                parentTransform.rotation = Quaternion.Euler(newX, newY, euler.z);
            }
            else
            {
                startX = -1;
                startY = -1;
            }

            float speed = flySpeed;
            if (UnityInput.Current.GetKey(KeyCode.LeftShift))
                speed *= 2f;
            if (W)
            {
                GorillaTagger.Instance.rigidbody.transform.position += GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.forward * Time.deltaTime * speed;
            }

            if (S)
            {
                GorillaTagger.Instance.rigidbody.transform.position += GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.forward * Time.deltaTime * -speed;
            }

            if (A)
            {
                GorillaTagger.Instance.rigidbody.transform.position += GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.right * Time.deltaTime * -speed;
            }

            if (D)
            {
                GorillaTagger.Instance.rigidbody.transform.position += GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.right * Time.deltaTime * speed;
            }

            if (Space)
            {
                GorillaTagger.Instance.rigidbody.transform.position += new Vector3(0f, Time.deltaTime * speed, 0f);
            }

            if (Ctrl)
            {
                GorillaTagger.Instance.rigidbody.transform.position += new Vector3(0f, Time.deltaTime * -speed, 0f);
            }
        }

        private static float driveSpeed = 0f;
        public static int driveInt = 0;
        public static void ChangeDriveSpeed()
        {
            speedboostCycle++;
            if (speedboostCycle > 3)
            {
                speedboostCycle = 0;
            }

            float[] speedamounts = new float[] { 10f, 30f, 50f, 3f };
            driveSpeed = speedamounts[speedboostCycle];

            string[] speedNames = new string[] { "Normal", "Fast", "Ultra Fast", "Slow" };
            GetIndex("cdSpeed").overlapText = "Change Drive Speed <color=grey>[</color><color=green>" + speedNames[speedboostCycle] + "</color><color=grey>]</color>";
        }

        public static Vector2 lerpygerpy = Vector2.zero;
        public static void Drive()
        {
            Vector2 joy = leftJoystick;
            lerpygerpy = Vector2.Lerp(lerpygerpy, joy, 0.05f);

            Vector3 addition = GorillaTagger.Instance.bodyCollider.transform.forward * lerpygerpy.y + GorillaTagger.Instance.bodyCollider.transform.right * lerpygerpy.x;// + new Vector3(0f, -1f, 0f);
            Physics.Raycast(GorillaTagger.Instance.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, out var Ray, 512f, NoInvisLayerMask());

            if (Ray.distance < 0.2f && (Mathf.Abs(lerpygerpy.x) > 0.05f || Mathf.Abs(lerpygerpy.y) > 0.05f))
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = addition * 10f;
            }
        }

        private static bool lastaomfg = false;
        public static void Dash()
        {
            if (rightPrimary && !lastaomfg)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * flySpeed;
            }
            lastaomfg = rightPrimary;
        }

        public static void IronMan()
        {
            if (leftPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(flySpeed * -GorillaTagger.Instance.leftHandTransform.right, ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
            }
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(flySpeed * GorillaTagger.Instance.rightHandTransform.right, ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
            }
        }

        private static float loaoalsode = 0f;
        private static BalloonHoldable GetTargetBalloon()
        {
            foreach (BalloonHoldable balloo in GetBalloons())
            {
                if (balloo.IsMyItem())
                {
                    return balloo;
                }
            }
            if (Time.time > loaoalsode)
            {
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must equip a balloon.");
                loaoalsode = Time.time + 1f;
            }
            return null;
        }

        public static Vector3 rightgrapplePoint;
        public static Vector3 leftgrapplePoint;
        public static SpringJoint rightjoint;
        public static SpringJoint leftjoint;
        public static bool isLeftGrappling = false;
        public static bool isRightGrappling = false;
        public static void SpiderMan()
        {
            if (leftGrab)
            {
                if (!isLeftGrappling)
                {
                    isLeftGrappling = true;
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.leftHandTransform.forward * 5f;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            true,
                            999999f
                        });
                    } else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, true, 999999f);
                    }
                    RPCProtection();
                    RaycastHit lefthit;
                    if (Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out lefthit, 512f, NoInvisLayerMask()))
                    {
                        leftgrapplePoint = lefthit.point;

                        leftjoint = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        leftjoint.autoConfigureConnectedAnchor = false;
                        leftjoint.connectedAnchor = leftgrapplePoint;

                        float leftdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, leftgrapplePoint);

                        leftjoint.maxDistance = leftdistanceFromPoint * 0.8f;
                        leftjoint.minDistance = leftdistanceFromPoint * 0.25f;

                        leftjoint.spring = 10f;
                        leftjoint.damper = 50f;
                        leftjoint.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, leftgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out var Ray, 512f, NoInvisLayerMask());
                GameObject NewPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                NewPointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                NewPointer.GetComponent<Renderer>().material.color = buttonDefaultA - new Color32(0, 0, 0, 128);
                NewPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                NewPointer.transform.position = Ray.point;
                UnityEngine.Object.Destroy(NewPointer.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Collider>());
                UnityEngine.Object.Destroy(NewPointer, Time.deltaTime);

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                liner.material.shader = Shader.Find("GUI/Text Shader");
                liner.startColor = GetBGColor(0f) - new Color32(0, 0, 0, 128);
                liner.endColor = GetBGColor(0.5f) - new Color32(0, 0, 0, 128);
                liner.startWidth = 0.025f;
                liner.endWidth = 0.025f;
                liner.positionCount = 2;
                liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, Ray.point);
                UnityEngine.Object.Destroy(line, Time.deltaTime);

                isLeftGrappling = false;
                UnityEngine.Object.Destroy(leftjoint);
            }

            if (rightGrab)
            {
                if (!isRightGrappling)
                {
                    isRightGrappling = true;
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.rightHandTransform.forward * 5f;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            false,
                            999999f
                        });
                        RPCProtection();
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, false, 999999f);
                    }
                    RaycastHit righthit;
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out righthit, 512f, NoInvisLayerMask()))
                    {
                        rightgrapplePoint = righthit.point;

                        rightjoint = GorillaTagger.Instance.gameObject.AddComponent<SpringJoint>();
                        rightjoint.autoConfigureConnectedAnchor = false;
                        rightjoint.connectedAnchor = rightgrapplePoint;

                        float rightdistanceFromPoint = Vector3.Distance(GorillaTagger.Instance.bodyCollider.attachedRigidbody.position, rightgrapplePoint);

                        rightjoint.maxDistance = rightdistanceFromPoint * 0.8f;
                        rightjoint.minDistance = rightdistanceFromPoint * 0.25f;

                        rightjoint.spring = 10f;
                        rightjoint.damper = 50f;
                        rightjoint.massScale = 12f;
                    }
                }

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, rightgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out var Ray, 512f, NoInvisLayerMask());
                GameObject NewPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                NewPointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                NewPointer.GetComponent<Renderer>().material.color = buttonDefaultA - new Color32(0, 0, 0, 128);
                NewPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                NewPointer.transform.position = Ray.point;
                UnityEngine.Object.Destroy(NewPointer.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Collider>());
                UnityEngine.Object.Destroy(NewPointer, Time.deltaTime);

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                liner.material.shader = Shader.Find("GUI/Text Shader");
                liner.startColor = GetBGColor(0f) - new Color32(0, 0, 0, 128);
                liner.endColor = GetBGColor(0.5f) - new Color32(0, 0, 0, 128);
                liner.startWidth = 0.025f;
                liner.endWidth = 0.025f;
                liner.positionCount = 2;
                liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, Ray.point);
                UnityEngine.Object.Destroy(line, Time.deltaTime);

                isRightGrappling = false;
                UnityEngine.Object.Destroy(rightjoint);
            }
        }

        public static void GrapplingHooks()
        {
            if (leftGrab)
            {
                if (!isLeftGrappling)
                {
                    isLeftGrappling = true;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            true,
                            999999f
                        });
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, true, 999999f);
                    }
                    RPCProtection();
                    RaycastHit lefthit;
                    if (Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out lefthit, 512f, NoInvisLayerMask()))
                    {
                        leftgrapplePoint = lefthit.point;
                    }
                }

                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(leftgrapplePoint - GorillaTagger.Instance.leftHandTransform.position) * 0.5f;

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, leftgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.forward, out var Ray, 512f, NoInvisLayerMask());
                GameObject NewPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                NewPointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                NewPointer.GetComponent<Renderer>().material.color = buttonDefaultA - new Color32(0, 0, 0, 128);
                NewPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                NewPointer.transform.position = Ray.point;
                UnityEngine.Object.Destroy(NewPointer.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Collider>());
                UnityEngine.Object.Destroy(NewPointer, Time.deltaTime);

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                liner.material.shader = Shader.Find("GUI/Text Shader");
                liner.startColor = GetBGColor(0f) - new Color32(0, 0, 0, 128);
                liner.endColor = GetBGColor(0.5f) - new Color32(0, 0, 0, 128);
                liner.startWidth = 0.025f;
                liner.endWidth = 0.025f;
                liner.positionCount = 2;
                liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.leftHandTransform.position);
                liner.SetPosition(1, Ray.point);
                UnityEngine.Object.Destroy(line, Time.deltaTime);

                isLeftGrappling = false;
                UnityEngine.Object.Destroy(leftjoint);
            }

            if (rightGrab)
            {
                if (!isRightGrappling)
                {
                    isRightGrappling = true;
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            89,
                            false,
                            999999f
                        });
                        RPCProtection();
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(89, false, 999999f);
                    }
                    RaycastHit righthit;
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out righthit, 512f, NoInvisLayerMask()))
                    {
                        rightgrapplePoint = righthit.point;
                    }
                }

                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(rightgrapplePoint - GorillaTagger.Instance.rightHandTransform.position) * 0.5f;

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                UnityEngine.Color thecolor = Color.red;
                liner.startColor = thecolor; liner.endColor = thecolor; liner.startWidth = 0.025f; liner.endWidth = 0.025f; liner.positionCount = 2; liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, rightgrapplePoint);
                liner.material.shader = Shader.Find("GorillaTag/UberShader");
                UnityEngine.Object.Destroy(line, Time.deltaTime);
            }
            else
            {
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out var Ray, 512f, NoInvisLayerMask());
                GameObject NewPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                NewPointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                NewPointer.GetComponent<Renderer>().material.color = buttonDefaultA - new Color32(0, 0, 0, 128);
                NewPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                NewPointer.transform.position = Ray.point;
                UnityEngine.Object.Destroy(NewPointer.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(NewPointer.GetComponent<Collider>());
                UnityEngine.Object.Destroy(NewPointer, Time.deltaTime);

                GameObject line = new GameObject("Line");
                LineRenderer liner = line.AddComponent<LineRenderer>();
                liner.material.shader = Shader.Find("GUI/Text Shader");
                liner.startColor = GetBGColor(0f) - new Color32(0, 0, 0, 128);
                liner.endColor = GetBGColor(0.5f) - new Color32(0, 0, 0, 128);
                liner.startWidth = 0.025f;
                liner.endWidth = 0.025f;
                liner.positionCount = 2;
                liner.useWorldSpace = true;
                liner.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                liner.SetPosition(1, Ray.point);
                UnityEngine.Object.Destroy(line, Time.deltaTime);

                isRightGrappling = false;
                UnityEngine.Object.Destroy(rightjoint);
            }
        }

        public static void DisableSpiderMan()
        {
            isLeftGrappling = false;
            UnityEngine.Object.Destroy(leftjoint);
            isRightGrappling = false;
            UnityEngine.Object.Destroy(rightjoint);
        }

        public static void NetworkedGrappleMods()
        {
            if (GetIndex("Spider Man").enabled || GetIndex("Grappling Hooks").enabled)
            {
                if (isLeftGrappling || isRightGrappling)
                {
                    BalloonHoldable tb = GetTargetBalloon();
                    Traverse.Create(tb).Field("balloonState").SetValue(0);
                    Traverse.Create(tb).Field("maxDistanceFromOwner").SetValue(float.MaxValue);
                    tb.rigidbodyInstance.isKinematic = true;
                    tb.gameObject.GetComponent<BalloonDynamics>().stringLength = 512f;
                    tb.gameObject.GetComponent<BalloonDynamics>().stringStrength = 512f;
                    Traverse.Create(tb.gameObject.GetComponent<BalloonDynamics>()).Field("enableDynamics").SetValue(false);
                    if (tb != null)
                    {
                        if (isLeftGrappling)
                        {
                            if (!((LineRenderer)Traverse.Create(tb).Field("lineRenderer").GetValue()).enabled)
                            {
                                tb.currentState = TransferrableObject.PositionState.InLeftHand;
                            }
                            tb.transform.position = leftgrapplePoint;
                            tb.transform.LookAt(GorillaTagger.Instance.leftHandTransform.position);
                        }
                        else
                        {
                            if (isRightGrappling)
                            {
                                if (!((LineRenderer)Traverse.Create(tb).Field("lineRenderer").GetValue()).enabled)
                                {
                                    tb.currentState = TransferrableObject.PositionState.InRightHand;
                                }
                                tb.transform.position = rightgrapplePoint;
                                tb.transform.LookAt(GorillaTagger.Instance.rightHandTransform.position);
                                tb.transform.Rotate(Vector3.left, 90f, Space.Self);
                            }
                        }
                    }
                }
                else
                {
                    BalloonHoldable tb = GetTargetBalloon();
                    int balloonState = (int)Traverse.Create(tb).Field("balloonState").GetValue();
                    if (balloonState != 1 && balloonState != 2 && balloonState != 5 && balloonState != 6)
                    {
                        Traverse.Create(tb).Field("balloonState").SetValue(0);
                    }
                    tb.rigidbodyInstance.isKinematic = false;
                    tb.gameObject.GetComponent<BalloonDynamics>().stringLength = 0.5f;
                    tb.gameObject.GetComponent<BalloonDynamics>().stringStrength = 0.9f;
                    Traverse.Create(tb.gameObject.GetComponent<BalloonDynamics>()).Field("enableDynamics").SetValue(true);
                    if (tb != null)
                    {
                        tb.currentState = TransferrableObject.PositionState.Dropped;
                    }
                }
            }
        }

        private static bool lastWasGrab = false;
        private static bool lastWasRightGrab = false;
        public static void NetworkedPlatforms()
        {
            if (GetIndex("Platforms").enabled)
            {
                if (leftGrab && !lastWasGrab)
                {
                    CoroutineManager.RunCoroutine(CreateLeftPlatform());
                }
                if (rightGrab && !lastWasRightGrab)
                {
                    CoroutineManager.RunCoroutine(CreateRightPlatform());
                }
                lastWasGrab = leftGrab;
                lastWasRightGrab = rightGrab;
            }
        }

        public static IEnumerator CreateLeftPlatform()
        {
            yield return Fun.CreateGetPiece(1924370326, piece =>
            {
                BuilderTable.instance.RequestGrabPiece(piece, true, new Vector3(-0.03f, -0.03f, -0.27f), new Quaternion(-0.6f, 0.5f, -0.4f, 0.5f));
            });
        }

        public static IEnumerator CreateRightPlatform()
        {
            yield return Fun.CreateGetPiece(1924370326, piece =>
            {
                BuilderTable.instance.RequestGrabPiece(piece, false, new Vector3(0.03f, -0.03f, -0.27f), new Quaternion(0.5f, -0.6f, -0.5f, 0.4f));
            });
        }

        public static void UpAndDown()
        {
            if ((rightTrigger > 0.5f) || rightGrab)
            {
                ZeroGravity();
            }
            if (rightTrigger > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.up * Time.deltaTime * flySpeed * 3f;
            }

            if (rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.up * Time.deltaTime * flySpeed * -3f;
            }
        }

        public static void LeftAndRight()
        {
            if ((rightTrigger > 0.5f) || rightGrab)
            {
                ZeroGravity();
            }
            if (rightTrigger > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.right * Time.deltaTime * flySpeed * -3f;
            }

            if (rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.right * Time.deltaTime * flySpeed * 3f;
            }
        }

        public static void ForwardsAndBackwards()
        {
            if ((rightTrigger > 0.5f) || rightGrab)
            {
                ZeroGravity();
            }
            if (rightTrigger > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.forward * Time.deltaTime * flySpeed * 3f;
            }

            if (rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.forward * Time.deltaTime * flySpeed * -3f;
            }
        }

        public static void AutoWalk()
        {
            Vector2 joy = leftJoystick;

            float armLength = 0.45f;
            float animSpeed = 9f;

            if (leftJoystickClick)
            {
                animSpeed *= 1.5f;
            }

            if (Mathf.Abs(joy.y) > 0.05f || Mathf.Abs(joy.x) > 0.05f)
            {
                GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.forward * (Mathf.Sin(Time.time * animSpeed) * (joy.y * armLength)) + GorillaTagger.Instance.bodyCollider.transform.right * ((Mathf.Sin(Time.time * animSpeed) * (joy.x * armLength)) - 0.2f) + new Vector3(0f, -0.3f + (Mathf.Cos(Time.time * animSpeed) * 0.2f), 0f);
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.forward * (-Mathf.Sin(Time.time * animSpeed) * (joy.y * armLength)) + GorillaTagger.Instance.bodyCollider.transform.right * ((-Mathf.Sin(Time.time * animSpeed) * (joy.x * armLength)) + 0.2f) + new Vector3(0f, -0.3f + (Mathf.Cos(Time.time * animSpeed) * -0.2f), 0f);
            }
            /*if (rightPrimary) this shit LACED
            {
                GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.2f + new Vector3(0f, -1f, 0f) + -GorillaTagger.Instance.bodyCollider.transform.forward;
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.2f + new Vector3(0f, -1f, 0f) + -GorillaTagger.Instance.bodyCollider.transform.forward;
            }*/
        }

        public static void AutoFunnyRun()
        {
            if (rightGrab)
            {
                if (bothHands)
                {
                    float time = Time.frameCount;
                    GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * MathF.Cos(time) / 10) + new Vector3(0, -0.5f - (MathF.Sin(time) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * -0.05f);
                    GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * MathF.Cos(time + 180) / 10) + new Vector3(0, -0.5f - (MathF.Sin(time + 180) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * 0.05f);
                }
                else
                {
                    float time = Time.frameCount;
                    GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * MathF.Cos(time) / 10) + new Vector3(0, -0.5f - (MathF.Sin(time) / 7), 0);
                }
            }
        }

        public static void AutoPinchClimb()
        {
            if (rightGrab)
            {
                float time = Time.frameCount / 3f;
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.right * (0.4f + (MathF.Cos(time) * 0.4f))) + (GorillaTagger.Instance.headCollider.transform.up * (MathF.Sin(time) * 0.6f)) + (GorillaTagger.Instance.headCollider.transform.forward * 0.75f);
                GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.right * -(0.4f + (MathF.Cos(time) * 0.4f))) + (GorillaTagger.Instance.headCollider.transform.up * (MathF.Sin(time) * 0.6f)) + (GorillaTagger.Instance.headCollider.transform.forward * 0.75f);
            }
        }

        public static void AutoElevatorClimb()
        {
            if (rightGrab)
            {
                float time = Time.frameCount / 3f;
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.right * (0.4f + (MathF.Cos(time) * 0.4f))) + (GorillaTagger.Instance.headCollider.transform.up * (MathF.Sin(time) * 0.6f)) + (GorillaTagger.Instance.headCollider.transform.forward * 0.75f);
            }
        }

        private static List<Vector3> posArchive = null;
        public static Vector3[] GetAllTreeBranchPositions()
        {
            if (posArchive != null)
                return posArchive.ToArray();

            posArchive = new List<Vector3> { };

            Vector3[] TreeBranchOffsets = new Vector3[]
            {
                new Vector3(-2.383f, 3.784f, 0.738f),
                new Vector3(1.55f, 5.559f, -1.56f),
                new Vector3(-2.225f, 7.214f, 0.063f),
                new Vector3(1.365f, 6.62f, 0.82f),
                new Vector3(0.405f, 8.865f, -2.759f),
                new Vector3(-2.227f, 9.763f, 2.071f),
                new Vector3(2.421f, 10.91f, 1.313f),
                new Vector3(1.618f, 13.169f, -1.216f),
                new Vector3(2.175f, 12.959f, -0.229f),
                new Vector3(1.855f, 13.837f, 1.215f),
                new Vector3(-0.265f, 14.953f, 2.935f),
                new Vector3(-2.049f, 14.962f, -1.708f),
                new Vector3(-1.249f, 18.93f, -1.62f),
            };

            string[] SmallTreeTargets = new string[] {
                "Environment Objects/LocalObjects_Prefab/Forest/Terrain/SmallTrees/Group1",
                "Environment Objects/LocalObjects_Prefab/Forest/Terrain/SmallTrees/Group2"
            };

            foreach (string SmallTreeTarget in SmallTreeTargets)
            {
                GameObject TreeGroupGO = GameObject.Find(SmallTreeTarget);

                for (int i = 0; i < TreeGroupGO.transform.childCount; i++)
                {
                    GameObject v = TreeGroupGO.transform.GetChild(i).gameObject;

                    Vector3 oldlocalscale = v.transform.localScale;
                    v.transform.localScale *= 5;

                    foreach (Vector3 TreeBranchOffset in TreeBranchOffsets)
                        posArchive.Add(v.transform.TransformPoint(TreeBranchOffset));
                    
                    v.transform.localScale = oldlocalscale;
                }
            }

            return posArchive.ToArray();
        }

        public static Vector3 leftPos = Vector3.zero;
        public static Vector3 rightPos = Vector3.zero;
        public static void AutoBranch()
        {
            if (rightGrab)
            {
                float dist = float.MaxValue;
                Vector3 closeDist = Vector3.zero;
                Vector3 compareDist = GorillaTagger.Instance.bodyCollider.transform.position;

                foreach (Vector3 treeBranchPos in GetAllTreeBranchPositions())
                {
                    float foundDist = Vector3.Distance(compareDist, treeBranchPos);
                    if (foundDist < dist)
                    {
                        dist = foundDist;
                        closeDist = treeBranchPos;
                    }
                }

                if (dist < 3f)
                    rightPos = Vector3.Lerp(rightPos, closeDist, 0.2f);
                else
                    rightPos = Vector3.Lerp(rightPos, GorillaTagger.Instance.rightHandTransform.position, 0.2f);
                
                GorillaTagger.Instance.rightHandTransform.position = rightPos;

                Vector3 lastFoundDist = closeDist;
                dist = float.MaxValue;
                closeDist = Vector3.zero;
                compareDist = GorillaTagger.Instance.bodyCollider.transform.position;

                foreach (Vector3 treeBranchPos in GetAllTreeBranchPositions())
                {
                    float foundDist = Vector3.Distance(compareDist, treeBranchPos);
                    if (foundDist < dist && treeBranchPos != lastFoundDist)
                    {
                        dist = foundDist;
                        closeDist = treeBranchPos;
                    }
                }

                if (dist < 3f)
                    leftPos = Vector3.Lerp(leftPos, closeDist, 0.2f);
                else
                    leftPos = Vector3.Lerp(leftPos, GorillaTagger.Instance.leftHandTransform.position, 0.2f);

                GorillaTagger.Instance.leftHandTransform.position = leftPos;
            } else
            {
                leftPos = GorillaTagger.Instance.bodyCollider.transform.position;
                rightPos = GorillaTagger.Instance.bodyCollider.transform.position;
            }
        }

        public static void ForceTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = true;
        }

        public static void NoTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = false;
        }

        public static void LowGravity()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.unscaledDeltaTime * (6.66f / Time.unscaledDeltaTime)), ForceMode.Acceleration);
        }

        public static void ZeroGravity()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.unscaledDeltaTime * (9.81f / Time.unscaledDeltaTime)), ForceMode.Acceleration);
        }

        public static void HighGravity()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * (Time.unscaledDeltaTime * (7.77f / Time.unscaledDeltaTime)), ForceMode.Acceleration);
        }

        public static void ReverseGravity()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (19.62f / Time.deltaTime)), ForceMode.Acceleration);
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.rotation = Quaternion.Euler(180f, 0f, 0f);
        }

        public static void UnflipCharacter()
        {
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.rotation = Quaternion.identity;
        }

        private static List<object[]> playerPositions = new List<object[]> { };
        public static void Rewind()
        {
            if (rightTrigger > 0.5f)
            {
                if (playerPositions.Count > 0)
                {
                    object[] targetPos = playerPositions[playerPositions.Count - 1];

                    TeleportPlayer((Vector3)targetPos[0]);

                    GorillaTagger.Instance.leftHandTransform.position = (Vector3)targetPos[1];
                    GorillaTagger.Instance.leftHandTransform.rotation = (Quaternion)targetPos[2];

                    GorillaTagger.Instance.rightHandTransform.position = (Vector3)targetPos[3];
                    GorillaTagger.Instance.rightHandTransform.rotation = (Quaternion)targetPos[4];

                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity = (Vector3)targetPos[5] * -1f;

                    playerPositions.RemoveAt(playerPositions.Count - 1);
                }
            } else
            {
                playerPositions.Add(new object[] {
                    GorillaTagger.Instance.bodyCollider.transform.position,

                    GorillaTagger.Instance.leftHandTransform.position,
                    GorillaTagger.Instance.leftHandTransform.rotation,

                    GorillaTagger.Instance.rightHandTransform.position,
                    GorillaTagger.Instance.rightHandTransform.rotation,

                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity
                });

                if (playerPositions.Count > 8640)
                    playerPositions.RemoveAt(0);
            }
        }

        public static void ClearRewind()
        {
            playerPositions.Clear();
        }

        private static Vector3 walkPos;
        private static Vector3 walkNormal;

        public static void WallWalk()
        {
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) || GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (walkPos != Vector3.zero && rightGrab)
            {
                //GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -10, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -9.81f, ForceMode.Acceleration);
                ZeroGravity();
            }
        }

        public static void WeakWallWalk()
        {
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) || GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (walkPos != Vector3.zero && rightGrab)
            {
                //GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -10, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -5f, ForceMode.Acceleration);
                ZeroGravity();
            }
        }

        public static void StrongWallWalk()
        {
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) || GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (walkPos != Vector3.zero && rightGrab)
            {
                //GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -10, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -50f, ForceMode.Acceleration);
                ZeroGravity();
            }
        }

        // Credits to Intelligence for the idea (You can't code a mod menu but you are insanely good at making competitive cheats)
        public static void LegitimateWallWalk()
        {
            float range = 0.2f;
            float power = -3f;

            if (leftGrab)
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);

                if (Physics.Raycast(TrueLeftHand().position, -ray.normal, out var Ray, range, GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers))
                {
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Ray.normal * power, ForceMode.Acceleration);
                }
            }

            if (rightGrab)
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);

                if (Physics.Raycast(TrueRightHand().position, -ray.normal, out var Ray, range, GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers))
                {
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Ray.normal * power, ForceMode.Acceleration);
                }
            }
        }

        public static void SpiderWalk()
        {
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) || GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.GTPlayer).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.GTPlayer.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (walkPos != Vector3.zero)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -9.81f, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.rotation = Quaternion.Lerp(GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.parent.rotation, Quaternion.LookRotation(walkNormal) * Quaternion.Euler(90f, 0f, 0f), Time.deltaTime);
                ZeroGravity();
            }
        }

        public static void TeleportToRandom()
        {
            closePosition = Vector3.zero;
            TeleportPlayer(GetRandomVRRig(false).transform.position);
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private static int rememberPageNumber = 0;
        public static void EnterTeleportToPlayer()
        {
            rememberPageNumber = pageNumber;
            buttonsType = 29;
            pageNumber = 0;
            List<ButtonInfo> tpbuttons = new List<ButtonInfo> { new ButtonInfo { buttonText = "Exit Teleport to Player", method = () => ExitTeleportToPlayer(), isTogglable = false, toolTip = "Returns you back to the movement mods." } };
            foreach (Player plr in PhotonNetwork.PlayerListOthers)
            {
                string clrtxt = "#ffffff";
                try
                {
                    clrtxt = ColorToHex(GetVRRigFromPlayer(plr).playerColor);
                }
                catch { }
                tpbuttons.Add(new ButtonInfo { buttonText = "TeleportPlayer"+tpbuttons.Count.ToString(), overlapText = "<color="+clrtxt+">"+ToTitleCase(plr.NickName)+"</color>", method = () => TeleportToPlayer(plr), isTogglable = false, toolTip = "Teleports you to " + ToTitleCase(plr.NickName) + "." });
            }
            Buttons.buttons[29] = tpbuttons.ToArray();
        }

        public static void TeleportToPlayer(Player plr)
        {
            TeleportPlayer(GetVRRigFromPlayer(plr).headMesh.transform.position);
        }

        public static void ExitTeleportToPlayer()
        {
            Settings.EnableMovement();
            pageNumber = rememberPageNumber;
        }

        public static void EnterTeleportToMap() // Credits to Malachi for the positions
        {
            rememberPageNumber = pageNumber;
            buttonsType = 29;
            pageNumber = 0;
            List<ButtonInfo> tpbuttons = new List<ButtonInfo> { new ButtonInfo { buttonText = "Exit Teleport to Map", method = () => ExitTeleportToPlayer(), isTogglable = false, toolTip = "Returns you back to the movement mods." } };
            string[][] mapData = new string[][]
            {
                new string[] // Forest
                {
                    "Forest",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/TreeRoomSpawnForestZone",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Forest, Tree Exit"
                },
                new string[] // City
                {
                    "City",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/ForestToCity",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - City Front"
                },
                new string[] // Canyons
                {
                    "Canyons",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/ForestCanyonTransition",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Canyon"
                },
                new string[] // Clouds
                {
                    "Clouds",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/CityToSkyJungle",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Clouds From Computer"
                },
                new string[] // Caves
                {
                    "Caves",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/ForestToCave",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Cave"
                },
                new string[] // Beach
                {
                    "Beach",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/BeachToForest",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Beach for Computer"
                },
                new string[] // Mountains
                {
                    "Mountains",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/CityToMountain",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Mountain"
                },
                new string[] // Basement
                {
                    "Basement",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/CityToBasement",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Basement For Computer"
                },
                new string[] // Metropolis
                {
                    "Metropolis",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/MetropolisOnly",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Metropolis from Computer"
                },
                new string[] // Arcade
                {
                    "Arcade",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/CityToArcade",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - City frm Arcade"
                },
                new string[] // Rotating
                {
                    "Rotating",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/CityToRotating",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Rotating Map"
                },
                new string[] // Bayou
                {
                    "Bayou",
                    "Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Regional Transition/BayouOnly",
                    "Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - BayouComputer2"
                },
                new string[] // Virtual Stump
                {
                    "Virtual Stump",
                    "VSTUMP",
                    "VSTUMP"
                },
            };
            foreach (string[] Data in mapData)
            {
                tpbuttons.Add(new ButtonInfo { buttonText = "TeleportMap" + tpbuttons.Count.ToString(), overlapText = Data[0], method = () => TeleportToMap(Data[1], Data[2]), isTogglable = false, toolTip = "Teleports you to the " + Data[0] + " map." });
            }
            Buttons.buttons[29] = tpbuttons.ToArray();
        }

        public static void TeleportToMap(string zone, string pos)
        {
            if (zone == "VSTUMP")
            {
                ModIOLoginTeleporter tele = GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab/Arcade_prefab/MainRoom/VRArea/ModIOArcadeTeleporter/TeleportTriggers_1/VRHeadsetTrigger_1").GetComponent<ModIOLoginTeleporter>();

                tele.gameObject.transform.parent.parent.parent.parent.parent.parent.gameObject.SetActive(true); // wtf
                tele.gameObject.transform.parent.parent.parent.parent.gameObject.SetActive(true);

                tele.LoginAndTeleport();
            } else
            {
                GameObject.Find(zone).GetComponent<GorillaSetZoneTrigger>().OnBoxTriggered();
                TeleportPlayer(GameObject.Find(pos).transform.position);
            }
        }

        public static void TeleportGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true) && Time.time > teleDebounce)
                {
                    closePosition = Vector3.zero;
                    TeleportPlayer(NewPointer.transform.position + new Vector3(0f, 1f, 0f));
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    teleDebounce = Time.time + 0.5f;
                }
            }
        }

        public static void Airstrike()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true) && Time.time > teleDebounce)
                {
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, -20f, 0f);
                    TeleportPlayer(NewPointer.transform.position + new Vector3(0f, 30f, 0f));
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, -20f, 0f);
                    teleDebounce = Time.time + 0.5f;
                }
            }
        }

        public static GameObject CheckPoint = null;
        public static void Checkpoint()
        {
            if (rightGrab)
            {
                if (CheckPoint == null)
                {
                    CheckPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(CheckPoint.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(CheckPoint.GetComponent<SphereCollider>());
                    CheckPoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
                CheckPoint.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            if (CheckPoint != null)
            {
                if (rightPrimary)
                {
                    CheckPoint.GetComponent<Renderer>().material.color = bgColorA;
                    TeleportPlayer(CheckPoint.transform.position);
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    CheckPoint.GetComponent<Renderer>().material.color = buttonDefaultA;
                }
            }
        }

        public static void DisableCheckpoint()
        {
            if (CheckPoint != null)
            {
                UnityEngine.Object.Destroy(CheckPoint);
                CheckPoint = null;
            }
        }

        public static int selectedCheckpoint = -1;
        public static float selectedCheckpointDelay;
        public static List<GameObject> checkpoints = new List<GameObject> { };
        public static void AdvancedCheckpoints()
        {
            if (rightGrab)
            {
                bool isNearCheckpoint = false;
                foreach (GameObject checkpoint in checkpoints)
                {
                    if (Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, checkpoint.transform.position) < 0.2f)
                    {
                        isNearCheckpoint = true;
                        checkpoint.transform.position = GorillaTagger.Instance.rightHandTransform.transform.position;
                        break;
                    }
                }

                if (!isNearCheckpoint)
                {
                    GameObject newCheckpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(newCheckpoint.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(newCheckpoint.GetComponent<SphereCollider>());
                    newCheckpoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    newCheckpoint.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    newCheckpoint.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    newCheckpoint.GetComponent<Renderer>().material.color = GetBGColor(0f);

                    GameObject MeshHolder = new GameObject("Label");
                    MeshHolder.transform.parent = newCheckpoint.transform;
                    MeshHolder.transform.localPosition = Vector3.zero;
                    TextMesh newMesh = MeshHolder.AddComponent<TextMesh>();

                    Renderer MeshRender = newMesh.GetComponent<Renderer>();
                    MeshRender.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    newMesh.fontSize = 12;
                    newMesh.fontStyle = activeFontStyle;
                    newMesh.characterSize = 0.1f;
                    newMesh.anchor = TextAnchor.MiddleCenter;
                    newMesh.alignment = TextAlignment.Center;
                    newMesh.color = Color.white;
                    newMesh.text = (checkpoints.Count + 1).ToString();

                    MeshRender.material.renderQueue = newCheckpoint.GetComponent<Renderer>().material.renderQueue + 2;
                    checkpoints.Add(newCheckpoint);
                }
            }

            if (rightTrigger > 0.5f)
            {
                foreach (GameObject checkpoint in checkpoints)
                {
                    if (Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, checkpoint.transform.position) < 0.2f)
                    {
                        checkpoints.Remove(checkpoint);
                        UnityEngine.Object.Destroy(checkpoint);
                    }
                }
            }

            if (rightPrimary)
            {
                TeleportPlayer(checkpoints[selectedCheckpoint].transform.position);
            }

            foreach (GameObject checkpoint in checkpoints)
            {
                checkpoint.GetComponent<Renderer>().material.color = GetBGColor(0f);

                GameObject textObject = checkpoint.transform.Find("Label").gameObject;
                textObject.transform.LookAt(Camera.main.transform.position);
                textObject.transform.Rotate(0f, 180f, 0f);
            }

            if (Mathf.Abs(rightJoystick.y) > 0.5f && Time.time > selectedCheckpointDelay)
            {
                selectedCheckpointDelay = Time.time + 0.2f;
                selectedCheckpoint += rightJoystick.y > 0 ? 1 : -1;

                if (selectedCheckpoint < 0)
                    selectedCheckpoint = checkpoints.Count - 1;
            }

            if (selectedCheckpoint < 0 && checkpoints.Count > 0)
                selectedCheckpoint = 0;

            if (selectedCheckpoint > checkpoints.Count - 1)
                selectedCheckpoint = 0;

            GameObject go = new GameObject("Lbl");
            if (GetIndex("Hidden Labels").enabled) { go.layer = 19; }
            go.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            TextMesh textMesh = go.AddComponent<TextMesh>();
            textMesh.color = GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity.magnitude >= GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed ? Color.green : Color.white;
            textMesh.fontSize = 24;
            textMesh.fontStyle = activeFontStyle;
            textMesh.characterSize = 0.1f;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.text = (selectedCheckpoint + 1).ToString();

            go.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0f, 0.1f, 0f);
            go.transform.LookAt(Camera.main.transform.position);
            go.transform.Rotate(0f, 180f, 0f);
            UnityEngine.Object.Destroy(go, Time.deltaTime);
        }

        public static void DisableAdvancedCheckpoints()
        {
            selectedCheckpoint = 0;

            foreach (GameObject checkpoint in checkpoints)
                UnityEngine.Object.Destroy(checkpoint);
            
            checkpoints.Clear();
        }

        public static GameObject BombObject = null;
        public static void Bomb()
        {
            if (rightGrab)
            {
                if (BombObject == null)
                {
                    BombObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(BombObject.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(BombObject.GetComponent<SphereCollider>());
                    BombObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
                BombObject.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            if (BombObject != null)
            {
                if (rightPrimary)
                {
                    Vector3 dir = (GorillaTagger.Instance.bodyCollider.transform.position - BombObject.transform.position);
                    dir.Normalize();
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += 25f * dir;
                    UnityEngine.Object.Destroy(BombObject);
                    BombObject = null;
                }
                else
                {
                    BombObject.GetComponent<Renderer>().material.color = buttonDefaultA;
                }
            }
        }

        public static void DisableBomb()
        {
            if (BombObject != null)
            {
                UnityEngine.Object.Destroy(BombObject);
                BombObject = null;
            }
        }

        private static GameObject pearl = null;
        private static Texture2D pearltxt = null;
        private static Material pearlmat = null;
        private static bool isrighthandedpearl = false;
        public static void EnderPearl()
        {
            if (rightGrab || leftGrab)
            {
                if (pearl == null)
                {
                    pearl = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(pearl.GetComponent<Collider>());
                    pearl.transform.localScale = new Vector3(0.1f, 0.1f, 0.01f);
                    if (pearlmat == null)
                    {
                        pearlmat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

                        pearlmat.color = Color.white;
                        if (pearltxt == null)
                        {
                            pearltxt = LoadTextureFromResource("iiMenu.Resources.pearl.png");
                            pearltxt.filterMode = FilterMode.Point;
                            pearltxt.wrapMode = TextureWrapMode.Clamp;
                        }
                        pearlmat.mainTexture = pearltxt;

                        pearlmat.SetFloat("_Surface", 1);
                        pearlmat.SetFloat("_Blend", 0);
                        pearlmat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                        pearlmat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                        pearlmat.SetFloat("_ZWrite", 0);
                        pearlmat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        pearlmat.renderQueue = (int)RenderQueue.Transparent;

                        pearlmat.SetFloat("_Glossiness", 0f);
                        pearlmat.SetFloat("_Metallic", 0f);
                    }
                    pearl.GetComponent<Renderer>().material = pearlmat;
                }
                if (pearl.GetComponent<Rigidbody>() != null)
                {
                    UnityEngine.Object.Destroy(pearl.GetComponent<Rigidbody>());
                }
                isrighthandedpearl = rightGrab;
                pearl.transform.position = rightGrab ? GorillaTagger.Instance.rightHandTransform.position : GorillaTagger.Instance.leftHandTransform.position;
            } else
            {
                if (pearl != null)
                {
                    if (pearl.GetComponent<Rigidbody>() == null)
                    {
                        Rigidbody comp = pearl.AddComponent(typeof(Rigidbody)) as Rigidbody;
                        comp.velocity = isrighthandedpearl ? GorillaLocomotion.GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0) : GorillaLocomotion.GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                    }
                    Physics.Raycast(pearl.transform.position, pearl.GetComponent<Rigidbody>().velocity, out var Ray, 0.25f, GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers);
                    if (Ray.collider != null)
                    {
                        TeleportPlayer(pearl.transform.position);
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                                84,
                                true,
                                999999f
                            });
                        }
                        else
                        {
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(84, true, 999999f);
                        }
                        RPCProtection();
                        UnityEngine.Object.Destroy(pearl);
                    }
                }
            }
            if (pearl != null)
            {
                pearl.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                if (pearl.GetComponent<Rigidbody>() != null)
                {
                    pearl.GetComponent<Rigidbody>().AddForce(Vector3.up * (Time.deltaTime * (6.66f / Time.deltaTime)), ForceMode.Acceleration);
                }
            }
        }

        public static void DestroyEnderPearl()
        {
            if (pearl != null)
            {
                UnityEngine.Object.Destroy(pearl);
            }
        }

        public static void SpeedBoost()
        {
            float jspt = jspeed;
            float jmpt = jmulti;
            if (GetIndex("Factored Speed Boost").enabled)
            {
                jspt = (jspt / 6.5f) * GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed;
                jmpt = (jmpt / 1.1f) * GorillaLocomotion.GTPlayer.Instance.jumpMultiplier;
            }
            if (!GetIndex("Disable Max Speed Modification").enabled)
            {
                GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = jspeed;
            }
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = jmulti;
        }

        public static void GripSpeedBoost()
        {
            if (rightGrab)
            {
                SpeedBoost();
            }
        }

        public static void JoystickSpeedBoost()
        {
            if (rightJoystickClick)
            {
                SpeedBoost();
            }
        }

        /*
        public static void DisableSpeedBoost()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 6.5f;
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 1.1f;
        }*/

        public static void UncapMaxVelocity()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 99999f;
        }

        public static void AlwaysMaxVelocity()
        {
            if (GetIndex("Uncap Max Velocity").enabled)
            {
                Toggle("Uncap Max Velocity");
            }
            else
            {
                GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 99999f;
            }
        }

        public static void UpdateClipColliders(bool enabledd)
        {
            foreach (MeshCollider v in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                v.enabled = enabledd;
            }
        }

        public static void Noclip()
        {
            if (rightTrigger > 0.5f || UnityInput.Current.GetKey(KeyCode.E))
            {
                if (noclip == false)
                {
                    noclip = true;
                    UpdateClipColliders(false);
                }
            }
            else
            {
                if (noclip == true)
                {
                    noclip = false;
                    UpdateClipColliders(true);
                }
            }
        }

        private static bool wasDisabledAlready = false;
        private static bool invisMonke = false;
        public static void Invisible()
        {
            bool hit = rightSecondary || Mouse.current.rightButton.isPressed;
            if (invisType == 1)
            {
                invisMonke = hit;
            }
            if (invisType == 2)
            {
                invisMonke = !hit;
            }
            if (invisMonke)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(99999f, 99999f, 99999f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = new Vector3(99999f, 99999f, 99999f);
                }
                catch { }
            }
            if (hit == true && lastHit2 == false && invisType == 0)
            {
                invisMonke = !invisMonke;
            }
            lastHit2 = hit;
            if (invisMonke)
            {
                wasDisabledAlready = GorillaTagger.Instance.offlineVRRig.enabled;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = !wasDisabledAlready;
            }
        }

        private static bool ghostMonke = false;
        public static void Ghost()
        {
            bool hit = rightPrimary || Mouse.current.leftButton.isPressed;
            if (GetIndex("Non-Togglable Ghost").enabled)
            {
                ghostMonke = hit;
            }
            GorillaTagger.Instance.offlineVRRig.enabled = !ghostMonke;
            if (hit == true && lastHit == false)
            {
                ghostMonke = !ghostMonke;
            }
            lastHit = hit;
        }

        public static void EnableRig()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            ghostException = false;
        }

        public static void RigGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = NewPointer.transform.position + new Vector3(0, 1, 0);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = NewPointer.transform.position + new Vector3(0, 1, 0);
                    } catch { }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void GrabRig()
        {
            if (rightGrab)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(new Vector3(0f, GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles.y, 0f));
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(new Vector3(0f, GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles.y, 0f));
                } catch { }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static Vector3 offsetLH = Vector3.zero;
        public static Vector3 offsetRH = Vector3.zero;
        public static Vector3 offsetH = Vector3.zero;
        public static void EnableSpazRig()
        {
            ghostException = true;
            offsetLH = GorillaTagger.Instance.offlineVRRig.leftHand.trackingPositionOffset;
            offsetRH = GorillaTagger.Instance.offlineVRRig.rightHand.trackingPositionOffset;
            offsetH = GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset;
        }

        public static void SpazRig()
        {
            if (rightPrimary)
            {
                float spazAmount = 0.1f;
                ghostException = true;
                GorillaTagger.Instance.offlineVRRig.leftHand.trackingPositionOffset = offsetLH + new Vector3(UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount));
                GorillaTagger.Instance.offlineVRRig.rightHand.trackingPositionOffset = offsetRH + new Vector3(UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount));
                GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = offsetH + new Vector3(UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount), UnityEngine.Random.Range(-spazAmount, spazAmount));
            }
            else
            {
                ghostException = false;
                GorillaTagger.Instance.offlineVRRig.leftHand.trackingPositionOffset = offsetLH;
                GorillaTagger.Instance.offlineVRRig.rightHand.trackingPositionOffset = offsetRH;
                GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = offsetH;
            }
        }

        public static void DisableSpazRig()
        {
            ghostException = false;
            GorillaTagger.Instance.offlineVRRig.leftHand.trackingPositionOffset = offsetLH;
            GorillaTagger.Instance.offlineVRRig.rightHand.trackingPositionOffset = offsetRH;
            GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = offsetH;
        }

        public static void SpazHands()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                } catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                try {
                    GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                } catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

                float spazAmount = 360f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount)));
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount)));

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.leftHandTransform.position + GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.forward * 3f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.forward * 3f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void SpazRealHands()
        {
            if (rightPrimary)
            {
                float spazAmount = 360f;
                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount)));
                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position = GorillaTagger.Instance.leftHandTransform.position + GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.forward * 3f;

                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount), UnityEngine.Random.Range(0, spazAmount)));
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position = GorillaTagger.Instance.rightHandTransform.position + GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.forward * 3f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FreezeRigLimbs()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                } catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                } catch { }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FixRigHandRotation()
        {
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation *= Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.leftHand.trackingRotationOffset);
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation *= Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.rightHand.trackingRotationOffset);
        }

        public static void FreezeRigBody()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                var leftHandTransform = TrueLeftHand();
                var rightHandTransform = TrueRightHand();

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = leftHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = rightHandTransform.position;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = leftHandTransform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = rightHandTransform.rotation;

                FixRigHandRotation();

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void SpinRigBody()
        {
            Patches.TorsoPatch.enabled = true;
            Patches.TorsoPatch.mode = 0;
        }

        public static void SpazRigBody()
        {
            Patches.TorsoPatch.enabled = true;
            Patches.TorsoPatch.mode = 1;
        }

        public static void ReverseRigBody()
        {
            Patches.TorsoPatch.enabled = true;
            Patches.TorsoPatch.mode = 2;
        }

        public static GameObject recBodyRotary;
        public static void RecRoomBody()
        {
            Patches.TorsoPatch.enabled = true;
            Patches.TorsoPatch.mode = 3;

            if (recBodyRotary == null)
                recBodyRotary = new GameObject("ii_recBodyRotary");
            recBodyRotary.transform.rotation = Quaternion.Lerp(recBodyRotary.transform.rotation, Quaternion.Euler(0f, GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 6.5f);
        }

        public static void FixBody()
        {
            Patches.TorsoPatch.enabled = false;
            if (recBodyRotary != null)
                UnityEngine.Object.Destroy(recBodyRotary);
        }

        public static void FakeOculusMenu() // I swear I thought the oculus menu had their arms crossed
        {
            if (leftPrimary)
            {
                Safety.NoFinger();
                GorillaLocomotion.GTPlayer.Instance.inOverlay = true;
                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.localPosition = new Vector3(238f, -90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.localPosition = new Vector3(-190f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, -49f, 0f);
            }
           
        }

        public static void FakeReportMenu()
        {
            if (leftPrimary)
                Safety.NoFinger();

            GorillaLocomotion.GTPlayer.Instance.inOverlay = leftPrimary;
        }

        public static void EnableFakeBrokenController()
        {
            GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHandTriggerCollider").GetComponent<Collider>().enabled = false;
        }

        public static void FakeBrokenController()
        {
            Vector3 Position = leftPrimary ? GorillaTagger.Instance.leftHandTransform.position : GorillaTagger.Instance.rightHandTransform.position;
            Quaternion Rotation = leftPrimary ? GorillaTagger.Instance.leftHandTransform.rotation : GorillaTagger.Instance.rightHandTransform.rotation;

            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position = Position;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position = Position;
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation = Rotation;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation = Rotation;

            Safety.NoFinger();
        }

        public static void DisableFakeBrokenController()
        {
            GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHandTriggerCollider").GetComponent<Collider>().enabled = true;
        }

        public static Vector3 deadPosition = Vector3.zero;
        public static Vector3 lvel = Vector3.zero;
        public static void FakePowerOff()
        {
            if (leftJoystickClick)
            {
                if (deadPosition == Vector3.zero)
                {
                    deadPosition = GorillaTagger.Instance.rigidbody.transform.position;
                    lvel = GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity;
                }
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.rigidbody.transform.position = deadPosition;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = lvel;
            } else
            {
                deadPosition = Vector3.zero;
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void AutoDance()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                Vector3 bodyOffset = (GorillaTagger.Instance.bodyCollider.transform.right * (Mathf.Cos((float)Time.frameCount / 20f) * 0.3f)) + (new Vector3(0f, Mathf.Abs(Mathf.Sin((float)Time.frameCount / 20f) * 0.2f), 0f));
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f) + bodyOffset;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f) + bodyOffset;
                } catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                } catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.forward * 0.2f + GorillaTagger.Instance.offlineVRRig.transform.right * -0.4f + GorillaTagger.Instance.offlineVRRig.transform.up * (0.3f + (Mathf.Sin((float)Time.frameCount / 20f) * 0.2f));
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.forward * 0.2f + GorillaTagger.Instance.offlineVRRig.transform.right * 0.4f + GorillaTagger.Instance.offlineVRRig.transform.up * (0.3f + (Mathf.Sin((float)Time.frameCount / 20f) * -0.2f));

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void AutoGriddy()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                Vector3 bodyOffset = GorillaTagger.Instance.offlineVRRig.transform.forward * (5f * Time.deltaTime);
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + bodyOffset;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + bodyOffset;
                } catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * -0.33f) + (GorillaTagger.Instance.offlineVRRig.transform.forward * (0.5f * Mathf.Cos((float)Time.frameCount / 10f))) + (GorillaTagger.Instance.offlineVRRig.transform.up * (-0.5f * Mathf.Abs(Mathf.Sin((float)Time.frameCount / 10f))));
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * 0.33f) + (GorillaTagger.Instance.offlineVRRig.transform.forward * (0.5f * Mathf.Cos((float)Time.frameCount / 10f))) + (GorillaTagger.Instance.offlineVRRig.transform.up * (-0.5f * Mathf.Abs(Mathf.Sin((float)Time.frameCount / 10f))));

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void AutoTPose()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1f;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void Helicopter()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position += new Vector3(0f, 0.05f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position += new Vector3(0f, 0.05f, 0f);
                } catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                } catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1f;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void Beyblade()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1f;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void Fan()
        {
            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                    GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + GorillaTagger.Instance.offlineVRRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position - (GorillaTagger.Instance.offlineVRRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + GorillaTagger.Instance.offlineVRRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        private static Vector3 headPos = Vector3.zero;
        private static Vector3 headRot = Vector3.zero;

        private static Vector3 handPos_L = Vector3.zero;
        private static Vector3 handRot_L = Vector3.zero;

        private static Vector3 handPos_R = Vector3.zero;
        private static Vector3 handRot_R = Vector3.zero;

        public static void GhostAnimations()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;

            if (headPos == Vector3.zero)
                headPos = GorillaTagger.Instance.headCollider.transform.position;
            if (headRot == Vector3.zero)
                headRot = GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles;

            if (handPos_L == Vector3.zero)
                handPos_L = GorillaTagger.Instance.leftHandTransform.transform.position;
            if (handRot_L == Vector3.zero)
                handRot_L = GorillaTagger.Instance.leftHandTransform.transform.rotation.eulerAngles;

            if (handPos_R == Vector3.zero)
                handPos_R = GorillaTagger.Instance.rightHandTransform.transform.position;
            if (handRot_R == Vector3.zero)
                handRot_R = GorillaTagger.Instance.rightHandTransform.transform.rotation.eulerAngles;

            float positionSpeed = 0.01f; 
            float rotationSpeed = 2.0f; 
            float positionThreshold = 0.05f;
            float rotationThreshold = 11.5f; 
            if (Vector3.Distance(headPos, GorillaTagger.Instance.headCollider.transform.position) > positionThreshold)
                headPos += Vector3.Normalize(GorillaTagger.Instance.headCollider.transform.position - headPos) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(headRot), GorillaTagger.Instance.headCollider.transform.rotation) > rotationThreshold)
                headRot = Quaternion.RotateTowards(Quaternion.Euler(headRot), GorillaTagger.Instance.headCollider.transform.rotation, rotationSpeed).eulerAngles;

            if (Vector3.Distance(handPos_L, GorillaTagger.Instance.leftHandTransform.transform.position) > positionThreshold)
                handPos_L += Vector3.Normalize(GorillaTagger.Instance.leftHandTransform.transform.position - handPos_L) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(handRot_L), GorillaTagger.Instance.leftHandTransform.transform.rotation) > rotationThreshold)
                handRot_L = Quaternion.RotateTowards(Quaternion.Euler(handRot_L), GorillaTagger.Instance.leftHandTransform.transform.rotation, rotationSpeed).eulerAngles;

            if (Vector3.Distance(handPos_R, GorillaTagger.Instance.rightHandTransform.transform.position) > positionThreshold)
                handPos_R += Vector3.Normalize(GorillaTagger.Instance.rightHandTransform.transform.position - handPos_R) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(handRot_R), GorillaTagger.Instance.rightHandTransform.transform.rotation) > rotationThreshold)
                handRot_R = Quaternion.RotateTowards(Quaternion.Euler(handRot_R), GorillaTagger.Instance.rightHandTransform.transform.rotation, rotationSpeed).eulerAngles;



            GorillaTagger.Instance.offlineVRRig.transform.position = headPos - new Vector3(0f, 0.15f, 0f);
            try
            {
                GorillaTagger.Instance.myVRRig.transform.position = headPos - new Vector3(0f, 0.15f, 0f);
            }
            catch { }

            GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(new Vector3(0f, headRot.y, 0f));
            try
            {
                GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(new Vector3(0f, headRot.y, 0f));
            }
            catch { }

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(headRot);

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = handPos_L;
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = handPos_R;

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(handRot_L);
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(handRot_R);

            GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = leftTrigger;
            GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = leftGrab ? 1 : 0;
            GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = leftPrimary || leftSecondary ? 1 : 0;

            GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
            GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
            GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

            GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = rightTrigger;
            GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = rightGrab ? 1 : 0;
            GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = rightPrimary || rightSecondary ? 1 : 0;

            GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
            GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
            GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);


            FixRigHandRotation();
        }

        public static void DisableGhostAnimations()
        {
            headPos = Vector3.zero;
            headRot = Vector3.zero;

            handPos_L = Vector3.zero;
            handRot_L = Vector3.zero;

            handPos_R = Vector3.zero;
            handRot_R = Vector3.zero;

            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void MinecraftAnimations()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;

            GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            try
            {
                GorillaTagger.Instance.myVRRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            } catch { }

            GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            try
            {
                GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            } catch { }

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            if (rightPrimary)
            {
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + (GorillaTagger.Instance.bodyCollider.transform.right * -0.25f) + (GorillaTagger.Instance.bodyCollider.transform.up * -1f) + (GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin((float)Time.frameCount / 10f));
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + (GorillaTagger.Instance.bodyCollider.transform.right * 0.25f) + (GorillaTagger.Instance.bodyCollider.transform.up * -1f) + -(GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin((float)Time.frameCount / 10f));
            } else
            {
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + (GorillaTagger.Instance.bodyCollider.transform.right * -0.25f) + (GorillaTagger.Instance.bodyCollider.transform.up * -1f);
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + (GorillaTagger.Instance.bodyCollider.transform.right * 0.25f) + (GorillaTagger.Instance.bodyCollider.transform.up * -1f);
            }

            if (rightSecondary)
            {
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + (GorillaTagger.Instance.bodyCollider.transform.right * 0.25f) + Vector3.Lerp(GorillaTagger.Instance.rightHandTransform.forward, - GorillaTagger.Instance.rightHandTransform.up, 0.5f) * 2f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }

            FixRigHandRotation();
        }

        public static void StareAtNearby()
        {
            GorillaTagger.Instance.offlineVRRig.headConstraint.LookAt(GetClosestVRRig().headMesh.transform.position);
            GorillaTagger.Instance.offlineVRRig.head.rigTarget.LookAt(GetClosestVRRig().headMesh.transform.position);
        }

        public static void StareAtGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.headConstraint.LookAt(whoCopy.headMesh.transform.position);
                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.LookAt(whoCopy.headMesh.transform.position);
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        public static void EnableFloatingRig()
        {
            offsetH = GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset;
        }

        public static void FloatingRig()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = offsetH + new Vector3(0f, 0.65f + (Mathf.Sin((float)Time.frameCount / 40f) * 0.2f), 0f);
        }

        public static void DisableFloatingRig()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = offsetH;
        }

        public static void Bees()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            if (Time.time > beesDelay)
            {
                VRRig target = GetRandomVRRig(false);//GorillaParent.instance.vrrigs[UnityEngine.Random.Range(0, GorillaParent.instance.vrrigs.Count - 1)];

                GorillaTagger.Instance.offlineVRRig.transform.position = target.transform.position + new Vector3(0f, 1f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position = target.transform.position + new Vector3(0f, 1f, 0f);
                } catch { }

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = target.transform.position;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = target.transform.position;

                beesDelay = Time.time + 0.777f;
            }
        }

        public static void SizeChanger()
        {
            float increment = 0.05f;
            if (!GetIndex("Disable Size Changer Buttons").enabled)
            {
                if (leftTrigger > 0.5f)
                {
                    increment = 0.2f;
                }
                if (leftGrab)
                {
                    increment = 0.01f;
                }
                if (rightTrigger > 0.5f)
                {
                    sizeScale += increment;
                }
                if (rightGrab)
                {
                    sizeScale -= increment;
                }
                if (rightPrimary)
                {
                    sizeScale = 1f;
                }
            }
            if (sizeScale < 0.05f)
            {
                sizeScale = 0.05f;
            }

            GorillaTagger.Instance.offlineVRRig.transform.localScale = Vector3.one * sizeScale;
            GorillaTagger.Instance.offlineVRRig.NativeScale = sizeScale;
            typeof(GorillaLocomotion.GTPlayer).GetField("nativeScale", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GorillaLocomotion.GTPlayer.Instance, sizeScale);
        }

        public static void DisableSizeChanger()
        {
            sizeScale = 1f;

            GorillaTagger.Instance.offlineVRRig.transform.localScale = Vector3.one * sizeScale;
            GorillaTagger.Instance.offlineVRRig.NativeScale = sizeScale;
            typeof(GorillaLocomotion.GTPlayer).GetField("nativeScale", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GorillaLocomotion.GTPlayer.Instance, sizeScale);
        }

        public static void EnableSlipperyHands()
        {
            EverythingSlippery = true;
        }

        public static void DisableSlipperyHands()
        {
            EverythingSlippery = false;
        }

        public static void EnableGrippyHands()
        {
            EverythingGrippy = true;
        }

        public static void DisableGrippyHands()
        {
            EverythingGrippy = false;
        }

        public static GameObject stickpart = null;
        public static void StickyHands()
        {
            if (stickpart == null)
            {
                stickpart = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                FixStickyColliders(stickpart);
                stickpart.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                stickpart.GetComponent<Renderer>().enabled = false;
            }
            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true))
                stickpart.transform.position = TrueLeftHand().position;

            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
                stickpart.transform.position = TrueRightHand().position;

            if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) && GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false))
                stickpart.transform.position = Vector3.zero;
        }

        public static void DisableStickyHands()
        {
            if (stickpart != null)
            {
                UnityEngine.Object.Destroy(stickpart);
                stickpart = null;
            }
        }

        private static bool leftisclimbing = false;
        private static bool rightisclimbing = false;
        private static GameObject climb = null;
        public static void ClimbyHands()
        {
            if (climb == null)
            {
                climb = new GameObject("GR");
                climb.AddComponent<GorillaClimbable>();
            }
            if (leftGrab)
            {
                if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) && !leftisclimbing)
                {
                    climb.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftisclimbing = true;
                    GorillaLocomotion.GTPlayer.Instance.BeginClimbing(climb.AddComponent<GorillaClimbable>(), GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller/GorillaHandClimber").GetComponent<GorillaHandClimber>());
                }
            } else
            {
                leftisclimbing = false;
            }
            if (rightGrab)
            {
                if (GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false) && !rightisclimbing)
                {
                    climb.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightisclimbing = true;
                    GorillaLocomotion.GTPlayer.Instance.BeginClimbing(climb.AddComponent<GorillaClimbable>(), GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller/GorillaHandClimber").GetComponent<GorillaHandClimber>());
                }
            }
            else
            {
                rightisclimbing = false;
            }
        }

        public static void DisableClimbyHands()
        {
            if (climb != null)
            {
                UnityEngine.Object.Destroy(climb);
                climb = null;
            }
        }

        public static void EnableSlideControl()
        {
            oldSlide = GorillaLocomotion.GTPlayer.Instance.slideControl;
            GorillaLocomotion.GTPlayer.Instance.slideControl = 1f;
        }

        public static void EnableWeakSlideControl()
        {
            oldSlide = GorillaLocomotion.GTPlayer.Instance.slideControl;
            GorillaLocomotion.GTPlayer.Instance.slideControl = oldSlide*2f;
        }

        public static void DisableSlideControl()
        {
            GorillaLocomotion.GTPlayer.Instance.slideControl = oldSlide;
        }

        public static Vector3[] lastLeft = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        public static Vector3[] lastRight = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static void PunchMod()
        {
            int index = -1;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    index++;

                    Vector3 they = vrrig.rightHandTransform.position;
                    Vector3 notthem = GorillaTagger.Instance.offlineVRRig.head.rigTarget.position;
                    float distance = Vector3.Distance(they, notthem);

                    if (distance < 0.25f)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.rightHandTransform.position - lastRight[index]) * 10f;
                    }
                    lastRight[index] = vrrig.rightHandTransform.position;

                    they = vrrig.leftHandTransform.position;
                    distance = Vector3.Distance(they, notthem);

                    if (distance < 0.25f)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.Normalize(vrrig.leftHandTransform.position - lastLeft[index]) * 10f;
                    }
                    lastLeft[index] = vrrig.leftHandTransform.position;
                }
            }
        }

        private static VRRig sithlord = null;
        private static bool sithright = false;
        private static float sithdist = 1f;
        public static void Telekinesis()
        {
            if (sithlord == null)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    try
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            if (vrrig.rightIndex.calcT < 0.5f && vrrig.rightMiddle.calcT > 0.5f)
                            {
                                Vector3 dir = vrrig.transform.Find("RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").up;
                                Physics.SphereCast(vrrig.rightHandTransform.position + (dir * 0.1f), 0.3f, dir, out var Ray, 512f, NoInvisLayerMask());
                                {
                                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                                    if (possibly && possibly == GorillaTagger.Instance.offlineVRRig)
                                    {
                                        sithlord = vrrig;
                                        sithright = true;
                                        sithdist = Ray.distance;
                                    }
                                }
                            }
                            if (vrrig.leftIndex.calcT < 0.5f && vrrig.leftMiddle.calcT > 0.5f)
                            {
                                Vector3 dir = vrrig.transform.Find("RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").up;
                                Physics.SphereCast(vrrig.leftHandTransform.position + (dir * 0.1f), 0.3f, dir, out var Ray, 512f, NoInvisLayerMask());
                                {
                                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                                    if (possibly && possibly == GorillaTagger.Instance.offlineVRRig)
                                    {
                                        sithlord = vrrig;
                                        sithright = false;
                                        sithdist = Ray.distance;
                                    }
                                }
                            }
                        }
                    } catch { }
                }
            } else
            {
                if (sithright ? (sithlord.rightIndex.calcT < 0.5f && sithlord.rightMiddle.calcT > 0.5f) : (sithlord.leftMiddle.calcT < 0.5f && sithlord.leftMiddle.calcT > 0.5f))
                {
                    Transform hand = sithright ? sithlord.rightHandTransform : sithlord.leftHandTransform;
                    Vector3 dir = sithright ? sithlord.transform.Find("RigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").up : sithlord.transform.Find("RigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").up;
                    TeleportPlayer(Vector3.Lerp(GorillaTagger.Instance.bodyCollider.transform.position, hand.position + dir * sithdist, 0.1f));
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ZeroGravity();
                } else
                {
                    sithlord = null;
                }
            }
        }

        public static void SolidPlayers()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && Vector3.Distance(vrrig.transform.position, GorillaTagger.Instance.headCollider.transform.position) < 5f)
                {
                    Vector3 pointA = vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f);
                    Vector3 pointB = vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f);
                    GameObject bodyCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(bodyCollider.GetComponent<Rigidbody>());
                    bodyCollider.GetComponent<Renderer>().enabled = false;
                    bodyCollider.transform.position = Vector3.Lerp(pointA, pointB, 0.5f);
                    bodyCollider.transform.rotation = vrrig.transform.rotation;
                    bodyCollider.transform.localScale = new Vector3(0.3f, 0.55f, 0.3f);
                    UnityEngine.Object.Destroy(bodyCollider, Time.deltaTime * 2);

                    for (int i = 0; i < bones.Count<int>(); i += 2)
                    {
                        pointA = vrrig.mainSkin.bones[bones[i]].position;
                        pointB = vrrig.mainSkin.bones[bones[i + 1]].position;
                        bodyCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        UnityEngine.Object.Destroy(bodyCollider.GetComponent<Rigidbody>());
                        bodyCollider.GetComponent<Renderer>().enabled = false;
                        bodyCollider.transform.position = Vector3.Lerp(pointA, pointB, 0.5f);
                        bodyCollider.transform.LookAt(pointB);
                        bodyCollider.transform.localScale = new Vector3(0.2f, 0.2f, Vector3.Distance(pointA, pointB));
                        UnityEngine.Object.Destroy(bodyCollider, Time.deltaTime * 2);
                    }
                }
            }
        }

        public static int pullPowerInt = 0;
        public static void ChangePullModPower()
        {
            float[] powers = new float[]
            {
                0.05f,
                0.1f,
                0.2f,
                0.4f
            };
            string[] powerNames = new string[]
            {
                "Normal",
                "Medium",
                "Strong",
                "Powerful"
            };
            pullPowerInt++;
            if (pullPowerInt > powers.Length - 1)
            {
                pullPowerInt = 0;
            }
            pullPower = powers[pullPowerInt];
            GetIndex("Change Pull Mod Power").overlapText = "Change Pull Mod Power <color=grey>[</color><color=green>" + powerNames[pullPowerInt] + "</color><color=grey>]</color>";
        }

        private static float pullPower = 0.05f;
        private static bool lasttouchleft = false;
        private static bool lasttouchright = false;
        public static void PullMod()
        {
            if (((!GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true) && lasttouchleft) || (!GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false) && lasttouchright)) && rightGrab)
            {
                Vector3 vel = GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity;
                GorillaLocomotion.GTPlayer.Instance.transform.position += new Vector3(vel.x * pullPower, 0f, vel.z * pullPower);
            }
            lasttouchleft = GorillaLocomotion.GTPlayer.Instance.IsHandTouching(true);
            lasttouchright = GorillaLocomotion.GTPlayer.Instance.IsHandTouching(false);
        }

        public static GameObject leftThrow = null;
        public static GameObject rightThrow = null;
        public static void ThrowControllers()
        {
            if (leftPrimary)
            {
                if (leftThrow != null)
                {
                    GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position = leftThrow.transform.position;
                    GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation = leftThrow.transform.rotation;
                }
                else
                {
                    leftThrow = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftThrow.GetComponent<Renderer>().enabled = false;
                    UnityEngine.Object.Destroy(leftThrow.GetComponent<BoxCollider>());
                    UnityEngine.Object.Destroy(leftThrow.GetComponent<Rigidbody>());

                    leftThrow.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                    leftThrow.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    Rigidbody comp = leftThrow.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    comp.velocity = GorillaLocomotion.GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                    try
                    {
                        if (GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller").GetComponent<GorillaVelocityEstimator>() == null)
                        {
                            GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller").AddComponent<GorillaVelocityEstimator>();
                        }
                        comp.angularVelocity = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller").GetComponent<GorillaVelocityEstimator>().angularVelocity;
                    } catch { }
                }
            }
            else
            {
                if (leftThrow != null)
                {
                    UnityEngine.Object.Destroy(leftThrow);
                    leftThrow = null;
                }
            }

            if (rightPrimary)
            {
                if (rightThrow != null)
                {
                    GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position = rightThrow.transform.position;
                    GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation = rightThrow.transform.rotation;
                }
                else
                {
                    rightThrow = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightThrow.GetComponent<Renderer>().enabled = false;
                    UnityEngine.Object.Destroy(rightThrow.GetComponent<BoxCollider>());
                    UnityEngine.Object.Destroy(rightThrow.GetComponent<Rigidbody>());

                    rightThrow.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                    rightThrow.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    Rigidbody comp = rightThrow.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    comp.velocity = GorillaLocomotion.GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                    try
                    {
                        if (GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller").GetComponent<GorillaVelocityEstimator>() == null)
                        {
                            GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller").AddComponent<GorillaVelocityEstimator>();
                        }
                        comp.angularVelocity = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller").GetComponent<GorillaVelocityEstimator>().angularVelocity;
                    } catch { }
                }
            }
            else
            {
                if (rightThrow != null)
                {
                    UnityEngine.Object.Destroy(rightThrow);
                    rightThrow = null;
                }
            }
        }

        public static void StickLongArms()
        {
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.leftHandTransform.position + (GorillaTagger.Instance.leftHandTransform.forward * (armlength - 0.917f));
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.rightHandTransform.position + (GorillaTagger.Instance.rightHandTransform.forward * (armlength - 0.917f));
        }

        public static void EnableSteamLongArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(armlength, armlength, armlength);
        }

        public static void DisableSteamLongArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static void MultipliedLongArms()
        {
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * armlength;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * armlength;
        }

        public static void VerticalLongArms()
        {
            Vector3 lefty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position;
            lefty.y *= armlength;
            Vector3 righty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position;
            righty.y *= armlength;
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - lefty;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - righty;
        }

        public static void HorizontalLongArms()
        {
            Vector3 lefty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position;
            lefty.x *= armlength;
            lefty.z *= armlength;
            Vector3 righty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position;
            righty.x *= armlength;
            righty.z *= armlength;
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - lefty;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - righty;
        }

        public static GameObject lvT = null;
        public static GameObject rvT = null;
        public static void CreateVelocityTrackers()
        {
            lvT = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(lvT.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(lvT.GetComponent<Rigidbody>());
            lvT.GetComponent<Renderer>().enabled = false;
            lvT.AddComponent<GorillaVelocityTracker>();

            rvT = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(rvT.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(rvT.GetComponent<Rigidbody>());
            rvT.GetComponent<Renderer>().enabled = false;
            rvT.AddComponent<GorillaVelocityTracker>();
        }

        public static void DestroyVelocityTrackers()
        {
            UnityEngine.Debug.Log(lvT);
            UnityEngine.Debug.Log(rvT);
        }

        public static void VelocityLongArms()
        {
            lvT.transform.position = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position;
            rvT.transform.position = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position;
            GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.transform.position -= lvT.GetComponent<GorillaVelocityTracker>().GetAverageVelocity(true, 0) * 0.0125f;
            GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position -= rvT.GetComponent<GorillaVelocityTracker>().GetAverageVelocity(true, 0) * 0.0125f;
        }

        public static void FlickJump()
        {
            if (rightPrimary)
            {
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.rightHandTransform.position + new Vector3(0f, -1.5f, 0f);
            }
        }

        public static Vector3 longJumpPower = Vector3.zero;
        public static void LongJump()
        {
            if (rightPrimary)
            {
                if (longJumpPower == Vector3.zero)
                {
                    longJumpPower = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity / 125f;
                    longJumpPower.y = 0f;
                }
                GorillaLocomotion.GTPlayer.Instance.transform.position += longJumpPower;
            }
            else
            {
                longJumpPower = Vector3.zero;
            }
        }

        public static void BunnyHop()
        {
            Physics.Raycast(GorillaTagger.Instance.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, out var Ray, 512f, GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers);

            if (Ray.distance < 0.15f)
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.x, (GorillaLocomotion.GTPlayer.Instance.jumpMultiplier * 2.727272727f), GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.z);
            }
        }

        public static void Strafe()
        {
            Vector3 funnyDir = GorillaTagger.Instance.bodyCollider.transform.forward * GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed;
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(funnyDir.x, GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.y, funnyDir.z);
            Vector3 lol = GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity;
            lol.y = (lol.y < 0 ? 0 : lol.y);
        }

        public static void DynamicStrafe()
        {
            float power = new Vector3(GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.x, 0, GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.z).magnitude;
            Vector3 funnyDir = GorillaTagger.Instance.bodyCollider.transform.forward * power;
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(funnyDir.x, GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.y, funnyDir.z);
        }

        public static void GripBunnyHop()
        {
            if (rightGrab)
            {
                BunnyHop();
            }
        }

        public static void GripStrafe()
        {
            if (rightGrab)
            {
                Strafe();
            }
        }

        public static void GripDynamicStrafe()
        {
            if (rightGrab)
            {
                DynamicStrafe();
            }
        }

        public static void GroundHelper()
        {
            if (rightGrab)
            {
                Vector3 x3 = GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity;
                if (x3.y > 0f)
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(x3.x, 0f, x3.z);
                }
            }
        }

        private static float preBounciness = 0f;
        private static PhysicMaterialCombine whateverthisis = PhysicMaterialCombine.Maximum;
        private static float preFrictiness = 0f;

        public static void PreBouncy()
        {
            preBounciness = GorillaTagger.Instance.bodyCollider.material.bounciness;
            whateverthisis = GorillaTagger.Instance.bodyCollider.material.bounceCombine;
            preFrictiness = GorillaTagger.Instance.bodyCollider.material.dynamicFriction;
        }

        public static void Bouncy()
        {
            GorillaTagger.Instance.bodyCollider.material.bounciness = 1f;
            GorillaTagger.Instance.bodyCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
            GorillaTagger.Instance.bodyCollider.material.dynamicFriction = 0f;
        }

        public static void PostBouncy()
        {
            GorillaTagger.Instance.bodyCollider.material.bounciness = preBounciness;
            GorillaTagger.Instance.bodyCollider.material.bounceCombine = whateverthisis;
            GorillaTagger.Instance.bodyCollider.material.dynamicFriction = preFrictiness;
        }

        public static void DisableAir()
        {
            Patches.ForcePatch.enabled = true;
        }

        public static void EnableAir()
        {
            Patches.ForcePatch.enabled = false;
        }

        public static void DisableWater()
        {
            foreach (WaterVolume lol in UnityEngine.Object.FindObjectsOfType<WaterVolume>())
            {
                GameObject v = lol.gameObject;
                v.layer = LayerMask.NameToLayer("TransparentFX");
            }
        }

        public static void SolidWater()
        {
            foreach (WaterVolume lol in UnityEngine.Object.FindObjectsOfType<WaterVolume>())
            {
                GameObject v = lol.gameObject;
                v.layer = LayerMask.NameToLayer("Default");
            }
        }

        public static void FixWater()
        {
            foreach (WaterVolume lol in UnityEngine.Object.FindObjectsOfType<WaterVolume>())
            {
                GameObject v = lol.gameObject;
                v.layer = LayerMask.NameToLayer("Water");
            }
        }

        public static GameObject airSwimPart = null;
        public static void AirSwim()
        {
            if (airSwimPart == null)
            {
                airSwimPart = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume"));
                airSwimPart.transform.localScale = new Vector3(5f, 5f, 5f);
                airSwimPart.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                GorillaLocomotion.GTPlayer.Instance.audioManager.UnsetMixerSnapshot(0.1f);
                airSwimPart.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 2.5f, 0f);
            }
        }

        public static void DisableAirSwim()
        {
            if (airSwimPart != null)
            {
                UnityEngine.Object.Destroy(airSwimPart);
            }
        }

        public static void FastSwim()
        {
            if (GorillaLocomotion.GTPlayer.Instance.InWater)
            {
                GorillaLocomotion.GTPlayer.Instance.gameObject.GetComponent<Rigidbody>().velocity *= 1.069f;
            }
        }

        public static void PiggybackGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    TeleportPlayer(whoCopy.transform.position + new Vector3(0f, 0.5f, 0f));
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                }
            }
        }

        public static void CopyMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position;
                    GorillaTagger.Instance.offlineVRRig.transform.rotation = whoCopy.transform.rotation;

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = whoCopy.leftHandTransform.position;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = whoCopy.rightHandTransform.position;

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = whoCopy.leftHandTransform.rotation;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = whoCopy.rightHandTransform.rotation;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = whoCopy.leftIndex.calcT;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = whoCopy.leftMiddle.calcT;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = whoCopy.leftThumb.calcT;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = whoCopy.rightIndex.calcT;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = whoCopy.rightMiddle.calcT;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = whoCopy.rightThumb.calcT;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = whoCopy.headMesh.transform.rotation;
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void FollowPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    Vector3 look = whoCopy.transform.position - GorillaTagger.Instance.offlineVRRig.transform.position;
                    look.Normalize();

                    Vector3 position = GorillaTagger.Instance.offlineVRRig.transform.position + (look * ((flySpeed / 2f) * Time.deltaTime));

                    GorillaTagger.Instance.offlineVRRig.transform.position = position;
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = position;
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.transform.LookAt(whoCopy.transform.position);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.LookAt(whoCopy.transform.position);
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * -1f);
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * 1f);

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                    FixRigHandRotation();

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void OrbitPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position + new Vector3(Mathf.Cos((float)Time.frameCount / 20f), 0.5f, Mathf.Sin((float)Time.frameCount / 20f));
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = whoCopy.transform.position + new Vector3(Mathf.Cos((float)Time.frameCount / 20f), 0.5f, Mathf.Sin((float)Time.frameCount / 20f));
                    } catch { }
                    GorillaTagger.Instance.offlineVRRig.transform.LookAt(whoCopy.transform.position);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.LookAt(whoCopy.transform.position);
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * -1f);
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + (GorillaTagger.Instance.offlineVRRig.transform.right * 1f);

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                    FixRigHandRotation();

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void JumpscareGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.headMesh.transform.position + (whoCopy.headMesh.transform.forward * (UnityEngine.Random.Range(10f, 50f) / 100f));
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = whoCopy.headMesh.transform.position + (whoCopy.headMesh.transform.forward * (UnityEngine.Random.Range(10f, 50f) / 100f));
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.LookAt(whoCopy.headMesh.transform.position);
                    Quaternion dirLook = GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation;

                    GorillaTagger.Instance.offlineVRRig.transform.rotation = dirLook;
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.rotation = dirLook;
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = whoCopy.headMesh.transform.position + (whoCopy.headMesh.transform.right * 0.2f);
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = whoCopy.headMesh.transform.position + (whoCopy.headMesh.transform.right * -0.2f);

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = dirLook;

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                    FixRigHandRotation();

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void AnnoyPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    Vector3 position = whoCopy.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);

                    GorillaTagger.Instance.offlineVRRig.transform.position = position;
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = position;
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.transform.LookAt(whoCopy.transform.position);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.LookAt(whoCopy.transform.position);
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = whoCopy.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = whoCopy.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f, UnityEngine.Random.Range(-10f, 10f) / 10f);

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);

                    /*if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.RPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                            91,
                            false,
                            999999f
                        });
                        RPCProtection();
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(91, false, 999999f);
                    }*/
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void ConfusePlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position - new Vector3(0f, 2f, 0f);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = whoCopy.transform.position - new Vector3(0f, 2f, 0f);
                    }
                    catch { }

                    if (Time.time > splashDel)
                    {
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", GetPlayerFromVRRig(whoCopy), new object[]
                        {
                            whoCopy.transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f),UnityEngine.Random.Range(-0.5f, 0.5f),UnityEngine.Random.Range(-0.5f, 0.5f)),
                            Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0,360))),
                            4f,
                            100f,
                            true,
                            false
                        });
                        RPCProtection();
                        splashDel = Time.time + 0.1f;
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void IntercourseGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    if (!GetIndex("Reverse Intercourse").enabled)
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * -(0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f)));
                        try
                        {
                            GorillaTagger.Instance.myVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * -(0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f)));
                        } catch { }

                        GorillaTagger.Instance.offlineVRRig.transform.rotation = whoCopy.transform.rotation;
                        try
                        {
                            GorillaTagger.Instance.myVRRig.transform.rotation = whoCopy.transform.rotation;
                        } catch { }

                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * -0.2f) + whoCopy.transform.up * -0.4f;
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * 0.2f) + whoCopy.transform.up * -0.4f;

                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = whoCopy.transform.rotation;
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = whoCopy.transform.rotation;
                    } else
                    {
                        GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * (0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f)));
                        try
                        {
                            GorillaTagger.Instance.myVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * (0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f)));
                        } catch { }

                        GorillaTagger.Instance.offlineVRRig.transform.rotation = whoCopy.transform.rotation;
                        try
                        {
                            GorillaTagger.Instance.myVRRig.transform.rotation = whoCopy.transform.rotation;
                        } catch { }

                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * -0.2f) + whoCopy.transform.up * -0.4f;
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * 0.2f) + whoCopy.transform.up * -0.4f;

                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = whoCopy.transform.rotation;
                    }

                    FixRigHandRotation();

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = whoCopy.transform.rotation;

                    if ((Time.frameCount % 45) == 0)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                                64,
                                false,
                                999999f
                            });
                            RPCProtection();
                        }
                        else
                        {
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(64, false, 999999f);
                        }
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void HeadGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                GameObject NewPointer = GunData.NewPointer;

                if (isCopying && whoCopy != null)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;

                    GorillaTagger.Instance.offlineVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * (0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f))) + (whoCopy.transform.up * -0.4f);
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.position = whoCopy.transform.position + (whoCopy.transform.forward * (0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f))) + (whoCopy.transform.up * -0.4f);
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    try
                    {
                        GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    } catch { }

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * 0.2f) + whoCopy.transform.up * -0.4f;
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (whoCopy.transform.position + whoCopy.transform.right * -0.2f) + whoCopy.transform.up * -0.4f;

                    GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                    GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(whoCopy.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                    GorillaTagger.Instance.offlineVRRig.leftIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.leftThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.leftIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.leftThumb.LerpFinger(1f, false);

                    GorillaTagger.Instance.offlineVRRig.rightIndex.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.calcT = 0f;
                    GorillaTagger.Instance.offlineVRRig.rightThumb.calcT = 0f;

                    GorillaTagger.Instance.offlineVRRig.rightIndex.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightMiddle.LerpFinger(1f, false);
                    GorillaTagger.Instance.offlineVRRig.rightThumb.LerpFinger(1f, false);

                    FixRigHandRotation();

                    if ((Time.frameCount % 45) == 0)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, new object[]{
                                64,
                                true,
                                999999f
                            });
                            RPCProtection();
                        }
                        else
                        {
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(64, true, 999999f);
                        }
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig possibly = Ray.collider.GetComponentInParent<VRRig>();
                    if (possibly && possibly != GorillaTagger.Instance.offlineVRRig)
                    {
                        isCopying = true;
                        whoCopy = possibly;
                    }
                }
            }
            else
            {
                if (isCopying)
                {
                    isCopying = false;
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void RemoveCopy()
        {
            isCopying = false;
            whoCopy = null;
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static void SpazHead()
        {
            if (GorillaTagger.Instance.offlineVRRig.enabled)
            {
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = UnityEngine.Random.Range(0f, 360f);
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = UnityEngine.Random.Range(0f, 360f);
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = UnityEngine.Random.Range(0f, 360f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
            }
        }

        public static void RandomSpazHead()
        {
            if (headspazType)
            {
                SpazHead();
                if (Time.time > headspazDelay)
                {
                    headspazType = false;
                    headspazDelay = Time.time + UnityEngine.Random.Range(1000f, 4000f) / 1000f;
                }
            }
            else
            {
                Fun.FixHead();
                if (Time.time > headspazDelay)
                {
                    headspazType = true;
                    headspazDelay = Time.time + UnityEngine.Random.Range(200f, 1000f) / 1000f;
                }
            }
        }

        private static Vector3 headoffs = Vector3.zero;
        public static void EnableSpazHead()
        {
            headoffs = GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset;
        }

        public static void SpazHeadPosition()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = headoffs + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
        }

        public static void FixHeadPosition()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingPositionOffset = headoffs;
        }

        public static void RandomSpazHeadPosition()
        {
            if (headspazType)
            {
                SpazHeadPosition();
                if (Time.time > headspazDelay)
                {
                    headspazType = false;
                    headspazDelay = Time.time + UnityEngine.Random.Range(1000f, 4000f) / 1000f;
                }
            }
            else
            {
                FixHeadPosition();
                if (Time.time > headspazDelay)
                {
                    headspazType = true;
                    headspazDelay = Time.time + UnityEngine.Random.Range(200f, 1000f) / 1000f;
                }
            }
        }

        public static bool idiotfixthingy = false;
        public static void LaggyRig()
        {
            ghostException = true;
            if (Time.time > laggyRigDelay)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                idiotfixthingy = true;
                laggyRigDelay = Time.time + 0.5f;
            } else
            {
                if (idiotfixthingy)
                {
                    idiotfixthingy = false;
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
            }
        }

        public static void SmoothRig()
        {
            PhotonNetwork.SerializationRate = 30;
        }

        public static void DisableSmoothRig()
        {
            PhotonNetwork.SerializationRate = 10;
        }

        public static void UpdateRig()
        {
            ghostException = true;
            if (rightPrimary && !lastprimaryhit)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                idiotfixthingy = true;
            }
            else
            {
                if (idiotfixthingy)
                {
                    idiotfixthingy = false;
                } else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
                
            }
            lastprimaryhit = rightPrimary;
        }
    }
}
