using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeTask8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу, в котором записан граф, который Вы хотите обработать:");
            string Path = Console.ReadLine();

            // Чтение графа из файла
            Graph Graph = Graph.ReadGraph(Path);

            // Если удалось прочитать граф
            if (Graph != null)
            {
                // Выделение блоков
                Graph.Block();
                Console.WriteLine();
            }

            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
