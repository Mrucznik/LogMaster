using System;
using System.Text.RegularExpressions;

namespace AccountRecovery
{
    public abstract class Akcja
    {
        protected DateTime Data;

        public virtual void Parsuj(GroupCollection grupy)
        {
            Data = new DateTime(int.Parse(grupy[1].Value), int.Parse(grupy[2].Value), int.Parse(grupy[3].Value),
                int.Parse(grupy[4].Value), int.Parse(grupy[5].Value), int.Parse(grupy[6].Value));
        }

        public virtual void Wyswietl()
        {
            Global.LogAction($"{Data} ");
        }
        public abstract string GenerujZapytanie();
        public virtual string GeneratePawnCode()
        {
            return null; 
        }
    }

    public class PustaAkcja : Akcja
    {
        public override void Parsuj(GroupCollection grupy)
        {
        }

        public override void Wyswietl()
        {
            Global.LogActionLine("Pusta akcja");
        }

        public override string GenerujZapytanie()
        {
            return "Puste zapytanie";
        }
    }
}
