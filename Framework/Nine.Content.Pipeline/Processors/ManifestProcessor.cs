﻿#region File Description
//-----------------------------------------------------------------------------
// ManifestPipeline.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Nine.Content.Pipeline.Processors
{
    // processor takes in a filename and returns a list of files in the content project being built or
    // copied to the output directory
    [ContentProcessor(DisplayName = "Manifest Processor - XNA Framework")]
    public class ManifestProcessor : ContentProcessor<string, string[]>
    {
        public override string[] Process(string input, ContentProcessorContext context)
        {
            string contentProject;
            string baseDirectory;
            FindContentProject(input, out contentProject, out baseDirectory);

            context.Logger.LogImportantMessage(string.Format("Processing manifest {0} at {1}", contentProject, baseDirectory));

            // we add a dependency on the content project itself to ensure our manifest is
            // rebuilt anytime the content project is modified
            context.AddDependency(contentProject);

            // create a list which we will fill with all the files being copied or built.
            // these will all be relative to the content project's root directory. built
            // content will not have an extension whereas copied content will maintain
            // its extension for loading.
            List<string> files = new List<string>();

            // we can now open up the content project for parsing which will allow us to
            // see what files are being built or copied
            XDocument document = XDocument.Load(contentProject);

            // we need the xmlns for us to find nodes in the document
            XNamespace xmlns = document.Root.Attribute("xmlns").Value;

            // we need the content root directory from the file to know where copied files will end up
            string contentRootDirectory = document.Descendants(xmlns + "ContentRootDirectory").First().Value;

            // first find all assets that are set to compile into XNB files
            var compiledAssets = document.Descendants(xmlns + "Compile");
            foreach (var asset in compiledAssets)
            {
                // get the include path and name
                string includePath = asset.Attribute("Include").Value;
                string name = asset.Descendants(xmlns + "Name").First().Value;
                var link = asset.Descendants(xmlns + "Link").FirstOrDefault();
                if (link != null)
                    includePath = link.Value;

                // if the include path is a manifest, skip it
                if (includePath.EndsWith(".manifest"))
                    continue;

                // if the file is not in the base directory, skip it
                includePath = includePath.Replace('/', '\\');
                if (!includePath.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
                    continue;

                // combine the two into the asset path if the include path
                // has a directory. otherwise we just use the name.
                if (includePath.Contains('\\'))
                {
                    string dir = includePath.Substring(0, includePath.LastIndexOf('\\'));
                    string assetPath = Path.Combine(dir, name);
                    files.Add(assetPath);
                }
                else
                {
                    files.Add(name);
                }
            }

            // next we find all assets that are set to copy to the output directory. we are going
            // to leverage LINQ to do this for us. this is the logic employed:
            //  1) we select all nodes that are children of an ItemGroup.
            //  2) from that set we find nodes that have a CopyToOutputDirectory node and make sure it is not set to None
            //  3) we then select that node's Include attribute as that is the value we want. we must also prepend
            //     the output directory to make the file path relative to the game instead of the content.
            var copiedAssetFiles = from node in document.Descendants(xmlns + "ItemGroup").Descendants()
                                   where node.Descendants(xmlns + "CopyToOutputDirectory").Count() > 0 &&
                                         node.Descendants(xmlns + "CopyToOutputDirectory").First().Value != "None"
                                   select Path.Combine(contentRootDirectory, node.Attribute("Include").Value);

            // we can now just add all of those files to our list
            files.AddRange(copiedAssetFiles);

            // lastly we want to override the manifest file with this list. this allows us to 
            // easily see what files were included in the build for debugging.
            using (FileStream fileStream = new FileStream(input, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    // now write all the files into the manifest
                    foreach (var file in files)
                    {
                        writer.WriteLine(file);
                    }
                }
            }

            // just return the list which will be automatically serialized for us without
            // needing a ContentTypeWriter like we would have needed pre- XNA GS 3.1
            return files.ToArray();
        }

        private static void FindContentProject(string input, out string contentProject, out string baseDirectory)
        {
            baseDirectory = "";
            string contentDirectory = input.Replace('/', '\\');
            contentDirectory = contentDirectory.Substring(0, contentDirectory.LastIndexOf('\\'));

            while (true)
            {
                var lastFolderSeperator = contentDirectory.LastIndexOf('\\');
                if (lastFolderSeperator < 0)
                    throw new InvalidOperationException("Could not locate content project.");

                baseDirectory = Path.Combine(contentDirectory.Substring(lastFolderSeperator, contentDirectory.Length - lastFolderSeperator), baseDirectory);

                contentDirectory = contentDirectory.Substring(0, lastFolderSeperator);
                string[] contentProjects = Directory.GetFiles(contentDirectory, "*.contentproj");

                if (contentProjects.Length == 0)
                    continue;
                if (contentProjects.Length != 1)
                    throw new InvalidOperationException("Multiple content project found.");

                contentProject = contentProjects[0];
                break;
            }

            baseDirectory = baseDirectory.Replace("\\", "");
            baseDirectory += "\\";
        }
    }
}
