using System;
using System.Collections.Generic;
using System.Numerics;
using Matrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using Vector = System.Collections.Generic.List<double>;

namespace mef3d
{
    class Program
    {
        public static List<Matrix> localKs = new List<Matrix>();
        public static List<Vector> localbs = new List<Vector>();
        static void Main(string[] args)
        {
            
            Matrix K = new Matrix();
            Vector b = new Vector();
            Vector T = new Vector();
            Console.WriteLine("IMPLEMENTACION DEL METODO DE LOS ELEMENTOS FINITOS\n"
         + "\t- TRANSFERENCIA DE CALOR\n" + "\t- 3 DIMENSIONES\n"
         + "\t- FUNCIONES DE FORMA LINEALES\n" + "\t- PESOS DE GALERKIN\n"
         + "\t- ELEMENTOS TETRAHEDROS\n"
         + "*********************************************************************************\n\n");

            mesh m = new mesh();
            tools.leerMallayCondiciones(ref m);
            Sel.createLocalSystems(ref m,ref localKs,ref localbs);
            math_tools.zeroes(ref K,m.getSize((int)sizesE.NODES));
            math_tools.zeroes(ref b,m.getSize((int)sizesE.NODES));
            Sel.assembly(ref m,ref localKs,ref localbs,ref K,ref b);
            Sel.applyNeumann(ref m,ref b);
            Sel.applyDirichlet(ref m,ref K,ref b);
            math_tools.zeroes(ref T,b.Count);
            Sel.calculate(ref K,ref b,ref T);
            Console.Write("La respuesta es: \n");
            Sel.showVector(T);

        }
    }
}
