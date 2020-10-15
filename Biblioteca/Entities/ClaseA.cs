using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Entities
{
    public class ClaseA
    {
        private readonly IClaseB claseB;

        public ClaseA(IClaseB claseB)
        {
            this.claseB = claseB;
        }

        public void RealizarAlgo()
        {
            claseB.HacerAlgo();
        }
    }

    public interface IClaseB
    {
        public void HacerAlgo();
    }

    public class ClaseB : IClaseB
    {
        public void HacerAlgo() { }
    }

    public class ClaseB2 : IClaseB
    {
        public void HacerAlgo() { }
    }
}
