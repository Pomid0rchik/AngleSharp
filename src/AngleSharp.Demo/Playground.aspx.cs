using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Attributes;
using AngleSharp.Scripting;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Forms;
using AngleSharp.Html.InputTypes;
using AngleSharp.Html.Parser;

namespace AngleSharp.Demo
{
    public partial class Playground : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*IConfiguration configuration = new Configuration()
            {
                Services
            }
            BrowsingContext context = new BrowsingContext();*/
            var formatter = new MyFormatter();
            var parser = new HtmlParser(new HtmlParserOptions
            {
                IsNotConsumingCharacterReferences = true,
            });
            var html = "<p><h3>&amp;foo</h3></p>";
            var document = parser.ParseDocument(html);
            //Console.WriteLine(document.DocumentElement.ToHtml(formatter));
            Context.Response.Cookies.Add(new HttpCookie("Testing", document.DocumentElement.ToHtml(formatter)));
            Context.Response.Write(document.DocumentElement.ToHtml(formatter));
            
        }

        public class MyFormatter : IMarkupFormatter
        {
            public string CloseTag(IElement element, bool selfClosing) => HtmlMarkupFormatter.Instance.CloseTag(element, selfClosing);
            public string Comment(IComment comment) => HtmlMarkupFormatter.Instance.Comment(comment);
            public string Doctype(IDocumentType doctype) => HtmlMarkupFormatter.Instance.Doctype(doctype);
            public string LiteralText(ICharacterData text) => HtmlMarkupFormatter.Instance.LiteralText(text);
            public string OpenTag(IElement element, bool selfClosing) => HtmlMarkupFormatter.Instance.OpenTag(element, selfClosing);
            public string Processing(IProcessingInstruction processing) => HtmlMarkupFormatter.Instance.Processing(processing);
            public string Text(ICharacterData text) => text.Data;
        }
    }
}