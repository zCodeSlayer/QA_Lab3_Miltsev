using System;
using System.Collections.Generic;
using System.Text;

namespace LR2_Miltsev
{
    /**
     * Класс Row
     *
     * Класс, предназначенный для описания работы графа
     */
    class Row
    {
        /** исходное событие работы  */
        private int _a;
        /** завершающее событие работы */
        private int _b;
        /** время выполнения работы */
        private int _t; 
        private bool _flag;

        public Row(int a, int b, int t)
        {
            _a = a;
            _b = b;
            _t = t;
            _flag = false;
        }

        /** получение информации о работе графа */
        public override string ToString()
        {
            return $"A: {_a.ToString()} B: {_b.ToString()} t: {_t.ToString()}";
        }

        /** получение исходного события работы */
        public int GetA()
        {
            return _a;
        }

        /** получение завершающего события работы */
        public int GetB()
        { 
            return _b;
        }

        /** получение времени выполнения работы */
        public int GetT()
        {
            return _t;
        }
    }
}
