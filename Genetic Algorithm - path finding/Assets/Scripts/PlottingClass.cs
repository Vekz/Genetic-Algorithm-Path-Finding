using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.IO;

public class PlottingClass 
{
    #region Fields

    #endregion


    #region Methods Which Create Specific Plots in pdf

    /// <summary>
    /// Plots average fitness graph to pdf in defalut directory
    /// </summary>
    /// <param name="Data"> List of lists of every creatures' fitness in given generation</param>
    public void AverageFitnessPerGeneration(List<List<float>> Data)
    {
        #region Functions

        /// Licz średnią
        double CalculateAverage(List<float> generation)
        {
            float sum = 0f;

            foreach(float fitness in generation)
            {
                sum += fitness;
            }

            return (double)(sum / generation.Count);
        }

        ///funkcja tworząca szereg funkcyjny funkcji avg(x) 
        FunctionSeries functionSeries(List<List<float>> list)
        {
            int Length = list.Count;
            FunctionSeries function = new FunctionSeries();
            for(int x = 0; x < Length; x ++)
            {
                for(int y = 0; y < Length; y++)
                {
                    DataPoint point = new DataPoint(x, CalculateAverage(list[x]));
                    function.Points.Add(point);
                }
            }

            return function;
        }

        #endregion

        /// Tworzenie Osi wykresu
        LinearAxis linearAxisX = new LinearAxis
        {
            Title = "Generation Number",
            Position = AxisPosition.Bottom,
            Minimum = 0,
            Maximum = 300,
            MajorGridlineStyle = LineStyle.Dot,
            MajorGridlineColor = OxyColor.FromRgb(128, 128, 128)
        };

        LinearAxis linearAxisY = new LinearAxis
        {
            Title = "Average Fitness",
            Position = AxisPosition.Left,
            MajorGridlineStyle = LineStyle.Dot,
            MajorGridlineColor = OxyColor.FromRgb(128,128,128)
        };

        PlotModel PM = new PlotModel();

        PM.Axes.Add(linearAxisX);
        PM.Axes.Add(linearAxisY);
        PM.Series.Add(functionSeries(Data));

        /// Przykład z dokumentacji OxyPlot na Tworzenie pdf
        /// TODO: Dynamiczna nazwa PDFu, inne formatowanie?
        using (var stream = File.Create("LatestAvgFit.pdf"))
        {
            var pdfExporter2 = new PdfExporter { Width = 600, Height = 400 };
            pdfExporter2.Export(PM, stream);
        }

         

    }

    
    #endregion

}
