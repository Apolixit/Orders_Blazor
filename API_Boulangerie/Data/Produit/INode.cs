using API_Orders.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Orders
{
    public interface INode
    {
        int InternalID { get; }
        string ID_Node { get; }
        string Name { get; }
        string Parent_ID { get; }
        bool IsLeaf { get; }
    }

    public static class GenericHelpers
    {
        //https://stackoverflow.com/questions/19648166/nice-universal-way-to-convert-list-of-items-to-tree

        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(this IEnumerable<T> collection, Func<T, K> id_selector, Func<T, K> parent_id_selector, K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    value = c,
                    children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }
}
