using System;

namespace Intech.Lib.Util.Date
{
    public class Periodo
    {
        public Periodo(DateTime dataInicio, DateTime dataTermino)
        {
            this.DataInicio = dataInicio;
            this.DataTermino = dataTermino;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
    }
}