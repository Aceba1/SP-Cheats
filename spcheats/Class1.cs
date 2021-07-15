using System;
using UnityEngine;
using HarmonyLib;
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
            new GameObject().AddComponent<tGUI>();
            tGUI.BlockInjector = ModExists("BlockInjector");
            tGUI.GUIDisp = new GameObject();
            tGUI.GUIDisp.AddComponent<tGUI.GUIDisplay>();
            tGUI.GUIDisp.SetActive(false);
            var harmony = new Harmony("aceba1.sessionmods");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        
    }
    //[HarmonyPatch(typeof(Damageable), "DealActualDamage")]
    //static class HarmonyPatchHacks //HeX#3692 was here coding some shenanigans
    //{
    //    public static bool Banana;
    //    static bool Prefix(Damageable __instance)
    //    {
    //        if (Singleton.Manager<ManPlayer>.inst.PlayerIndestructible)
    //        {
    //            try
    //            {
    //                if (Banana)
    //                {
    //                    return !__instance.Block.tank.IsFriendly();
    //                }
    //            }
    //            catch
    //            {
    //                return true;
    //            }
    //            return false;
    //        }
    //        return true;
    //        // return !Singleton.Manager<ManPlayer>.inst.PlayerIndestructible;
    //    }
    //}
    public class tGUI : MonoBehaviour
    {
        static public bool BlockInjector = false;
        static private Rect window = new Rect(0f, 0f, 300, 250);
        static private int SelectedBlockType = 0;
        static private bool ShowGUI = false;
        static public GameObject GUIDisp;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ShowGUI = !ShowGUI;
                GUIDisp.SetActive(ShowGUI);
            }
        }

        static private void OnGUIWindow(int ID)
        {
            try
            {
                try
                {
                    SelectedBlockType = int.Parse(GUILayout.TextField(SelectedBlockType.ToString()));
                    GUILayout.Label(Enum.GetName(typeof(BlockTypes), SelectedBlockType));
                }
                catch
                {
                }
               if (GUILayout.Button("Discover Selected Block"))
                {
                    try
                    {
                        Singleton.Manager<ManLicenses>.inst.DiscoverBlock((BlockTypes)SelectedBlockType);
                    }
                    catch
                    {
                    }
                }
                if (GUILayout.Button("Spawn Selected Block"))
                {
                    try
                    {
                        Singleton.Manager<ManSpawn>.inst.SpawnBlock((BlockTypes)SelectedBlockType, Singleton.playerTank.transform.position + new Vector3(10f, 20f, 10f), Quaternion.Euler(0f, 0f, 0f));
                    }
                    catch
                    {
                    }
                }
                if (GUILayout.Button("Discover All Blocks"))
                {
                    foreach (object obj in Enum.GetValues(typeof(BlockTypes)))
                    {
                        BlockTypes blockType = (BlockTypes)obj;
                        try
                        {
                            Singleton.Manager<ManLicenses>.inst.DiscoverBlock(blockType);
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
                                Singleton.Manager<ManLicenses>.inst.DiscoverBlock(blockType);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (GUILayout.Button("Fill Inv w/ 100 Of All"))
                {
                    foreach (object obj in Enum.GetValues(typeof(BlockTypes)))
                    {
                        BlockTypes blockType = (BlockTypes)obj;
                        try
                        {
                            Singleton.Manager<ManPlayer>.inst.PlayerInventory.HostAddItem(blockType, 100);
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
                                Singleton.Manager<ManPlayer>.inst.PlayerInventory.HostAddItem(blockType, 100);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (GUILayout.Button("Add 1m Block Bucks"))
                {
                    try
                    {
                        Singleton.Manager<ManPlayer>.inst.AddMoney(1000000);
                    }
                    catch
                    {
                    }
                }
                //try
                //{
                    
                //    Singleton.Manager<ManPlayer>.inst.PlayerIndestructible = GUILayout.Toggle(Singleton.Manager<ManPlayer>.inst.PlayerIndestructible, "Invincibility for ALL");
                //    if (Singleton.Manager<ManPlayer>.inst.PlayerIndestructible)
                //    {
                //        HarmonyPatchHacks.Banana = GUILayout.Toggle(HarmonyPatchHacks.Banana, "Only for Allies");
                //        GUILayout.Label("TURN OFF INVINCIBILITIES BEFORE SWITCHING GAMEMODES");
                //    }
                //}
                //catch
                //{
                //}
            }
            catch
            {
            }
            GUI.DragWindow();
        }
        internal class GUIDisplay : MonoBehaviour
        {
            public void OnGUI()
            {
                tGUI.window = GUI.Window(25346, window, OnGUIWindow, "Cheat Menu");
            }
        }
    }
}