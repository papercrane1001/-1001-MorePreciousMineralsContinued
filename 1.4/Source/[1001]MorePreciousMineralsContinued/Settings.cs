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


        public override void ExposeData()
        {
            //Log.Message("Point 0");
            base.ExposeData();
            //Log.Message("Point 1: " + percentIncrease.ToString());
            Scribe_Values.Look(ref percentIncrease, "MorePreciousMineralsContinued.percentIncrease", 50);
            //Log.Message("Point 2: " + percentIncrease.ToString());
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                //Log.Message("Point 3");
                if(percentIncrease < 0) { percentIncrease = 0; }
                else if(percentIncrease >= 500) { percentIncrease = 500; }
                //Log.Message("Point 4: " + percentIncrease.ToString());
                SettingsController.percentIncreaseBuffer = percentIncrease.ToString();
                //Log.Message("Point 5: " + percentIncrease.ToString());
                if (DefDatabase<GenStepDef>.AllDefs.Count() > 0)
                {
                    GenStepDef def = DefDatabase<GenStepDef>.GetNamed("PreciousLump");
                    if (def.genStep != null && def.genStep as GenStep_PreciousLump != null)
                    {
                        GenStep_PreciousLump gsLump = (GenStep_PreciousLump)def.genStep;
                        gsLump.totalValueRange = new FloatRange(valueRangeMin, valueRangeMax);
                        //Log.Message("Point 6: " + percentIncrease.ToString());
                    }
                }
                
            }
        }


    }


    public class SettingsController : Mod
    {
        public static string percentIncreaseBuffer = "100";
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
            //Log.Message("Point3");
            //Log.Message("Point 7: " + Settings.percentIncrease.ToString());
            Widgets.Label(new UnityEngine.Rect(inRect.xMin, inRect.yMin, 100, 32), "MorePreciousMineralsContinued.percentIncrease".Translate());
            percentIncreaseBuffer = Widgets.TextField(new Rect(inRect.xMin + 110, inRect.yMin, 100, 32), percentIncreaseBuffer);
            //Log.Message("Point 8: " + percentIncreaseBuffer.ToString());
            if (int.TryParse(percentIncreaseBuffer.Trim(), out int i))
            {
                if(i > 0 && i <= 500) { Settings.percentIncrease = i; }
                else if(percentIncreaseBuffer.Trim() == "") { Settings.percentIncrease = 0; }
                percentIncreaseBuffer = Settings.percentIncrease.ToString();
            }
            //Log.Message(Settings.percentIncrease.ToString());
            GenStepDef def = DefDatabase<GenStepDef>.GetNamed("PreciousLump");
            if (def.genStep as GenStep_PreciousLump != null)
            {
                GenStep_PreciousLump gsLump = (GenStep_PreciousLump)def.genStep;
                gsLump.totalValueRange = new FloatRange(Settings.valueRangeMin, Settings.valueRangeMax);
            }
        }
    }
}
