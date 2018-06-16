using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PracticeTask8
{
    public class Graph
    {
        // Количество вершин графа
        int Rows;
        // Количество ребер графа
        int Columns;
        // Граф задается матрицей инциденций
        byte[,] IncidenceMatrix;
        // Количество блоков
        int Blocks;
        // Количество обследованных вершин
        int CheckedCount;
        // Порядковые номера обхода вершин
        int[] Checked;
        // Стек с номерами ребер, составляющими блок
        Stack<int> EdgesStack;
        // Список уже использованных ребер
        List<int> UsedEdges;

        // Конструктор
        public Graph(int Rows, int Columns, byte[,] Matrix)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            IncidenceMatrix = Matrix;
            Blocks = 0;
            CheckedCount = 0;
            EdgesStack = new Stack<int>(Columns);
            Checked = new int[Rows];
            for (int i = 0; i < Rows; i++)
            {
                Checked[i] = 0;
            }
            UsedEdges = new List<int>(Columns);
        }

        // Чтение графа из файла
        public static Graph ReadGraph(string P)
        {
            try
            {
                FileStream File = new FileStream(P, FileMode.Open);
                StreamReader sr = new StreamReader(File);
                // Размер графа
                int Size = Int32.Parse(sr.ReadLine());
                // Количество ребер в графе
                int Edges = Int32.Parse(sr.ReadLine());
                // Матрица инциденций
                byte[,] Matrix = new byte[Size, Edges];
                for (int i = 0; i < Size; i++)
                {
                    string vals = sr.ReadLine();
                    if (vals.Length > Edges * 2 - 1)
                        vals = vals.Remove(Edges * 2 - 1);
                    // Чтение строки матрицы
                    byte[] Row = vals.Split(' ').Select(n => Byte.Parse(n)).ToArray();
                    for (int j = 0; j < Edges; j++)
                    {
                        if (Row[j] != 0 && Row[j] != 1)
                        {
                            throw new Exception();
                        }
                        Matrix[i, j] = Row[j];
                    }
                }

                sr.Close();
                File.Close();

                return new Graph(Size, Edges, Matrix);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Не удается открыть файл, проверьте его наличие и правильность пути.");
                return null;
            }
            catch
            {
                Console.WriteLine("В файле содержатся некорректные данные.");
                return null;
            }
        }
        
        // Выделение блоков графа методом поиска в глубину
        public void Block()
        {
            CheckedCount = 0;
            DFS(0, -1);
        }

        // Поиск в глубину
        int DFS(int Pos, int Parent)
        {
            Checked[Pos] = ++CheckedCount;
            // Minim - минимальное расстояние от Pos до входа
            int Minim = Checked[Pos];
            // Перебор всех ребер, входящих или исходящих из вершины Pos
            for (int Edge = 0; Edge < Columns; Edge++)
            {
                if (IncidenceMatrix[Pos, Edge] == 1)
                {
                    int NextVerit = 0;
                    while (NextVerit < Rows && IncidenceMatrix[NextVerit, Edge] == 0 || NextVerit == Pos)
                        NextVerit++;
                    {
                        if (NextVerit != Parent)
                        {
                            int t, cur_size = EdgesStack.Count;
                            // Если этого ребра еще нет в стеке
                            if (!UsedEdges.Contains(Edge))
                            {
                                UsedEdges.Add(Edge);
                                EdgesStack.Push(Edge);
                            }

                            //Если вершина еще не посещена
                            if (Checked[NextVerit] == 0)
                            {
                                // Продолжаем обход из этой вершины
                                t = DFS(NextVerit, Pos);
                                if (t >= Checked[Pos])
                                {
                                    //++Child;
                                    Console.Write("Блоку {0} принадлежат ребра: ", ++Blocks);
                                    while (EdgesStack.Count != cur_size)
                                    {
                                        Console.Write("{0}, ", EdgesStack.Pop());
                                    }
                                    Console.WriteLine();
                                }
                            }
                            else
                                t = Checked[NextVerit];
                            Minim = Math.Min(Minim, t);
                        }
                    }
                }
            }
            return Minim;             
        }
    }
}
