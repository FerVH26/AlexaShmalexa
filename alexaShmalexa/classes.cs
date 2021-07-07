using System;
using System.Collections.Generic;
using System.Text;

namespace mef3d
{

    public enum indicatorsE {NOTHING};
    public enum linesE {NOLINE,SINGLELINE,DOUBLELINE}
    public enum modesE {NOMODE,INT_FLOAT,INT_FLOAT_FLOAT_FLOAT,INT_INT_INT_INT_INT}
    public enum parametersE { CONSTANT, FUNCTION_VECTORX,FUNCTION_VECTORY,FUNCTION_VECTORZ };
    public enum sizesE { NODES, ELEMENTS, DIRICHLET, NEUMANN };

    public abstract class item
    {
        public int id;
        public float x;
        public float y;
        public float z;
        public int node1;
        public int node2;
        public int node3;
        public int node4;
        public int node5;
        public int node6;
        public int node7;
        public int node8;
        public int node9;
        public int node10;

        public float value;

        public void setId(int identifier)
        {
            id = identifier;
        }

        public void setX(float x_coord)
        {
            x = x_coord;
        }

        public void setY(float y_coord)
        {
            y = y_coord;
        }

        public void setZ(float z_coord)
        {
            z = z_coord;
        }

        public void setNode1(int node_1)
        {
            node1 = node_1;
        }

        public void setNode2(int node_2)
        {
            node2 = node_2;
        }

        public void setNode3(int node_3)
        {
            node3 = node_3;
        }

        public void setNode4(int node_4)
        {
            node4 = node_4;
        }
        public void setNode5(int node_5)
        {
            node5 = node_5;
        }
        public void setNode6(int node_6)
        {
            node6 = node_6;
        }
        public void setNode7(int node_7)
        {
            node7 = node_7;
        }
        public void setNode8(int node_8)
        {
            node8 = node_8;
        }
        public void setNode9(int node_9)
        {
            node9 = node_9;
        }
        public void setNode10(int node_10)
        {
            node10 = node_10;
        }


        public void setValue(float value_to_assign)
        {
            value = value_to_assign;
        }

        public int getId()
        {
            return id;
        }

        public float getX()
        {
            return x;
        }

        public float getY()
        {
            return y;
        }

        public float getZ()
        {
            return z;
        }

        public int getNode1()
        {
            return node1;
        }

        public int getNode2()
        {
            return node2;
        }

        public int getNode3()
        {
            return node3;
        }

        public int getNode4()
        {
            return node4;
        }
        public int getNode5()
        {
            return node5;
        }
        public int getNode6()
        {
            return node6;
        }
        public int getNode7()
        {
            return node7;
        }
        public int getNode8()
        {
            return node8;
        }
        public int getNode9()
        {
            return node9;
        }
        public int getNode10()
        {
            return node10;
        }

        public float getValue()
        {
            return value;
        }

        public abstract void setValues(int a, float b, float c,float d, int e, int f, int g, int h, float i);

    }

    public class node : item
    {
        public override void setValues(int a, float b, float c,float d, int e, int f, int g, int h, float i)
        {
            this.id = a;
            this.x = b;
            this.y = c;
            this.z = d;
        }
    }

    public class element : item
    {
        public override void setValues(int a, float b, float c, float d, int e, int f, int g, int h, float i)
        {
            id = a;
            node1 = e;
            node2 = f;
            node3 = g;
            node4 = h;
        }
    }

    public class condition : item
    {
        public override void setValues(int a, float b, float c, float d, int e, int f, int g, int h, float i)
        {
            node1 = e;
            value = i;
        }
    }

    public class mesh
    {
        float[] parameters = new float[4];
        int[] sizes = new int[4];
        protected node[] node_list;
        protected element[] element_list;
        int[] indices_dirich;
        protected condition[] dirichlet_list;
        protected condition[] neumann_list;

        public void setParameters(float EI,float fx, float fy, float fz)
        {
            parameters[(int)parametersE.CONSTANT] = EI;
            parameters[(int)parametersE.FUNCTION_VECTORX] = fx;
            parameters[(int)parametersE.FUNCTION_VECTORY] = fy;
            parameters[(int)parametersE.FUNCTION_VECTORZ ] = fz;

        }

        public void setSizes(int nnodes, int neltos, int ndirich, int nneu)
        {
            sizes[(int)sizesE.NODES] = nnodes;
            sizes[(int)sizesE.ELEMENTS] = neltos;
            sizes[(int)sizesE.DIRICHLET] = ndirich;
            sizes[(int)sizesE.NEUMANN] = nneu;
        }

        public int getSize(int s)
        {
            return sizes[s];
        }

        public float getParameter(int p)
        {
            return parameters[p];
        }

        public void createData()
        {
            node_list = new node[sizes[(int)sizesE.NODES]];
            element_list = new element[sizes[(int)sizesE.ELEMENTS]];
            indices_dirich = new int[sizes[(int)sizesE.DIRICHLET]];
            dirichlet_list = new condition[sizes[(int)sizesE.DIRICHLET]];
            neumann_list = new condition[sizes[(int)sizesE.NEUMANN]];
        }

        public node[] getNodes()
        {
            return node_list;
        }

        public element[] getElements()
        {
            return element_list;
        }

        public int[] getDirichletIndices()
        {
            return indices_dirich;
        }

        public condition[] getDirichlet()
        {
            return dirichlet_list;
        }

        public condition[] getNeumann()
        {
            return neumann_list;
        }

        public node getNode(int i)
        {
            return node_list[i];
        }

        public element getElement(int i)
        {
            return element_list[i];
        }

        public condition getCondition(int i, int type)
        {
            if (type == sizes[(int)sizesE.DIRICHLET]) return dirichlet_list[i];
            else return neumann_list[i];
        }

    }
}
