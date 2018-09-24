using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SinglePlayerCheats
{
    public class Class1
    {
        public static void Patch()
        {
            new GameObject().AddComponent<tGUI>();
        }
    }
    public class tGUI : MonoBehaviour
    {
        Rect window = new Rect(0f, 0f, 300, 75);
        int SelectedBlockType = 0;
        bool ShowGUI = false;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                ShowGUI = !ShowGUI;
            }
        }
        void OnGUI()
        {
            if (ShowGUI && !Singleton.Manager<ManNetwork>.inst.IsMultiplayer())
            {
                window = GUI.Window(25346, window, OnGUIWindow, "Cheat Menu");
            }
        }
        void OnGUIWindow(int ID)
        {
            if (!ShowGUI)
            {
                return;
            }
            try
            {
                if (GUI.Button(new Rect(0, 15, 100f, 20f), "Fill Inv"))
                {
                    foreach (object obj in Enum.GetValues(typeof(BlockTypes)))
                    {
                        BlockTypes blockType = (BlockTypes)obj;
                        try
                        {
                            Singleton.Manager<ManPlayer>.inst.PlayerInventory.AddBlocks(blockType, 256);
                        }
                        catch
                        {
                        }
                    }
                }
                try
                {
                    SelectedBlockType = int.Parse(GUI.TextField(new Rect(60f, 35f, 40f, 20f), this.SelectedBlockType.ToString()));
                    GUI.Label(new Rect(100f, 35f, 200f, 20f), Enum.GetName(typeof(BlockTypes), this.SelectedBlockType));
                }
                catch
                {
                }
                if (GUI.Button(new Rect(0f, 35f, 60f, 20f), "Spawn"))
                {
                    try
                    {
                        Singleton.Manager<ManSpawn>.inst.SpawnBlock((BlockTypes)this.SelectedBlockType, Singleton.playerTank.transform.position + new Vector3(10f, 20f, 10f), Quaternion.Euler(0f, 0f, 0f));
                    }
                    catch
                    {
                    }
                }
                if (GUI.Button(new Rect(0f, 55f, 200f, 20f), "Add .5 (mil) money"))
                {
                    try
                    {
                        Singleton.Manager<ManPlayer>.inst.AddMoney(500000);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            GUI.DragWindow();
        }
    }
}
