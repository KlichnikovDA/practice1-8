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
        // Минимальные номера
        int[] Minim;
        // Стек с номерами ребер, составляющими блок
        Stack<int> EdgesStack;

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
            Minim = new int[Rows];
            for (int i = 0; i < Rows; i++)
            {
                Checked[i] = Minim[i] = 0;
            }
        }

        // Чтение графа из файла
        public static Graph ReadGraph(string P)
        {
            try
            {
                FileStream File = new FileStream(P, FileMode.Open);
                StreamReader sr = new StreamReader(File);
                // Размер графа
                int Size;
                // Количество ребер в графе
                int Edges;
                // Флаг правильности ввода
                bool ok = Int32.TryParse(sr.ReadLine(), out Size);
                ok = Int32.TryParse(sr.ReadLine(), out Edges);
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
                            Console.WriteLine("В файле содержатся некорректные данные.");
                            return null;
                        }
                        Matrix[i, j] = Row[j];
                    }
                }

                sr.Close();
                File.Close();

                return new Graph(Size, Edges, Matrix);
            }
            catch
            {
                Console.WriteLine("Не удается открыть файл, проверьте его наличие и правильность пути.");
                return null;
            }
        }
        
        // Выделение блоков графа методом поиска в глубину
        public void Block()
        {
            CheckedCount = 0;
            // Проход по всем вершинам графа
            for (int v = 0; v < Rows; v++)
                // Если вершина еще не была пройдена
                if (Checked[v] == 0)
                    // Начинаем поиск из нее
                    DFS(v, -1);
        }

        // Поиск в глубину с выделением ребер, составляющих блоки
        void DFS(int Pos, int Parent)
        {
            // Отмечаем вершину
            Checked[Pos] = Minim[Pos] = ++CheckedCount;

            // Перебираем смежные вершины
            for (int Edge = 0; Edge < Columns; Edge++)
            {
                // Смежная вершина
                int NextVerit = 0;
                // Перебираем ребра, исходящие из (или входящие в) исходной вершины
                if (IncidenceMatrix[Pos, Edge] == 1)
                {
                    // Ищем вторую вершину данного ребра 
                    while (NextVerit < Rows && IncidenceMatrix[NextVerit, Edge] == 0 || NextVerit==Pos)
                        NextVerit++;
                    // Если нашли еще не пройденную вершину, смежную исходной, то
                    if (NextVerit < Rows && Checked[NextVerit] == 0)
                    {
                        // Записываем номер ребра, соединяющего их
                        EdgesStack.Push(NextVerit);
                        // Продолжаем поиск из этой вершины
                        DFS(NextVerit, Pos);
                        // Переопределяем минимальный номер для вершины
                        Minim[Pos] = Math.Min(Minim[NextVerit], Minim[Pos]);
                        // Нашли корень дерева или точку сочленения
                        if (Minim[NextVerit] > Minim[Pos])
                        // Печатаем блок
                        {
                            Console.Write("Блок {0} состоит из ребер под номерами ", ++Blocks);
                            while (EdgesStack.Count > 0)
                                Console.Write("{0}, ", EdgesStack.Pop() + 1);
                        }
                    }
                }
                else
                {
                    // Если вершина уже исследована и 
                    if (Checked[NextVerit] < Checked[Pos] && NextVerit != Parent)
                    {
                        EdgesStack.Push(Edge);
                        Minim[Pos] = Math.Min(Minim[NextVerit], Minim[Pos]);
                    }
                }
            }                
        }
    }
}
