using System;
using System.Collections.Generic;
using System.Text;

namespace LR2_Miltsev
{
    /*!
     * \f$ t_i=(2t_i+3t_j) / 5 \f$
     * 
    ![Пример сетевого графа](https://studref.com/htm/img/40/8998/343.png)
    *
    */

    /**
     * 
     * Класс NetworkGraph
     *
     * сетевой граф представляет собой список вершин, связанных ребрами через список указателей, а так же связанную с ним выходную таблицу данных
     */
    class NetworkGraph
    {
        /** Необработанная (исходная) таблица, предназначенная для дальнейшей обработки для превращения в сетевой граф */
        private Data _dataTable;
        /** выходная (итоговая) таблица сетевого графа */
        private Data _ngTable;
        /** начальная вершина сетевого графа */
        private Node _initialNode;
        /** список всех вершин */
        private List<Node> _nodes; 

        /** Конструктор класса */
        public NetworkGraph()
        {
            _dataTable = null;
            _ngTable = null;
            _initialNode = null;
            _nodes = new List<Node>(); ;
        }

        /** Получить информацию о сетевом графе */
        public Data GetNGTable()
        {
            return _ngTable;
        }

        /** Получить начальную вершину */
        public Node GetInitialNode()
        {
            return _initialNode;
        }

        /** поиск узла в графе */
        public Node SearchNode(int val) 
        {
            foreach (Node node in _nodes)
            {
                if (node.GetValue() == val)
                    return node;
            }
            return null;
        }

        /** получение вершин, из которых не выходят ребра (завершающие) */
        public List<int> GetAllFinishingNodes() 
        {
            List<int> FinishingNodes = new List<int>();
            foreach (Node node in _nodes)
            {
                if (node.GetChildren().Count == 0)
                    FinishingNodes.Add(node.GetValue());
            }
            return FinishingNodes;
        }

        /** получение всех вершин */
        public List<int> GetAllNodes() 
        {
            List<int> allNodes = new List<int>();
            foreach (Node node in _nodes)
                allNodes.Add(node.GetValue());
            return allNodes;
        }

        /** удаление узла из графа */
        public void RemoveNode(int val) 
        {
            Node removeable = this.SearchNode(val);
            foreach (Node node in _nodes) 
            {
                node.RemoveChild(val);
            }
            _nodes.Remove(removeable);
            
        }

        /** получение вершин, в которые нет ни одного вхождения (начальные) */
        public List<int> GetAllInitialNodes() 
        {
            List<int> notInitial = new List<int>();
            foreach(Node node in _nodes)
            {
                if(node.GetChildren().Count != 0)
                {
                    foreach(NextNode nn in node.GetChildren())
                    {
                        if (notInitial.Contains(nn._nextNode.GetValue()) == false)
                            notInitial.Add(nn._nextNode.GetValue());
                    }
                }
            }
            List<int> initial = new List<int>();
            foreach (int i in this.GetAllNodes())
            {
                if (notInitial.Contains(i) == false)
                    initial.Add(i);
            }
            return initial;
        }

