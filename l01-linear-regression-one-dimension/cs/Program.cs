using System;
using System.Linq;
namespace l01_linear_regression_one_dimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Data()
            {
                rows = new (double, double)[]{
                (0.8,1),
                (1,2),
                (2,3),
                (2.2,4),
                (2.3,5)
            }
            };
            var model = LinearRegression.train(data, 1000);

            Console.WriteLine($"priave for 6 bed rooms is {model.predict(6)}");
        }
    }
    class Model
    {
        public double a;
        public double b;
        public double predict(double x) => a * x + b;
    }
    class Data
    {
        public (double y, double x)[] rows;
        public int N { get { return rows.Length; } }
    }
    class LinearRegression
    {
        public static Model train(Data data, int maxNumerOfIterations)
        {
            var model = new Model() { a = 1, b = 1 };
            var iteration = 0;
            //var stepSize = 0.01d;
            var oldError = double.MaxValue;
            var error = MSE.calc(model, data);
            var threshold = 0.0001;
            while (iteration < maxNumerOfIterations && oldError - error > threshold || error > oldError)
            {

                var Ga = MSE.GradientA(model,data);
                var Gb = MSE.GradientB(model,data);
                var stepSize = 1d / (2 + iteration);
                Console.WriteLine($"{iteration}:\ta={model.a}\tb={model.b}\tGa={Ga}\tGb={Gb}\tE={error}\tStep={stepSize}");
                model.a -= Ga * stepSize;
                model.b -= Gb * stepSize;
                oldError = error;
                error = MSE.calc(model, data);
                iteration++;
            }
            return model;
        }
    }
    class MSE
    {
        public static double calc(Model model, Data data)
        {
            return data.rows.Select(r => Math.Pow(r.y - model.predict(r.x), 2)).Sum() / data.N;
        }
        public static double GradientA(Model model, Data data)
        {
            return data.rows.Select(r => -2 * r.x * (r.y - model.predict(r.x))).Sum() / data.N;
        }
        public static double GradientB(Model model, Data data)
        {
            return data.rows.Select(r => -2 * (r.y - model.predict(r.x))).Sum() / data.N;
        }
    }
}
