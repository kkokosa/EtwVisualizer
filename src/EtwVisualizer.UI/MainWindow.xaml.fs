namespace ViewModels

open System
open System.Windows
open FSharp.ViewModule
open FSharp.ViewModule.Validation
open FsXaml

open LiveCharts
open LiveCharts.Wpf
open System.Windows.Media

type MainView = XAML<"MainWindow.xaml", true>

type EtwEvent = { 
    Timestamp : double
    Value     : double
}

type MainViewModel() as self = 
    inherit ViewModelBase()    

    member this.SeriesCollection = 
        let mapper = LiveCharts.Configurations.Mappers.Xy<EtwEvent>().X(fun e -> e.Timestamp)
                                                                     .Y(fun e -> e.Value)
        let coll = new SeriesCollection(mapper)

        let series1 = new StackedAreaSeries()
        series1.Title <- "Gen#0"
        series1.Values <- new ChartValues<EtwEvent>( [| 
            { Timestamp = 1.0; Value = 2.0 };
            { Timestamp = 4.0; Value = 6.0 };
            { Timestamp = 5.0; Value = 4.0 };
            { Timestamp = 7.0; Value = 3.0 };
            { Timestamp = 9.0; Value = 0.0 }
        |] )
        series1.LineSmoothness <- 0.0 // for LineSeries

        let series2 = new LineSeries()
        series2.Title <- "% Time in GC"
        series2.Values <- new ChartValues<EtwEvent>( [| 
            { Timestamp = 1.0; Value = 0.0 };
            { Timestamp = 4.0; Value = 4.0 };
            { Timestamp = 5.0; Value = 3.0 };
            { Timestamp = 7.0; Value = 1.0 };
            { Timestamp = 9.0; Value = 4.0 }
        |] )
        series2.LineSmoothness <- 0.0 // for LineSeries
        series2.Fill <- Brushes.Transparent
        series2.AreaLimit <- 0.0

        System.Windows.Controls.Panel.SetZIndex(series1, 1)
        System.Windows.Controls.Panel.SetZIndex(series2, 2)
        //series.ColumnPadding <- 1.0
        coll.Add(series2)
        coll.Add(series1)
        coll
