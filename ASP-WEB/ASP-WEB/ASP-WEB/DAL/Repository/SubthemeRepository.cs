﻿using ASP_WEB.DAL.Context;
using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.DAL.Repository
{
    public class SubthemeRepository : GenericRepository<Subtheme>, IGenericRepository<Subtheme>
    {
        public List<Subtheme> GetSubthemeByTheme(int themeID)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.ThemeID == themeID).ToList<Subtheme>();
            }
        }

        public override Subtheme GetByID(object id)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                int ID = Convert.ToInt32(id.ToString());
                return context.Subtheme.Where(st => st.SubthemeID == ID).Include(st => st.Office).Include(st => st.Theme).SingleOrDefault<Subtheme>();
            }
        }

        public List<Subtheme> Search(string searchString)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.Description.Contains(searchString)).Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }

        public override Subtheme Insert(Subtheme subtheme)
        {
            /*
             public void AddContact(Contact contact)
        {
            using (Labo1Context context = new Labo1Context())
            {

                if (contact.Departement != null)
                {
                    foreach (var dep in contact.Departement)
                    {
                        context.Entry<Departement>(dep).State = EntityState.Unchanged;
                    }
                }
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
        }
             */
            using (IntegratieContext context = new IntegratieContext())
            {
                if (subtheme.Office != null)
                {
                    foreach (var office in subtheme.Office)
                    {
                        context.Entry<Office>(office).State = EntityState.Unchanged;
                    }
                }
                context.Subtheme.Add(subtheme);
                context.SaveChanges();
            }
            return subtheme;
            //return base.Insert(entity);
        }

        public override void Update(Subtheme subtheme)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                var currentSubtheme = (from s in context.Subtheme.Include(s => s.Office)
                                       where s.OfficeID == subtheme.OfficeID
                                       select s).SingleOrDefault<Subtheme>();
                currentSubtheme.Office.Clear();
                context.SaveChanges();
            }
            using (IntegratieContext context = new IntegratieContext())
            {
                foreach (var office in subtheme.Office)
                {
                    context.Entry<Office>(office).State = EntityState.Added;
                }
                context.Entry<Subtheme>(subtheme).State = EntityState.Modified;
                context.SaveChanges();
            }

            // base.Update(entityToUpdate);
        }

        public override void Delete(Subtheme subtheme)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                context.Entry<Subtheme>(subtheme).State = EntityState.Deleted;
                context.SaveChanges();
            }
            // base.Delete(entityToDelete);
        }
    }
}
