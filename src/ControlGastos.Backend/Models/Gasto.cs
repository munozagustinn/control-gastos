using System;
using System.Runtime.Serialization;

namespace ControlGastos.Backend.Models
{
    [DataContract]
    public class Gasto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Categoria { get; set; }

        [DataMember]
        public decimal Importe { get; set; }
    }
}
