using System;
using System.Collections.Generic;
using System.Text;

using Vector = System.Collections.Generic.List<double>;
using Matrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;


namespace mef3d
{
    public static class math_tools
    {
        public static void zeroes(ref Matrix M, int n)
        {
            //Se crean n filas
            for (int i = 0; i < n; i++)
            {
                //Se crea una fila de n ceros
                Vector auxrow = new Vector();

                for (int j = 0; j < n; j++)
                {
                    auxrow.Add(0.0);
                }

                //Se ingresa la fila en la matriz
                M.Add(auxrow);
            }
        }
        public static void zeroes(ref Matrix M, int n, int m)
        {
            //Se itera n veces
            //Se crean n filas
            for (int i = 0; i < n; i++)
            {
                //Se crea una fila de n ceros
                Vector auxrow = new Vector();

                for (int j = 0; j < m; j++)
                {
                    auxrow.Add(0.0);
                }

                //Se ingresa la fila en la matriz
                M.Add(auxrow);
            }
        }

        public static void zeroes(ref Vector v, int n)
        {
            //Se itera n veces
            for (int i = 0; i < n; i++)
            {
                //En cada iteración se agrega un cero al vector
                v.Add(0.0);
            }
        }

        public static void copyMatrix(Matrix A, ref Matrix copy)
        {
            //Se inicializa la copia con ceros
            //asegurándose de sus dimensiones
            zeroes(ref copy, A.Count);
            //Se recorre la matriz original
            for (int i = 0; i < A.Count; i++)
                for (int j = 0; j < A[0].Count; j++)
                    //Se coloca la celda actual de la matriz original
                    //en la misma posición dentro de la copia
                    copy[i][j] = A[i][j];
        }

        public static double calculateMember(int i, int j, int r, Matrix A, Matrix B)
        {
            double member = 0;
            for (int k = 0; k < r; k++)
                member += A[i][k] * B[k][j];
            return member;
        }

        public static Matrix productMatrixMatrix(Matrix A, Matrix B, int n, int r, int m)
        {
            Matrix R = new Matrix();

            math_tools.zeroes(ref R,n,m);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    R[i][j] = calculateMember(i, j, r, A, B);

            return R;
        }

        public static void productMatrixVector(Matrix A, Vector v, ref Vector R)
        {
            for (int f = 0; f < A.Count; f++)
            {
                double cell = 0.0;
                for (int c = 0; c < v.Count; c++)
                {
                    cell += A[f][c] * v[c];
                }
                R[f] += cell;
            }
        }

        public static void productRealMatrix(double real, Matrix M, ref Matrix R)
        {
            math_tools.zeroes(ref R, M.Count);
            for (int i = 0; i < M.Count; i++)
                for (int j = 0; j < M[0].Count; j++)
                    R[i][j] = real * M[i][j];
        }

        public static void getMinor(ref Matrix M, int i, int j)
        {
            //Se elimina la fila i
            M.RemoveAt(i);
            //Se recorren las filas restantes
            for (int h = 0; h < M.Count; h++)
                //En cada fila se elimina la columna j
                M[h].RemoveAt(j);
        }

        public static double determinant(Matrix M)
        {
            //Caso trivial: si la matriz solo tiene una celda, ese valor es el determinante

            if (M.Count == 1)
            {
                return M[0][0];
            }
            else
            {
                //Se implementa la siguiente formulación del siguiente enlace:
                //(Entrar con cuenta UCA)
                //              https://goo.gl/kbWdmu

                //Se inicia un acumulador
                double det = 0.0;
                //Se recorre la primera fila
                for (int i = 0; i < M[0].Count; i++)
                {
                    //Se obtiene el menor de la posición actual
                    Matrix minor = new Matrix();
                    copyMatrix(M, ref minor);
                    getMinor(ref minor, 0, i);

                    //Se calculala contribución de la celda actual al determinante
                    //(valor alternante * celda actual * determinante de menor actual)
                    double aux = M[0][i];
                    det += Math.Pow(-1, i) * aux * determinant(minor);
                }
                return det;
            }
        }

        public static void cofactors(Matrix M, ref Matrix Cof)
        {
            //La matriz de cofactores se define así:
            //(Entrar con cuenta UCA)
            //          https://goo.gl/QK7BZo

            //Se prepara la matriz de cofactores para que sea de las mismas
            //dimensiones de la matriz original
            zeroes(ref Cof, M.Count);
            //Se recorre la matriz original
            for (int i = 0; i < M.Count; i++)
            {
                for (int j = 0; j < M[0].Count; j++)
                {
                    //Se obtiene el menor de la posición actual
                    Matrix minor = new Matrix();
                    copyMatrix(M, ref minor);
                    getMinor(ref minor, i, j);
                    //Se calcula el cofactor de la posición actual
                    //      alternante * determinante del menor de la posición actual
                    Cof[i][j] = Math.Pow(-1, i + j) * determinant(minor);
                }
            }
        }

        public static void transpose(Matrix M, ref Matrix T)
        {
            //Se prepara la matriz resultante con las mismas dimensiones
            //de la matriz original
            zeroes(ref T, M[0].Count, M.Count);
            //Se recorre la matriz original
            for (int i = 0; i < M.Count; i++)
                for (int j = 0; j < M[0].Count; j++)
                {
                    //La posición actual se almacena en la posición con índices
                    //invertidos de la matriz resultante
                    double aux = M[i][j];
                    T[j][i] = aux;
                }
        }

        public static void inverseMatrix(Matrix M, ref Matrix Minv)
        {
            //Se utiliza la siguiente fórmula:
            //      (M^-1) = (1/determinant(M))*Adjunta(M)
            //             Adjunta(M) = transpose(Cofactors(M))

            //Se preparan las matrices para la de cofactores y la adjunta
            Matrix Cof = new Matrix();
            Matrix Adj = new Matrix();
            //Se calcula el determinante de la matriz
            double det = determinant(M);
            //Si el determinante es 0, se aborta el programa
            //No puede dividirse entre 0 (matriz no invertible)
            if (det == 0)
            {
                Console.WriteLine("Determinante Cero");
                Environment.Exit(1);
            }

            //Se calcula la matriz de cofactores
            cofactors(M, ref Cof);
            //Se calcula la matriz adjunta
            transpose(Cof, ref Adj);
            //Se aplica la fórmula para la matriz inversa
            productRealMatrix(1 / det, Adj, ref Minv);
        }
    }
}
