using System;
using System.Linq;
using EntityFramework.Data;

namespace ParadiseSuperMarket.Models
{
    public class DatabaseToPDFDataMapper
    {
        public static IQueryable<EntityFramework.Data.Dates> ReadDates()
        {
            ParadiseSupermarketChainEntities supermarketEntities = new ParadiseSupermarketChainEntities();
            IQueryable<EntityFramework.Data.Dates> dates =
                                   from d in supermarketEntities.Dates
                                   select d;
            return dates;
        }

        public static IQueryable<EntityFramework.Data.Sales> ReadProductsByDate(int dateId)
        {
            ParadiseSupermarketChainEntities supermarketEntities = new ParadiseSupermarketChainEntities();
            IQueryable<EntityFramework.Data.Sales> sales =
                from s in supermarketEntities.Sales
                where s.DateId == dateId
                select s;
            
            return sales;
        }
    }
}