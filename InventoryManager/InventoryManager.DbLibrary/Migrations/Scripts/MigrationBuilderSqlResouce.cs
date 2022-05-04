using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.DbLibrary.Migrations.Scripts
{
    public static class MigrationBuilderSqlResouce
    {
        public static OperationBuilder<SqlOperation> SqlResource(this MigrationBuilder mb, string relativeFileName)
        {
            using (var stream = Assembly.GetAssembly(typeof(MigrationBuilderSqlResouce)).GetManifestResourceStream(relativeFileName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);  
                    var data = ms.ToArray();
                    var text = Encoding.UTF8.GetString(data, 3, data.Length - 3);

                    return mb.Sql(text);
                }
            }

        }
    }
}
