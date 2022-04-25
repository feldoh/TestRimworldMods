using RimWorld;
using Verse;

namespace PlagueGun
{
    public class Projectile_PlagueBullet: Bullet
    {
        #region Properties
        public ThingDef_PlagueBullet Def
        {
            get
            {
                return this.def as ThingDef_PlagueBullet;
            }
        }
        #endregion Properties

        #region overrides

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                var rand = Rand.Value; // This is a random percentage between 0% and 100%
                if (rand <= Def.AddHediffChance) // If the percentage falls under the chance, success!
                {
                    /*
                     * Messages.Message flashes a message on the top of the screen. 
                     * You may be familiar with this one when a colonist dies, because
                     * it makes a negative sound and mentions "So and so has died of _____".
                     * 
                     * Here, we're using the "Translate" function. More on that later in
                     * the localization section.
                     */
                    Messages.Message("DEX_PlagueBullet_SuccessMessage"
                        .Translate(this.launcher.Label, hitPawn.Label), MessageTypeDefOf.NeutralEvent);
                    
                    //This checks to see if the character has a health differential, or hediff on them already.
                    var plagueOnPawn = hitPawn?.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                    var randomSeverity = Rand.Range(0.15f, 0.30f);
                    if (plagueOnPawn != null)
                    {
                        //If they already have plague, add a random range to its severity.
                        //If severity reaches 1.0f, or 100%, plague kills the target.
                        plagueOnPawn.Severity += randomSeverity;
                    }
                    else
                    {
                        //These three lines create a new health differential or Hediff,
                        //put them on the character, and increase its severity by a random amount.
                        Hediff hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn, null);
                        hediff.Severity = randomSeverity;
                        hitPawn.health.AddHediff(hediff, null, null);
                    }
                }
                else //failure!
                {
                    /*
                     * Motes handle all the smaller visual effects in RimWorld.
                     * Dust plumes, symbol bubbles, and text messages floating next to characters.
                     * This mote makes a small text message next to the character.
                     */
                    MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "TST_PlagueBullet_FailureMote".Translate(Def.AddHediffChance), 12f);
                }
            }
        }

        #endregion overrides
    }
}