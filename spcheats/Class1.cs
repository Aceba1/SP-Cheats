﻿using System;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace SinglePlayerCheats
{
    public class Class1
    {
        public static bool ModExists(string name)
        {
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith(name))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Patch()
        {
            new GameObject().AddComponent<tGUI>().BlockInjector = ModExists("BlockInjector");
            var harmony = HarmonyInstance.Create("aceba1.sessionmods");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        
    }
    [HarmonyPatch(typeof(Damageable), "DealActualDamage")]
    static class HarmonyPatchHacks //HeX#3692 was here coding some shenanigans
    {
        public static bool Banana;
        static bool Prefix(Damageable __instance)
        {
            if (Singleton.Manager<ManPlayer>.inst.PlayerIndestructible)
            {
                try
                {
                    if (Banana)
                    {
                        return !__instance.Block.tank.IsFriendly();
                    }
                }
                catch
                {
                    return true;
                }
                return false;
            }
            return true;
            // return !Singleton.Manager<ManPlayer>.inst.PlayerIndestructible;
        }
    }
    public class tGUI : MonoBehaviour
    {
        public bool BlockInjector = false;
        private Rect window = new Rect(0f, 0f, 300, 250);
        private int SelectedBlockType = 0;
        private bool ShowGUI = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ShowGUI = !ShowGUI;
            }
        }

        private void OnGUI()
        {
            if (ShowGUI && !Singleton.Manager<ManNetwork>.inst.IsMultiplayer())
            {
                window = GUI.Window(25346, window, OnGUIWindow, "Cheat Menu");
            }
        }

        private void OnGUIWindow(int ID)
        {
            try
            {
                if (GUILayout.Button("Fill Inv (32 of each block)"))
                {
                    foreach (object obj in Enum.GetValues(typeof(BlockTypes)))
                    {
                        BlockTypes blockType = (BlockTypes)obj;
                        try
                        {
                            Singleton.Manager<ManPlayer>.inst.PlayerInventory.AddBlocks(blockType, 32);
                        }
                        catch
                        {
                        }
                    }
                    if (BlockInjector)
                    {
                        foreach (int i in SPCheats.BlockInjector.CustomBlocks)
                        {
                            BlockTypes blockType = (BlockTypes)i;
                            try
                            {
                                Singleton.Manager<ManPlayer>.inst.PlayerInventory.AddBlocks(blockType, 32);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                try
                {
                    SelectedBlockType = int.Parse(GUILayout.TextField(this.SelectedBlockType.ToString()));
                    GUILayout.Label(Enum.GetName(typeof(BlockTypes), this.SelectedBlockType));
                }
                catch
                {
                }
                if (GUILayout.Button("Spawn Block"))
                {
                    try
                    {
                        Singleton.Manager<ManSpawn>.inst.SpawnBlock((BlockTypes)this.SelectedBlockType, Singleton.playerTank.transform.position + new Vector3(10f, 20f, 10f), Quaternion.Euler(0f, 0f, 0f));
                    }
                    catch
                    {
                    }
                }
                if (GUILayout.Button("Add 1 (mil) money"))
                {
                    try
                    {
                        Singleton.Manager<ManPlayer>.inst.AddMoney(1000000);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    
                    Singleton.Manager<ManPlayer>.inst.PlayerIndestructible = GUILayout.Toggle(Singleton.Manager<ManPlayer>.inst.PlayerIndestructible, "Invincibility for ALL");
                    if (Singleton.Manager<ManPlayer>.inst.PlayerIndestructible)
                    {
                        HarmonyPatchHacks.Banana = GUILayout.Toggle(HarmonyPatchHacks.Banana, "Only for Allies");
                        GUILayout.Label("TURN OFF INVINCIBILITIES BEFORE SWITCHING GAMEMODES");
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
            GUI.DragWindow();
        }
    }
}