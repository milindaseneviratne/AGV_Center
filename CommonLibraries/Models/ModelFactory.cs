using CommonLibraries.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class ModelFactory
    {
        public void Convert()
        {
            sqlDbContextEF context = new sqlDbContextEF();

            var query = context.agvStationTestFlow
                .Select(a => new agvStationTestFlow
                {
                    Id= a.Id,
                    Dest_Station = a.Dest_Station
                }).ToList();

            ToDataTable(query);
        }
        private DataTable ToDataTable<T>(T entity) where T : class
        {
         
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
            return table;
        }
    }
}
