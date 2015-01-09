using System;
using System.Data.Entity;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.EF.Test
{
    [TestClass]
    public class ClienteTest
    {
        [TestMethod]
        public void AgregarCliente()
        {
            Database.SetInitializer<ModelContainer>(new DropCreateDatabaseAlways<ModelContainer>());
            using (var context = new ModelContainer())
            {
                context.Database.Create();
                var cliente = new ClienteDominio
                {
                    FechaAlta = DateTime.Now,
                    Codigo = "1000",
                    Nombre = "Nombre",
                    Apellido = "apellido",
                    Cuil = "20364062479",
                    Barrio = "barrio test",
                    Direccion = "Direccion",
                    //Localidad = new LocalidadDominio
                    //{
                    //    FechaAlta = DateTime.Now,
                    //    Nombre = "Paraná"
                    //},
                    Sexo = Sexo.Masculino
                };

                context.Set<ClienteDominio>().Add(cliente);
                //context.Entry(cliente).State = System.Data.EntityState.Added;
                context.SaveChanges();
            }
        }
    }
}
