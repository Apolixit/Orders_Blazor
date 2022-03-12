using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Data
{
    interface ICopy<T>
    {
        void Copy(T other);
    }
}
