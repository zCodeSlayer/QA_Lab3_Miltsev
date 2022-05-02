using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LR2_Miltsev
{
    /**
     * Класс Data
     *
     * Класс, предназначенный для работы с данными графа
     */
    class Data
    {
        /** список рёбер графа */
        private List<Row> _rows = new List<Row>(); 

        /** Конструктор, в котором происходит считывание данных из файла с расширением .txt. В качестве параметра передаётся название файла */
        public Data(string fileName)
        {
            if (fileName != "")
            {
                StreamReader sr = new StreamReader(fileName);
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] arrayLine = line.Split(' ');
                    _rows.Add(new Row(Convert.ToInt32(arrayLine[0]),
                        Convert.ToInt32(arrayLine[1]),
                        Convert.ToInt32(arrayLine[2])));
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }

        /** Получить все начальные события всех работ графика */
        public List<int> GetAllA()
        {
            List<int> allA = new List<int>();
            foreach (Row row in _rows)
            {
                allA.Add(row.GetA());
            }
            return allA;
        }

        /** Получить все завершающие события всех работ графика */
        public List<int> GetAllB()
        {
            List<int> allB = new List<int>();
            foreach (Row row in _rows)
            {
                allB.Add(row.GetB());
            }
            return allB;
        }

        /**Получить время всех работ сетевого графика (для каждой по отдельности) */
        public List<int> GetAllT()
        {
            List<int> allT = new List<int>();
            foreach (Row row in _rows)
            {
                allT.Add(row.GetT());
            }
            return allT;
        }

        /** получить список всех рёбер графа */
        public List<Row> GetRows()
        {
            return _rows;
        }

        /** Получение множества событий */
        public List<int> GetAllEvents() 
        {
            List<int> list = new List<int>();
            foreach(Row r in _rows)
            {
                if (list.Contains(r.GetA()) == false)
                    list.Add(r.GetA());
                if (list.Contains(r.GetB()) == false)
                    list.Add(r.GetB());
            }
            return list;
        }

        /** получение множества событий, из которых исходят работы */
        public List<int> GetAllEventsA() 
        {
            List<int> list = new List<int>();
            foreach (Row r in _rows)
            {
                if (list.Contains(r.GetA()) == false)
                    list.Add(r.GetA());
            }
            return list;
        }

        /** получение множества событий, в которые входят работы */
        public List<int> GetAllEventsB() 
        {
            List<int> list = new List<int>();
            foreach (Row r in _rows)
            {
                if (list.Contains(r.GetB()) == false)
                    list.Add(r.GetB());
            }
            return list;
        }

        /** удаление строки из таблицы */
        public void RemoveRow(Row row) 
        {
            _rows.Remove(row);
        }

        /** добавление строки в конец таблицы */
        public void AddRow(Row row) 
        {
            _rows.Add(row);
        }

        /** добавление строки в начало таблицы */
        public void AddRowToTop(Row row) 
        {
            _rows.Insert(0, row);
        }

        /** вывод таблицы на экран */
        public void ShowData() 
        {
            Console.WriteLine("A | B | t");
            foreach(Row row in _rows)
            {
                Console.WriteLine($"{row.GetA().ToString()} | {row.GetB().ToString()} | {row.GetT().ToString()}");
            }
        }
    }
}
