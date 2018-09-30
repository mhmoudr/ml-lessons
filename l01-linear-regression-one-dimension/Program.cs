using System;
namespace l01_linear_regression_one_dimension
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new Data(
                new double[] { 1, 2, 3, 4, 5 },
                new double[] { 0.8, 1, 2, 2.2, 2.3 });
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
        public readonly double[] x;
        public readonly double[] y;
        public readonly int size;
        public int N { get { return size; } }
        public Data(double[] px, double[] py)
        {
            if (px.Length != py.Length)
                throw new Exception("Invalid data");
            x = px;
            y = py;
            size = px.Length;
        }
    }
    class LinearRegression
    {
        public static Model train(Data data, int maxNumerOfIterations)
        {
            var model = new Model() { a = 1, b = 1 };
            var iteration = 0;
            var stepSize = 0.01d;
            while (iteration < maxNumerOfIterations)
            {
                var Ga = 0d;
                var Gb = 0d;
                for (int i = 0; i < data.size; i++)
                {
                    Ga += -2 * data.x[i] * (data.y[i] - (model.a * data.x[i] + model.b));
                    Gb += -2 * (data.y[i] - (model.a * data.x[i] + model.b));
                }
                Ga /= data.size;
                Gb /= data.size;
                Console.WriteLine($"{iteration}:\ta={model.a}\tb={model.b}\tGa={Ga}\tGb={Gb}");
                model.a -= Ga * stepSize;
                model.b -= Gb * stepSize;
                iteration++;
            }
            return model;
        }
    }
}
