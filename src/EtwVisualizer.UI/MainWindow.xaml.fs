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
        let rnd = System.Random()
        let mapper = LiveCharts.Configurations.Mappers.Xy<EtwEvent>().X(fun e -> e.Timestamp)
                                                                     .Y(fun e -> e.Value)
        let coll = new SeriesCollection(mapper)

        let series1 = new StackedAreaSeries()
        series1.Title <- "Gen#0"
        series1.Values <- new ChartValues<EtwEvent>(
            [| 
                for i in 0..99 -> { Timestamp = (float i); Value = rnd.NextDouble() * 100.0 }
            |] )
        series1.LineSmoothness <- 0.0 // for LineSeries

        let series2 = new LineSeries()
        series2.Title <- "% Time in GC"
        series2.Values <- new ChartValues<EtwEvent>( 
            [| 
                for i in 0..99 -> { Timestamp = (float i); Value = rnd.NextDouble() * 40.0 + 20.0}
            |] )

        System.Windows.Controls.Panel.SetZIndex(series1, 1)
        System.Windows.Controls.Panel.SetZIndex(series2, 2)

        coll.Add(series2)
        coll.Add(series1)
        coll

    member this.SectionsCollection =
        let coll = new SectionsCollection()
        let section1 = new AxisSection( Value = 4.0, SectionWidth = 4.0, Label = "Gen0" )
        System.Windows.Controls.Panel.SetZIndex(section1, 3)
        coll.Add(section1)
        coll

