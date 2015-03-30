CREATE TABLE [dbo].[Cliente] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Codigo]           NVARCHAR (10)  NULL,
    [Nombre]           NVARCHAR (100) NOT NULL,
    [Apellido]         NVARCHAR (100) NOT NULL,
    [Cuil]             NVARCHAR (11)  NOT NULL,
    [Fecha_Nacimiento] DATETIME       NULL,
    [Sexo]             INT            NOT NULL,
    [Direccion]        NVARCHAR (100) NOT NULL,
    [CalleA]           NVARCHAR (100) NULL,
    [CalleB]           NVARCHAR (100) NULL,
    [Barrio]           NVARCHAR (100) NULL,
    [Manzana]          NVARCHAR (2)   NULL,
    [Piso]             NVARCHAR (2)   NULL,
    [Departamento]     NVARCHAR (2)   NULL,
    [Telefono_Fijo]    NVARCHAR (11)  NULL,
    [Celular]          NVARCHAR (11)  NULL,
    [Email]            NVARCHAR (100) NULL,
    [Fecha_Alta]       DATETIME       NOT NULL DEFAULT getdate(),
    [Localidad_Id]     INT            NOT NULL,
    CONSTRAINT [PK_dbo.Cliente] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Cliente_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[Cliente]([Localidad_Id] ASC);

