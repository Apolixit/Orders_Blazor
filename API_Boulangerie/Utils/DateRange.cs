using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Utils
{
    public class DateRange
    {
        public DateTime dtDebut { get; set; }
        public DateTime dtFin { get; set; }

        public DateRange()
        {
            this.dtDebut = DateTime.MinValue;
            this.dtFin = DateTime.MaxValue;
        }

        public DateRange(DateTime dt_)
        {
            this.dtDebut = new DateTime(dt_.Year, dt_.Month, dt_.Day, 0, 0, 0);
            this.dtFin = new DateTime(dt_.Year, dt_.Month, dt_.Day, 23, 59, 59);
        }

        public bool IsEmpty()
        {
            return this.dtDebut == DateTime.MinValue && this.dtFin == DateTime.MaxValue;
        }

        public bool IsInside(DateTime dt)
        {
            return !IsEmpty() && this.dtDebut <= dt && dt <= this.dtFin;
        }
    }
}
