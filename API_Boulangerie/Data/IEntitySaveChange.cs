using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie
{
    interface IEntitySaveChange
    {
        void onInsert();
        void onUpdate();
    }
}
