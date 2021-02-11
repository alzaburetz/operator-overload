using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    //Задача 1 Мы логистическая компания.И у нас есть калькулятор для расчета расстояния перевозки грузов который складывает отрезки пути 
    //public class Calculator 
    //{ 
    //    public int SummTracks(params int[] path) 
    //    { 
    //        int result = 0; 
    //        foreach (var pathItem in path) 
    //            result += pathItem; 
    //        return result; 
    //    } 
    //}
    //И у нас возникла проблема.Из за того что часть товаров возят по дорогам и измеряют километрами,
    //а часть по морю и измеряют морскими милями часто получается ситуация когда разработчики
    //по ошибке складывают мили с километрами и получают неверные данные 
    //Как нам используя знания о конструировании типов создать типизацию при которой такая ошибка будет невозможна
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Retarded implementation
            List<Distance> distances = new List<Distance>();
            distances.Add(new Distance(1, typeof(Kilometers)));
            distances.Add(new Distance(2, typeof(Miles)));
            distances.Add(new Distance(5, typeof(Miles)));
            distances.Add(new Distance(10, typeof(Kilometers)));

            Console.WriteLine($"Sum of all distances: {SumDistanceRetarded(distances).Value} kilometers");
            #endregion


            List<Distance_> normal_distances = new List<Distance_>();

            normal_distances.Add(new Miles(2));
            normal_distances.Add(new Kilometers(3));

            Console.WriteLine($"Sum of all distances: {SumDistance(normal_distances).Value} {normal_distances[0].GetType().ToString()}");
        }

        static Distance SumDistanceRetarded(IReadOnlyList<Distance> distances)
        {
            Distance result = new Distance(0, typeof(Kilometers));
            foreach (var distance in distances)
            {
                result += distance;
            }
            return result;
        }

        static Distance_ SumDistance(IReadOnlyList<Distance_> distances)
        {
            if (distances[0].GetType() == typeof(Kilometers))
            {
                var result = new Kilometers();
                foreach (var distance in distances)
                {
                    result += distance;
                }
                return result;
            }
            else
            {
                var result = new Miles();
                foreach (var distance in distances)
                {
                    result += distance;
                }
                return result;
            }
        }
    }

    /// <summary>
    /// Retarded Implementation of a distance class
    /// </summary>
    public class Distance
    {
        public double Value { get; set; }
        public Type Type { get; set; }
        public static Distance operator + (Distance a, Distance b)
        {
            if (a.Type == typeof(Kilometers) && b.Type == typeof(Kilometers))
            {
                return new Distance(a.Value + b.Value, a.Type);
            }
            else if (a.Type == typeof(Miles) && b.Type == typeof(Kilometers))
            {
                return new Distance(a.Value * 1.852 + b.Value, b.Type);
            }
            else if (a.Type == typeof(Kilometers) && b.Type == typeof(Miles))
            {
                return new Distance(a.Value + b.Value * 1.852, a.Type);
            }
            else if (a.Type == typeof(Miles) && b.Type == typeof(Miles))
            {
                return new Distance((a.Value + b.Value) * 1.852, typeof(Kilometers));
            }
            return new Distance(a.Value, typeof(Kilometers));
        }

        public Distance(double value, Type type)
        {
            Value = value;
            Type = type;
        }

        public Distance() { }
    }

    /// <summary>
    /// Normal Implementation of a distance class no operator overload
    /// </summary>
    public class Distance_
    {
        public double Value { get; set; }

    }

    public class Kilometers : Distance_
    {
        public static Kilometers operator +(Kilometers a, Distance_ b)
        {
            if (b.GetType() == typeof(Miles))
            {
                return new Kilometers() { Value = a.Value + b.Value * 1.852 };
            }
            else
            {
                return new Kilometers() { Value = a.Value + b.Value };
            }
        }

        public Kilometers() { }
        public Kilometers(double value)
        {
            Value = value;
        }
    }

    public class Miles : Distance_
    {
        public static Miles operator +(Miles a, Distance_ b)
        {
            if (b.GetType() == typeof(Miles))
            {
                return new Miles() { Value = a.Value + b.Value };
            }
            else
            {
                return new Miles() { Value = a.Value + b.Value / 1.852 };
            }
        }

        public Miles() { }
        public Miles(double value)
        {
            Value = value;
        }
    }
}
