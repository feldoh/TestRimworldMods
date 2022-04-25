using System;
using RimWorld;
using Verse;

namespace PlagueGun
{
    public class ThingDef_PlagueBullet: ThingDef 
    {
        public float AddHediffChance = 0.05f; //The default chance of adding a hediff.
        public HediffDef HediffToAdd;
        
        public override void ResolveReferences()
        {
            HediffToAdd = HediffDefOf.Plague;
        }
    }
    
}