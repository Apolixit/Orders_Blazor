using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders
{
    interface IEntitySaveChange
    {
        void onInsert();
        void onUpdate();
    }
}
