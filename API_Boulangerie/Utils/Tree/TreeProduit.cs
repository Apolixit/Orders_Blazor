using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Boulangerie.Utils.Tree
{
    public class TreeProduit
    {
        public string value { get; set; }
        public string label { get; set; }
        public int internalID { get; set; }
        public bool leaf { get; set; }
        public IEnumerable<TreeProduit> children { get; set; }

        public static IEnumerable<TreeProduit> Build(IEnumerable<TreeItem<IProduitNode>> tree)
        {
            return tree.Select(x => new TreeProduit() {
                value = x.value.ID_Node,
                label = x.value.Nom,
                internalID = x.value.InternalID,
                leaf = x.value.isLeaf,
                children = Build(x.children)
            });
        }
    }
}
