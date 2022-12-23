package com.rawas

private object TypesHelper {
  type Data = List[(Double, Double)]

  def Data(vs: (Double, Double)*): List[(Double, Double)] = List(vs: _*)
}

import TypesHelper._

import scala.annotation.tailrec

object Application extends App {
  private val data = Data(
    (0.8, 1)
    , (1, 2)
    , (2, 3)
    , (2.2, 4)
    , (2.3, 5)
  )

  val model = LinearRegression.train(data, 10000)
  println("Model training complete")
}


object LinearRegression {

  implicit class FoldLazy[A](trav: Iterable[A]) {
    def foldLeft1[B](z: B)(f: (B, A) => (B, Boolean)): B = {
      @ tailrec def go(it: Iterator[A], z: B): B =
        if (it.hasNext) {
          val (z1, stop) = f(z, it.next)
          if (stop) z1 else go(it, z1)
        } else z
      go(trav.iterator, z)
    }
  }

  def train(data: Data, maxNumberOfIteration: Int): Model = {
    (1 to maxNumberOfIteration).foldLeft1(Model(1, 1)) {
      case (model: Model, i: Int) =>
        val Ga = MSE.GradientA(model, data)
        val Gb = MSE.GradientB(model, data)
        val stepSize = 0.2 //1d / (i + 2)
        val err = MSE.calc(model, data)
        println(s"$i : $err, $Ga, $Gb, ${model.a}, ${model.b}")
        val newModel = Model(model.a - Ga * stepSize, model.b - Gb * stepSize)
        val newErr = MSE.calc(newModel, data)
        (newModel, math.abs(err - newErr) < 0.0001)
    }
  }
}

case class Model(a: Double, b: Double) {
  def predict(x: Double): Double = a * x + b
}

object MSE {
  def calc(model: Model, data: Data): Double = data.map {
    case (x, y) => math.pow(y - model.predict(x), 2)
  }.sum / data.length

  def GradientA(model: Model, data: Data): Double = data.map {
    case (x, y) => x * (y - model.predict(x))
  }.sum * -2 / data.length

  def GradientB(model: Model, data: Data): Double = data.map {
    case (x, y) => y - model.predict(x)
  }.sum * -2 / data.length
}
