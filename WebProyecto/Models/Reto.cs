//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebProyecto.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reto
    {
        public int id_equipo1 { get; set; }
        public int id_equipo2 { get; set; }
        public int id_cancha { get; set; }
        public System.DateTime fecha { get; set; }
        public System.TimeSpan horaInicio { get; set; }
        public System.TimeSpan horaFinal { get; set; }
        public Nullable<int> precio { get; set; }
        public string resultado { get; set; }
        public string ganador { get; set; }
    
        public virtual Cancha Cancha { get; set; }
        public virtual Equipos Equipos { get; set; }
        public virtual Equipos Equipos1 { get; set; }
    }
}
