using System;
using System.Collections.Generic;
using System.Text;

using Vector = System.Collections.Generic.List<double>;
//Se define un arreglo de vectores como una matriz
using Matrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace mef3d
{
    public static class Sel
    {
        public static void showMatrix(Matrix K)
        {
            for (int i = 0; i < K.Count; i++)
            {
                Console.Write("[\t");
                for (int j = 0; j < K[i].Count; j++)
                {
                    Console.Write(Math.Round(K[i][j], 2) + "\t");
                }
                Console.Write("]\n");
            }
        }

        public static void showKs(List<Matrix> Ks)
        {
            for (int i = 0; i < Ks.Count; i++)
            {
                Console.Write("K del elemento " + i + 1 + ":\n");
                showMatrix(Ks[i]);
                Console.Write("*************************************\n");
            }
        }

        public static void showVector(Vector b)
        {
            Console.Write("[\t");
            for (int i = 0; i < b.Count; i++)
            {
                Console.Write(Math.Round(b[i], 2) + "\t");
            }
            Console.Write("]\n");
        }

        public static void showbs(List<Vector> bs)
        {
            for (int i = 0; i < bs.Count; i++)
            {
                Console.Write("b del elemento " + i + 1 + ":\n");
                showVector(bs[i]);
                Console.Write("*************************************\n");
            }
        }

        public static double calculateLocalJaco(int ind, ref mesh m)
        {
            double J, a, b, c, d, e, f, g, h, i;
            element el = m.getElement(ind);

            node n1 = m.getNode(el.getNode1() - 1);
            node n2 = m.getNode(el.getNode2() - 1);
            node n3 = m.getNode(el.getNode3() - 1);
            node n4 = m.getNode(el.getNode4() - 1);

            a = n2.getX() - n1.getX(); b = n3.getX() - n1.getX(); c = n4.getX() - n1.getX();
            d = n2.getY() - n1.getY(); e = n3.getY() - n1.getY(); f = n4.getY() - n1.getY();
            g = n2.getZ() - n1.getZ(); h = n3.getZ() - n1.getZ(); i = n4.getZ() - n1.getZ();

            J = a * e * i + d * h * c + g * b * f - g * e * c - a * h * f - d * b * i;

            return J;
        }

        public static double calculateLocalC1(int ind, ref mesh m)
        {
            double c1;
            element el = m.getElement(ind);
            node n1 = m.getNode(el.getNode1() - 1);
            node n2 = m.getNode(el.getNode2() - 1);

            c1 = 1 / Math.Pow(n2.getX() - n1.getX(), 2);
            return c1;
        }

        public static double calculateLocalC2(int ind, ref mesh m)
        {
            double c2;
            element el = m.getElement(ind);
            node n1 = m.getNode(el.getNode1() - 1);
            node n2 = m.getNode(el.getNode2() - 1);
            node n8 = m.getNode(el.getNode8() - 1);

            c2 = (1 / (n2.getX() - n1.getX())) * (4 * n1.getX() + 4 * n2.getX() + 8 * n8.getX());
            return c2;
        }

        public static double calculateLocalElementA(int i, ref mesh m)
        {
            double A;
            double c2 = calculateLocalC2(i,ref m);
            double c1 = calculateLocalC1(i, ref m);
            A = -(1 / (192 * Math.Pow(c2, 2))) * Math.Pow(4 * c1 - c2, 4) - (1 / 24 * c2) * Math.Pow(4 * c1 - c2, 3)
                - (1 / (3840 * Math.Pow(c2, 3))) * Math.Pow(4 * c1 - c2, 5) + (1 / (3840 * Math.Pow(c2, 3))) * Math.Pow(4 * c1 + 3 * c2, 5);
            return A;
        }

        public static double calculateLocalElementB(int i, ref mesh m)
        {
            double B;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            B = -(1 / (192 * Math.Pow(c2, 2))) * Math.Pow(4 * c1 + c2, 4) + (1 / 24 * c2) * Math.Pow(4 * c1 + c2, 3)
                + (1 / (3840 * Math.Pow(c2, 3))) * Math.Pow(4 * c1 + c2, 5) - (1 / (3840 * Math.Pow(c2, 3))) * Math.Pow(4 * c1 - 3 * c2, 5);
            return B;
        }

        public static double calculateLocalElementC(int i, ref mesh m)
        {
            double C;
            double c2 = calculateLocalC2(i, ref m);
            C = (4 / 15) * Math.Pow(c2, 2);
            return C;
        }

        public static double calculateLocalElementD(int i, ref mesh m)
        {
            double D;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            D = (1 / (192 * Math.Pow(c2, 2))) * Math.Pow(4 * c2 - c1, 4)
                - (1 / (3840 * Math.Pow(c2, 3))) * Math.Pow(4 * c2 + c1, 5)
                + (1 / (7680 * Math.Pow(c2, 2))) * Math.Pow(4 * c2 + 8 * c1, 5)
                - (7 / (7680 * Math.Pow(c2, 2))) * Math.Pow(4 * c2 - 8 * c1, 5)
                + (1 / (768 * Math.Pow(c2, 2))) * Math.Pow(-8 * c1, 5)
                - (c1 / (96 * Math.Pow(c2, 3))) * Math.Pow(4 * c2 - 8 * c1, 4)
                + ((2 * c1 - 1) / (192 * Math.Pow(c2, 3))) * Math.Pow(-8 * c1, 4);
            return D;
        }

        public static double calculateLocalElementE(int i, ref mesh m)
        {
            double E;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            E = (8 / 3) * Math.Pow(c1, 2)+(1/30)*Math.Pow(c2,2);
            return E;
        }

        public static double calculateLocalElementF(int i, ref mesh m)
        {
            double F;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            F = (2 / 3) * c1*c2 + (1 / 30) * Math.Pow(c2, 2);
            return F;
        }

        public static double calculateLocalElementG(int i, ref mesh m)
        {
            double G;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            G = -(16/ 3) * Math.Pow(c1, 2) - (4 / 3) * c1 * c2 - (16 / 3) * Math.Pow(c2, 2);
            return G;
        }

        public static double calculateLocalElementH(int i, ref mesh m)
        {
            double H;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            H = (2 / 3) * Math.Pow(c1, 2) - (4 / 3) * c1 * c2 - (16 / 3) * Math.Pow(c2, 2);
            return H;
        }

        public static double calculateLocalElementI(int i, ref mesh m)
        {
            double I;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            I = -(16 / 3) * Math.Pow(c1, 2) - (2 / 3) * Math.Pow(c2, 2);
            return I;
        }

        public static double calculateLocalElementJ(int i, ref mesh m)
        {
            double J;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            J = (2 / 15) * Math.Pow(c2, 2);
            return J;
        }
        public static double calculateLocalElementK(int i, ref mesh m)
        {
            double K;
            double c2 = calculateLocalC2(i, ref m);
            double c1 = calculateLocalC1(i, ref m);
            K = (2 / 15) * Math.Pow(c2, 2);
            return K;
        }

        public static Matrix calculateLocalU(int element, ref mesh m)
        {
            Matrix u=new Matrix();
            double A = calculateLocalElementA(element, ref m);
            double B = calculateLocalElementB(element, ref m);
            double C = calculateLocalElementC(element, ref m);
            double D = calculateLocalElementD(element, ref m);
            double E = calculateLocalElementE(element, ref m);
            double F = calculateLocalElementF(element, ref m);
            double G = calculateLocalElementG(element, ref m);
            double H = calculateLocalElementH(element, ref m);
            double I = calculateLocalElementI(element, ref m);
            double J = calculateLocalElementJ(element, ref m);
            double K = calculateLocalElementK(element, ref m);
            math_tools.zeroes(ref u, 10);

            u[0][0] = A; u[0][1] = E; u[0][4] = -F; u[0][6] = -F; u[0][7] = G; u[0][8] = F; u[0][9] = F;
            u[1][0] = E; u[1][1] = B; u[1][4] = -H; u[1][6] = -H; u[1][7] = I; u[1][8] = H; u[1][9] = H;
            u[4][0] =-F; u[4][1] = -H; u[4][4] = C; u[4][6] = J; u[4][7] = -K; u[4][8] = -C; u[4][9] = -J;
            u[6][0] = -F; u[6][1] = -H; u[6][4] = J; u[6][6] = C; u[6][7] = -K; u[6][8] = -J; u[6][9] = -C;
            u[7][0] = G; u[7][1] = I; u[7][4] = -K; u[7][6] = -K; u[7][7] = D; u[7][8] = K; u[7][9] = K;
            u[8][0] = F; u[8][1] = H; u[8][4] = -C; u[8][6] = -J; u[8][7] = K; u[8][8] = C; u[8][9] = J;
            u[9][0] = F; u[9][1] = H; u[9][4] = -J; u[9][6] = -C; u[9][7] = K; u[9][8] = J; u[9][9] = C;


            return u;
        }

        public static void samparUenK(ref Matrix MatrixA,ref Matrix MatrixB,int indr, int indc)
        {
            for(int i = 0; i < MatrixA.Count; i++)
            {
                for(int j = 0; j < MatrixB.Count; j++)
                {
                    MatrixA[indr + i][indc + j] = MatrixB[i][j];
                }
            }
        }

        public static void samparUenK(ref Matrix MatrixA, ref Vector VectorB, int indr, int indc)
        {
            for (int i = 0; i < MatrixA.Count; i++)
            {
                for (int j = 0; j < VectorB.Count; j++)
                {
                    MatrixA[indr + i][indc + j] = VectorB[i];
                }
            }
        }


        public static Matrix calculateLocalK(int element,ref mesh m)
        {
            Matrix K = new Matrix();
            
            double EI = m.getParameter(0);
            element el = m.getElement(element);
            Matrix matrixAux = new Matrix();
            Matrix u = new Matrix();
            double J = calculateLocalJaco(element, ref m);
            u = calculateLocalU(element, ref m);

            math_tools.zeroes(ref matrixAux, 30);
            samparUenK(ref matrixAux,ref u, 0,0);
            samparUenK(ref matrixAux, ref u, 10, 10);
            samparUenK(ref matrixAux, ref u, 10, 10);

            math_tools.productRealMatrix(EI * J, matrixAux, ref K);

            return K;
        }

        public static Vector calculatelocalT()
        {
            Vector t = new Vector();
            math_tools.zeroes(ref t, 10);
            t[0] = 59;
            t[1] = -1;
            t[2] = -1;
            t[3] = -1;
            t[4] = 4;
            t[5] = 4;
            t[6] = 4;
            t[7] = 4;
            t[8] = 4;
            t[9] = 4;
            return t;
        }

        public static Vector createlocalb(int element, ref mesh m)
        {
            Vector b = new Vector();
            Vector f = new Vector();
            f.Add(m.getParameter((int)parametersE.FUNCTION_VECTORX));
            f.Add(m.getParameter((int)parametersE.FUNCTION_VECTORY));
            f.Add(m.getParameter((int)parametersE.FUNCTION_VECTORZ));
            double J = calculateLocalJaco(element, ref m);
            Matrix aux = new Matrix();
            math_tools.zeroes(ref aux, 30, 3);
            Vector T = calculatelocalT();
            samparUenK(ref aux, ref T, 0, 0);
            samparUenK(ref aux, ref T, 10, 1);
            samparUenK(ref aux, ref T, 10, 2);
            Matrix res = new Matrix();
            math_tools.productRealMatrix(J / 120, aux,ref res);
            math_tools.productMatrixVector(res, f,ref b);

            return b;
        }

        public static void createLocalSystems(ref mesh m,ref List<Matrix> localKs,ref List<Vector> localbs )
        {
            for(int i = 0; i < m.getSize((int)sizesE.ELEMENTS);i++)
            {
                localKs.Add(calculateLocalK(i, ref m));
                localbs.Add(createlocalb(i, ref m));
            }
        }

        public static void assemblyK(element e, Matrix localK, ref Matrix K)
        {
            int index1 = e.getNode1() - 1;
            int index2 = e.getNode2() - 1;
            int index3 = e.getNode3() - 1;
            int index4 = e.getNode4() - 1;
            int index5 = e.getNode5() - 1;
            int index6 = e.getNode6() - 1;
            int index7 = e.getNode7() - 1;
            int index8 = e.getNode8() - 1;
            int index9 = e.getNode9() - 1;
            int index10 = e.getNode10() - 1;


            K[index1][index1] += localK[0][0];
            K[index1][index2] += localK[0][1];
            K[index1][index3] += localK[0][2];
            K[index1][index4] += localK[0][3];
            K[index1][index5] += localK[0][4];
            K[index1][index6] += localK[0][5];
            K[index1][index7] += localK[0][6];
            K[index1][index8] += localK[0][7];
            K[index1][index9] += localK[0][8];
            K[index1][index10] += localK[0][9];

            K[index2][index1] += localK[1][0];
            K[index2][index2] += localK[1][1];
            K[index2][index3] += localK[1][2];
            K[index2][index4] += localK[1][3];
            K[index2][index5] += localK[1][4];
            K[index2][index6] += localK[1][5];
            K[index2][index7] += localK[1][6];
            K[index2][index8] += localK[1][7];
            K[index2][index9] += localK[1][8];
            K[index2][index10] += localK[1][9];

            K[index3][index1] += localK[2][0];
            K[index3][index2] += localK[2][1];
            K[index3][index3] += localK[2][2];
            K[index3][index4] += localK[2][3];
            K[index3][index5] += localK[2][4];
            K[index3][index6] += localK[2][5];
            K[index3][index7] += localK[2][6];
            K[index3][index8] += localK[2][7];
            K[index3][index9] += localK[2][8];
            K[index3][index10] += localK[2][9];

            K[index4][index1] += localK[3][0];
            K[index4][index2] += localK[3][1];
            K[index4][index3] += localK[3][2];
            K[index4][index4] += localK[3][3];
            K[index4][index5] += localK[3][4];
            K[index4][index6] += localK[3][5];
            K[index4][index7] += localK[3][6];
            K[index4][index8] += localK[3][7];
            K[index4][index9] += localK[3][8];
            K[index4][index10] += localK[3][9];

            K[index5][index1] += localK[4][0];
            K[index5][index2] += localK[4][1];
            K[index5][index3] += localK[4][2];
            K[index5][index4] += localK[4][3];
            K[index5][index5] += localK[4][4];
            K[index5][index6] += localK[4][5];
            K[index5][index7] += localK[4][6];
            K[index5][index8] += localK[4][7];
            K[index5][index9] += localK[4][8];
            K[index5][index10] += localK[4][9];

            K[index6][index1] += localK[5][0];
            K[index6][index2] += localK[5][1];
            K[index6][index3] += localK[5][2];
            K[index6][index4] += localK[5][3];
            K[index6][index5] += localK[5][4];
            K[index6][index6] += localK[5][5];
            K[index6][index7] += localK[5][6];
            K[index6][index8] += localK[5][7];
            K[index6][index9] += localK[5][8];
            K[index6][index10] += localK[5][9];

            K[index7][index1] += localK[6][0];
            K[index7][index2] += localK[6][1];
            K[index7][index3] += localK[6][2];
            K[index7][index4] += localK[6][3];
            K[index7][index5] += localK[6][4];
            K[index7][index6] += localK[6][5];
            K[index7][index7] += localK[6][6];
            K[index7][index8] += localK[6][7];
            K[index7][index9] += localK[6][8];
            K[index7][index10] += localK[6][9];

            K[index8][index1] += localK[7][0];
            K[index8][index2] += localK[7][1];
            K[index8][index3] += localK[7][2];
            K[index8][index4] += localK[7][3];
            K[index8][index5] += localK[7][4];
            K[index8][index6] += localK[7][5];
            K[index8][index7] += localK[7][6];
            K[index8][index8] += localK[7][7];
            K[index8][index9] += localK[7][8];
            K[index8][index10] += localK[7][9];

            K[index9][index1] += localK[8][0];
            K[index9][index2] += localK[8][1];
            K[index9][index3] += localK[8][2];
            K[index9][index4] += localK[8][3];
            K[index9][index5] += localK[8][4];
            K[index9][index6] += localK[8][5];
            K[index9][index7] += localK[8][6];
            K[index9][index8] += localK[8][7];
            K[index9][index9] += localK[8][8];
            K[index9][index10] += localK[8][9];

            K[index10][index1] += localK[9][0];
            K[index10][index2] += localK[9][1];
            K[index10][index3] += localK[9][2];
            K[index10][index4] += localK[9][3];
            K[index10][index5] += localK[9][4];
            K[index10][index6] += localK[9][5];
            K[index10][index7] += localK[9][6];
            K[index10][index8] += localK[9][7];
            K[index10][index9] += localK[9][8];
            K[index10][index10] += localK[9][9];
        }

        public static void assemblyB(element e, Vector localb, ref Vector b)
        {
            int index1 = e.getNode1()-1;
            int index2 = e.getNode2() - 1;
            int index3 = e.getNode3() - 1;
            int index4 = e.getNode4() - 1;
            int index5 = e.getNode5() - 1;
            int index6 = e.getNode6() - 1;
            int index7 = e.getNode7() - 1;
            int index8 = e.getNode8() - 1;
            int index9 = e.getNode9() - 1;
            int index10 = e.getNode10() - 1;

            b[index1] += localb[0];
            b[index2] += localb[1];
            b[index3] += localb[2];
            b[index4] += localb[3];
            b[index5] += localb[4];
            b[index6] += localb[5];
            b[index7] += localb[6];
            b[index8] += localb[7];
            b[index9] += localb[8];
            b[index10] += localb[9];
            
        }

        public static void assembly(ref mesh m,ref List<Matrix> localKs,ref List<Vector> localbs, ref Matrix K, ref Vector b)
        {
            for(int i = 0; i < m.getSize((int)sizesE.ELEMENTS); i++)
            {
                element e = m.getElement(i);
                assemblyK(e, localKs[i], ref K);
                assemblyB(e, localbs[i], ref b);
            }
        }

        public static void applyNeumann(ref mesh m, ref Vector b)
        {
            for(int i = 0; i < m.getSize((int)sizesE.NEUMANN); i++)
            {
                condition c = m.getCondition(i, (int)sizesE.NEUMANN);
                b[c.getNode1() - 1] += c.getValue();
            }
        }

        public static void applyDirichlet(ref mesh m, ref Matrix K, ref Vector b)
        {
            for (int i = 0; i < m.getSize((int)sizesE.DIRICHLET); i++)
            {
                condition c = m.getCondition(i, (int)sizesE.DIRICHLET);
                int index = c.getNode1() - 1;

                K.RemoveAt(index);
                b.RemoveAt(index);

                for (int row = 0; row < K.Count; row++)
                {
                    double cell = K[row][index];
                    K[row].RemoveAt(index);
                    b[row] += -1 * c.getValue() * cell;
                }
            }
        }

        public static void calculate(ref Matrix K, ref Vector b, ref Vector T)
        {
            //Se utiliza lo siguiente:
            //      K*T = b
            // (K^-1)*K*T = (K^-1)*b
            //     I*T = (K^-1)*b
            //      T = (K^-1)*b
            //Se prepara la inversa de K
            Matrix Kinv = new Matrix();
            //Se calcula la inversa de K
            math_tools.inverseMatrix(K, ref Kinv);
            //Se multiplica la inversa de K por b, y el resultado se almacena en T
            math_tools.productMatrixVector(Kinv, b, ref T);
        }
    }
}
