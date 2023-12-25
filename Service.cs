using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DoomMkLang
{
    public class Service
    {
        private readonly string _loreFolderLocation = "E:\\Werkmap\\Gamezzz Map\\FPS\\Doom\\Original Doom\\_Files\\Mods\\_Addons\\DoomKrakken's Monster Randomizer\\git\\lore\\default";
        private readonly string _languageFileLocation = "E:\\Werkmap\\Gamezzz Map\\FPS\\Doom\\Original Doom\\_Files\\Mods\\_Addons\\DoomKrakken's Monster Randomizer\\git\\language.def_lore";

        public void DoTheThings()
        {
            var loreEntries = ReadLoreEntries();

            if(loreEntries?.Any() != true)
            {
                return;
            }

            WriteToLanguage(loreEntries);
        }


        private IEnumerable<LoreEntry> ReadLoreEntries()
        {
            var result = new List<LoreEntry>();

            var directory = new DirectoryInfo(_loreFolderLocation);
            var loreFiles = directory.GetFiles();
            foreach (var loreFile in loreFiles)
            {
                var rawText = File.ReadAllText(loreFile.FullName);

                //TODO dit zou een regex ding kunnen zijn?

                //var eugh = rawText.Split(new string[] { "TAG, TAB, REL, TXT" }, StringSplitOptions.None);

                // split the raw text according to the 4 main parts
                // for the first three parts we will split the text again with the next main part so we only have what we need
                var tagSplit = rawText.Split("TAG", StringSplitOptions.None);
                var tagTextRaw = tagSplit[1];
                var tagTextRawSplit = tagTextRaw.Split("TAB", StringSplitOptions.None);
                var tagText = tagTextRawSplit[0];

                var tabSplit = rawText.Split("TAB", StringSplitOptions.None);
                var tabTextRaw = tabSplit[1];
                var tabTextRawSplit = tabTextRaw.Split("REL", StringSplitOptions.None);
                var tabText = tabTextRawSplit[0];

                var relSplit = rawText.Split("REL", StringSplitOptions.None);
                var relTextRaw = relSplit[1];
                var relTextRawSplit = relTextRaw.Split("TXT", StringSplitOptions.None);
                var relText = relTextRawSplit[0];

                var txtSplit = rawText.Split("TXT", StringSplitOptions.None);
                var txtText = txtSplit[1];

                result.Add(new LoreEntry
                {
                    Tag = tagText,
                    Tab = tabText,
                    RelatedTags = relText,
                    Text = txtText,
                });

                //TODO dis niet nodig, je moet object maken en dan fixen we het bij writen wel :)
                // tagText = "TAG\r\n" + tagText;
                // tabText = "TAB\r\n" + tabText;
                // relText = "REL\r\n" + relText;
                // txtText = "TXT\r\n" + txtText;


                // var shizzle = "";
            }

            return result;
        }

        private void WriteToLanguage(IEnumerable<LoreEntry> loreEntries)
        {
            // var languageFile = File.Open(_languageFileLocation);
        }

        public class LoreEntry
        {
            public string Tag {get; set;}
            public string Tab {get; set;}
            public string RelatedTags {get; set;}
            public string Text {get; set;}
        }
    }
}