        /** Отображение сетевого графика с упорядоченным списком работ */
        public void DisplaySortNG()
        {
            List<int> inptemp = _ngTable.GetAllA();
            List<int> outtemp = _ngTable.GetAllB();
            List<int> res = new List<int>();

            while (inptemp.Count > 0)
            {
                for (int i = 0; i < inptemp.Count; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < outtemp.Count; j++)
                    {
                        if (inptemp[i] == outtemp[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        int tt = inptemp[i];
                        res.Add(inptemp[i]);
                        inptemp.RemoveAt(i);
                        outtemp.RemoveAt(i);
                        for (int l = 0; l < inptemp.Count; l++)
                        {
                            if (inptemp[l] == tt)
                            {
                                inptemp.RemoveAt(l);
                                outtemp.RemoveAt(l);
                                l = 0;
                                i = 0;
                            }
                        }
                    }
                }
            }
            inptemp.Clear();
            outtemp.Clear();
            List<int> temptime = new List<int>();
            for (int i = 0; i < res.Count; i++)
            {
                for (int j = 0; j < _ngTable.GetAllA().Count; j++)
                {
                    if (res[i] == _ngTable.GetAllA()[j])
                    {
                        inptemp.Add(_ngTable.GetAllA()[j]);
                        outtemp.Add(_ngTable.GetAllB()[j]);
                        temptime.Add(_ngTable.GetAllT()[j]);
                    }
                }

            }
            Console.WriteLine("A | B | t ");
            for (int i = 0; i < inptemp.Count; i++)
            {
                Console.WriteLine($"{inptemp[i]} | {outtemp[i]} | {temptime[i]}");
            }
        }

        /** построение сетевого графика */
        public void CreateNetworkGraph(Data dataTable) 
        {
            _dataTable = dataTable;

            List<int> events = _dataTable.GetAllEvents();
            
            // инициализация событий в сетевом графе
            foreach (int e in events)
                _nodes.Add(new Node(e));

            int choise;
            foreach (Row row in _dataTable.GetRows()) // обработка строк таблицы исходных данных
            {
                // добавить очередную работу
                Node nodeA = this.SearchNode(row.GetA());
                Node nodeB = this.SearchNode(row.GetB());

                
                Node parallelWork;
                parallelWork = nodeA.SearchChild(row.GetB());
                int parallel_weight = nodeA.SearchChildWeight(row.GetB());
                nodeA.AddChild(nodeB, row.GetT());



                // выполнить анализ сетевого графа после добавления работы
                if (nodeA.GetValue() == nodeB.GetValue()) // проверка на наличие петель
                {
                    Console.WriteLine($"При обработки строки {row.ToString()} Обнаружена петля. Работа будет удалена.");
                    nodeA.RemoveChild(row.GetB(), row.GetT());
                    continue;
                }

                if (parallelWork != null) // проверить наличие параллельной работы
                {
                    //Console.WriteLine("Обработка строки (#1)" + row.ToString() + "\nОбнаружена параллельная работа (#2):" + nodeA.GetNodesWork(this.SearchNode(row.GetB()), parallelWork._weight) + "\nВы можете: \nУдалить работу (#1) (1)\nУдалить работу (#2) (2)");
                    Console.WriteLine("Обработка строки (#1)" + row.ToString() + "\nОбнаружена параллельная работа (#2):" + nodeA.GetNodesWork(row.GetB(), parallel_weight) + "\nВы можете: \nУдалить работу (#1) (1)\nУдалить работу (#2) (2)");
                    choise = Convert.ToInt32(Console.ReadLine());

                    switch (choise)
                    {
                        case 1:
                            nodeA.RemoveChild(row.GetB(), row.GetT());
                            continue;
                            break;
                        case 2:
                            nodeA.RemoveChild(row.GetB(), parallel_weight);
                            continue;
                            break;
                    }
                }

                List<int> cycleWay = new List<int>() { nodeA.GetValue() };
                bool checkCycle = nodeB.CheckCycles(nodeB, nodeA, ref cycleWay); // проверить на появление циклов
                bool CycleContinueFlag = false;

                while (checkCycle)
                {
                    if (CycleContinueFlag == true)
                        break;
                    List<int> way = new List<int>();
                    foreach (int i in cycleWay)
                    {
                        way.Add(i);
                    }
                    //Console.WriteLine($"Обработка строки {row.ToString()}\nОбнаружен цикл: {way}\nВы можете:\nУдалить работу {nodeA.GetNodesWork(this.SearchNode(row.GetB()), row.GetT())} (1)\nУдалить работу {nodeB.GetNodesWork(nodeB, way[2])} (2)");
                    Console.WriteLine($"Обработка строки {row.ToString()}\nОбнаружен цикл\nПуть цикла: {string.Join(" - ", cycleWay)}\nВы можете:\nУдалить работу {nodeA.GetNodesWork(row.GetB(), row.GetT())} (1)\nУдалить работу {nodeB.GetNodesWork(way[2])} (2)");

                    choise = Convert.ToInt32(Console.ReadLine());

                    switch (choise)
                    {
                        case 1:
                            nodeA.RemoveChild(row.GetB(), row.GetT());
                            CycleContinueFlag = true;
                            break;
                        case 2:
                            NextNode nodeBChild;
                            nodeBChild._nextNode = nodeB.SearchChild(way[2]);
                            nodeBChild._weight = nodeB.SearchChildWeight(way[2]);
                            nodeB.RemoveChild(nodeBChild._nextNode.GetValue(), nodeBChild._weight);
                            cycleWay.Clear();
                            cycleWay.Add(nodeA.GetValue());
                            checkCycle = nodeB.CheckCycles(nodeB, nodeA, ref cycleWay);
                            break;
                    }
                }
            }

            //определить начальное и завершающее соыбтие в сетевом графе

            //определение начальных состояний
            List<int> initialNodes = this.GetAllInitialNodes();
            Node fictiveInitialNode = null;

            while (initialNodes.Count != 1)
            {
                if (fictiveInitialNode == null) // фиктивная не введена
                {
                    Console.WriteLine("В сетевом графе нет единственного начального события.\nВы можете:\nВыбрать одно начальное событие (1)\nВвести фиктивное начальное событие и удалить ненужные события (2)");
                    choise = Convert.ToInt32(Console.ReadLine());
                    switch(choise)
                    {
                        case 1:
                            Console.WriteLine($"Введите событие, которое оставить (доступны: {string.Join(", ", initialNodes)}): ");
                            int noRemove = Convert.ToInt32(Console.ReadLine());
                            foreach(int rem in initialNodes)
                            {
                                if (rem != noRemove)
                                    this.RemoveNode(rem);
                            }
                            break;
                        case 2:
                            fictiveInitialNode = new Node(-1);
                            _nodes.Add(fictiveInitialNode);
                            break;
                    }
                }
                else // если есть фиктивная, то определить какие события оставить, а какие удалить
                {
                    foreach(int n in initialNodes)
                    {
                        if(n != -1)
                        {
                            Console.WriteLine($"Событие {n.ToString()} удалить (1) или оставить (2): ");
                            choise = Convert.ToInt32(Console.ReadLine());
                            switch(choise)
                            {
                                case 1:
                                    this.RemoveNode(n);
                                    break;
                                case 2:
                                    fictiveInitialNode.AddChild(this.SearchNode(n), 0);
                                    break;
                            }
                        }
                    }
                }
                initialNodes = this.GetAllInitialNodes();
            }
            _initialNode = this.SearchNode(initialNodes[0]);

            // определение заверщающего состояния

            var finishingNodes = this.GetAllFinishingNodes();
            Node fictiveFinishingNode = null;

            while(finishingNodes.Count != 1)
            {
                if (fictiveFinishingNode == null) // пока фиктивная еще не введена
                {
                    Console.WriteLine("В сетевом графе нет единственного конечного события.\nВы можете:\nВыбрать одно конечное событие (1)\nВвести фиктивное конечное событие и удалить ненужные события (2)");
                    choise = Convert.ToInt32(Console.ReadLine());
                    switch (choise)
                    {
                        case 1:
                            Console.WriteLine($"Введите событие, которое оставить (доступны: {string.Join(", ", finishingNodes)}): ");
                            int removeFin = Convert.ToInt32(Console.ReadLine());
                            foreach (int rem in finishingNodes)
                            {
                                if (rem != removeFin)
                                    this.RemoveNode(rem);
                            }
                            break;
                        case 2:
                            fictiveFinishingNode = new Node(-2);
                            _nodes.Add(fictiveFinishingNode);
                            break;
                    }
                }
                else // если есть фиктивная, то определить какие события оставить, а какие удалить
                {
                    foreach(int node in finishingNodes)
                    {
                        if(node != -2)
                        {
                            Console.WriteLine($"Событие {node.ToString()} удалить (1) или оставить (2): ");
                            choise = Convert.ToInt32(Console.ReadLine());
                            switch (choise)
                            {
                                case 1:
                                    this.RemoveNode(node);
                                    break;
                                case 2:
                                    this.SearchNode(node).AddChild(fictiveFinishingNode, 0);
                                    break;
                            }
                        }
                    }
                }
                finishingNodes = this.GetAllFinishingNodes();
            }

            // формирование выходной таблицы графа
            _ngTable = new Data("");
            foreach(Node node in _nodes)
            {
                if (node.GetChildren().Count != 0)
                {
                    foreach (NextNode child in node.GetChildren())
                        _ngTable.AddRow(new Row(node.GetValue(), child._nextNode.GetValue(), child._weight));
                }
            }
        }
    }
}
