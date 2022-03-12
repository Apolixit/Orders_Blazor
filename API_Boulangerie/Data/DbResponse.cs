using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Data
{
    public class DbResponse<T>
    {
        public T data { get; }
        public DbState statut { get; }

        public DbResponse(T data_, DbState statut_)
        {
            data = data_;
            statut = statut_;
        }
    }

    public enum DbState
    {
        /// <summary>
        /// Tout s'est bien passé
        /// </summary>
        OK,
        /// <summary>
        /// L'élément passé en paramètre n'a pas été trouvé dans la base de donnée
        /// </summary>
        NOT_FOUND,
        /// <summary>
        /// Une erreur est survenue lors de la communication avec la BDD (voir les logs)
        /// </summary>
        ERROR,
        /// <summary>
        /// L'élément passé en paramètre n'est pas cohérent (null ou bien id < 0)
        /// </summary>
        INVALID_INPUT
    }
}
