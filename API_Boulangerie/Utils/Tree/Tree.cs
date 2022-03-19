using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Orders.Utils.Tree
{
    public class TreeItem<T>
    {
        public T value { get; set; }
        public IEnumerable<TreeItem<T>> children { get; set; }

        public static IEnumerable<TreeItem<INode>> GetTree(IEnumerable<INode> list, string parent)
        {
            return list.Where(x => x.Parent_ID == parent).Select(x => new TreeItem<INode>
            {
                value = x,
                children = GetTree(list, x.ID_Node)
            }).ToList();
        }
    }
}
