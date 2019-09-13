using System;
using System.Linq;

namespace l01_linear_regression_multi_dimension
{
    class Program
    {

        static void Main(string[] args)
        {
            // the following code is just to generate some sample data with known betas then try to model this data.
            //var m = new Model { b = new[] { 3d, 4, -7, 11 } };
            //Action<double[]> f = (double[] x) => { 
            //    Console.WriteLine($"({m.Predict(x)}d, new[]{{{string.Join("d,",x)}}}),"); 
            //};
            //f(new[] { 1d, 1, 1 });
            //f(new[] { 1d, 2, 3 });
            //f(new[] { 4d, 1, 1 });
            //f(new[] { 5d, -3, 0 });
            //f(new[] { 7d, 1, 11 });
            //f(new[] { 2d, 11, 1 });
            //f(new[] { 13d, 6, 5 });
            //f(new[] { 8d, 1, 1 });
            //f(new[] { 6d, 13, 7 });
            //f(new[] { 0d, -6, 0 });
            //f(new[] { 3d, 3, 1 });


            var data = new Data()
            {
                Rows = new[] {
                (11d, new[]{1d,1d,1}),
                (1d, new[]{1d,2d,3}),
                (20d, new[]{4d,1d,1}),
                (14d, new[]{5d,-3d,0}),
                (-41d, new[]{7d,1d,11}),
                (54d, new[]{2d,11d,1}),
                (39d, new[]{13d,6d,5}),
                (32d, new[]{8d,1d,1}),
                (32d, new[]{6d,13d,7}),
                (-13d, new[]{0d,-6d,0}),
                (25d, new[]{3d,3d,1})
            }
            };
            var model = LinearRegression.Train(data, 10000);

            Console.WriteLine($"{model.Predict(new[] { 1.0, 2, 3 })}");
        }
    }

    class Model
    {
        public double[] b;
        public double Predict(double[] x) => x.Zip(b, (xx, bb) => xx * bb).Sum() + b[x.Length];
    }

    public class Data
    {
        public (double y, double[] x)[] Rows;
        public int NumberOfFeatures { get { return Rows[0].x.Length; } }
    }

    class LinearRegression
    {
        public static Model Train(Data data, int maxNumerOfIterations)
        {
            var model = new Model { b = Enumerable.Repeat(10d, data.NumberOfFeatures + 1).ToArray() };
            var iteration = 0;
            var stepSize = 0.01d;
            var oldError = double.MaxValue;
            var error = MSE.Calc(model, data);
            var threshold = 0.0001;
            while (iteration < maxNumerOfIterations)
            {
                var gradients = MSE.Gradients(model, data);
                for (int i = 0; i < model.b.Length; i++)
                    model.b[i] -= gradients[i] * stepSize;
                Console.WriteLine($"iteration {iteration}, error {error}");
                Console.WriteLine($"\t Mode: [ {String.Join("| \t", model.b.Select(b => b.ToString()))} ]");
                Console.WriteLine($"\t Gradients: [ {String.Join(", ", gradients.Select(b => b.ToString()))} ]");
                oldError = error;
                error = MSE.Calc(model, data);
                iteration++;
            }
            return model;
        }
    }

    class MSE
    {
        public static double Calc(Model model, Data data) => 
            data.Rows.Select(i => Math.Pow(i.y - model.Predict(i.x), 2)).Sum() / data.Rows.Length;
        
        private static double GradientBeta_0(Model model, Data data) =>
            data.Rows.Select(i => i.y - model.Predict(i.x)).Sum() * (-2) / data.Rows.Length;

        private static double GradientBeta_k(Model model, Data data, int k) =>
            data.Rows.Select(i => (i.y - model.Predict(i.x)) * i.x[k]).Sum() * (-2) / data.Rows.Length;

        public static double[] Gradients(Model model, Data data) =>
            Enumerable.Range(0, data.NumberOfFeatures)
                    .Select(k => GradientBeta_k(model, data, k))
                    .Concat(new[] { GradientBeta_0(model, data) }).ToArray();
        
    }
}
