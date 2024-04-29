using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Interfaces
{
    public interface ITree<T> where T : class
    {
        public long Id { get; set; }

        public long ParentId { get; set; }

        public IEnumerable<T>? Children { get; set; }
    }


    public static class TreeExtensions
    {
        public static IEnumerable<T> BuildTree<T>(this IEnumerable<T> source, long parentId = 0)
            where T : class, ITree<T>
        {
            var children = source.Where(p => p.ParentId == parentId).ToList();
            foreach (T item in children)
            {
                item.Children = source.BuildTree(item.Id);
            }
            return children;
        }


        public static IEnumerable<TableTreeNode<T>> ToTableTreeNode<T>(this IEnumerable<T> source, TableTreeNode<T>? parent = null)
            where T : class, ITree<T>
        {
            var result = new List<TableTreeNode<T>>();
            foreach (T item in source)
            {
                if (item is not null)
                {
                    TableTreeNode<T> node = new TableTreeNode<T>(item);

                    if (parent != null)
                        node.Parent = parent;

                    if (item.Children != null)
                    {
                        node.Items = (item.Children).ToTableTreeNode(node);
                        if (node.Items.Any())
                            node.HasChildren = true;
                    }

                    result.Add(node);
                }
            }
            return result;
        }

    }
}
