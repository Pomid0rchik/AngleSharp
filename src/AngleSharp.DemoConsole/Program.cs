using System;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Js;

namespace AngleSharp.DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            EventScriptingExample();
        }

        static async Task SimpleScriptingSample()
        {
            //We require a custom configuration
            var config = Configuration.Default.WithJs();

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //This is our sample source, we will set the title and write on the document
            var source = @"<!doctype html>
        <html>
        <head><title>Sample</title></head>
        <body>
        <script>
        document.title = 'Simple manipulation...';
        document.write('<span class=greeting>Hello World!</span>');
        </script>
        </body>";

            var document = await context.OpenAsync(req => req.Content(source));

            //Modified HTML will be output
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }

        public static async void ExtendedScriptingSample()
        {
            //We require a custom configuration with JavaScript and CSS
            var config = Configuration.Default.WithCss();

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //This is our sample source, we will do some DOM manipulation
            var source = @"<!doctype html>
        <html>
        <head><title>Sample</title></head>
        <style>
        .bold {
        font-weight: bold;
        }
        .italic {
        font-style: italic;
        }
        span {
        font-size: 12pt;
        }
        div {
        background: #777;
        color: #f3f3f3;
        }
        </style>
        <body>
        <div id=content></div>
        <script>
        (function() {
        var doc = document;
        var content = doc.querySelector('#content');
        var span = doc.createElement('span');
        span.id = 'myspan';
        span.classList.add('bold', 'italic');
        span.textContent = 'Some sample text';
        content.appendChild(span);
        var script = doc.querySelector('script');
        script.parentNode.removeChild(script);
        })();
        </script>
        </body>";

            var document = await context.OpenAsync(req => req.Content(source));

            //HTML will have changed completely (e.g., no more script element)
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }

        public async static void EventScriptingExample()
        {
            //We require a custom configuration
            var config = Configuration.Default.WithJs();

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //This is our sample source, we will trigger the load event
            var source = @"<!doctype html>
        <html>
        <head><title>Event sample</title></head>
        <body>
        <script>
        console.log('Before setting the handler!');

        document.addEventListener('load', function() {
        console.log('Document loaded!');
        });

        document.addEventListener('hello', function() {
        console.log('hello world from JavaScript!');
        });

        console.log('After setting the handler!');
        </script>
        </body>";

            var document = await context.OpenAsync(req => req.Content(source));

            //HTML should be output in the end
            Console.WriteLine(document.DocumentElement.OuterHtml);

            //Register Hello event listener from C# (we also have one in JS)
            document.AddEventListener("hello", (s, ev) =>
            {
                Console.WriteLine("hello world from C#!");
            });

            var e = document.CreateEvent("event");
            e.Init("hello", false, false);
            document.Dispatch(e);
        }
    }
}
