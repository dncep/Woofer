﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EntityComponentSystem.Scenes;

using WooferGame.Controller.Commands;
using WooferGame.Meta.LevelEditor;
using WooferGame.Meta.LevelEditor.Systems;
using WooferGame.Meta.LevelEditor.Systems.ComponentView;
using WooferGame.Meta.LevelEditor.Systems.InputModes;

namespace WooferGame.Meta.LevelSelect
{
    class LevelSelect : Scene
    {
        public LevelSelect() : base(Woofer.Controller)
        {
            Systems.Add(new EditorInputSystem());

            Systems.Add(new TextInputSystem());
            Systems.Add(new EnumerationSelectViewSystem());

            Systems.Add(new BackToMainMenuSystem());

            Systems.Add(new ModalFocusSystem() { CurrentSystem = "back_to_menu" });
        }

        private bool Initialized = false;
        protected override void Tick()
        {
            if(!Initialized)
            {
                List<string> sceneList = new List<string>();
                foreach (string path in Directory.GetFiles(Path.Combine(Woofer.DirectoryPath, "scenes"))) {
                    FileInfo file = new FileInfo(path);
                    if (file.Extension == ".scn")
                    {
                        string plainName = file.Name;
                        plainName = plainName.Substring(0, plainName.Length - ".scn".Length);
                        sceneList.Add(plainName);
                    }
                }
                Events.InvokeEvent(new StartEnumSelectEvent("Select Scene", sceneList, l =>
                {
                    Scene scene = new WooferLoadOperation(Woofer.Controller, l, "scenes").Load();
                    Editor.AttachEditor(scene, l, "scenes");
                    Woofer.Controller.CommandFired(new SceneChangeCommand(scene));
                }, null));
                Events.InvokeEvent(new ForceModalChangeEvent("enum_select", null));
                Initialized = true;
            }
        }
    }
}