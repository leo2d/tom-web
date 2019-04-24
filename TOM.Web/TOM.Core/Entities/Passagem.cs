using System;

namespace TOM.Core.Entities
{
    public class Passagem : BaseEntity<Passagem>
    {
        public virtual decimal ValorPassagem { get; set; }
        public virtual DateTime DataVoo { get; set; }
        public virtual int IdVoo { get; set; }

        public virtual Voo Voo { get; set; }
    }
}
