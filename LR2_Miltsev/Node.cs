using System;
using System.Collections.Generic;
using System.Text;

namespace LR2_Miltsev
{
    /**
     * Структура NextNode
     *
     * вспомогательная структура для работы с потомком вершины графа
     */
    struct NextNode
    {
        /** потомок вершины */
        public Node _nextNode;
        /** вес потомка */
        public int _weight; 
    };

    /**
     * Класс Node
     *
     * Класс, описывающий вершину графа
     */
    class Node 
    {
        /** потомоки текущей вершины */
        private List<NextNode> _children;
        /** вес текущей вершины */
        private int _value; 

        /** Конструктор, описывающий вершину (её вес и потомков) */
        public Node(int value)
        {
            _value = value;
            _children = new List<NextNode>();
        }

        /** получить список всех потомков вершины */
        public List<NextNode> GetChildren()
        {
            return _children;
        }

        /** получить вес вершины */
        public int GetValue()
        {
            return _value;
        }

        /** добавление потомка */
        public void AddChild(Node child, int weight) 
        {
            NextNode next;
            next._nextNode = child;
            next._weight = weight;
            _children.Add(next);
        }

        /** удаление потомка */
        public void RemoveChild(int removableChildVal, int weught = -1) 
        {
            NextNode removeObject;
            removeObject._nextNode = null;
            removeObject._weight = weught;

            foreach(NextNode nn in _children)
            {
                if (nn._nextNode.GetValue() == removableChildVal)
                {
                    removeObject = nn;
                    break;
                }
            }

            if(removeObject._nextNode != null)
                _children.Remove(removeObject);
        }

        /** проверка на наличие циклов из вершины */
        public bool CheckCycles(Node currentNode, Node checkNode, ref List<int> cycleWay) 
        {
            if (currentNode == checkNode)
            {
                cycleWay.Add(currentNode._value);
                return true;
            } else if (currentNode._children.Count != 0)
            {
                foreach(NextNode child in currentNode._children)
                {
                    cycleWay.Add(currentNode._value);
                    bool res = child._nextNode.CheckCycles(child._nextNode, checkNode, ref cycleWay);
                    if (res == true)
                        return true;
                }
            }
            if (cycleWay.Count > 0)
                cycleWay.RemoveAt(cycleWay.Count - 1);
            return false;
        }

        /** поиск потомкв по заданному весу */
        public int SearchChildWeight(int childValue, int weight = -1)
        {
            foreach (NextNode nn in _children)
            {
                if ((nn._nextNode._value == childValue) && (weight == -1 || nn._weight == weight))
                    return nn._weight;
            }
            return 0;
        }

        /** поиск потомка по значению */
        public Node SearchChild(int childValue, int weight = -1) 
        {
            if (_children.Count == 0)
                return null;
            foreach(NextNode nn in _children)
            {
                if ((nn._nextNode._value == childValue) && (weight == -1 || nn._weight == weight))
                    return nn._nextNode;
            }
            return null;
        }

        /** поиск потомкв по заданному весу */
        public int SearchChildWeight(int childValue)
        {
            foreach (NextNode nn in _children)
            {
                if (nn._nextNode._value == childValue) 
                    return nn._weight;
            }
            return -1;
        }

        /** получение строки работы между вершиной и потомком */
        public string GetNodesWork(int nextNodeVal, int weight = -1) 
        {
            Node child = this.SearchChild(nextNodeVal, weight);
            if (child != null)
            {
                int child_weight = this.SearchChildWeight(nextNodeVal, weight);
                return $"A: {this._value.ToString()} B: {child.GetValue().ToString()} t: {child_weight.ToString()}";
            }
            return null;
        }

        /** вывод всех путей из начальной вершины до конечной */
        public List<int> DisplayAllWays(List<int> val) 
        {
            val.Add(this._value);
            if (this._children.Count == 0)
            {
                foreach (int i in val)
                    Console.Write(i.ToString() + " ");
                Console.Write("\n");
            } else
            {
                foreach(NextNode nn in _children)
                {
                    val = nn._nextNode.DisplayAllWays(val);
                }
            }
            if(val.Count - 1 > 0)    
                val.RemoveAt(val.Count - 1);
            return val;
        }
    }
}
