using System;
using System.Collections.Generic;
using System.Data.Entity;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.EF
{
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class ModelContainer : DbContext, IModelContainer
    {
        #region Constructor(s)

        public ModelContainer()
            : base("name=MasterEdicionesDbContext")
        {
            Database.SetInitializer(new ModelDbInitializer());
        }

        #endregion

        public new IDbSet<TEntidad> Set<TEntidad>() where TEntidad : class
        {
            return base.Set<TEntidad>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public class ModelDbInitializer : DropCreateDatabaseIfModelChanges<ModelContainer>
        {
            protected override void Seed(ModelContainer context)
            {
                var provincias = new List<ProvinciaDominio>
                {
                    new ProvinciaDominio {Nombre = "Entre Rios", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Santa Fe", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Córdoba", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "San Luis", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Salta", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "San Juan", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Chaco", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Chubut", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Santa Cruz", FechaAlta = DateTime.Now},
                };

                var zonaSinDefinir = new ZonaDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Zona sin definir",
                    FechaAlta = DateTime.Now
                };

                var localidades = new List<LocalidadDominio>
                {
                    new LocalidadDominio
                    {
                        Nombre = "Paraná",
                        Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Crespo",
                        Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Gualeguychú",
                        Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Oro Verde",
                        Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Santa Fe",
                        Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Coronda",
                        Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))
                    },
                    new LocalidadDominio
                    {
                        Nombre = "Venado Tuerto",
                        Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))
                    },
                };
                
                provincias.ForEach(p => context.Set<ProvinciaDominio>().Add(p));
                localidades.ForEach(l =>
                {
                    context.Set<LocalidadDominio>().Add(l);
                    l.FechaAlta = DateTime.Now;
                    l.Zona = zonaSinDefinir;
                });

                var cf = new IvaDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Consumidor Final",
                    Codigo = 1,
                    Alicuota = 0,
                    HabilitarEliminar = false
                };

                var monotributo = new IvaDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Monotributo",
                    Codigo = 2,
                    Alicuota = 0,
                    HabilitarEliminar = false
                };

                var ri = new IvaDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Responsable Inscripto",
                    Codigo = 3,
                    Alicuota = 21,
                    HabilitarEliminar = false
                };

                var exento = new IvaDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Exento",
                    Codigo = 4,
                    Alicuota = 0,
                    HabilitarEliminar = false
                };

                var noInscripto = new IvaDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "No Inscripto",
                    Codigo = 5,
                    Alicuota = 0,
                    HabilitarEliminar = false
                };

                var cliente = new ClienteDominio
                {
                    FechaAlta = DateTime.Now,
                    Codigo = 1,
                    Nombre = "Gonzalo",
                    Apellido = "Sanchez Bolaños",
                    Dni = 111111,
                    Cuil = "00-00000000-0",
                    Localidad = localidades[0],
                    Direccion = "San Juan 500",
                    Iva = cf
                };

                var cobrador = new CobradorDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Jose",
                    Apellido = "Lopez",
                    Dni = 35625222,
                    Localidad = localidades[0],
                    Localidades = new List<LocalidadDominio>
                    {
                        localidades[2],
                        localidades[3]
                    }
                };

                var vendedor = new VendedorDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Juan",
                    Apellido = "Lopez",
                    Dni = 35625222,
                    Localidad = localidades[0],
                    Localidades = new List<LocalidadDominio>
                    {
                        localidades[2],
                        localidades[3]
                    }
                };
                
                // Entidades sin definir
                var editorialSinDefinir = new EditorialDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Editorial sin definir",
                    FechaAlta = DateTime.Now
                };

                var rubroSinDefinir = new RubroDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Rubro sin definir",
                    FechaAlta = DateTime.Now
                };

                var gastoSinDefinir = new GastoDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Sin definir",
                    Descripcion = "Gasto sin definir",
                };

                var planPagoContado = new PlanPagoDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Contado",
                    Descripcion = "Contado",
                    Monto = 0,
                    CantidadCuotas = 0,
                    Tipo = TipoPlanPago.Contado
                };

                var planPago10 = new PlanPagoDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "10 x 100",
                    Descripcion = "10 cuotas de $100 c/una",
                    Monto = 100,
                    CantidadCuotas = 10,
                    Tipo = TipoPlanPago.Financiado
                };

                var enciclopedia = new ProductoDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Enciclopedia",
                    Descripcion = "Enciclopedia",
                    PrecioCosto = 80.25m,
                    PrecioVenta = 132.50m,
                    Rubro = rubroSinDefinir,
                    Editorial = editorialSinDefinir
                };

                var diccionario = new ProductoDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Diccionario",
                    Descripcion = "Diccionario",
                    PrecioCosto = 20.25m,
                    PrecioVenta = 42.50m,
                    Rubro = rubroSinDefinir,
                    Editorial = editorialSinDefinir
                };

                context.Set<PlanPagoDominio>().Add(planPagoContado);
                context.Set<PlanPagoDominio>().Add(planPago10);
                context.Set<GastoDominio>().Add(gastoSinDefinir);
                context.Set<ClienteDominio>().Add(cliente);
                context.Set<CobradorDominio>().Add(cobrador);
                context.Set<VendedorDominio>().Add(vendedor);
                context.Set<ZonaDominio>().Add(zonaSinDefinir);
                context.Set<EditorialDominio>().Add(editorialSinDefinir);
                context.Set<RubroDominio>().Add(rubroSinDefinir);
                context.Set<ProductoDominio>().Add(enciclopedia);
                context.Set<ProductoDominio>().Add(diccionario);
                context.Set<IvaDominio>().Add(cf);
                context.Set<IvaDominio>().Add(monotributo);
                context.Set<IvaDominio>().Add(ri);
                context.Set<IvaDominio>().Add(exento);
                context.Set<IvaDominio>().Add(noInscripto);
                context.SaveChanges();
            }
        }
    }
}
