type Float = f64;
struct Point {x: Float,y: Float }
type Data = Vec<Point>;

use std::time::Instant;

fn main() {
    let data:Data = vec!(
        Point{x:1.,y:4.}
        , Point{x:2.,y:5.5}
        , Point{x:2.,y:6.}
        , Point{x:3.,y:9.}
        , Point{x:3.,y:10.}
    );
    let start = Instant::now();
    let iterations = 1000000;
    let m = LinearRegression::fit(data,iterations);
    let duration = start.elapsed();
    println!("{} , {} ",m.a,m.b);
    println!("Model build is done in {:?} s, for {} iterations!" , duration , iterations);
}

struct Model {
    a: Float,
    b: Float
}

impl Model {
    fn predict(&self, x: Float) -> Float {
        self.b + self.a * x
    }
}

struct MSE;
impl MSE{
    fn calc(data: &Data, model:&Model) -> Float {
        data.iter().map(|d| (d.y - model.predict(d.x)).powi(2) ).sum::<Float>() / (data.len() as Float)
    }
    fn gradient_a(data:&Data, model:&Model) -> Float {
        data.iter().map(|d| d.x * (d.y - model.predict(d.x))).sum::<Float>() / (-2. * data.len() as Float)
    }
    fn gradient_b(data:&Data, model:&Model) -> Float {
        data.iter().map(|d| d.y - model.predict(d.x)).sum::<Float>() / (-2. * data.len() as Float)
    }
}

struct LinearRegression ;
impl LinearRegression {
    fn fit(data:Data,iterations:i32) -> Model{
        (0 .. iterations).fold( Model{a:10.,b:10.}, |m,idx| {
            let ga = MSE::gradient_a(&data, &m);
            let gb = MSE::gradient_b(&data, &m);
            let step_size = 0.1;
            let err = MSE::calc(&data,&m);
            println!("{idx} : {err}, {ga}, {gb}, {}, {}",m.a,m.b);
            Model{a:m.a-ga* step_size, b:m.b-gb* step_size }
        })
    }
}