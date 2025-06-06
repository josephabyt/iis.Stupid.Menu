﻿using BepInEx;
using GorillaNetworking;
using iiMenu.Classes;
using iiMenu.Menu;
using iiMenu.Mods;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using static iiMenu.Menu.Main;

namespace iiMenu.UI
{
    public class Main : MonoBehaviour
    {
        private static Main instance;

        private string inputText = "goldentrophy";

        private string r = "255";
        private string g = "128";
        private string b = "0";

        public static bool isOpen = true;
        public static bool lastCondition = false;

        public static Texture2D icon;

        private void Start()
        {
            instance = this;

            if (File.Exists("iisStupidMenu/iiMenu_HideGUI.txt"))
                isOpen = false;
        }

        private void OnGUI()
        {
            bool isKeyboardCondition = UnityInput.Current.GetKey(KeyCode.Backslash);

            if (isKeyboardCondition && !lastCondition)
            {
                isOpen = !isOpen;
                if (isOpen)
                {
                    if (File.Exists("iisStupidMenu/iiMenu_HideGUI.txt"))
                        File.Delete("iisStupidMenu/iiMenu_HideGUI.txt");
                } else
                {
                    if (!File.Exists("iisStupidMenu/iiMenu_HideGUI.txt"))
                        File.WriteAllText("iisStupidMenu/iiMenu_HideGUI.txt", "true");
                }
            }
            lastCondition = isKeyboardCondition;

            if (isOpen)
            {
                GUI.skin.textField.fontSize = 13;
                GUI.skin.button.fontSize = 20;
                GUI.skin.textField.font = activeFont;
                GUI.skin.button.font = activeFont;
                GUI.skin.label.font = activeFont;
                GUI.skin.label.richText = true;
                GUI.skin.textField.fontStyle = activeFontStyle;
                GUI.skin.button.fontStyle = activeFontStyle;
                GUI.skin.label.fontStyle = activeFontStyle;

                Color guiColor = GetIndex("Swap GUI Colors").enabled ? textColor : GetBGColor(0f);

                GUI.color = guiColor;
                GUI.backgroundColor = guiColor;

                string roomText = translate ? TranslateText("Not connected to room") : "Not connected to room";
                try
                {
                    if (PhotonNetwork.InRoom)
                        roomText = (translate ? TranslateText("Connected to room") : "Connected to room") + " " + PhotonNetwork.CurrentRoom.Name;
                } catch { }
                GUI.Label(new Rect(10, Screen.height - 35, Screen.width, 40), roomText);
                
                if (icon == null)
                    icon = LoadTextureFromResource("iiMenu.Resources.icon.png");

                try
                {
                    if (icon != null)
                    {
                        Rect pos = new Rect(Screen.width - 70, Screen.height - 70, 64, 64);
                        Matrix4x4 matrix = GUI.matrix;

                        GUIUtility.RotateAroundPivot(Mathf.Sin(Time.time * 2f) * 10f, pos.center);
                        GUI.DrawTexture(pos, icon);
                        GUI.matrix = matrix;

                        GUIStyle style = new GUIStyle(GUI.skin.label);
                        style.alignment = TextAnchor.LowerRight;
                        GUI.Label(new Rect(Screen.width - 590, Screen.height - 75, 512, 64), (translate ? TranslateText("Build") : "Build")+" "+PluginInfo.Version+"\n"+(serverLink.Replace("https://", "")), style);
                    }
                }
                catch { }

                GUI.Box(new Rect(Screen.width - 250, 10, 240, 120), "", GUI.skin.box);

                inputText = GUI.TextField(new Rect(Screen.width - 200, 20, 180, 20), inputText);

                r = GUI.TextField(new Rect(Screen.width - 240, 20, 30, 20), r);
                g = GUI.TextField(new Rect(Screen.width - 240, 50, 30, 20), g);
                b = GUI.TextField(new Rect(Screen.width - 240, 80, 30, 20), b);

                if (GUI.Button(new Rect(Screen.width - 200, 50, 85, 30), translate ? TranslateText("Name") : "Name"))
                    ChangeName(inputText.Replace("\\n", "\n"));
                
                if (GUI.Button(new Rect(Screen.width - 105, 50, 85, 30), translate ? TranslateText("Color") : "Color"))
                {
                    UnityEngine.Color color = new Color32(byte.Parse(r), byte.Parse(g), byte.Parse(b), 255);
                    ChangeColor(color);
                }

                bool Create = false;
                try
                {
                    Create = UnityInput.Current.GetKey(KeyCode.LeftControl);
                } catch { }

                string targetText = Create ? "Create" : "Join";
                if (GUI.Button(new Rect(Screen.width - 200, 90, 85, 30), translate ? TranslateText(targetText) : targetText))
                {
                    if (Create)
                        iiMenu.Mods.Important.CreateRoom(inputText.Replace("\\n", "\n"), true);
                    else
                        PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(inputText.Replace("\\n", "\n"), JoinType.Solo);
                }
                if (GUI.Button(new Rect(Screen.width - 105, 90, 85, 30), "Queue"))
                {
                    NetworkSystem.Instance.ReturnToSinglePlayer();
                    rejRoom = inputText;
                }

                try
                {
                    GUI.color = guiColor;
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10f);

                    GUILayout.BeginVertical();
                    GUILayout.Space(10f);

                    List<string> alphabetized = new List<string>();

                    int categoryIndex = 0;
                    foreach (ButtonInfo[] buttonlist in Buttons.buttons)
                    {
                        foreach (ButtonInfo v in buttonlist)
                        {
                            try
                            {
                                if (v.enabled && (!hideSettings || (hideSettings && !Buttons.categoryNames[categoryIndex].Contains("Settings"))))
                                {
                                    string buttonText = (v.overlapText == null) ? v.buttonText : v.overlapText;
                                    if (translate)
                                        buttonText = TranslateText(buttonText);

                                    if (inputTextColor != "green")
                                        buttonText = buttonText.Replace(" <color=grey>[</color><color=green>", " <color=grey>[</color><color=" + inputTextColor + ">");

                                    if (lowercaseMode)
                                        buttonText = buttonText.ToLower();

                                    alphabetized.Add(buttonText);
                                }
                            }
                            catch { }
                        }
                        categoryIndex++;
                    }

                    Regex notags = new Regex("<.*?>");
                    string[] sortedButtons = alphabetized
                        .OrderByDescending(s => NoRichtextTags(s).Length)
                        .ToArray();

                    int index = 0;
                    foreach (string v in sortedButtons)
                    {
                        if (advancedArraylist)
                        {
                            string text = $"<color=#{ColorToHex(GetBGColor(index * 0.1f))}>| </color><color=#{ColorToHex(textColor)}>{v}</color>";

                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.richText = true;
                            labelStyle.fontStyle = (FontStyle)((int)activeFontStyle % 2);

                            Vector2 size = labelStyle.CalcSize(new GUIContent(text));
                            GUILayout.Space(size.y);

                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            Rect labelRect = new Rect(lastRect.x, lastRect.y, size.x + 5, index == sortedButtons.Length - 1 ? size.y + 5 : size.y);

                            Color oldColor = GUI.color;
                            GUI.color = new Color(0f, 0f, 0f, 0.4f);
                            GUI.DrawTexture(labelRect, Texture2D.whiteTexture);
                            GUI.color = oldColor;

                            GUI.Label(labelRect, text, labelStyle);
                        }
                        else
                            GUILayout.Label(v);

                        index++;
                    }

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                catch { }


                foreach (KeyValuePair<string, Assembly> Plugin in Settings.LoadedPlugins)
                {
                    try
                    {
                        if (!Settings.disabledPlugins.Contains(Plugin.Key))
                            PluginOnGUI(Plugin.Value);
                    }
                    catch (Exception e) { LogManager.Log("Error with OnGUI plugin " + Plugin.Key + ": " + e.ToString()); }
                }
            }
        }
    }
}