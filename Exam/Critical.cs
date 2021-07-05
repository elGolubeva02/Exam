using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    /// <summary>
    /// Определение критического пути в цепочке работ
    /// </summary>
    public class Critical
    {
        public int max, maxind; //значение и индекс критического пути
        public string s = ""; //итерация пути
        public List<List<Rbt>> itog = new List<List<Rbt>>(); //пути и функции
        public List<Rbt> pyt; //пути
        public List<Rbt> dan = Flrd(); //исходные данные
        /// <summary>
        /// Построение путей
        /// </summary>
        public void Way()
        {
            pyt = dan.FindAll(x => x.point1 == dan[Nach(dan)].point1);//запись точки начала в лист путей
            foreach (Rbt rb in pyt) //построение путей
            {
                Mv(dan, rb);
                itog.Add(RtPrs(dan, s));
                s = "";
            }
        }
        /// <summary>
        /// Нахождение критического пути
        /// </summary>
        public void Reshenie()
        {
            Way();
            maxind = 0;
            max = itog[0][0].length;
            for (int i = 0; i < pyt.Count; i++) // подсчет стоимости путей
            {
                if (FnlMv(itog[i]) >= max) // выбор максимального
                {
                    max = FnlMv(itog[i]);
                    maxind = i;
                }
            }
            Vivod();
        }
        /// <summary>
        /// Запись решения в csv файл
        /// </summary>
       
        public void Vivod()
        {
            foreach (Rbt rb in itog[maxind])
            {
                string s = (rb.point1 + " - " + rb.point2);
                Debug.WriteLine(s);
            }
            Debug.WriteLine(max);
            using (StreamWriter sw = new StreamWriter(@"Вывод.csv", false, Encoding.Default, 10))
            {
                foreach (Rbt rb in itog[maxind])
                {
                    string s = (rb.point1 + " - " + rb.point2);
                    sw.WriteLine(s);
                }
                sw.WriteLine(max);
            }
        }
        /// <summary>
        /// Структура, хранящее перемещения и их значения
        /// </summary>
        public struct Rbt
        {
            public int point1; //точка начала перемещения
            public int point2; //точка окончания перемещения
            public int length; //значение перемещения
            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }
        /// <summary>
        /// Нахождение начальной точки
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int Nach(List<Rbt> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }
        /// <summary>
        /// Нахождение конечной точки
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int Kon(List<Rbt> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }
        /// <summary>
        /// Построение перемещений и подсчет их значений
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="minel"></param>
        /// <returns></returns>
        int Mv(List<Rbt> ls, Rbt minel)
        {
            int ret = 0;
            Rbt rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);//поиск возможных вариантов передвижения
            s += rb.point1.ToString() + "-" + rb.point2.ToString();//запись передвижения
            if (rb.point2 == ls[Kon(ls)].point2)//проверка на окончание пути
            {
                s += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)//определение значения найденного перемещения
                {
                    if (ls[i].point1 == rb.point2)
                    {
                        s += ",";
                        ret = Mv(ls, ls[i]) + rb.length;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        /// <returns></returns>
        public static List<Rbt> Flrd()
        {
            List<Rbt> ret = new List<Rbt>();
            try
            {

                using (StreamReader sr = new StreamReader("Ввод.csv"))
                {
                    while (sr.EndOfStream != true)
                    {
                        string[] str1 = sr.ReadLine().Split(';');
                        string[] str2 = str1[0].Split('-');
                        ret.Add(new Rbt { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                    }
                }
                return ret;
            }
            catch
            {
                Console.WriteLine("Ошибка");
                Console.ReadKey();
                System.Environment.Exit(1);
                return ret;
            }
        }
        /// <summary>
        /// Проверка на наличие ветвлений
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<Rbt> RtPrs(List<Rbt> ls, string s)
        {
            List<List<Rbt>> ret = new List<List<Rbt>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Rbt>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = ret[0][0].length, maxind = 0;
            for (int i = 0; i < ret.Count; i++)
            {
                if (FnlMv(ret[i]) >= max)
                {
                    max = FnlMv(ret[i]);
                    maxind = i;
                }
            }
            return ret[maxind];
        }
        /// <summary>
        /// Подсчет общего значения пути
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int FnlMv(List<Rbt> ls)
        {
            int ret = 0;
            foreach (Rbt rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }
    }
}
