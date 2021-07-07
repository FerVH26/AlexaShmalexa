using Vector = System.Collections.Generic.List<double>;
//Se define un arreglo de vectores como una matriz
using System.Runtime.Intrinsics.X86;
using Matrix = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using System.IO;
using System;

namespace mef3d{
    public static class tools{

        public static void obtenerDatos(ref StreamReader file,int nlines,int n,int mode, item[] item_list){
        
            string line;
            line = file.ReadLine();
            line = file.ReadLine();
            if(nlines==((int) linesE.DOUBLELINE)) file.ReadLine(); 
            
            for(int i=0;i<n;i++){
                switch(mode){
                
                    case ((int) modesE.INT_FLOAT):
                        int e; float r;
                        string phrase = file.ReadLine();
                        string[] words = phrase.Split(' ');
                    
                        e = Int32.Parse(words[0]); 
                        r = float.Parse(words[1]);
                        //Se instancian el entero y el real del objeto actual
                        //Converter nothing to 0
                        item_list[i].setValues( 0, 0, 0, e, 0,0,0, 0, r); // 00 sin saber porque
                        break;
                
                    //Se extraen tres enteros
                    case (int) modesE.INT_INT_INT_INT_INT:
                        int e1,e2,e3, e4;
                        string phrase2 = file.ReadLine();
                        string[] words2 = phrase2.Split(' ');
                        e1 = Int32.Parse(words2[0]);
                        e2 = Int32.Parse(words2[1]);
                        e3 = Int32.Parse(words2[2]);
                        e4 = Int32.Parse(words2[3]);
                        //Se instancia los tres enteros en el objeto actual
                        item_list[i].setValues(e1,0,0,e2,e3,e4,0,0,0);// agregue 00 pero no se porque
                        break;
                
                    case (int)modesE.INT_FLOAT_FLOAT_FLOAT:
                        int e5; float r0,rr,rrr,xd;
                        string phrase3 = file.ReadLine();
                        string[] words3 = phrase3.Split("       ");
                        e5 = Int32.Parse(words3[0]);
                        r0 = float.Parse(words3[1]);
                        rr = float.Parse(words3[2]);
                        rrr = float.Parse(words3[3]);
                            xd = 0;
                        item_list[i].setValues(e5,r0,rr,rrr,0,0,0,0,xd); //00 ni idea
                        break;

                }
            }
        }

        public static void correctConditions(int n,condition[] list,ref int[] indices){
            for(int i=0;i<n;i++)
                indices[i] = list[i].getNode1();

            for(int i=0;i<n-1;i++){
                int pivot = list[i].getNode1();
                for(int j=i;j<n;j++)
                    if(list[j].getNode1()>pivot)
                        list[j].setNode1(list[j].getNode1()-1);
            }
        }

        public static void addExtension(char[] newfilename,char[] filename,char[] extension){
            int ori_length = (filename).Length;
            int ext_length = (extension).Length;
            int i;
            for(i=0;i<ori_length;i++)
                newfilename[i] = filename[i];
            for(i=0;i<ext_length;i++)
                newfilename[ori_length+i] = extension[i];
            newfilename[ori_length+i] = '\0';
        }
         public static void leerMallayCondiciones(ref mesh m){
            
            String filename;
            StreamReader file = null;
            
            float EI,fx, fy, fz;
            int nnodes,neltos,ndirich,nneu;

            do{
                Console.WriteLine("Ingrese el nombre del archivo que contiene los datos de la malla: ");
                filename = Console.ReadLine();
                file = new StreamReader(filename);
            }while(file.Equals(null)); 

            string phrase = file.ReadLine();
            string[] words = phrase.Split(' ');
            EI = float.Parse(words[0]);
            fx = float.Parse(words[1]);
            fy = float.Parse(words[2]);
            fz = float.Parse(words[3]);

            string phrase2 = file.ReadLine();
            string[] words2 = phrase2.Split(' ');
            nnodes = Int32.Parse(words2[0]);
            neltos = Int32.Parse(words2[1]);
            ndirich = Int32.Parse(words2[2]);
            nneu = Int32.Parse(words2[3]);

            m.setParameters(EI, fx, fy, fz);
            m.setSizes(nnodes,neltos,ndirich,nneu);
            m.createData();
            
            obtenerDatos(ref file,(int) linesE.SINGLELINE,nnodes,(int) modesE.INT_FLOAT_FLOAT_FLOAT,m.getNodes());
            obtenerDatos(ref file,(int) linesE.DOUBLELINE,neltos,(int) modesE.INT_INT_INT_INT_INT,m.getElements());
            obtenerDatos(ref file,(int) linesE.DOUBLELINE,ndirich,(int) modesE.INT_FLOAT,m.getDirichlet());
            obtenerDatos(ref file,(int) linesE.DOUBLELINE,nneu,(int) modesE.INT_FLOAT,m.getNeumann());

            file.Close();
        }

        public static bool findIndex(int v, int s, int[] arr){
            for(int i=0;i<s;i++)
                if(arr[i]==v) return true;
            return false;
        }

        public static void writeResults(mesh m,Vector T){
            char[] outputfilename = new char[150];
            int[] dirich_indices = m.getDirichletIndices();
            condition[] dirich = m.getDirichlet();

            String filename;
            StreamWriter file = null;
            do{
                Console.WriteLine("Ingrese el nombre del archivo donde desea guardar los datos: ");
                filename = Console.ReadLine();

                file = new StreamWriter(filename);
            }while(file.Equals(null)); 

            file.Write("GiD Post Results File 1.0\n");
            file.Write("Result \"Temperature\" \"Load Case 1\" 1 Scalar OnNodes\nComponentNames \"T\"\nValues\n");

            int Tpos = 0;
            int Dpos = 0;
            int n = m.getSize((int)sizesE.NODES);
            int nd = m.getSize((int)sizesE.DIRICHLET);
            for(int i=0;i<n;i++){
                if(findIndex(i+1,nd,dirich_indices)){
                    file.Write(i+1 + " " + dirich[Dpos].getValue() + "\n");
                    Dpos++;
                }else{
                    file.Write(i+1 + " " + T[Tpos] + "\n");
                    Tpos++;
                }
            }

            file.Write("End values\n");

            file.Close();
        }
    } 
}
