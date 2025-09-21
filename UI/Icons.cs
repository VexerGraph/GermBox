using NeoModLoader.api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GermBox.UI
{
    public class Icons
    {
        private static List<string> iconPaths;
        private static readonly string folderPath = Path.Combine(BasicMod<ModClass>.Instance.GetDeclaration().FolderPath, "GameResources\\ui\\icons");

        public static void Init()
        {
            iconPaths = GetIconPaths();
        }

        public static string Random()
        {
            if (!iconPaths.Any()) return "tab_pathogen";

            return iconPaths[Randy.randomInt(0,iconPaths.Count)];
        }

        private static List<string> GetIconPaths() {
            List<string> paths = new List<string>();
            if (Directory.Exists(folderPath))
            {
                string[] pngFiles = Directory.GetFiles(folderPath, "*.png");
                foreach (string pngFile in pngFiles) { 
                    string fileName = Path.GetFileNameWithoutExtension(pngFile);
                    paths.Add(Path.Combine("ui/icons/", fileName));
                }
            }
            else {
                Debug.LogWarning("Directory does not exist.");
            }
            return paths;
        }
    }
}
