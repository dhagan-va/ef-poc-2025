using EdiMettle.Database;
using EdiMettle.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.Repositories
{
    public class RepositoryThing
    {
        /// <summary>
        /// Adds a new T837 Institutional Claim to the database.
        /// </summary>
        /// <param name="newClaim">Fully populated Institutional Claim</param>
        /// <returns>Claim after having been added to the database</returns>
        public static InstitutionalClaim AddClaim(InstitutionalClaim newClaim)
        {
            using MettleEntities db = new();

            var claimEntity = db.InstitutionalClaims.Add(newClaim).Entity;

            return claimEntity;
        }
    }
}
