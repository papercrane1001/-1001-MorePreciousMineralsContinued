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
    public class Settings : ModSettings
    {
        public int percentIncrease = 50;

        public int valueRangeMin => 5000 * (1 + percentIncrease / 100) - 1500;
        public int valueRangeMax => 5000 * (1 + percentIncrease / 100) + 1500;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref percentIncrease, "MorePreciousMineralsContinued.percentIncrease", 50);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                percentIncrease = int.Parse(MPMCSettingsController.percentIncreaseBuffer);
                if (percentIncrease < 0) { percentIncrease = 0; }
                else if (percentIncrease >= 500) { percentIncrease = 500; }
                MPMCSettingsController.percentIncreaseBuffer = percentIncrease.ToString();
                if (DefDatabase<GenStepDef>.AllDefs.Count() > 0)
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


    public class MPMCSettingsController : Mod
    {
        public static string percentIncreaseBuffer = "100";
        public Settings settings;

        public MPMCSettingsController(ModContentPack content) : base(content)
        {
            settings = GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            //return base.SettingsCategory();
            return "MorePreciousMineralsContinued.ModName".Translate();
        }
        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            //Log.Message("MPMCPoint DoSettings_0: " + percentIncrease.ToString());
            Widgets.Label(new UnityEngine.Rect(inRect.xMin, inRect.yMin, 100, 32), "MorePreciousMineralsContinued.percentIncrease".Translate());
            percentIncreaseBuffer = Widgets.TextField(new Rect(inRect.xMin + 110, inRect.yMin, 100, 32), percentIncreaseBuffer);
            //Log.Message("Point 8: " + percentIncreaseBuffer.ToString());
            if (int.TryParse(percentIncreaseBuffer.Trim(), out int i))
            {
                if (i > 0 && i <= 500) { settings.percentIncrease = i; }
                else if (percentIncreaseBuffer.Trim() == "") { settings.percentIncrease = 0; }
                percentIncreaseBuffer = settings.percentIncrease.ToString();
            }
            //Log.Message(Settings.percentIncrease.ToString());
            GenStepDef def = DefDatabase<GenStepDef>.GetNamed("PreciousLump");
            if (def.genStep as GenStep_PreciousLump != null)
            {
                GenStep_PreciousLump gsLump = (GenStep_PreciousLump)def.genStep;
                gsLump.totalValueRange = new FloatRange(settings.valueRangeMin, settings.valueRangeMax);
            }
        }

        //public override void WriteSettings()
        //{
        //    base.WriteSettings();
        //}
    }
}
