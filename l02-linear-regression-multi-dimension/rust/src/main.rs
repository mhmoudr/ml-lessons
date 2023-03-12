type Float = f64;
struct Point {y: Float, x: Vec<Float>}
type Data = Vec<Point>;

use std::time::Instant;

fn main() {
    let data: Data = vec!(
        Point { y: 1.1, x: vec!(1., 500., -1.) }
        , Point { y: 1.7, x: vec!(1., 200., -3.) }
        , Point { y: 2.2, x: vec!(2., 1500., -1.) }
        , Point { y: 2.7, x: vec!(2., 300., -7.) }
        , Point { y: 3.0, x: vec!(2., 200., -6.) }
        , Point { y: 3.5, x: vec!(3., 1200., -3.) }
        , Point { y: 3.7, x: vec!(3., 900., -3.) }
        , Point { y: 4.7, x: vec!(4., 200., -6.) }
    );
    let start = Instant::now();
    let iterations = (10 as i32).pow(6);
    let m = LinearRegression::fit(&data, iterations);
    let duration = start.elapsed();
    println!("{:?}", m.b);
    println!("Model build is done in {:?} s, for {} iterations!", duration, iterations);
}

struct Model {
    b: Vec<Float>
}

impl Model {
    fn predict(&self, x: &Vec<Float>) -> Float {
        x.iter().zip(self.b.clone()).map(|x| x.0 * x.1).sum::<Float>() + self.b.last().unwrap()
    }
}

struct MSE;
impl MSE{
    fn calc(data: &Data, model:&Model) -> Float {
        data.iter().map(|d| (d.y - model.predict(&d.x)).powi(2) ).sum::<Float>() / (data.len() as Float)
    }
    fn gradient_b(data:&Data, model:&Model , i:usize) -> Float {
        data.iter().map(|d| d.x[i] * (d.y - model.predict(&d.x))).sum::<Float>() / (-2. * data.len() as Float)
    }
    fn gradient_b0(data:&Data, model:&Model) -> Float {
        data.iter().map(|d| d.y - model.predict(&d.x)).sum::<Float>() / (-2. * data.len() as Float)
    }
    fn gradients(data:&Data, model:&Model) -> Vec<Float> {
        (0 .. data[0].x.len()).map(|idx| MSE::gradient_b(data,model,idx)).chain(vec!(MSE::gradient_b0(data,model))).collect()
    }
}

struct LinearRegression ;
impl LinearRegression {
    fn fit(data:&Data,iterations:i32) -> Model{
        let init_model = Model{b:(0..=data[0].x.len()).map(|_|1.).collect()};
        println!("{:?}",init_model.b);
        (0 .. iterations).fold(init_model, |m, idx| {
            let gb = MSE::gradients(&data, &m);
            let step_size = 0.000001;
            let err = MSE::calc(&data,&m);
            println!("{idx} : {err}, {:?}, {:?}",m.b,gb);
            Model{b:m.b.iter().zip(gb).map(|i|i.0-i.1* step_size).collect() }
        })
    }
}