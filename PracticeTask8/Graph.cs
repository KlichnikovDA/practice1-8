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
        // Количество обследованных вершин
        int CheckedCount;
        // Порядковые номера пройденных вершин
        byte[] Positions;

        // Конструктор
        public Graph(int Rows, int Columns, byte[,] Matrix)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            IncidenceMatrix = Matrix;
            CheckedCount = 0;
            for (int i = 0; i < Rows; i++)
            {
                Positions[i] = 0;
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
                Size = sr.Read();
                bool ok = Int32.TryParse(sr.ReadLine(), out Edges);
               // Матрица инциденций
                byte[,] Matrix = new byte[Size, Size];
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

        // Запись графа в файл
        public void WriteGraph(string P)
        {
            P = Path.GetDirectoryName(P) + Path.GetFileNameWithoutExtension(P) + "output" + Path.GetExtension(P);
            FileStream File;
            try
            {
                File = new FileStream(P, FileMode.Truncate);
            }
            catch (FileNotFoundException)
            {
                File = new FileStream(P, FileMode.CreateNew);
            }
            StreamWriter sw = new StreamWriter(File);
            // Размер графа
            sw.WriteLine(Rows + " " + Columns);
            sw.WriteLine();
            // Матрица инциденций
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    sw.Write(IncidenceMatrix[i, j] + " ");
                sw.WriteLine();
            }

            Console.WriteLine("Информация об обработанном графе записана в файл " + P);

            sw.Close();
            File.Close();
        }

        // Поиск блоков посредством обхода в глубину
        public void DepthSearch(int Pos, int Enter)
        {
            CheckedCount++;

        }
    }
}
