package com.rawas

private object TypesHelper {
  type Data = List[(Double, List[Double])]

  def Data(vs: (Double, List[Double])*): List[(Double, List[Double])] = List(vs: _*)
}

import TypesHelper._

import scala.annotation.tailrec

object Application extends App {
  private val data = Data(
    (11, List(1, 1, 1)),
    (1, List(1, 2, 3)),
    (20, List(4, 1, 1)),
    (14, List(5, -3, 0)),
    (-41, List(7, 1, 11)),
    (54, List(2, 11, 1)),
    (39, List(13, 6, 5)),
    (32, List(8, 1, 1)),
    (32, List(6, 13, 7)),
    (-13, List(0, -6, 0)),
    (25, List(3, 3, 1))
  )

  private val model = LinearRegression.train(data, 10000)
  println(s"Model training complete: $model")
}


object LinearRegression {

  implicit class FoldLazy[A](trav: Iterable[A]) {
    def foldLeft1[B](z: B)(f: (B, A) => (B, Boolean)): B = {
      @tailrec def go(it: Iterator[A], z: B): B =
        if (it.hasNext) {
          val (z1, stop) = f(z, it.next)
          if (stop) z1 else go(it, z1)
        } else z

      go(trav.iterator, z)
    }
  }

  def train(data: Data, maxNumberOfIteration: Int): Model = {
    (1 to maxNumberOfIteration).foldLeft1(Model(List.fill(1 + data.head._2.length)(1d))) {
      case (model: Model, i: Int) =>
        val Gs = MSE.Gradients(model, data)
        val stepSize = 0.002 //1d / (i + 2)
        val err = MSE.calc(model, data)
        println(s"$i : $err, $Gs, $model")
        val newModel = Model(model.b.zip(Gs).map { case (b, g) => b - g * stepSize })
        val newErr = MSE.calc(newModel, data)
        (newModel, math.abs(err - newErr) < 0.0001)
    }
  }
}

case class Model(b: List[Double]) {
  def predict(x: List[Double]): Double = x.zip(b).map { case (x, bt) => x * bt }.sum + b.last
}

object MSE {
  def calc(model: Model, data: Data): Double = data.map {
    case (y, xs) => math.pow(y - model.predict(xs), 2)
  }.sum / data.length

  private def GradientBk(model: Model, data: Data, k: Int): Double = data.map {
    case (y, xs) => xs(k) * (y - model.predict(xs))
  }.sum * -2 / data.length

  private def GradientB0(model: Model, data: Data): Double = data.map {
    case (y, xs) => y - model.predict(xs)
  }.sum * -2 / data.length

  def Gradients(model: Model, data: Data): List[Double] =
    data.head._2.indices.map(i => GradientBk(model, data, i)).toList :::
      List(GradientB0(model, data))
}
