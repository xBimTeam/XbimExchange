using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieSQL.Model;

namespace Xbim.COBieSQL
{
    /// <summary>
    /// This is the main class to access COBie data using Entity Framework and SQL database as a backend.
    /// You can define various providers for the database in an App.config or Web.config of your application.
    /// </summary>
    public class CobieContext: DbContext
    {
        #region Constructors
        public CobieContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            
        }

        public CobieContext()
        {

        }
        #endregion

        #region DbSets

        public DbSet<Facility> Facilities { get; set; }

        #endregion
    }
}
