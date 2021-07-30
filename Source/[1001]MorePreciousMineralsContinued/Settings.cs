using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using UnityEngine;
using Verse;

using RimWorld;

namespace _1001_MorePreciousMineralsContinued
{
    class Settings : ModSettings
    {
        public static int percentIncrease = 50;

        public static int valueRangeMin { get { return 5000 * (1 + percentIncrease / 100) - 1500; } }
        public static int valueRangeMax { get { return 5000 * (1 + percentIncrease / 100) + 1500; } }

        //public static 

        public override void ExposeData()
        {
            Log.Message("Point 0");
            base.ExposeData();
            Log.Message("Point 1");
            Scribe_Values.Look(ref percentIncrease, "MorePreciousMineralsContinued.percentIncrease", 50);
            Log.Message("Point 2");
            if(Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if(percentIncrease < 0) { percentIncrease = 0; }
                else if(percentIncrease >= 200) { percentIncrease = 200; }
                SettingsController.percentIncreaseBuffer = percentIncrease.ToString();

                if(DefDatabase<GenStepDef>.AllDefs.Count() > 0)
                {
                    GenStepDef def = DefDatabase<GenStepDef>.GetNamed("PreciousLump");
                    if (def.genStep != null && def.genStep as GenStep_PreciousLump != null)
                    {
                        GenStep_PreciousLump gsLump = (GenStep_PreciousLump)def.genStep;
                        gsLump.totalValueRange = new FloatRange(valueRangeMin, valueRangeMax);
                    }
                }
                
            }
        }


    }


    public class SettingsController : Mod
    {
        public static string percentIncreaseBuffer = "50";
        public SettingsController(ModContentPack content) : base(content)
        {
            base.GetSettings<Settings>();
        }
        public override string SettingsCategory()
        {
            //return base.SettingsCategory();
            return "MorePreciousMineralsContinued.ModName".Translate();
        }
        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            Log.Message("Point3");


            //base.DoSettingsWindowContents(inRect);
            Widgets.Label(new UnityEngine.Rect(inRect.xMin, inRect.yMin, 100, 32), "MorePreciousMineralsContinued.percentIncrease".Translate());
            percentIncreaseBuffer = Widgets.TextField(new Rect(inRect.xMin + 110, inRect.yMin, 100, 32), percentIncreaseBuffer);
            if(int.TryParse(percentIncreaseBuffer.Trim(), out int i))
            {
                if(i > 0 && i <= 200) { Settings.percentIncrease = i; }
                else if(percentIncreaseBuffer.Trim() == "") { Settings.percentIncrease = 0; }
                percentIncreaseBuffer = Settings.percentIncrease.ToString();
            }
            Log.Message(Settings.percentIncrease.ToString());
            GenStepDef def = DefDatabase<GenStepDef>.GetNamed("PreciousLump");
            if (def.genStep as GenStep_PreciousLump != null)
            {
                GenStep_PreciousLump gsLump = (GenStep_PreciousLump)def.genStep;
                gsLump.totalValueRange = new FloatRange(Settings.valueRangeMin, Settings.valueRangeMax);
            }
        }
    }
}
