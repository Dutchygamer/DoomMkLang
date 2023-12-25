namespace DoomMkLang
{
    class Program
    {
//- voor iedere .txt in een bepaalde map
// 	- open file
// 	- lees txt
// 	- iedere regel omvatten met ""
// 	- iedere enter vervangen door \n
// 	- de regel na TAG moet in SWWM_LORETAG_{filename_min_extensie_allcaps} komen
// 	- de regel na TAB moet in SWWM_LORETAB_{filename_min_extensie_allcaps} komen
// 	- de regel na REL moet in SWWM_LOREREL_{filename_min_extensie_allcaps} komen
// 	- de regel na TXT moet in SWWM_LORETXT_{filename_min_extensie_allcaps} komen
// 	- dump in locale list en pak volgende file
//- write naar language.def_lore
// 	- altijd beginnen met volgende regels
// 	[default]
// 	// always overrule this entry so we can hook up our new entries
// 	SWWM_LOREREL_DEMONINVASION = "Hell;UAC;Demolitionist;WhiteScar;Nukuri;Doomguy;Anarukon;Saya;Archdemons;Zombification;DemonicForces";
// 	- daarna per ingelezen bestand de entries wegschrijven


        static void Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("I live!");
        }
    }
}