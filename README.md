#Varshaman - C# Implementation For Statsd As Cross Cutting Concern

Statsd is a state of the art measurement tool created by [Etsy]. It allows creation of graphs and metrics in unobtrusive way with minimal performance impact.

**Varshaman** is a C# wrapper for statsd that gives the ability to implement metrics creation as a cross cutting concern by integrating statsd excellent C# client [statsd-csharp-client] and [PostSharp].

##Usage

Use the [Nuget package] or clone this repo.

###Before You Start

Make sure to configure the statsd server IP and port, as well as an optional application prefix, that will be embedded into all metrics sent by the application. This should be done once in the app start file:

	var metricsConfig = new MetricsConfig
	{
		StatsdServerName = //IP,
		StatsdServerPort = //port, most of the times 8125
		Prefix = //app-prefix,
	};

	Metrics.Configure(metricsConfig);

###Attributes Over Methods

**Varshaman** exposes 3 attributes that can decorate a given method:

+ Timing - measures how much time took for a given method to be executed.
+ Counter - counts how many time a given method was called.
+ Gauge - constant value when a given method is called.

These 3 attributes makes calls statsd server and pass the relevant metrics data. 

You can use metric collection as a cross cutting concern for your application:

	[Timing("do-with-timing")]
	public void DoWithTiming()
	{

	}

	[Counter("do-with-counter")]
	public void DoWithCounter()
	{

	}

	[Gauge("do-with-gauge", 3)]
	public void DoWithGuage()
	{

	}

###Wrapping Code Blocks

You can wrap specific code blocks for timing measurements:

	//wrapping a code block
	using (Metrics.StartTimer("wrapping-code-block"))
	{
		//do something
	}

	//wrapping a Func
	var result = Metrics.Time(() =>
	{
		//some Func
	}, "wrapping-func");

	//wrapping an Action
	Metrics.Time(() => //some Action), "wrapping-action");

###Direct Calls

You can call the metric collector directly using the Metrics static class:

	Metrics.Counter("counter");
	Metrics.Gauge("gauge", 2);
	Metrics.Timer("timer", 23);

>Note: The code was built for .Net 3.5.

**Enjoy measuring!**

[Nuget package]: https://nuget.org/packages/Varshaman/
[Etsy]: https://github.com/etsy/statsd/
[statsd-csharp-client]: https://github.com/goncalopereira/statsd-csharp-client
[PostSharp]: http://www.postsharp.net/