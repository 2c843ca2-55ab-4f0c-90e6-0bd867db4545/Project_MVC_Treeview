using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.Mvc;
using System.IO;

namespace MVCTreeView.Models
{
    public class Composite
    {
    }

    public interface IComposite<T>
    {
        T Parent { get; }
        IEnumerable<T> Children { get; }
    }


    public class CompositeThing : IComposite<CompositeThing>
    {

        public CompositeThing()
        {

            Children = new List<CompositeThing>();

        }
        public string Name { get; set; }
        public CompositeThing Parent { get; set; }
        public IEnumerable<CompositeThing> Children { get; set; }

    }

    public static class TreeRenderHtmlHelper
    {
        public delegate void Func<T>(T value);

        public static void ForEach<T>(this IEnumerable<T> values, Func<T> function)
        {
            foreach (T value in values)
            {
                function(value);
            }
        }

        public static string RenderTree<T>(this HtmlHelper htmlHelper,IEnumerable<T> rootLocations,Func<T, string> locationRenderer)where T : IComposite<T>
        {
            return new TreeRenderer<T>(rootLocations, locationRenderer).Render();

        }
    }

    public class TreeRenderer<T> where T : IComposite<T>
    {
        

        private readonly Func<T, string> locationRenderer;

        private readonly IEnumerable<T> rootLocations;

        private HtmlTextWriter writer;

        public TreeRenderer(IEnumerable<T> rootLocations, Func<T, string> locationRenderer)
        {
            this.rootLocations = rootLocations;
            this.locationRenderer = locationRenderer;
            
        }

        public string Render()
        {

            writer = new HtmlTextWriter(new StringWriter());

            RenderLocations(rootLocations);

            return writer.InnerWriter.ToString();

        }

        /// <summary>

        /// Recursively walks the location tree outputting it as hierarchical UL/LI elements

        /// </summary>

        /// <param name="locations"></param>

        private void RenderLocations(IEnumerable<T> locations)
        {

            if (locations == null) return;

            if (locations.Count() == 0) return;

            InUl(() => locations.ForEach(location => InLi(() =>
            {

                writer.Write(locationRenderer(location));

                RenderLocations(location.Children);

            })));

        }



        private void InUl(Action action)
        {

            writer.WriteLine();

            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            action();

            writer.RenderEndTag();

            writer.WriteLine();

        }

        private void InLi(Action action)
        {

            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            action();

            writer.RenderEndTag();

            writer.WriteLine();

        }

    }
}