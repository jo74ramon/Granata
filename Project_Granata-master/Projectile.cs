using System;
namespace Granata
{
    public class Projectile
    {
        public string Tipo {get;set;}
        public int[] ProjectilePosition {get;set;}
        public int Direction {get;set;}
        public int Damage {get;set;}
        public int Frames {get;set;}
        public int SplashDamage {get;set;} 
        public string Symbol {get;set;}
        

        public Projectile(string tipo, int[] projectilePosition, int direction, int damage, int frames, int splashdamage, string symbol)
        {
            this.Tipo = tipo; //tipo 1 PP, tipo 2 Granada, tipo 3 Sticky
            this.ProjectilePosition = projectilePosition;
            this.Direction = direction;
            this.Damage = damage;
            this.Frames = frames;
            this.SplashDamage = splashdamage;
            this.Symbol = symbol;

        }
    }
}
