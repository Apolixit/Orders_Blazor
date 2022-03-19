using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Data
{
    interface ICopy<T>
    {
        void Copy(T other);
    }
}
