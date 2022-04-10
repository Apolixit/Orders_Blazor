using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Data
{
    public class DbResponse<T>
    {
        public T? data { get; }
        public DbState statut { get; }

        public DbResponse(T? data_, DbState statut_)
        {
            data = data_;
            statut = statut_;
        }
    }

    public enum DbState
    {   
        /// <summary>
        /// Everything is ok
        /// </summary>
        OK,
        /// <summary>
        /// The element hasn't been found in database
        /// </summary>
        NOT_FOUND,
        /// <summary>
        /// Error communication with database
        /// </summary>
        ERROR,
        /// <summary>
        /// Incoherent parameter (null or also id < 0)
        /// </summary>
        INVALID_INPUT
    }
}
