package com.ml

import scala.annotation.tailrec
import scala.io.Source

object Application extends App {
  private val filename = "../../train.csv"
  private val file = Source.fromFile(filename)
  private val lines = file.getLines().toList
  file.close()
  private val data = Data(
    lines.head.split(",").zipWithIndex.init.toMap ,
    lines.tail.map(_.split(",")).map(row=>Row(row.last,row.init.toList))
  )
  private val model = ID3.train(data,10)
  println(s"testing $model")
}

object ID3 {
  def train(data:Data,maxDepth:Int):Tree = {

    def train(data:Data,maxDepth:Int,level:Int):Node = {
      val (bestSplitCol,gain) = ID3.bestSplit(data)
      val splitIdx = data.columns(bestSplitCol)
      new SplitNode(gain,bestSplitCol,data
        .rows
        .groupBy(r => r.factos(splitIdx))
        .map { case (key, grp) =>
          //provide a prediction with the most frequent response in the data.
          if(level >= maxDepth)
            (key,LeafNode(grp.groupBy(r=>r.label).maxBy(g=>g._2.length)._1))
          //check if all remaining data is the same
          else if (grp.forall(_.label == grp.head.label))
            (key,LeafNode(grp.head.label))
          else
            (key,train(Data(data.columns, grp),maxDepth,level + 1))
        }
      )
    }

    val res = train(data,maxDepth,1).asInstanceOf[SplitNode]

    Tree(res.gain,res.splittingColumn,res.children)
  }
  private def bestSplit (data:Data) : ( String, Double) =
    data.columns.keys.map(col => (col, InformationGain.calc(data, col))).toList.maxBy{case (_,gain) => gain}
}

trait Node
case class LeafNode( response:String) extends Node
class SplitNode(val gain:Double,val splittingColumn:String,val children:Map[String,Node]) extends Node
case class Tree (override val gain:Double,override val splittingColumn:String, override val children:Map[String,Node])
  extends SplitNode(gain, splittingColumn, children) {

  def predict(columns: Map[String,Int], factors:List[String]): String = predict(columns,factors,this)

  @tailrec private def predict(columns: Map[String, Int], factors: List[String], node:Node): String = {
    node match {
      case ln:LeafNode =>  ln.response
      case sn:SplitNode =>
        val splitIdx = columns(sn.splittingColumn)
        val childNode = sn.children(factors(splitIdx))
        predict(columns,factors,childNode)
    }
  }
}

case class Row(label:String, factos:List[String])
case class Data(columns: Map[String,Int] , rows:List[Row])

object Entropy{
  private val log2:Double = math.log(2)
  def calc (data:List[String]): Double = data.
    groupBy(v=>v).
    map { case (_,g) =>
      val p = g.length.toDouble / data.length
      -p * math.log(p) / log2
    }.sum
}

object InformationGain{
  def calc(data:Data, column:String):Double = {
    val colIdx = data.columns(column)
    InformationGain.calc(data.rows.map(r=>(r.label,r.factos(colIdx))))
  }
  private def calc (data:List[(String,String)]):Double = {
    Entropy.calc(data.map{case (label,response) => label}) -
    data
      .groupBy{case (label,factor) => factor}
      .map{case key -> group =>(group.length.toDouble/data.length) * Entropy.calc(group.map{case (label,factor) => label})}
      .sum
  }
}