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

            if (loreEntries?.Any() != true)
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
                //TODO we moeten alleen op de eerste instantie van de genoemde text splitsen, niet op alles wat erna komt!

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
                    Tag = tagText.Replace("\r\n", ""),
                    Tab = tabText.Replace("\r\n", ""),
                    RelatedTags = relText.Replace("\r\n", ""),
                    Text = txtText,
                });
            }

            return result;
        }

        private void WriteToLanguage(IEnumerable<LoreEntry> loreEntries)
        {
            // var languageFile = File.OpenWrite(_languageFileLocation);
            var languageText = string.Empty;

            // we always want to add this bit first which marks the file as a Doom language file and appends the DemomInvasion entry's tags with our own tags
            var defaultText = "[default]\r\n";
            defaultText += "// always overrule this entry so we can hook up our new entries\r\n";
            defaultText += "SWWM_LOREREL_DEMONINVASION = \"Hell;UAC;Demolitionist;WhiteScar;Nukuri;Doomguy;Anarukon;Saya;Archdemons;FormerHumans;DemonicForces\";\r\n";
            languageText += defaultText;

            // then add the text per lore entry
            foreach (var loreEntry in loreEntries)
            {
                var loreEntryText = string.Empty;

                var identifier = loreEntry.GetTagTitle();

                loreEntryText += $"SWWM_LORETAG_{identifier} = \"{loreEntry.Tag}\"\r\n";
                loreEntryText += $"SWWM_LORETAB_{identifier} = \"{loreEntry.Tab}\"\r\n";
                loreEntryText += $"SWWM_LOREREL_{identifier} = \"{loreEntry.RelatedTags}\"\r\n";

                // the actual lore text is a bit more complex
                // basicly we want to split the massive string into separate strings based on the newLine (\r\n)
                // after that we want to surround the splitted strings into "" and suffix it with a \n
                // this means that each newLine split (which includes empty lines) will get converted into a separate line in the language file
                loreEntryText += $"SWWM_LORETXT_{identifier} = \r\n";
                bool skipLine = true;
                var loreTextLines = loreEntry.Text.Split("\r\n");
                foreach (var loreTextLine in loreTextLines)
                {
                    // we want to skip the first newLine
                    if (skipLine == true && loreTextLine == "")
                    {
                        skipLine = false;
                        continue;
                    }

                    //TODO **{text}** moet je vervangen door \cf{text}\c-
                    //TODO laatste regel in entry heeft geen \n in de quotes nodig
                    
                    // each entry should end with a \n within the quotes, and a \n outside the quotes so it becomes a new line in the language file
                    loreEntryText += $"\"{loreTextLine}\\n\"\n";
                }

                languageText += loreEntryText;
            }

            // write everything to the file in the most simple way possible
            File.WriteAllText(_languageFileLocation, languageText);
        }

        private class LoreEntry
        {
            public string Tag { get; set; }
            public string GetTagTitle()
            {
               return Tag.Replace(" ", "").ToUpper(); 
            }

            public string Tab { get; set; }
            public string RelatedTags { get; set; }
            public string Text { get; set; }
        }
    }
}